using Evolucional.Matriculas.Api.DTOs.Matriculas;
using Evolucional.Matriculas.Api.Exceptions;
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
    public class MatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;

        private const string TurmasCacheKey = "turmas";

        private readonly ICacheService _cacheService;

        public MatriculaService(IMatriculaRepository matriculaRepository, 
            IAlunoRepository alunoRepository, ITurmaRepository turmaRepository, ICacheService cacheService)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _cacheService = cacheService;
        }

        public async Task<Matricula> CreateAsync(CriarMatriculaDto dto)
        {
            var aluno = await _alunoRepository.GetByIdAsync(dto.AlunoId);
            if (aluno == null)
                return null;

            var turma = await _turmaRepository.GetByIdAsync(dto.TurmaId);
            if (turma == null) 
                return null;

            if (!aluno.Ativo)
                throw new AlunoInativoException();

            if (turma.VagasDisponiveis <= 0)
                throw new TurmaSemVagaException();

            var matriculaExiste = 
                await _matriculaRepository.ExistsAsync(
                    dto.AlunoId, dto.TurmaId);

            if (matriculaExiste)
                throw new MatriculaDuplicadaException();

            var matricula = 
                await _matriculaRepository.CreateAsync(dto.AlunoId,dto.TurmaId);

            if (matricula == null)
                throw new TurmaSemVagaException();

            _cacheService.Remove(TurmasCacheKey);

            return matricula;

        }

    }
}