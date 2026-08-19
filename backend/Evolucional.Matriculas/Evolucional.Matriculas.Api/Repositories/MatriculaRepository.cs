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
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public MatriculaRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> ExistsAsync(int alunoId, int turmaId)
        {
            const string sql = @"
                  SELECT 
                     COUNT(1) 
                  FROM 
                     Matricula 
                  WHERE 
                     AlunoId = @AlunoId 
                     AND TurmaId = @TurmaId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(
                    sql, 
                    new { 
                        AlunoId = alunoId, 
                        TurmaId = turmaId 
                    });

                return count > 0;
            }
        }


        public async Task<Matricula> CreateAsync(int alunoId, int turmaId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction()) {
                    try
                    {
                        const string updateTurmaSql = @"
                             UPDATE Turma
                             SET VagasDisponiveis = VagasDisponiveis - 1
                             WHERE Id = @TurmaId AND VagasDisponiveis > 0;";

                        var rowsAffected = await connection.ExecuteAsync(
                            updateTurmaSql,
                            new
                            {
                                TurmaId = turmaId,
                            },
                            transaction);

                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();
                            return null;
                        }

                        const string insertMatriculaSql = @"
                             INSERT INTO Matricula 
                                    (AlunoId, TurmaId, DataMatricula)
                             VALUES 
                                    (@AlunoId, @TurmaId, @DataMatricula);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        var matricula = new Matricula
                        {
                            AlunoId = alunoId,
                            TurmaId = turmaId,
                            DataMatricula = DateTime.UtcNow
                        };

                        matricula.Id = await connection.ExecuteScalarAsync<int>(
                            insertMatriculaSql,
                            matricula,
                            transaction);

                        transaction.Commit();

                        return matricula;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}