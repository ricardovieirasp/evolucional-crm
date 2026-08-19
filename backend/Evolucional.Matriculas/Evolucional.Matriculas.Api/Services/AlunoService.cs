using Evolucional.Matriculas.Api.DTOs.Alunos;
using Evolucional.Matriculas.Api.DTOs.Common;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Evolucional.Matriculas.Api.Services
{
    public class AlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<PagedResultDto<AlunoDto>> GetAllAsync(int page, int pageSite, string nome)
        {
            var alunos = await _alunoRepository.GetAllAsync(page, pageSite, nome);

            var total = await _alunoRepository.CountAsync(nome);

            return new PagedResultDto<AlunoDto>
            {
                Items = alunos.Select(aluno => new AlunoDto
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    Email = aluno.Email,
                    DataNascimento = aluno.DataNascimento,
                    Ativo = aluno.Ativo,
                    DataCadastro = aluno.DataCadastro
                }),
                Total = total,
                Page = page,
                PageSize = pageSite
            };
        }

        public async Task<AlunoDto> GetByIdAsync(int id)
        {
            var aluno = await _alunoRepository.GetByIdAsync(id);

            if (aluno == null)
            {
                return null;
            }

            return new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                DataNascimento = aluno.DataNascimento,
                Ativo = aluno.Ativo,
                DataCadastro = aluno.DataCadastro
            };
        }
    }
}