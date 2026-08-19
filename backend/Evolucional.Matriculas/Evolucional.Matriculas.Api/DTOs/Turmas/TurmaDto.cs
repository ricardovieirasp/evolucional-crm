using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evolucional.Matriculas.Api.DTOs.Turmas
{
    public class TurmaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int VagasDisponiveis { get; set; }

    }
}