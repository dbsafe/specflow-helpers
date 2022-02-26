using DbSafe;
using PgDbSafe;
using Specflow.Steps.Db.Pg;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql.Tests
{
    [Binding]
    public class DatabaseTestSteps : PgSteps
    {
        // Database deployed to localhost
        private static string _connectionString = "Host=localhost;Port=5432;Database=dbsafe;Username=dbsafe;Password=dbsafe";

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
            var dbSafe = PgDbSafeManager.Initialize("specflow-helpers-demo.xml");
            dbSafe.PassConnectionString(_connectionString);
            dbSafe.ExecuteScripts("delete-data")
                .ExecuteScripts("reseed-tables");

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
