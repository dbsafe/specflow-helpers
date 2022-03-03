using DbSafe;
using Specflow.Steps.Db.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Specflow.Steps.Db.Sql
{

    // Kept for backward compatibility
    public static class SqlDatabaseHelper
    {
        public static bool IsObjectValid(string connectionString, string objectName)
        {
            var command = $"IF OBJECT_ID ('{objectName}', 'U') IS NOT NULL SELECT 1 AS res ELSE SELECT 0 AS res;";

            using (var conn = CreateDbConnection(connectionString))
            {
                using (var comm = CreateDbCommand(command, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader(CommandBehavior.KeyInfo);
                    reader.Read();
                    return reader.GetInt32(0) == 1;
                }
            }
        }

        public static IEnumerable<string> GetColumnNames(string connectionString, string tableName)
        {
            var result = new List<string>();
            var command = $"SELECT TOP 1 * FROM {tableName};";

            using (var conn = CreateDbConnection(connectionString))
            {
                using (var comm = CreateDbCommand(command, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader(CommandBehavior.KeyInfo);
                    var tableSchema = reader.GetSchemaTable();

                    for (int i = 0; i < tableSchema.Rows.Count; i++)
                    {
                        var row = tableSchema.Rows[i];
                        result.Add(row["ColumnName"].ToString());
                    }
                }
            }

            return result;
        }

        public static Object.Collections.DataCollection BuildDataCollection(string connectionString, string tableName, IEnumerable<string> fields, FormatterManager formatter)
        {
            return BuildDataCollection(connectionString, tableName, fields, null, formatter);
        }

        private static IEnumerable<FieldFilter> AddQuotationMarks(IEnumerable<FieldFilter> filters)
        {
            FieldFilter AddQuotationMarks(FieldFilter filter)
            {
                var valuesWithQuotationMarks = filter.FieldValues.Split(',').Select(a => $"'{a.Trim()}'");
                return new FieldFilter
                {
                    FieldName = filter.FieldName,
                    FieldValues = string.Join(",", valuesWithQuotationMarks)
                };
            }

            return filters.Select(AddQuotationMarks);
        }

        public static Object.Collections.DataCollection BuildDataCollection(string connectionString, string tableName, IEnumerable<string> fields, IEnumerable<FieldFilter> filters, FormatterManager formatter)
        {
            var rows = new List<Object.Collections.DataRow>();
            var command = $"SELECT {string.Join(",", fields)} FROM {tableName}";

            if (filters != null && filters.Any())
            {
                filters = AddQuotationMarks(filters);
                var filtering = new List<string>();
                foreach (var filter in filters)
                {
                    filtering.Add($"{filter.FieldName} IN ({filter.FieldValues})");
                }

                command = $"{command}{Environment.NewLine}{"WHERE"} {string.Join(" AND ", filtering)}";
            }

            using (var conn = CreateDbConnection(connectionString))
            {
                using (var comm = CreateDbCommand(command, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader(CommandBehavior.KeyInfo);
                    while (reader.Read())
                    {
                        var dataRow = new Object.Collections.DataRow();
                        rows.Add(dataRow);

                        var dataCells = new List<Object.Collections.DataCell>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var dbValue = reader[i];
                            var value = dbValue.GetType() == typeof(DBNull) ? null : formatter.Format(tableName, columnName, dbValue);
                            var cell = new KeyValuePair<string, string>(columnName, value);
                            var dataCell = Object.Collections.DataCell.Load(cell);
                            dataCells.Add(dataCell);
                        }

                        dataRow.Values = dataCells.ToArray();
                    }
                }
            }

            return new Object.Collections.DataCollection
            {
                Rows = rows.ToArray(),
            };
        }

        private static SqlConnection CreateDbConnection(string connectionString) => new SqlConnection(connectionString);

        private static SqlCommand CreateDbCommand(string command, SqlConnection conn) => new SqlCommand(command, conn);
    }
}
