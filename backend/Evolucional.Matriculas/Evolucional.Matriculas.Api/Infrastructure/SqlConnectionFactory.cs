using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Evolucional.Matriculas.Api.Infrastructure
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TesteEscola"].ConnectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}