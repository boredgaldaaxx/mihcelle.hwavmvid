using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mihcelle.Hwavmvid.Data;
using Mihcelle.Hwavmvid.Shared.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Mihcelle.Hwavmvid.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Logoutcontroller : ControllerBase
    {

        public UserManager<ApplicationUser> usermanager { get; set; }
        public SignInManager<ApplicationUser> signinmanager { get; set; }

        public Logoutcontroller(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signinmanager)
        {
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
        }

        [Authorize]
        [HttpGet]
        public async Task Get()
        {
            await this.signinmanager.SignOutAsync();
        }
    }
}
