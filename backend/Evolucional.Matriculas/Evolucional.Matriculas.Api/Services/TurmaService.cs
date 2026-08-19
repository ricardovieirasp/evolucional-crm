using Evolucional.Matriculas.Api.DTOs.Turmas;
using Evolucional.Matriculas.Api.Models;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Evolucional.Matriculas.Api.Services
{
    public class TurmaService
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaService(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;
        }

        public async Task<IEnumerable<TurmaDto>> GetAllAsync()
        {
            var turmas = await _turmaRepository.GetAllAsync();
            return turmas.Select(MapToDto);

        }

        private static TurmaDto MapToDto(Turma turma)
        {
            return new TurmaDto
            {
                Id = turma.Id,
                Nome = turma.Nome,
                VagasDisponiveis = turma.VagasDisponiveis
            };
        }
    }
}