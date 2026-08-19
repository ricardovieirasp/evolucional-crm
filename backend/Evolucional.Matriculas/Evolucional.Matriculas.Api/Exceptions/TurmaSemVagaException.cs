using System;

namespace Evolucional.Matriculas.Api.Exceptions
{
    public class TurmaSemVagaException : Exception
    {
        public TurmaSemVagaException()
            : base("A turma não possui vagas disponíveis.")
        {
        }
    }
}