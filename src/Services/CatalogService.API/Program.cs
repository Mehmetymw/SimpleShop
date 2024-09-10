using CatalogService.API.Extensions;
using CatalogService.API.Repositories;
using CatalogService.API.Seeding;
using CatalogService.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSerilog();
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICatalogService, CatalogService.API.Services.CatalogService>();
builder.Services.AddScoped<ICatalogSeedService, CatalogSeedService>();

var app = builder.Build();

//CatalogDB Data Seeding
await app.SeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers(); 

app.Run();
