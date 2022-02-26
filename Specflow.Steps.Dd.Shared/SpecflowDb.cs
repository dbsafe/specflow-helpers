using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Shared
{
    public interface ISpecflowDbSchema
    {
        IEnumerable<string> GetColumnNames(string tableName);
        bool IsObjectValid(string objectName);
        Object.Collections.DataCollection BuildDataCollection(string tableName, IEnumerable<string> fields, FormatterManager formatter);
    }

    public class SpecflowDb
    {
        private readonly ISpecflowDbSchema _specflowDbSchema;

        public SpecflowDb(ISpecflowDbSchema specflowDbSchema)
        {
            _specflowDbSchema = specflowDbSchema;
        }

        public void AssertTableSchema(string tableName, Table table, string connectionString)
        {
            var columnNames = _specflowDbSchema.GetColumnNames(tableName).Select(a => a.ToUpper());
            foreach (var header in table.Header)
            {
                Assert.IsTrue(columnNames.Contains(header.ToUpper()), $"Column '{header}' not found");
            }
        }

        public void AssertTableSchema(string tableName, Object.Collections.DataCollection dataCollection, string connectionString)
        {
            var columnNames = _specflowDbSchema.GetColumnNames(tableName).Select(a => a.ToUpper());

            Assert.IsTrue(dataCollection.Rows.Length > 0, $"Table '{tableName}'. The number of rows cannot be zero");
            foreach (var cell in dataCollection.Rows[0].Values)
            {
                Assert.IsTrue(columnNames.Contains(cell.Name.ToUpper()), $"Column '{cell.Name}' not found");
            }
        }

        public void AssertTableName(string tableName, string connectionString)
        {
            var tableFound = _specflowDbSchema.IsObjectValid(tableName);
            Assert.IsTrue(tableFound, $"Table '{tableName}' not found");
        }
    }
}
