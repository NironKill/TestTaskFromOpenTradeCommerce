using JiraManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JiraManager.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.Tickets).WithOne(t => t.User).HasForeignKey(t => t.UserId);

            builder.HasIndex(u => u.UserName).IsUnique();
        }
    }
}
