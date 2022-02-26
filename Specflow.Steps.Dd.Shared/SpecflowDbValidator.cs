using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.Object.Collections;
using System;
using System.Data.Common;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Shared
{
    public class SpecflowDbValidator<TDbConnection, TDbCommand> : ISpecflowDbValidator where TDbConnection : DbConnection where TDbCommand : DbCommand
    {
        private readonly FormatterManager _formatter;
        private readonly SpecflowDb<TDbConnection, TDbCommand> _specflowDb;
        private readonly ISpecflowDbSchema<TDbConnection, TDbCommand> _specflowDbSchema;

        public SpecflowDbValidator(ISpecflowDbSchema<TDbConnection, TDbCommand> specflowDbSchema, FormatterManager formatter)
        {
            _formatter = formatter;
            _specflowDbSchema = specflowDbSchema;
            _specflowDb = new SpecflowDb<TDbConnection, TDbCommand>(_specflowDbSchema);
        }

        public void AssertTable(string tableName, Table table)
        {
            _specflowDb.AssertTableName(tableName);

            var expectedDataCollection = DataCollection.Load(table);
            _specflowDb.AssertTableSchema(tableName, expectedDataCollection);

            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = _specflowDbSchema.BuildDataCollection(tableName, fields, _formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Table '{tableName}'.{Environment.NewLine}{message}");
            }
        }
    }
}
