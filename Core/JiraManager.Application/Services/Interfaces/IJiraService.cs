using System.Net.Http.Headers;

namespace JiraManager.Application.Services.Interfaces
{
    public interface IJiraService
    {
        AuthenticationHeaderValue GetAuthenticationToken();
        string GetUrl();
    }
}
