using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards2.Entities
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(x => x.Activity)
                .HasMaxLength(200);

            builder.Property(x => x.RemainingWork)
                .HasPrecision(14, 2);
        }
    }
}
