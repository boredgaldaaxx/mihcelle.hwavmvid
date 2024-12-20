using Microsoft.EntityFrameworkCore;
using Mihcelle.Hwavmvid.Server;
using Mihcelle.Hwavmvid.Shared.Constants;
using Mihcelle.Hwavmvid.Shared.Models;

namespace Mihcelle.Hwavmvid.Framework.Modules.Mediafiles
{

    public class Applicationdbcontext : IModuleinsideframeworkinstallerinterface
    {

        public Mihcelle.Hwavmvid.Data.Applicationdbcontext _frameworkdb { get; set; }

        public Applicationdbcontext(Mihcelle.Hwavmvid.Data.Applicationdbcontext _frameworkdb)
        {
            this._frameworkdb = _frameworkdb;
        }

        public async Task Removemodule(string moduleid)
        {
            await this._frameworkdb.Applicationmodulesettings.Where(item => item.Moduleid == moduleid).ExecuteDeleteAsync();
            await this._frameworkdb.SaveChangesAsync();            
        }
        
        public Applicationmodulepackage applicationmodulepackage 
        {
            get
            {
                var package = new Applicationmodulepackage()
                {
                    Name = "Mediafiles",
                    Version = "1.0.0",
                    Icon = "A",
                    Assemblytype = "Mihcelle.Hwavmvid.Modules.Mediafiles.Mediafiles, Mihcelle.Hwavmvid",
                    Settingstype = "Mihcelle.Hwavmvid.Modules.Mediafiles.Settings, Mihcelle.Hwavmvid",
                    Description = Applicationmoduletype.Applicationmoduleframework,
                    Createdon = DateTime.Now,
                };

                return package;
            }
        }

    }
}
