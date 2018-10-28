
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI.Data
{
	public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
                : base(options)
        {
        }

        public virtual DbSet<Plant> Plant { get; set; }
    }
}