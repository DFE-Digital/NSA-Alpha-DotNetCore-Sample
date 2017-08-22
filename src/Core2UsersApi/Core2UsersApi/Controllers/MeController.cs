using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core2UsersApi.Controllers
{
    [Route("{directory}/[controller]")]
    public class MeController : Controller
    {
        [HttpGet]
        [Route("", Name = "GetSelf")]
        [Authorize]
        public IActionResult GetSelf(string directory)
        {
            var claims = User.Claims.Select(x => new {x.Type, x.Value}).ToArray();
            return new ObjectResult(new
            {
                directory,
                claims
            });
        }
    }
}
