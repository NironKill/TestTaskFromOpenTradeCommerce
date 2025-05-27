using JiraManager.Application.DTOs;

namespace JiraManager.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Create(UserDTO dto, CancellationToken cancellationToken);
    }
}
