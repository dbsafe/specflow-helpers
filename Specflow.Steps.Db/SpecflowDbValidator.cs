using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.Object.Collections;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db
{
    public class SpecflowDbValidator
    {
        private readonly string _connectionString;

        public SpecflowDbValidator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AssertTable(string tableName, Table table, FormatterManager formatter = null)
        {
            if (formatter == null)
            {
                formatter = new FormatterManager();
            }

            SpecflowDb.AssertTableName(tableName, _connectionString);

            var expectedDataCollection = DataCollection.Load(table);
            SpecflowDb.AssertTableSchema(tableName, expectedDataCollection, _connectionString);

            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = SqlDatabaseHelper.BuildDataCollection(_connectionString, tableName, fields, formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Table '{tableName}'.{Environment.NewLine}{message}");
            }
        }
    }
}
