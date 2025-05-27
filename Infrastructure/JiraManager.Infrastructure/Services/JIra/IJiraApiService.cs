using JiraManager.Application.DTOs;
using JiraManager.Infrastructure.Responses.JIra;

namespace JiraManager.Infrastructure.Services.JIra
{
    public interface IJiraApiService
    {
        Task<JiraUserResponse> CreateUser(UserDTO dto);
        Task<bool> CreateIssue(JiraUserResponse jiraUserResponse, TicketDTO ticketDTO, CancellationToken cancellationToken);

        Task<JiraUserResponse> GetUserByEmail(string email);
        Task<List<TicketDTO>> GetAllTicketByAccountId(string accountId);
    }
}
