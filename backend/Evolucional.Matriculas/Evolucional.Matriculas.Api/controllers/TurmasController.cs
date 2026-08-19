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
    [RoutePrefix("api/turmas")]
    public class TurmasController : ApiController
    {
        private readonly TurmaService _turmaService;

        public TurmasController() {
            var connectionFactory = new SqlConnectionFactory();
            var turmaREpository = new TurmaRepository(connectionFactory);
            _turmaService = new TurmaService(turmaREpository);
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var turmas = await _turmaService.GetAllAsync();
            
            return Ok(turmas);
        }
    }
}