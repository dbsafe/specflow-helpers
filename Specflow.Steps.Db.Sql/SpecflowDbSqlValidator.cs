using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.Db.Shared;
using Specflow.Steps.Object.Collections;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql
{
    public class SpecflowDbSqlValidator : ISpecflowDbValidator
    {
        private readonly string _connectionString;
        private readonly FormatterManager _formatter;
        private readonly SpecflowDb _specflowDb;

        public SpecflowDbSqlValidator(string connectionString, FormatterManager formatter)
        {
            _connectionString = connectionString;
            _formatter = formatter;
            _specflowDb = new SpecflowDb();
        }

        public void AssertTable(string tableName, Table table)
        {
            _specflowDb.AssertTableName(tableName, _connectionString);

            var expectedDataCollection = DataCollection.Load(table);
            _specflowDb.AssertTableSchema(tableName, expectedDataCollection, _connectionString);

            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = SqlDatabaseHelper.BuildDataCollection(_connectionString, tableName, fields, _formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Table '{tableName}'.{Environment.NewLine}{message}");
            }
        }
    }
}
