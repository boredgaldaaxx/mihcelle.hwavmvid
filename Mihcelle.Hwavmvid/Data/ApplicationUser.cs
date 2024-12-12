using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mihcelle.Hwavmvid.Data
{
    public class ApplicationUser : IdentityUser
    {

        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [StringLength(410)]
        public override string Id { get; set; }

    }
}
