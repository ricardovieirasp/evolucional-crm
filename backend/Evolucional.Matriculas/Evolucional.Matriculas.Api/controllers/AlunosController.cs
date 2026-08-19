using Evolucional.Matriculas.Api.DTOs.Alunos;
using Evolucional.Matriculas.Api.Exceptions;
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
                return BadRequest("A página deve ser maior que zero.");

            if (pageSize <= 0 || pageSize > 100)
                return BadRequest("O tamanho da página deve ser entre 0 e 100.");

            var result = await _alunoService.GetAllAsync(page, pageSize, nome);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID deve ser maior que zero.");

            var aluno = await _alunoService.GetByIdAsync(id);

            if (aluno == null)
                return NotFound();

            return Ok(aluno);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] CriarAlunosDto alunoDto)
        {
            try
            {
                if (alunoDto == null)
                    return BadRequest("O corpo da requisição não pode ser nulo.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdAluno = await _alunoService.CreateAsync(alunoDto);

                return CreatedAtRoute("GetById", new { id = createdAluno.Id }, createdAluno);
            }
            catch (InvalidBirthDateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] AtualizarAlunoDto dto)
        {
            if (id <= 0)
                return BadRequest("O ID deve ser maior que zero.");

            if (dto == null)
                return BadRequest("O corpo da requisição não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _alunoService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("O ID deve ser maior que zero.");

            var deleted = await _alunoService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}