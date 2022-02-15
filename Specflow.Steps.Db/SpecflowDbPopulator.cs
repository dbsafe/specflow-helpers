using SqlDbSafe;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db
{
    public class SpecflowDbPopulator
    {
        private readonly string _connectionString;

        public SpecflowDbPopulator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetTable(string tableName, Table table, bool setIdentityInsert)
        {
            SpecflowDb.AssertTableName(tableName, _connectionString);
            SpecflowDb.AssertTableSchema(tableName, table, _connectionString);
            PopulateTable(tableName, table, setIdentityInsert);
        }

        private void PopulateTable(string tableName, Table table, bool setIdentityInsert)
        {
            var datasetElement = DataConverter.BuildDatasetElementFromSpecFlowTable(tableName, table, setIdentityInsert);
            var sqlDatabaseClient = new SqlDatabaseClient(false)
            {
                ConnectionString = _connectionString
            };

            sqlDatabaseClient.WriteTable(datasetElement);
        }
    }
}
