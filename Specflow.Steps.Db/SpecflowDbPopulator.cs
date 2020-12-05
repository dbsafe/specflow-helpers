using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDbSafe;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db
{
    public class SpecflowDbPopulator
    {
        private readonly string _connectionString;

        public SpecflowDbPopulator(string connectionString, FormatterManager formatter)
        {
            _connectionString = connectionString;
        }

        public void SetTable(string tableName, Table table, bool setIdentityInsert)
        {
            AssertTableName(tableName);
            AssertTableSchema(tableName, table);
            PopulateTable(tableName, table, setIdentityInsert);
        }

        private void PopulateTable(string tableName, Table table, bool setIdentityInsert)
        {
            var datasetElement = DataConverter.BuildDatasetElement(tableName, table, setIdentityInsert);
            var sqlDatabaseClient = new SqlDatabaseClient(false)
            {
                ConnectionString = _connectionString
            };

            sqlDatabaseClient.WriteTable(datasetElement);
        }

        private void AssertTableSchema(string tableName, Table table)
        {
            var columnNames = SqlDatabaseSchemaHelper.GetColumnNames(_connectionString, tableName).Select(a => a.ToUpper());            
            foreach (var header in table.Header)
            {
                Assert.IsTrue(columnNames.Contains(header.ToUpper()), $"Column '{header}' not found");
            }
        }

        private void AssertTableName(string tableName)
        {
            var tableFound = SqlDatabaseSchemaHelper.IsObjectValid(_connectionString, tableName);
            Assert.IsTrue(tableFound, $"Table '{tableName}' not found");
        }
    }
}
