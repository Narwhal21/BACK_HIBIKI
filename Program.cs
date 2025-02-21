using MyMusicApp.Repositories;
using MyMusicApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var configuration = builder.Configuration;
var databaseProvider = configuration["DatabaseProvider"] ?? "PostgreSQL";

// Add repositories and services to the DI container
builder.Services.AddScoped<IAlbumRepository>(provider =>
    new AlbumRepository(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddScoped<IArtistaRepository>(provider =>
    new ArtistaRepository(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IArtistaService, ArtistaService>();

builder.Services.AddScoped<ICancionRepository>(provider =>
    new CancionRepository(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<ICancionService, CancionService>();

builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IPlaylistRepository>(provider =>
    new PlaylistRepository(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

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

// Enable CORS
app.UseCors("AllowAllOrigins");

// Map controllers
app.MapControllers();

// Run the application
app.Run();
