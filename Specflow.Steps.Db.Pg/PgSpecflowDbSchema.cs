using DbSafe;
using Npgsql;
using PgDbSafe;
using Specflow.Steps.Db.Shared;
using System;
using System.Collections.Generic;
using System.Data;

namespace Specflow.Steps.Db.Pg
{
    public class PgSpecflowDbSchema : ISpecflowDbSchema<NpgsqlConnection, NpgsqlCommand>
    {
        private readonly string _connectionString;

        public PgSpecflowDbSchema(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Object.Collections.DataCollection BuildDataCollection(string tableName, IEnumerable<string> fields, FormatterManager formatter)
        {
            var rows = new List<Object.Collections.DataRow>();
            var command = $"SELECT {string.Join(",", fields)} FROM {tableName}";

            using (var conn = CreateDbConnection(_connectionString))
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

        public IEnumerable<string> GetColumnNames(string tableName)
        {
            var result = new List<string>();
            var command = $"SELECT * FROM {tableName} LIMIT 1;";

            using (var conn = CreateDbConnection(_connectionString))
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

        public bool IsObjectValid(string objectName)
        {
            var command = $"SELECT to_regclass('{objectName}') IS NOT NULL AS res;";

            using (var conn = CreateDbConnection(_connectionString))
            {
                using (var comm = CreateDbCommand(command, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader(CommandBehavior.KeyInfo);
                    reader.Read();
                    return reader.GetBoolean(0);
                }
            }
        }

        private static NpgsqlConnection CreateDbConnection(string connectionString) => new NpgsqlConnection(connectionString);

        private static NpgsqlCommand CreateDbCommand(string command, NpgsqlConnection conn) => new NpgsqlCommand(command, conn);

        public AdoDatabaseClient<NpgsqlConnection, NpgsqlCommand> GetClient()
        {
            return new PgDatabaseClient(false)
            {
                ConnectionString = _connectionString
            };
        }
    }
}
