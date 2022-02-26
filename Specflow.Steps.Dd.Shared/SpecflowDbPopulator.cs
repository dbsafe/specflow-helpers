using System.Data.Common;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db.Shared
{
    public class SpecflowDbPopulator<TDbConnection, TDbCommand> : ISpecflowDbPopulator where TDbConnection : DbConnection where TDbCommand : DbCommand
    {
        private readonly SpecflowDb<TDbConnection, TDbCommand> _specflowDb;
        private readonly string _connectionString;
        private readonly ISpecflowDbSchema<TDbConnection, TDbCommand> _specflowDbSchema;

        public SpecflowDbPopulator(string connectionString, ISpecflowDbSchema<TDbConnection, TDbCommand> specflowDbSchema)
        {
            _connectionString = connectionString;
            _specflowDbSchema = specflowDbSchema;
            _specflowDb = new SpecflowDb<TDbConnection, TDbCommand>(_specflowDbSchema);
        }

        public void PopulateTable(string tableName, Table table, bool setIdentityInsert)
        {
            _specflowDb.AssertTableName(tableName);
            _specflowDb.AssertTableSchema(tableName, table);

            var datasetElement = DataConverter.BuildDatasetElementFromSpecFlowTable(tableName, table, setIdentityInsert);
            var sqlDatabaseClient = _specflowDbSchema.GetClient();
            sqlDatabaseClient.WriteTable(datasetElement);
        }
    }
}
