using JiraManager.Application.DTOs;

namespace JiraManager.Application.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task Create(TicketDTO dto, CancellationToken cancellationToken);

        Task<string> GetAccountId(Guid userId);
    }
}
