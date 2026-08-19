using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evolucional.Matriculas.Api.DTOs.Alunos
{
    public class AlunoDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}