using Specflow.Steps.Db.Shared;
using SqlDbSafe;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql
{
    public class SpecflowDbSqlPopulator : ISpecflowDbPopulator
    {
        private readonly string _connectionString;
        private readonly SpecflowDb _specflowDb;

        public SpecflowDbSqlPopulator(string connectionString)
        {
            _connectionString = connectionString;
            _specflowDb = new SpecflowDb();
        }

        public void PopulateTable(string tableName, Table table, bool setIdentityInsert)
        {
            _specflowDb.AssertTableName(tableName, _connectionString);
            _specflowDb.AssertTableSchema(tableName, table, _connectionString);

            var datasetElement = DataConverter.BuildDatasetElementFromSpecFlowTable(tableName, table, setIdentityInsert);
            var sqlDatabaseClient = new SqlDatabaseClient(false)
            {
                ConnectionString = _connectionString
            };

            sqlDatabaseClient.WriteTable(datasetElement);
        }
    }
}
