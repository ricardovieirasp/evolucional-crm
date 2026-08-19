using System;

namespace Evolucional.Matriculas.Api.Exceptions
{
    public class MatriculaDuplicadaException : Exception
    {
        public MatriculaDuplicadaException()
            : base("O aluno já está matriculado nesta turma.")
        {
        }
    }
}
