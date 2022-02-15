using DbSafe;
using SqlDbSafe;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql.Tests
{
    [Binding]
    public class DatabaseTestSteps : SqlSteps
    {
        private static string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Initial Catalog=specflow-helpers-demo";
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
            var connectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Initial Catalog=specflow-helpers-demo";

            var dbSafe = SqlDbSafeManager.Initialize("specflow-helpers-demo.xml");
            dbSafe.PassConnectionString(connectionString);
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
