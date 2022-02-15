using Specflow.Steps.Db.Shared;
using SqlDbSafe;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Sql
{
    public class SpecflowDbSqlPopulator : ISpecflowDbPopulator
    {
        private readonly string _connectionString;

        public SpecflowDbSqlPopulator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void PopulateTable(string tableName, Table table, bool setIdentityInsert)
        {
            SpecflowDb.AssertTableName(tableName, _connectionString);
            SpecflowDb.AssertTableSchema(tableName, table, _connectionString);

            var datasetElement = DataConverter.BuildDatasetElementFromSpecFlowTable(tableName, table, setIdentityInsert);
            var sqlDatabaseClient = new SqlDatabaseClient(false)
            {
                ConnectionString = _connectionString
            };

            sqlDatabaseClient.WriteTable(datasetElement);
        }
    }
}
