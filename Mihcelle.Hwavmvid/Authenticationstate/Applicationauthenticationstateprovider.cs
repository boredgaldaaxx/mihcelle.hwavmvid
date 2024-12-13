using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Mihcelle.Hwavmvid
{
    public class Applicationauthenticationstateprovider : AuthenticationStateProvider
    {


        private IHttpClientFactory _httpclientfactory { get; set; }
        private NavigationManager _navigationmanager { get; set; }

        public Applicationauthenticationstateprovider(IHttpClientFactory httpclientfactory, NavigationManager navigationmanager)
        {
            this._httpclientfactory = httpclientfactory;
            this._navigationmanager = navigationmanager;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            try
            {

                var client = this._httpclientfactory.CreateClient();
                client.BaseAddress = new Uri(this._navigationmanager.BaseUri);
                var claimsdictitems = await client.GetFromJsonAsync<List<KeyValuePair<string, string>>>("api/Applicationauthenticationstate");
                var authclaims = new List<Claim>();

                if (claimsdictitems != null && claimsdictitems.Any())
                {
                    foreach (var dicitem in claimsdictitems)
                    {
                        authclaims.Add(new Claim(dicitem.Key, dicitem.Value));
                    }

                    var authenticationstate = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(authclaims, "mihcelle.hwavmvid")));
                    return authenticationstate;
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            var anonymousid = new ClaimsIdentity();
            return new AuthenticationState(new ClaimsPrincipal(anonymousid));
        }

    }
}
