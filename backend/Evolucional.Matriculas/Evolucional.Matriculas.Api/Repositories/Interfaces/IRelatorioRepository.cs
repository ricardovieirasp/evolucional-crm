using Evolucional.Matriculas.Api.DTOs.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolucional.Matriculas.Api.Repositories.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<IEnumerable<AlunosPorTurmaDto>> GetAlunosPorTurmaAsync();
    }
}
