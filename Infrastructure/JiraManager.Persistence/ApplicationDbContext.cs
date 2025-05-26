using JiraManager.Application.Interfaces;
using JiraManager.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JiraManager.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public DbSet<Ticket> Tickets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
