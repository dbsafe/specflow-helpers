using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql
{
    public static class SpecflowDb
    {
        public static void AssertTableSchema(string tableName, Table table, string connectionString)
        {
            var columnNames = SqlDatabaseHelper.GetColumnNames(connectionString, tableName).Select(a => a.ToUpper());
            foreach (var header in table.Header)
            {
                Assert.IsTrue(columnNames.Contains(header.ToUpper()), $"Column '{header}' not found");
            }
        }

        public static void AssertTableSchema(string tableName, Object.Collections.DataCollection dataCollection, string connectionString)
        {
            var columnNames = SqlDatabaseHelper.GetColumnNames(connectionString, tableName).Select(a => a.ToUpper());

            Assert.IsTrue(dataCollection.Rows.Length > 0, $"Table '{tableName}'. The number of rows cannot be zero");
            foreach (var cell in dataCollection.Rows[0].Values)
            {
                Assert.IsTrue(columnNames.Contains(cell.Name.ToUpper()), $"Column '{cell.Name}' not found");
            }
        }

        public static void AssertTableName(string tableName, string connectionString)
        {
            var tableFound = SqlDatabaseHelper.IsObjectValid(connectionString, tableName);
            Assert.IsTrue(tableFound, $"Table '{tableName}' not found");
        }
    }
}
