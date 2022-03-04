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
    }

    public class FieldFilter
    {
        public string FieldName { get; set; }
        public string FieldValues { get; set; }
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
    }
}
