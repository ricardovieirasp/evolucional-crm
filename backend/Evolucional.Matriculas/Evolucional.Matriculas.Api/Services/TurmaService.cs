using Evolucional.Matriculas.Api.DTOs.Turmas;
using Evolucional.Matriculas.Api.Infrastructure.Cache;
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

        private const string TurmasCacheKey = "turmas";
        private readonly ICacheService _cacheService;

        public TurmaService(ITurmaRepository turmaRepository, ICacheService cacheService)
        {
            _turmaRepository = turmaRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<TurmaDto>> GetAllAsync()
        {
            var cached = _cacheService.Get<IEnumerable<TurmaDto>>(TurmasCacheKey);

            if (cached != null)
                return cached;

            var turmas = await _turmaRepository.GetAllAsync();
            
            var resultado = turmas.Select(MapToDto).ToList();

            _cacheService.Set(TurmasCacheKey, resultado, TimeSpan.FromMinutes(10));

            return resultado;
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