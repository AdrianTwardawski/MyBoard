using Bogus;
using MyBoards2.Entities;

namespace MyBoards2
{
    public class DataGenerator
    {
        public static void Seed(MyBoardsContext context)
        {
            var locale = "pl";

            Randomizer.Seed = new Random(911);   // generated data will be the same each seed

            var addressGenerator = new Faker<Address>(locale)
                //.StrictMode(true) // if its true, validation will be applied which checks property generating for each type property
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Street, f => f.Address.StreetName());

            //Address address = addressGenerator.Generate();

            var userGenerator = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Address, f => addressGenerator.Generate());
            //.RuleFor(u => u.Address, address);

            var users = userGenerator.Generate(100);

            context.AddRange(users);
            context.SaveChanges();

        }
    }
}
