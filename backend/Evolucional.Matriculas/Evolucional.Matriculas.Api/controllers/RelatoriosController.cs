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
    [RoutePrefix("api/relatorios")]
    public class RelatoriosController : ApiController
    {
        private readonly RelatorioService _relatorioService;

        public RelatoriosController()
        {
            var connectionFactory = new SqlConnectionFactory();
            var relatorioRepository = new RelatorioRepository(connectionFactory);
            _relatorioService = new RelatorioService(relatorioRepository);
        }

        [HttpGet]
        [Route("alunos-por-turma")]
        public async Task<IHttpActionResult> GetAlunosPorTurma()
        {
            var resultado = await _relatorioService.GetAlunosPorTurmaAsync();

            return Ok(resultado);
        }
    }
}