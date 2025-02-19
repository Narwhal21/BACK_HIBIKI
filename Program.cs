using MyMusicApp.Repositories;
using MyMusicApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

var databaseProvider = configuration["DatabaseProvider"] ?? "PostgreSQL";

// Add the AlbumRepository to the DI container
builder.Services.AddScoped<IAlbumRepository>(provider =>
    new AlbumRepository(builder.Configuration.GetConnectionString("PostgreSQL")));

// Add the AlbumService to the DI container
builder.Services.AddScoped<IAlbumService, AlbumService>();

// Add the ArtistaRepository to the DI container
builder.Services.AddScoped<IArtistaRepository>(provider =>
    new ArtistaRepository(builder.Configuration.GetConnectionString("PostgreSQL")));

// Add the ArtistaService to the DI container
builder.Services.AddScoped<IArtistaService, ArtistaService>();

// Add the CancionRepository to the DI container
builder.Services.AddScoped<ICancionRepository>(provider =>
    new CancionRepository(builder.Configuration.GetConnectionString("PostgreSQL")));

// Add the CancionService to the DI container
builder.Services.AddScoped<ICancionService, CancionService>();

// You can add other services or configurations here

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Use Swagger UI for API exploration in Development mode
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure middlewares
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
