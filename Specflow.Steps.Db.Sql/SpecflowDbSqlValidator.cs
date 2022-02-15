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

        public SpecflowDbSqlValidator(string connectionString, FormatterManager formatter)
        {
            _connectionString = connectionString;
            _formatter = formatter;
        }

        public void AssertTable(string tableName, Table table)
        {
            SpecflowDb.AssertTableName(tableName, _connectionString);

            var expectedDataCollection = DataCollection.Load(table);
            SpecflowDb.AssertTableSchema(tableName, expectedDataCollection, _connectionString);

            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = SqlDatabaseHelper.BuildDataCollection(_connectionString, tableName, fields, _formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Table '{tableName}'.{Environment.NewLine}{message}");
            }
        }
    }
}
