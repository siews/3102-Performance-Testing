using Microsoft.AspNet.Identity.EntityFramework;
using SgProperty.Migrations;
using SgProperty.Models;
using System.Data.Entity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SgProperty.DAL
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class SgPropertyContext : IdentityDbContext<ApplicationUser>
    {
        static SgPropertyContext()
        {
            Database.SetInitializer(new MySqlInitializer());
        }

        public SgPropertyContext()
      : base("SGpropertyDB", throwIfV1Schema: false)
        {

        }

        public static SgPropertyContext Create()
        {
            return new SgPropertyContext();
        }

        //public DbSet<Modek1> Agentz { get; set; }
        //public DbSet<Model2> Estatea { get; set; }

        public DbSet<Agent> agent { get; set; }
        public DbSet<District> district { get; set; }
        public DbSet<Estates> estates { get; set; }
        public DbSet<Population> population { get; set; }
        public DbSet<Property> property { get; set; }
        public DbSet<Agent_Manages_Property> agentManagesProperty { get; set; }
    }

    

}