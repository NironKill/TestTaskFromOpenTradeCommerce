using Microsoft.EntityFrameworkCore;

namespace JiraManager.Persistence.Common
{
    public static class Preparation
    {
        public static async Task Initialize(ApplicationDbContext context) => await context.Database.MigrateAsync();
    }
}
