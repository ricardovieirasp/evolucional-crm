using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evolucional.Matriculas.Api.DTOs.Common
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}