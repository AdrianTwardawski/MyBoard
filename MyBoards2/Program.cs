using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBoards2.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ignoring looped references
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<MyBoardsContext>(
    option => option
    .UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();
var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users.ToList();
if (!users.Any())
{
    var user1 = new User()
    {
        Email = "user1@test.com",
        FullName = "User One",
        Address = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka",
        }
    };

    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User Two",
        Address = new Address()
        {
            City = "Kraków",
            Street = "Długa",
        }
    };

    dbContext.Users.AddRange(user1, user2);

    dbContext.SaveChanges();
}

app.MapGet("tags", (MyBoardsContext db) =>
{
    var tags = db.Tags.ToList();
    return tags;
});

app.MapGet("epicUser", (MyBoardsContext db) =>
{
    var epic = db.Epics.First();
    var user = db.Users.First(u => u.FullName == "User One");
    return new {epic, user};
});

app.MapGet("toDoWorkItems", (MyBoardsContext db) =>
{
    var toDoWorkItems = db.WorkItems.Where(w => w.StateId == 1).ToList();
    return new { toDoWorkItems };
});

app.MapGet("newComments", async (MyBoardsContext db) =>
{
    var newComments = await db
        .Comments
        .Where(w => w.CreatedDate > new DateTime(2022, 7, 23))
        .ToListAsync();
    return newComments;
});

app.MapGet("top5NewestComments", async (MyBoardsContext db) =>
{
    var top5NewestComments = await db
        .Comments
        .OrderByDescending(w => w.CreatedDate)
        .Take(5)
        .ToListAsync();
    return top5NewestComments;
});

app.MapGet("statesCount", async (MyBoardsContext db) =>
{
    var statesCount = await db
        .WorkItems
        .GroupBy(w => w.StateId)
        .Select(g => new { stateId = g.Key, count = g.Count() })
        .ToListAsync();
    return statesCount;
});

app.MapGet("epics", async (MyBoardsContext db) =>
{
    var selectedEpics = await db
        .Epics
        .Where(w => w.StateId == 4)
        .OrderBy(w => w.Priority)
        .ToListAsync();
    return selectedEpics;
});

app.MapGet("topAuthorByComments", async (MyBoardsContext db) =>
{
    var authorsCommentCounts = await db
        .Comments
        .GroupBy(c => c.AuthorId)
        .Select(g => new { g.Key, Count = g.Count() })
        .ToListAsync();

    var topAuthor = authorsCommentCounts.First(a => a.Count == authorsCommentCounts.Max(acc => acc.Count));

    var userDetails = db.Users.First(u => u.Id == topAuthor.Key);

    return new { userDetails, commentCount = topAuthor.Count};
});

app.MapPost("updateEpic", async (MyBoardsContext db) =>
{
    Epic epic = await db.Epics.FirstAsync(epic => epic.Id == 1);

    epic.Area = "Updated area";
    epic.Priority = 1;
    epic.StartDate = DateTime.Now;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("updateEpic2", async (MyBoardsContext db) =>
{
    Epic epic = await db.Epics.FirstAsync(epic => epic.Id == 2);

    var onHoldState = await db.WorkItemStates.FirstAsync(a => a.Value == "On Hold");

    epic.StateId = onHoldState.Id;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("updateEpic3", async (MyBoardsContext db) =>
{
    Epic epic = await db.Epics.FirstAsync(epic => epic.Id == 3);

    var rejectedState = await db.WorkItemStates.FirstAsync(a => a.Value == "Rejected");

    epic.State = rejectedState;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("create", async(MyBoardsContext db) =>
{
    Tag tag = new Tag()
    {
        Value = "EF"
    };

    // await db.AddAsync(tag);
    await db.Tags.AddAsync(tag);
    await db.SaveChangesAsync();

    return tag;
});

app.MapPost("createMultiple", async (MyBoardsContext db) =>
{
    Tag mvcTag = new Tag()
    {
        Value = "MVC"
    };

    Tag aspTag = new Tag()
    {
        Value = "ASP"
    };

    //await db.Tags.AddRangeAsync(mvcTag, aspTag);

    var tags = new List<Tag>() { mvcTag, aspTag };
    await db.Tags.AddRangeAsync(tags);

    await db.SaveChangesAsync();

    return tags;
});

app.MapPost("createUser", async (MyBoardsContext db) =>
{
    Address address = new Address()
    {
        Id = Guid.Parse("b323dd7c-776a-4cf6-a92a-12df154b4a2c"),
        City = "Kraków",
        Country = "Poland",
        Street = "Długa"
    };

    var user = new User
    {
        Email = "user@test.com",
        FullName = "Test User",
        Address = address,
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return user;
});

app.MapGet("relatedData", async (MyBoardsContext db) =>
{
    var user = await db.Users
        .FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));
    var userComments = await db.Comments.Where(c => c.AuthorId == user.Id).ToListAsync();

    return user;
});

app.MapGet("relatedData2", async (MyBoardsContext db) =>
{
    var user = await db.Users
        .Include(u => u.Comments)
        .ThenInclude(c => c.WorkItem)
        .Include(u => u.Address)
        .FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    return user;
});

app.MapDelete("deleteWorkItem", async(MyBoardsContext db) =>
{

    var workItemTags = await db.WorkItemTag.Where(c => c.WorkItemId == 12).ToListAsync();
    db.WorkItemTag.RemoveRange(workItemTags);

    var workItem = await db.WorkItems.FirstAsync(c => c.Id == 16);

    db.RemoveRange(workItem);

    await db.SaveChangesAsync();    
});

app.MapDelete("deleteUser", async (MyBoardsContext db) =>
{

    var user = await db.Users
        .FirstAsync(u => u.Id == Guid.Parse("DC231ACF-AD3C-445D-CC08-08DA10AB0E61"));

    var userComments = db.Comments.Where(c => c.AuthorId == user.Id).ToList();
    db.RemoveRange(userComments);
    await db.SaveChangesAsync();

    db.Remove(user);
    await db.SaveChangesAsync();
});

app.MapDelete("deleteCascade", async (MyBoardsContext db) =>
{

    var user = await db.Users
        .Include(u => u.Comments)
        .FirstAsync(u => u.Id == Guid.Parse("7B84DC1C-597F-470F-CBEC-08DA10AB0E61"));

    db.Users.Remove(user);
    await db.SaveChangesAsync();
});

app.MapGet("changeTracker", async (MyBoardsContext db) =>
{
    var user = await db.Users
        .FirstAsync(u => u.Id == Guid.Parse("D00D8059-8977-4E5F-CBD2-08DA10AB0E61"));

    var entries1 = db.ChangeTracker.Entries();

    user.Email = "test@test.com";

    var entries2 = db.ChangeTracker.Entries();

    db.SaveChanges();

    return user;
});

app.MapGet("changeTracker2", async (MyBoardsContext db) =>
{
    var user = await db.Users
        .FirstAsync(u => u.Id == Guid.Parse("D00D8059-8977-4E5F-CBD2-08DA10AB0E61"));

    db.Users.Remove(user);

    var newUser = new User()
    {
        FullName = "New User"
    };

    db.Users.Add(newUser);

    var entries2 = db.ChangeTracker.Entries();

    db.SaveChanges();

    return user;
});

app.MapGet("deleteWithChangeTracker", async (MyBoardsContext db) =>
{
    // optimalized delete entry (select item is not needed)
    var workItem = new Epic()
    {
        Id = 2
    };

    var entry = db.Attach(workItem);
    entry.State = EntityState.Deleted;

    db.SaveChanges();

    return workItem;
});

app.MapGet("getWithoutTracking", async (MyBoardsContext db) =>
{
    // results won't be tracking by dbContext - this action will use less memory and will have better performance
    // its good to use it if we are sure data we want to get won't be modified
    var states = db.WorkItemStates
        .AsNoTracking()
        .ToList();

    var entries1 = db.ChangeTracker.Entries();
    return states;
});

app.MapGet("selectFromRawSQL", async (MyBoardsContext db) =>
{
    var states = db.WorkItemStates
        .FromSqlRaw(@"
            SELECT wis.Id, wis.Value
            FROM WorkItemStates wis
            JOIN WorkItems wi on wi.StateId = wis.Id
            GROUP BY wis.Id, wis.Value
            HAVING COUNT(*) > 85"
        )
        .ToList();
    return states;
});

app.MapGet("selectFromRawSQLWithParamAndUpdate", async (MyBoardsContext db) =>
{
    var minWorkItemsCount = "85";

    var states = db.WorkItemStates
        .FromSqlInterpolated($@"
            SELECT wis.Id, wis.Value
            FROM WorkItemStates wis
            JOIN WorkItems wi on wi.StateId = wis.Id
            GROUP BY wis.Id, wis.Value
            HAVING COUNT(*) > {minWorkItemsCount}"
        )
        .ToList();

    db.Database.ExecuteSqlRaw(@"
            UPDATE Comments
            SET UpdatedDate = GETDATE()
            WHERE AuthorId = '4EBB526D-2196-41E1-CBDA-08DA10AB0E61'"
    );
    return states;
});

app.MapGet("viewTopAuthors", async (MyBoardsContext db) =>
{
    var topAuthors = db.ViewTopAuthors.ToList();
    return topAuthors;
});

app.MapGet("ownedTypesAddresses", async (MyBoardsContext db) =>
{
    var addresses = db.Addresses.Where(a => a.Coordinate.Latitude > 10);
    return addresses;
});

app.MapGet("lazyLoading", async (MyBoardsContext db) =>
{
    var withAddress = true;

    var user = db.Users
          .First(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    if (withAddress)
    {
        var result = new { FullName = user.FullName, Address = $"{user.Address.Street}{user.Address.City}" };
        return result;
    }

    return new { FullName = user.FullName, Address = "-" };
});

app.Run();