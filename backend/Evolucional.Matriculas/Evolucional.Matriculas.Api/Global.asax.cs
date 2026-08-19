using System.Web.Http;
using System.Web.Mvc;

namespace Evolucional.Matriculas.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
