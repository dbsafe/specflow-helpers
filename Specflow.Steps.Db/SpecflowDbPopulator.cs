using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db
{
    public class SpecflowDbPopulator
    {
        private readonly SqlDatabaseClient _sqlDatabaseClient;

        public SpecflowDbPopulator(string connectionString)
        {
            _sqlDatabaseClient = new SqlDatabaseClient(connectionString);
        }

        public void SetTable(string tableName, Table table)
        {
            AssertTableName(tableName);
            AssertTableSchema(tableName, table);
        }

        private void AssertTableSchema(string tableName, Table table)
        {
            var columnNames = _sqlDatabaseClient.GetColumnNames(tableName).Select(a => a.ToUpper());            
            foreach (var header in table.Header)
            {
                Assert.IsTrue(columnNames.Contains(header.ToUpper()), $"Column '{header}' not found");
            }
        }

        private void AssertTableName(string tableName)
        {
            var tableFound = _sqlDatabaseClient.IsObjectValid(tableName);
            Assert.IsTrue(tableFound, $"Table '{tableName}' not found");
        }
    }

    public class SpecflowDbValidator
    {
        public void AssertTable(string tableName, Table table)
        {

        }
    }

    public class SqlDatabaseClient
    {
        private readonly string _connectionString;

        public SqlDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsObjectValid(string objectName)
        {
            var command = $"IF OBJECT_ID ('{objectName}', 'U') IS NOT NULL SELECT 1 AS res ELSE SELECT 0 AS res;";

            using (var conn = CreateDbConnection(_connectionString))
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

        public IEnumerable<string> GetColumnNames(string tablename)
        {
            var result = new List<string>();
            var command = $"SELECT TOP 1 * FROM {tablename};";

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

        private SqlConnection CreateDbConnection(string connectionString) => new SqlConnection(connectionString);

        private SqlCommand CreateDbCommand(string command, SqlConnection conn) => new SqlCommand(command, conn);
    }
}
