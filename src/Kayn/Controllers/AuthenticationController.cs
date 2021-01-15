using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Kayn.Controllers
{
    public sealed class AuthenticationController : KaynController
    {
        [HttpGet("~/signin")]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties {RedirectUri = "/"}, "Discord");
        }

        [HttpGet("~/signout"), HttpPost("~/signout")]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties {RedirectUri = "/"}, "Cookies");
        }
    }
}