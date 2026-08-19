using Evolucional.Matriculas.Api.DTOs.Alunos;
using Evolucional.Matriculas.Api.DTOs.Common;
using Evolucional.Matriculas.Api.Exceptions;
using Evolucional.Matriculas.Api.Models;
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

        public async Task<AlunoDto> CreateAsync(CriarAlunosDto dto) {

            ValidateBirthDate(dto.DataNascimento);

            var aluno = new Aluno
            {
                Nome = dto.Nome,
                Email = dto.Email,
                DataNascimento = dto.DataNascimento,
                Ativo = true,
                DataCadastro = DateTime.UtcNow
            };

            aluno.Id = await _alunoRepository.CreateAsync(aluno);

            return MapToDto(aluno);
        }

        private void ValidateBirthDate(DateTime dataNascimento) { 
            if (dataNascimento > DateTime.UtcNow)
                throw new InvalidBirthDateException();
        }

        public async Task<bool> UpdateAsync(int id, AtualizarAlunoDto dto) { 
            ValidateBirthDate(dto.DataNascimento);

            var aluno = await _alunoRepository.GetByIdAsync(id);
            if (aluno == null)
                return false;

            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;
            aluno.DataNascimento = dto.DataNascimento;

            return await _alunoRepository.UpdateAsync(aluno);
        }

        public async Task<bool> DeleteAsync(int id) {

            var aluno = await _alunoRepository.GetByIdAsync(id);

            if (aluno == null)
                return false;

            return await _alunoRepository.SoftDeleteAsync(id); ;
        }

        private static AlunoDto MapToDto(Aluno aluno)
        {
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