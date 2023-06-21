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
            //modelBuilder.Entity<User>()
                //.HasKey(x => new { x.Email, x.LastName });
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.
        //        UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyBoards2Db;Trusted_Connection=True;");
        // }
    }
}
