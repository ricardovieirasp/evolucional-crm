using System;

namespace Evolucional.Matriculas.Api.Exceptions
{
    public class AlunoInativoException : Exception
    {
        public AlunoInativoException()
            : base("O aluno está inativo e não pode ser matriculado.")
        {
        }
    }
}