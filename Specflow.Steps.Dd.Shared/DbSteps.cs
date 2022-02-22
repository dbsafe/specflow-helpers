using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Shared
{
    public interface ISpecflowDbPopulator
    {
        void PopulateTable(string tableName, Table table, bool setIdentityInsert);
    }

    public interface ISpecflowDbValidator
    {
        void AssertTable(string tableName, Table table);
    }

    public abstract class DbSteps
    {
        private readonly ISpecflowDbPopulator _populator;
        private readonly ISpecflowDbValidator _validator;

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

        [Given(@"table with identity columns '(.*)' contains the data")]
        public void SetTableWithIdentityColumns(string tableName, Table table)
        {
            _populator.PopulateTable(tableName, table, true);
        }

        [Then(@"table '(.*)' should contain the data")]
        public void AssertTable(string tableName, Table table)
        {
            _validator.AssertTable(tableName, table);
        }
    }
}
