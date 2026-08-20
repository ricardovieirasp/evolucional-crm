using Evolucional.Matriculas.Api.DTOs.Relatorios;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Evolucional.Matriculas.Api.Services
{
    public class RelatorioService
    {
        private readonly IRelatorioRepository _relatorioRepository;

        public RelatorioService(IRelatorioRepository relatorioRepository)
        {
            _relatorioRepository = relatorioRepository;
        }

        public Task<IEnumerable<AlunosPorTurmaDto>> GetAlunosPorTurmaAsync()
        {
            return _relatorioRepository.GetAlunosPorTurmaAsync();
        }
    }
}