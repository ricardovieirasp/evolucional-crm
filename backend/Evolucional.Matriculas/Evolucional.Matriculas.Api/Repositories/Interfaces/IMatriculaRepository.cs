using Evolucional.Matriculas.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolucional.Matriculas.Api.Repositories.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<bool> ExistsAsync (int alunoId, int turmaId);
        Task<Matricula> CreateAsync(int alunoId, int turmaId);

    }
}
