using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mihcelle.Hwavmvid.Shared.Models;
using System.Text.Json;
using Mihcelle.Hwavmvid.Data;

using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Mihcelle.Hwavmvid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Authcookiecontroller : ControllerBase
    {

        public UserManager<Applicationuser> usermanager { get; set; }
        public SignInManager<Applicationuser> signinmanager { get; set; }

        public Authcookiecontroller(UserManager<Applicationuser> usermanager, SignInManager<Applicationuser> signinmanager)
        {
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<string?> Get()
        {

            try
            {
                string authcookie = this.HttpContext.Request.Cookies[Shared.Constants.Authentication.Authcookiename];
                if (!string.IsNullOrEmpty(authcookie))
                {
                    return authcookie;
                }
            } catch (Exception exceptiion)
            {
                Console.Write(exceptiion.Message);
            }

            return null;
        }
    }
}
