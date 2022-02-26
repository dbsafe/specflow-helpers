using DbSafe;
using Specflow.Steps.Db.Shared;
using System.Data.SqlClient;

namespace Specflow.Steps.Db.Sql
{
    public class SqlSteps : DbSteps
    {
        public SqlSteps(string connectionString, FormatterManager formatter = null) 
            : base(
                  BuildPopulator(connectionString),
                  BuildValidator(connectionString, formatter))
        {
        }

        private static SpecflowDbPopulator<SqlConnection, SqlCommand> BuildPopulator(string connectionString)
        {
            return new SpecflowDbPopulator<SqlConnection, SqlCommand>(connectionString, new SqlSpecflowDbSchema(connectionString));
        }

        private static SpecflowDbValidator<SqlConnection, SqlCommand> BuildValidator(string connectionString, FormatterManager formatter)
        {
            return new SpecflowDbValidator<SqlConnection, SqlCommand>(new SqlSpecflowDbSchema(connectionString), formatter);
        }
    }
}
