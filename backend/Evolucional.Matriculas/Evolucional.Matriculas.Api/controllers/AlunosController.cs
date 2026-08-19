using Evolucional.Matriculas.Api.Repositories;
using Evolucional.Matriculas.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Evolucional.Matriculas.Api.Controllers
{
    [RoutePrefix("api/alunos")]
    public class AlunosController : ApiController
    {
        private readonly AlunoService _alunoService;

        public AlunosController()
        {
            _alunoService = new AlunoService(
                new AlunoRepository(
                    new Infrastructure.SqlConnectionFactory()));
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll(int page = 1, int pageSize = 10, string nome = null)
        {
            if (page <= 0)
            {
                return BadRequest("A página deve ser maior que zero.");
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                return BadRequest("O tamanho da página deve ser entre 0 e 100.");
            }

            var result = await _alunoService.GetAllAsync(page, pageSize, nome);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("O ID deve ser maior que zero.");
            }

            var aluno = await _alunoService.GetByIdAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return Ok(aluno);
        }


    }
}