using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PersonalProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public CustomUser CustomUser { get; set; }
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
        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<JobAdvertisment> JobAdvertisments { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Category> Category { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SignedUserApplication>()
                .HasRequired(j => j.Person)
                .WithMany(u => u.Applications)
                .HasForeignKey(j => j.ForeignKeyPerson)
                .WillCascadeOnDelete(false);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<PersonalProject.Models.Company> Companies { get; set; }

       // public System.Data.Entity.DbSet<PersonalProject.Models.PersonEditViewModel> PersonEditViewModels { get; set; }
    }
}