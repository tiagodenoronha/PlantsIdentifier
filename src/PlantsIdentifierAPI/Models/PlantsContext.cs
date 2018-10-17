
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI.Models
{
    public class PlantsContext : DbContext
    {
        public PlantsContext(DbContextOptions<PlantsContext> options)
                : base(options)
        {
        }

        public virtual DbSet<Plant> Plant { get; set; }
    }
}