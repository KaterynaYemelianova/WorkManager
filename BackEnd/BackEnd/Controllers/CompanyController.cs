using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class CompanyController : ControllerBase
    {
        [HttpPost]
        public async Task<HttpResponseMessage> SignUp([FromBody] AuthorizedDto<RoomDto> dto)
        {
            HttpResponseMessage m = await Execute(d => { }, dto);
            return m;
        }
    }
}
