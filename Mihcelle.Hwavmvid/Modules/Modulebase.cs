using Microsoft.AspNetCore.Components;
using Mihcelle.Hwavmvid.Modal;
using Mihcelle.Hwavmvid.Shared.Models;
using Mihcelle.Hwavmvid.Shared.Constants;
using Mihcelle.Hwavmvid.Data;

using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using static Microsoft.AspNetCore.Components.Web.RenderMode;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Mihcelle.Hwavmvid;
using Mihcelle.Hwavmvid.Components;

namespace Mihcelle.Hwavmvid.Modules
{
    public class Modulebase : ComponentBase
    {


        [Inject] public IServiceProvider iserviceprovider { get; set; }
        [Inject] public IHttpClientFactory ihttpclientfactory { get; set; }
        [Inject] public NavigationManager navigationmanager { get; set; }
        [Inject] public Applicationprovider applicationprovider { get; set; }
        [Inject] public Modalservice modalservice { get; set; }
        [Inject] public Applicationdbcontext applicationdbcontextmihcellehwavmvid { get; set; }

        [Parameter] public string Moduleid { get; set; }
        [Parameter] public Type Componenttype { get; set; }
        [Parameter] public string Containertype { get; set; }
        [Parameter] public Type Componentsettingstype { get; set; }

        public HttpClient httpclient { get; set; }
        protected Moduleservice<Modulepreferences> moduleservice { get; set; }
        protected Dictionary<string, object> servpara { get; set; }

        protected override Task OnInitializedAsync()
        {
            
            this.httpclient = this.ihttpclientfactory?.CreateClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated");

            Modulepreferences modulepreferences = new Modulepreferences();
            this.moduleservice = new Moduleservice<Modulepreferences>();
            this.moduleservice.Preferences = new Modulepreferences();
            this.moduleservice.Preferences.ModuleId = this.Moduleid;

            this.servpara = new Dictionary<string, object>();
            servpara.Add("Moduleparams", this.moduleservice);

            return base.OnInitializedAsync();
        }

        public string adminmodalelementid { get; set; } = "admin_modal_element_id_for_settings";
        public async Task Openmodulesettings()
        {

            await this.modalservice.ShowModal(this.adminmodalelementid);
            await InvokeAsync(() =>
            {
                this.StateHasChanged();
            });
        }

        public async Task Deletemodule(string moduleid)
        {
            await this.httpclient.DeleteAsync(string.Concat("api/module/", moduleid));
            this.navigationmanager.NavigateTo(this.navigationmanager.Uri, true);
        }

    }
}
