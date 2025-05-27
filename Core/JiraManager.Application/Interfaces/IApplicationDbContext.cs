using JiraManager.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JiraManager.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Ticket> Tickets { get; set; }

        DbSet<User> Users { get; set; }
        DbSet<IdentityUserToken<Guid>> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}