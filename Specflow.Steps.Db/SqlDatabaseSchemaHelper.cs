using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Specflow.Steps.Db
{
    public static class SqlDatabaseSchemaHelper
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

        public static IEnumerable<string> GetColumnNames(string connectionString, string tablename)
        {
            var result = new List<string>();
            var command = $"SELECT TOP 1 * FROM {tablename};";

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

        private static SqlConnection CreateDbConnection(string connectionString) => new SqlConnection(connectionString);

        private static SqlCommand CreateDbCommand(string command, SqlConnection conn) => new SqlCommand(command, conn);
    }
}
