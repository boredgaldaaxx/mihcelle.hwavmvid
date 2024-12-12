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
    public class Logincontroller : ControllerBase
    {

        public UserManager<ApplicationUser> usermanager { get; set; }
        public SignInManager<ApplicationUser> signinmanager { get; set; }

        public Logincontroller(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signinmanager)
        {
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
        }

        [AllowAnonymous]
        [HttpGet("{username}/{password}")]
        public async Task Get(string username, string password)
        {

            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var identityuser = await usermanager.FindByNameAsync(username);
                    if (identityuser != null)
                    {
                        var result = await signinmanager.PasswordSignInAsync(identityuser, password, true, false);
                        if (!result.Succeeded)
                        {
                            throw new HubException("user sign in failed..");
                        }
                    }
                }
            } catch (Exception exceptiion)
            {

            }
        }
    }
}
