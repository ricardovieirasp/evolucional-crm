using Evolucional.Matriculas.Api.DTOs.Matriculas;
using Evolucional.Matriculas.Api.Exceptions;
using Evolucional.Matriculas.Api.Infrastructure;
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
    [RoutePrefix("api/matriculas")]
    public class MatriculasController : ApiController
    {
        private readonly MatriculaService _matriculaService;

        public MatriculasController()
        {
            var connectionFactory = new SqlConnectionFactory();
            var matriculaRepository = new MatriculaRepository(connectionFactory);
            var alunoRepository = new AlunoRepository(connectionFactory);
            var turmaRepository = new TurmaRepository(connectionFactory);

            _matriculaService = new MatriculaService(
                matriculaRepository,
                alunoRepository,
                turmaRepository
            );
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(
            [FromBody] CriarMatriculaDto dto)
        {
            if (dto == null)
                return BadRequest("O corpo da requisição não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var matricula = await _matriculaService.CreateAsync(dto);

                if (matricula == null)
                    return NotFound();

                return Created("", matricula);

            }
            catch (AlunoInativoException ex)
            {
                return Content(System.Net.HttpStatusCode.Conflict, ex.Message);
            }
            catch (TurmaSemVagaException ex)
            {
                return Content(System.Net.HttpStatusCode.Conflict, ex.Message);

            }
            catch (MatriculaDuplicadaException ex)
            {
                return Content(System.Net.HttpStatusCode.Conflict, ex.Message);
            }
        }
    }
}