using DbSafe;
using Specflow.Steps.Db.Shared;

namespace Specflow.Steps.Db.Sql
{
    public class SqlSteps : DbSteps
    {
        public SqlSteps(string connectionString, FormatterManager formatter = null) 
            : base(
                  new SpecflowDbSqlPopulator(connectionString), 
                  new SpecflowDbSqlValidator(connectionString, formatter ?? new FormatterManager()))
        {
        }
    }
}
