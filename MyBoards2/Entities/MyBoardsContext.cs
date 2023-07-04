using Microsoft.EntityFrameworkCore;
using MyBoards2.Entities.Configurations;
using MyBoards2.Entities.ViewModels;
using System.Data.Common;

namespace MyBoards2.Entities
{
    public class MyBoardsContext : DbContext
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }
  
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags{ get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }
        public DbSet<WorkItemTag> WorkItemTag { get; set; }
        public DbSet<TopAuthor> ViewTopAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //new AddressConfiguration().Configure(modelBuilder.Entity<Address>());
            //new EpicConfiguration().Configure(modelBuilder.Entity<Epic>());

            // to samo co wyżej
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);  
        }
    }
}
