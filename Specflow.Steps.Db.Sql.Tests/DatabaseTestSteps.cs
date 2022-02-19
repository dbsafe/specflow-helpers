using DbSafe;
using SqlDbSafe;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql.Tests
{
    [Binding]
    public class DatabaseTestSteps : SqlSteps
    {
        // Database deployed to localhost
        private static string _connectionString = @"data source=localhost;initial catalog=ProductDatabase;User ID=dbsafe;Password=dbsafe;MultipleActiveResultSets=True;App=Specflow.Steps.Db.Sql.Tests";

        private static FormatterManager _formatter;

        static DatabaseTestSteps()
        {
            _formatter = new FormatterManager();
            // Adding formatters if needed.
            // _formatter.Register(typeof(decimal), new DecimalFormatter("0.00"));
        }

        private IDbSafeManager? _dbSafe;

        public DatabaseTestSteps() : base(_connectionString, _formatter)
        {
        }

        [BeforeScenario]
        public void Initialize()
        {
            var dbSafe = SqlDbSafeManager.Initialize("specflow-helpers-demo.xml");
            dbSafe.PassConnectionString(_connectionString);
            dbSafe.ExecuteScripts("delete-data");

            _dbSafe = dbSafe;
        }

        [AfterScenario]
        public void Cleanup()
        {
            _dbSafe?.Completed();
        }

        [When(@"I execute my operation")]
        public void ExecuteOperation()
        {
        }
    }
}
