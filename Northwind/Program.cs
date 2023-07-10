using Microsoft.EntityFrameworkCore;
using Northwind.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NorthwindContext>(
    option => option
    .UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnectionString"))
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("data", async (NorthwindContext db) =>
{
    var sampleData = await db.Products
        .Take(100)
        .ToListAsync();

    return sampleData;
});

app.MapGet("getOrderDetails", async (NorthwindContext db) =>
{
    Order order = await GetOrder(46, db);
    return new { OrderId = order.OrderId, Details = order.OrderDetails };
});

app.MapGet("getOrderWithShipper", async (NorthwindContext db) =>
{
    Order order = await GetOrder(46, db);
    return new { OrderId = order.OrderId, ShipVia = order.ShipVia, Shipper = order.ShipViaNavigation };
});

app.MapGet("getOrderWithCustomer", async (NorthwindContext db) =>
{
    Order order = await GetOrder(46, db);
    return new { OrderId = order.OrderId, Customer = order.Customer };
});

app.Run();

async Task<Order> GetOrder(int orderId, NorthwindContext db)
{
    Order order = await db.Orders
        .Include(o => o.OrderDetails)
        .Include(o => o.ShipViaNavigation)
        .Include(o => o.Customer)
        .FirstAsync(o => o.OrderId == orderId);

    return order; 
}

