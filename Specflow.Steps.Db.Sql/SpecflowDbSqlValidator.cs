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
        private readonly ISpecflowDbSchema _specflowDbSchema;

        public SpecflowDbSqlValidator(string connectionString, FormatterManager formatter)
        {
            _connectionString = connectionString;
            _formatter = formatter;
            _specflowDbSchema = new SqlSpecflowDbSchema(connectionString);
            _specflowDb = new SpecflowDb(_specflowDbSchema);
        }

        public void AssertTable(string tableName, Table table)
        {
            _specflowDb.AssertTableName(tableName, _connectionString);

            var expectedDataCollection = DataCollection.Load(table);
            _specflowDb.AssertTableSchema(tableName, expectedDataCollection, _connectionString);

            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = _specflowDbSchema.BuildDataCollection(tableName, fields, _formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Table '{tableName}'.{Environment.NewLine}{message}");
            }
        }
    }
}
