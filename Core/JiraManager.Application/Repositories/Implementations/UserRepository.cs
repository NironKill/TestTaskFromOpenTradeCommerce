using JiraManager.Application.Common.Enums;
using JiraManager.Application.DTOs;
using JiraManager.Application.Repositories.Interfaces;
using JiraManager.Application.Services.Interfaces;
using JiraManager.Domain;
using Microsoft.AspNetCore.Identity;

namespace JiraManager.Application.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IAccessTokenService _accessToken;

        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IAccessTokenService accessToken)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _accessToken = accessToken;
        }

        public async Task Create(UserDTO dto, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                UserName = dto.Name,
                LockoutEnabled = false
            };
            await _userManager.CreateAsync(user);

            if (!await _roleManager.RoleExistsAsync(Role.User.ToString()))
                await _roleManager.CreateAsync(new IdentityRole<Guid>(Role.User.ToString()));

            await _accessToken.Create(user.Id, cancellationToken);

            await _userManager.AddToRoleAsync(user, Role.User.ToString());
        }
    }
}
