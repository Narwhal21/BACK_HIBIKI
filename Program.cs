using MyMusicApp.Repositories;
using MyMusicApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins("http://localhost:5173") // Origen de tu frontend
            .AllowAnyHeader()
            .AllowAnyMethod());
});

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

// Add the UsuarioRepository to the DI container
builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(builder.Configuration.GetConnectionString("PostgreSQL")));

// Add the UsuarioService to the DI container
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Add the PlaylistRepository to the DI container
builder.Services.AddScoped<IPlaylistRepository>(provider =>
    new PlaylistRepository(builder.Configuration.GetConnectionString("PostgreSQL")));

// Add the PlaylistService to the DI container
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
app.UseCors("AllowLocalhost"); // Usar la política CORS

app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
