using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.Object.Collections;
using System;
using System.Collections.Generic;
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

        public void AssertTable(string tableName, Table table, IEnumerable<FieldFilter> filters)
        {
            _specflowDb.AssertTableName(tableName);

            var expectedDataCollection = DataCollection.Load(table);
            _specflowDb.AssertTableSchema(tableName, expectedDataCollection, filters);

            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = _specflowDbSchema.BuildDataCollection(tableName, fields, filters, _formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Table '{tableName}'.{Environment.NewLine}{message}");
            }
        }

        public void AssertQuery(string query, Table table)
        {
            var expectedDataCollection = DataCollection.Load(table);
            var fields = expectedDataCollection.Rows[0].Values.Select(a => a.Name);
            var actualDataCollection = _specflowDbSchema.BuildDataCollection(query, fields, Enumerable.Empty<FieldFilter>(), _formatter);

            if (!DataCompare.Compare(expectedDataCollection, actualDataCollection, out string message))
            {
                Assert.Fail($"Query '{query}'.{Environment.NewLine}{message}");
            }
        }

        public void AssertTableIsEmpty(string tableName, IEnumerable<FieldFilter> filters)
        {
            _specflowDb.AssertTableName(tableName);
            _specflowDb.AssertTableSchema(tableName, filters);
            var recordCount = _specflowDbSchema.GetRecordCount(tableName, filters);
            Assert.AreEqual(0, recordCount, $"Table '{tableName}' is not empty");
        }
    }

    public static class SpecflowDbValidatorHelper
    {
        public static IEnumerable<FieldFilter> AddQuotationMarks(IEnumerable<FieldFilter> filters)
        {
            FieldFilter AddQuotationMarks(FieldFilter filter)
            {
                var valuesWithQuotationMarks = filter.FieldValues.Split(',').Select(a => $"'{a.Trim()}'");
                return new FieldFilter
                {
                    FieldName = filter.FieldName,
                    FieldValues = string.Join(",", valuesWithQuotationMarks)
                };
            }

            return filters.Select(AddQuotationMarks);
        }
    }
}
