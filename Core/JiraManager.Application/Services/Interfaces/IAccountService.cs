using JiraManager.Application.DTOs;

namespace JiraManager.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Registration(LoginDTO dto, CancellationToken cancellationToken);
        Task Login(LoginDTO dto);
        Task Logout();
    }
}
