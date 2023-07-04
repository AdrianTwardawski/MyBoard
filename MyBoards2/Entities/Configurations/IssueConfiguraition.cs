using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards2.Entities.Configurations
{
    public class IssueConfiguraition : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.Property(x => x.Efford)
                .HasColumnType("decimal(5,2)");
        }
    }
}
