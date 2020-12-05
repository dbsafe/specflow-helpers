﻿using DbSafe;
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
        private readonly FormatterManager _formatter = new FormatterManager();

        private IDbSafeManager _dbSafe;

        public DatabaseTestSteps()
        {
            // Adding formatters if needed.
            // _formatter.Register(typeof(decimal), new DecimalFormatter("0.00"));
        }

        [BeforeScenario]
        public void Initialize()
        {
            var connectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Initial Catalog=specflow-helpers-demo";

            var dbSafe = SqlDbSafeManager.Initialize("specflow-helpers-demo.xml");
            dbSafe.PassConnectionString(connectionString);
            dbSafe.ExecuteScripts("delete-data");

            _dbSafe = dbSafe;

            _populator = new SpecflowDbPopulator(connectionString);
            _validator = new SpecflowDbValidator(connectionString);
        }

        [AfterScenario]
        public void Cleanup()
        {
            _dbSafe?.Completed();
        }

        [Given(@"table '(.*)' contains the data")]
        public void SetTableWithoutIdentityColumns(string tableName, Table table)
        {
            _populator.SetTable(tableName, table, false);
        }

        [Given(@"table with identity columns '(.*)' contains the data")]
        public void SetTableWithIdentityColumns(string tableName, Table table)
        {
            _populator.SetTable(tableName, table, true);
        }

        [When(@"I execute my operation")]
        public void ExecuteOperation()
        {
        }

        [Then(@"table '(.*)' should contain the data")]
        public void AssertTable(string tableName, Table table)
        {
            _validator.AssertTable(tableName, table, _formatter);
        }
    }
}
