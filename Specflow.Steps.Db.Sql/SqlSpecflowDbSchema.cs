using DbSafe;
using Specflow.Steps.Db.Shared;
using Specflow.Steps.Object;
using SqlDbSafe;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Specflow.Steps.Db.Sql
{
    public class SqlSpecflowDbSchema : ISpecflowDbSchema<SqlConnection, SqlCommand>
    {
        private readonly string _connectionString;

        public SqlSpecflowDbSchema(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Object.Collections.DataCollection BuildDataCollection(string tableName, IEnumerable<string> fields, IEnumerable<FieldFilter> filters, FormatterManager formatter)
        {
            return SqlDatabaseHelper.BuildDataCollection(_connectionString, tableName, fields, filters, formatter);
        }

        public int GetRecordCount(string tableName, IEnumerable<FieldFilter> filters)
        {
            return SqlDatabaseHelper.GetRecordCount(_connectionString, tableName, filters);
        }

        public AdoDatabaseClient<SqlConnection, SqlCommand> GetClient()
        {
            return new SqlDatabaseClient(false)
            {
                ConnectionString = _connectionString
            };
        }

        public IEnumerable<string> GetColumnNames(string tableName)
        {
            return SqlDatabaseHelper.GetColumnNames(_connectionString, tableName);
        }

        public bool IsObjectValid(string objectName)
        {
            return SqlDatabaseHelper.IsObjectValid(_connectionString, objectName);
        }
    }
}
