using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Capstone.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public DbSet<Sellers> Sellers { get; set; }
        public DbSet<Buyers> Buyers { get; set; }
        public DbSet<Items> Items { get; set; }

        public byte[] UserPhoto { get; set; }
        public byte[] ItemPhoto { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<File> Files { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        //Don't think I need these, can delete later
        public System.Data.Entity.DbSet<Capstone.Models.Sellers> Sellers { get; set; }

        public System.Data.Entity.DbSet<Capstone.Models.Buyers> Buyers { get; set; }

        public System.Data.Entity.DbSet<Capstone.Models.Items> Items { get; set; }
    }
}