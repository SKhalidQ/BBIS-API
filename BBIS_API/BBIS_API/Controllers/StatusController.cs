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
        public async Task<JsonResult> Options()
        {
            var status = (await DbAccessClass.DatabaseCheck(_context)) ? HttpContext.Response.StatusCode = 200 : HttpContext.Response.StatusCode = 500;

            return (status == 200) ? new JsonResult("API is active") : new JsonResult("Internal server error");
        }

        [HttpOptions]
        [ActionName("ClearDatabase")]
        public async Task<JsonResult> ResetDatabase()
        {
            var status = (await DbAccessClass.ClearDatabase(_context)) ? HttpContext.Response.StatusCode = 200 : HttpContext.Response.StatusCode = 500;

            return (status == 200) ? new JsonResult("API Reset") : new JsonResult("Internal server error");
        }
    }
}
