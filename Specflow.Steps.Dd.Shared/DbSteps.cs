using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.Object;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specflow.Steps.Db.Shared
{
    public interface ISpecflowDbPopulator
    {
        void PopulateTable(string tableName, Table table, bool setIdentityInsert);
    }

    public interface ISpecflowDbValidator
    {
        void AssertTable(string tableName, Table table, IEnumerable<FieldFilter> filters);
        void AssertQuery(string query, Table table);
        void AssertTableIsEmpty(string tableName, IEnumerable<FieldFilter> filters);
    }

    public abstract class DbSteps
    {
        private readonly ISpecflowDbPopulator _populator;
        private readonly ISpecflowDbValidator _validator;
        private readonly Dictionary<string, IEnumerable<FieldFilter>> _tableFilters = new Dictionary<string, IEnumerable<FieldFilter>>(20);

        public DbSteps(ISpecflowDbPopulator populator, ISpecflowDbValidator validator)
        {
            _populator = populator;
            _validator = validator;
        }

        [Given(@"table '(.*)' contains the data")]
        public void SetTableWithoutIdentityColumns(string tableName, Table table)
        {
            _populator.PopulateTable(tableName, table, false);
        }

        [Given(@"I filter table '(.*)' by")]
        public void SetFilterForTable(string tableName, Table filter)
        {
            Assert.IsTrue(filter.Header.Contains(nameof(FieldFilter.FieldName)), $"Column '{nameof(FieldFilter.FieldName)}' is missing in the filter");
            Assert.IsTrue(filter.Header.Contains(nameof(FieldFilter.FieldValues)), $"Column '{nameof(FieldFilter.FieldValues)}' is missing in the filter");

            var filters = filter.CreateSet<FieldFilter>();
            _tableFilters[tableName] = filters;
        }

        [Given(@"table with identity columns '(.*)' contains the data")]
        public void SetTableWithIdentityColumns(string tableName, Table table)
        {
            _populator.PopulateTable(tableName, table, true);
        }

        [Then(@"table '(.*)' should contain the data")]
        public void AssertTable(string tableName, Table table)
        {
            var tableHasFilter = _tableFilters.TryGetValue(tableName, out var filter);
            _validator.AssertTable(tableName, table, tableHasFilter ? filter : null);
        }

        [Then(@"query '(.*)' should return the data")]
        public void AssertQuery(string query, Table table)
        {
            _validator.AssertQuery(query, table);
        }

        [Then(@"table '(.*)' should be empty")]
        public void AssertTableIsEmpty(string tableName)
        {
            var tableHasFilter = _tableFilters.TryGetValue(tableName, out var filter);
            _validator.AssertTableIsEmpty(tableName, tableHasFilter ? filter : null);
        }
    }
}
