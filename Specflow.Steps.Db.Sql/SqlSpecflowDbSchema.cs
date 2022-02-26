using DbSafe;
using Specflow.Steps.Db.Shared;
using System.Collections.Generic;

namespace Specflow.Steps.Db.Sql
{
    public class SqlSpecflowDbSchema : ISpecflowDbSchema
    {
        private readonly string _connectionString;

        public SqlSpecflowDbSchema(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Object.Collections.DataCollection BuildDataCollection(string tableName, IEnumerable<string> fields, FormatterManager formatter)
        {
            return SqlDatabaseHelper.BuildDataCollection(_connectionString, tableName, fields, formatter);
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
