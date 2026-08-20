using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evolucional.Matriculas.Api.DTOs.Relatorios
{
    public class AlunosPorTurmaDto
    {
        public string NomeTurma { get; set; }
        public string QuantidadeAlunos { get; set; }
        public string VagasRestantes { get; set; }
    }
}