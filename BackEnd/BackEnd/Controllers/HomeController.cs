using System.Net.Http;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Hello()
        {
            return Request.CreateResponse("Hello World!!!");
        }
    }
}
