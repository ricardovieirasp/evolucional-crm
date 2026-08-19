using Dapper;
using Evolucional.Matriculas.Api.Infrastructure;
using Evolucional.Matriculas.Api.Models;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Evolucional.Matriculas.Api.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public TurmaRepository(SqlConnectionFactory connectionFactory) { 
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Turma>> GetAllAsync()
        {
            const string sql = @"
                SELECT
                     Id, 
                     Nome, 
                     Periodo, 
                     VagasTotal,
                     VagasDisponiveis
                FROM Turma
                ORDER BY Nome;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var turmas = await connection.QueryAsync<Turma>(sql);
                return turmas;
            }
        }

        public async Task<Turma> GetByIdAsync(int id)
        {
            const string sql = @"
                                SELECT
                                    Id,
                                    Nome,
                                    Periodo,
                                    VagasTotal,
                                    VagasDisponiveis
                                FROM Turma
                                WHERE Id = @Id;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Turma>(
                    sql,
                    new { Id = id });
            }
        }

    }
}