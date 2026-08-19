using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evolucional.Matriculas.Api.DTOs.Alunos
{
    public class CriarAlunosDto
    {
        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }
    }
}