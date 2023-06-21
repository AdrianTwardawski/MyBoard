using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MyBoards2.Entities
{
    public class MyBoardsContext : DbContext
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }
  
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags{ get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(x => x.State).IsRequired();
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.Efford).HasColumnType("decimal(5,2)");
                eb.Property(x => x.EndDate).HasPrecision(3);
                eb.Property(x => x.Activity).HasMaxLength(200);
                eb.Property(x => x.RemainingWork).HasPrecision(14,2);
                eb.Property(x => x.Priority).HasDefaultValue(1);
                eb.HasMany(w => w.Comments)
                    .WithOne(c => c.WorkItem)
                    .HasForeignKey(c => c.WorkItemId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(u => u.User)
                .HasForeignKey<Address>(a => a.UserId);
        }
    }
}
