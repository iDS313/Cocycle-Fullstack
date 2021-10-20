using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Cocycle.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string Name{ get; set; }
        public string Address { get; set; }
        public int AreaId { get; set; }
        public int StateId { get; set; }
        public int? PostCodeId { get; set; }
        public Boolean IsAvailable { get; set; }
        public string TravellingDistance { get; set; }
        public DateTime Created { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }

        [ForeignKey("PostCodeId")]
        public  PostCode PostCode { get; set; }
        public string ImageAddress { get; set; }
        public string Description { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("CustomName", Name));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure Null Column
            modelBuilder.Entity<Routes>()
                    .Property(p => p.Created)
                    .IsOptional();
         
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<RouteGroup> RouteGroup { get;  set; }
        public DbSet<Arranged> Arranged { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<RouteSchedule> RouteSchedules { get; set; }
        public DbSet<EmailAccount> EmailAccount { get; set; }
        public DbSet<PostCode> postCodes { get; set; }



        // public System.Data.Entity.DbSet<Cocycle.Models.ApplicationUser> ApplicationUsers { get; set; }
        // public DbSet<FeedBack> FeedBack { get; set; }




    }
}