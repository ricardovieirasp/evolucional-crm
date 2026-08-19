using Evolucional.Matriculas.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolucional.Matriculas.Api.Repositories.Interfaces
{
    public interface ITurmaRepository
    {
        Task<IEnumerable<Turma>> GetAllAsync();
    }
}
