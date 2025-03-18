using CRMApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.DbContexts
{
    public class CRMApiDbContext(DbContextOptions<CRMApiDbContext> options) : DbContext(options)
    {
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
 