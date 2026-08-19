using Evolucional.Matriculas.Api.Infrastructure;
using Evolucional.Matriculas.Api.Models;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Evolucional.Matriculas.Api.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AlunoRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Aluno>> GetAllAsync(int page, int pageSize, string nome)
        {
            var offset = (page - 1) * pageSize;

            const string sql = @"
                SELECT Id, Nome, Email, DataNascimento, Ativo, DataCadastro
                FROM Aluno
                WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')
                ORDER BY Nome
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var alunos = await connection
                    .QueryAsync<Aluno>(
                    sql, new { 
                        Nome = nome, 
                        Offset = offset, 
                        PageSize = pageSize 
                    });

                return alunos;
            }
        }

        public async Task<int> CountAsync(string nome)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM Aluno
                WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%');";

            using ( var connection = _connectionFactory.CreateConnection())
            {
                int count = await connection.ExecuteScalarAsync<int>(sql, new { 
                    Nome = string.IsNullOrWhiteSpace(nome) ? null : nome 
                });
                return count;
            }
        }

        public async Task<Aluno> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, Nome, Email, DataNascimento, Ativo, DataCadastro
                FROM Aluno
                WHERE Id = @Id;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                Aluno aluno = await connection.QuerySingleOrDefaultAsync<Aluno>(sql, new { Id = id });
                return aluno;
            }
        }

        public async Task<int> CreateAsync(Aluno aluno)
        {
            const string sql = @"
                INSERT INTO Aluno (Nome, Email, DataNascimento, Ativo, DataCadastro)
                VALUES (@Nome, @Email, @DataNascimento, @Ativo, @DataCadastro);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var connection = _connectionFactory.CreateConnection())
            {
                int id = await connection.ExecuteScalarAsync<int>(sql, aluno);
                return id;
            }
        }

        public async Task<bool> UpdateAsync(Aluno aluno)
        {
            const string sql = @"
                UPDATE Aluno
                SET Nome = @Nome,
                    Email = @Email,
                    DataNascimento = @DataNascimento,
                    Ativo = @Ativo
                WHERE Id = @Id;";
            
            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, aluno);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Aluno
                SET Ativo = 0
                WHERE Id = @Id;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

    }
}