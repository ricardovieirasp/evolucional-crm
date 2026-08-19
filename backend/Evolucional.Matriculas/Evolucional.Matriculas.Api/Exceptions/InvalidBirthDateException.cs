using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evolucional.Matriculas.Api.Exceptions
{
    public class InvalidBirthDateException : Exception
    {
        public InvalidBirthDateException():base("A data de nascimento não pode ser uma data futura.")
        { }
    }
}