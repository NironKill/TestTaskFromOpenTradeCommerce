using Microsoft.AspNetCore.Identity;

namespace JiraManager.Domain
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Ticket> Tickets { get; set; }
    }
}
