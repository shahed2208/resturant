using RestaurantManagementAPI.Services;
using RestaurantManagementAPI.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure strongly typed settings object
builder.Services.Configure<RestaurantDatabaseSettings>(
    builder.Configuration.GetSection("RestaurantDatabase"));

// Register MongoDB client and database
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetSection("RestaurantDatabase:ConnectionString").Value));

builder.Services.AddScoped<IMongoDatabase>(s =>
{
    var client = s.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration.GetSection("RestaurantDatabase:DatabaseName").Value);
});

// Register services
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<TableService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Class to represent database settings
public class RestaurantDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string MenuCollectionName { get; set; } = null!;
    public string OrdersCollectionName { get; set; } = null!;
    public string TablesCollectionName { get; set; } = null!;
}