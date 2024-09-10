using CatalogService.API.Configurations;
using CatalogService.API.Repositories;
using CatalogService.API.Seeding;
using CatalogService.API.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetValue<string>("MongoSettings:ConnectionString")));

builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICatalogService, CatalogService.API.Services.CatalogService>();
builder.Services.AddScoped<ICatalogSeedService, CatalogSeedService>();

var app = builder.Build();


#region Catalog Seeding

using var scope = app.Services.CreateScope();
var seedService = scope.ServiceProvider.GetRequiredService<ICatalogSeedService>();
await seedService.SeedAsync();

#endregion 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
