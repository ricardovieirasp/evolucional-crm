using Evolucional.Matriculas.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolucional.Matriculas.Api.Repositories.Interfaces
{
    public interface IAlunoRepository
    {
        Task<IEnumerable<Aluno>> GetAllAsync(int page, int pageSize, string nome);
        Task<int> CountAsync(string nome);
        Task<Aluno> GetByIdAsync(int id);
        Task<int> CreateAsync(Aluno aluno);
        Task<bool> UpdateAsync(Aluno aluno);
        Task<bool> SoftDeleteAsync(int id);
    }
}
