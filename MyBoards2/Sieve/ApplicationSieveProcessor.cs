using Microsoft.Extensions.Options;
using MyBoards2.Entities;
using Sieve.Models;
using Sieve.Services;

namespace MyBoards2.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {

        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Epic>(e => e.Priority)
                .CanSort()
                .CanFilter();

            mapper.Property<Epic>(e => e.Area)
                .CanSort()
                .CanFilter();

            mapper.Property<Epic>(e => e.StartDate)
                .CanSort()
                .CanFilter();

            mapper.Property<Epic>(e => e.Author.FullName)
                .CanSort()
                .CanFilter()
                .HasName("authorFullName"); // client side when sorthing "Author.FullName" => "authorFullName"

            return mapper;
        }
    }
}
