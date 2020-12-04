using DbSafe;
using Specflow.Steps.Db;
using SqlDbSafe;
using TechTalk.SpecFlow;

namespace Demo.Database.Tests
{
    [Binding]
    public class DatabaseTestSteps
    {
        private SpecflowDbPopulator _populator;
        private SpecflowDbValidator _validator;

        private IDbSafeManager _dbSafe;

        [BeforeScenario]
        public void Initialize()
        {
            var dbSafe = SqlDbSafeManager.Initialize("specflow-helpers-demo.xml");
            dbSafe.PassConnectionString(@"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Initial Catalog=specflow-helpers-demo");
            dbSafe.ExecuteScripts("delete-data");

            _dbSafe = dbSafe;

            _populator = new SpecflowDbPopulator();
            _validator = new SpecflowDbValidator();
        }

        [AfterScenario]
        public void Cleanup()
        {
            _dbSafe?.Completed();
        }

        [Given(@"table '(.*)' contains the data")]
        public void SetTable(string tableName, Table table)
        {
            _populator.SetTable(tableName, table);
        }

        [When(@"I execute my operation")]
        public void ExecuteOperation()
        {
        }

        [Then(@"table '(.*)' should contain the data")]
        public void AssertTable(string tableName, Table table)
        {
            _validator.AssertTable(tableName, table);
        }
    }
}
