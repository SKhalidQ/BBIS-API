using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBIS_API.DbAccess;
using BBIS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BBIS_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatusController : ControllerBase
    {

        private readonly DatabaseContext _context;

        public StatusController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("CurrentStatus")]
        public async Task<string> Options()
        {
            var status = (await DbAccessClass.DatabaseCheck(_context)) ? HttpContext.Response.StatusCode = 200: HttpContext.Response.StatusCode = 500;

            return (status == 200) ? "API is active." : "Internal Server Error";
        }
    }
}
