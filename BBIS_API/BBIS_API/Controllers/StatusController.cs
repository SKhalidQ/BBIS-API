using BBIS_API.DbAccess;
using BBIS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var status = (await DbAccessClass.DatabaseCheck(_context)) ? HttpContext.Response.StatusCode = 200 : HttpContext.Response.StatusCode = 500;

            return (status == 200) ? "API is active." : "Internal Server Error";
        }

        [HttpGet]
        [ActionName("ClearDatabase")]
        public async Task<string> ResetDatabase()
        {
            var status = (await DbAccessClass.ClearDatabase(_context)) ? HttpContext.Response.StatusCode = 200 : HttpContext.Response.StatusCode = 400;

            return (status == 200) ? "Database Cleared" : "Bad Request";
        }
    }
}
