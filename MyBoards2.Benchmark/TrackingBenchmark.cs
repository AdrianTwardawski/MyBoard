using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using MyBoards2.Entities;

namespace MyBoards2.Benchmark
{
    [MemoryDiagnoser]
    public class TrackingBenchmark
    {
        [Benchmark]
        public int WithTracking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyBoards2Db;Trusted_Connection=True;");
            var _dbContext = new MyBoardsContext(optionsBuilder.Options);

            var comments = _dbContext.Comments.ToList();

            return comments.Count;
        }

        [Benchmark]
        public int WithoutTracking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyBoards2Db;Trusted_Connection=True;");
            var _dbContext = new MyBoardsContext(optionsBuilder.Options);

            var comments = _dbContext.Comments
                .AsNoTracking()
                .ToList();
            return comments.Count;
        }
    }
}
