using DbSafe;
using SqlDbSafe;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql.Tests
{
    [Binding]
    public class DatabaseTestSteps : SqlSteps
    {
        // Database deployed to localhost
        private static readonly string _connectionString = "data source=localhost;initial catalog=ProductDatabase;User ID=dbsafe;Password=dbsafe;MultipleActiveResultSets=True;App=Specflow.Steps.Db.Pg.Tests";

        private static readonly FormatterManager _formatter;

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
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Use by Specflow")]
        public void ExecuteOperation()
        {
            // execute logic being tested
        }
    }
}
