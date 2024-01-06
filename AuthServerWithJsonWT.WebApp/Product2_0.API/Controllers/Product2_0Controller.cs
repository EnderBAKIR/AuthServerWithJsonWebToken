using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Product2_0.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class Product2_0Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProduct2_0()
        {
            var userName = HttpContext.User.Identity.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);//get data with token service claims

            return Ok($"Test İşlemleri  =>UserName: {userName}- UserId:{userIdClaim.Value}");
        }
    }
}
