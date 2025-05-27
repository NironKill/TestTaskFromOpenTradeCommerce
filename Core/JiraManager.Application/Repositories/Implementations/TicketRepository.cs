using JiraManager.Application.DTOs;
using JiraManager.Application.Interfaces;
using JiraManager.Application.Repositories.Interfaces;
using JiraManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace JiraManager.Application.Repositories.Implementations
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _user;

        public TicketRepository(IApplicationDbContext context, IUserRepository user)
        {
            _context = context;
            _user = user;
        }

        public async Task Create(TicketDTO dto, CancellationToken cancellationToken)
        {
            Ticket ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                TicketUrl = dto.TicketUrl,
                Key = dto.Key,
                TicketJiraId = dto.TicketJiraId,
                AccountId = dto.AccountId
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<string> GetAccountId(Guid userId) => await _context.Tickets.Where(x => x.UserId == userId).Select(x => x.AccountId).FirstOrDefaultAsync();
    }
}
