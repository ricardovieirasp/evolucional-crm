using Dapper;
using Evolucional.Matriculas.Api.DTOs.Relatorios;
using Evolucional.Matriculas.Api.Infrastructure;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Evolucional.Matriculas.Api.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public RelatorioRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            _connectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<AlunosPorTurmaDto>> GetAlunosPorTurmaAsync()
        {
            const string sql = @"
                SELECT
                    t.Nome AS NomeTurma,
                    COUNT(m.Id) AS QuantidadeAlunos,
                    t.VagasDisponiveis AS VagasRestantes
                FROM Turma t
                LEFT JOIN Matricula m
                     ON m.TurmaId = t.Id
                GROUP BY
                     t.Id,
                     t.Nome,
                     t.VagasDisponiveis
                ORDER BY
                     t.Nome;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection
                    .QueryAsync<AlunosPorTurmaDto>(sql);
            }
        }
    }
}