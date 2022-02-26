using DbSafe;
using Npgsql;
using Specflow.Steps.Db.Shared;

namespace Specflow.Steps.Db.Pg
{
    public class PgSteps : DbSteps
    {
        public PgSteps(string connectionString, FormatterManager formatter = null)
            : base(
                  BuildPopulator(connectionString),
                  BuildValidator(connectionString, formatter))
        {
        }

        private static SpecflowDbPopulator<NpgsqlConnection, NpgsqlCommand> BuildPopulator(string connectionString)
        {
            return new SpecflowDbPopulator<NpgsqlConnection, NpgsqlCommand>(connectionString, new PgSpecflowDbSchema(connectionString));
        }

        private static SpecflowDbValidator<NpgsqlConnection, NpgsqlCommand> BuildValidator(string connectionString, FormatterManager formatter)
        {
            return new SpecflowDbValidator<NpgsqlConnection, NpgsqlCommand>(new PgSpecflowDbSchema(connectionString), formatter);
        }
    }
}
