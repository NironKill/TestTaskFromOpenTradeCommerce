using JiraManager.Application.DTOs;
using JiraManager.Application.Services.Interfaces;
using JiraManager.Domain;
using Microsoft.AspNetCore.Identity;

namespace JiraManager.Application.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IAccessTokenService _accessToken;

        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, IAccessTokenService accessToken)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _accessToken = accessToken;
        }

        public async Task<bool> Registration(LoginDTO dto, CancellationToken cancellationToken)
        {
            User user = new User
            {
                UserName = dto.UserName,
            };

            IdentityResult result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
                    if (!roleResult.Succeeded)
                        return false;
                }

                await _accessToken.Create(user.Id, cancellationToken);

                IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!addToRoleResult.Succeeded)
                    return false;

                await _signInManager.SignInAsync(user, false);
                return true;
            }
            return false;
        }
        public async Task Login(LoginDTO dto)
        {
            User user = await _userManager.FindByNameAsync(dto.UserName);
            await _signInManager.SignInAsync(user, false);
        }
        public async Task Logout() => await _signInManager.SignOutAsync();
    }
}
