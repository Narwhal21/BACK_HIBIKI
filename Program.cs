using MyMusicApp.Repositories;
using MyMusicApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor.
builder.Services.AddControllers();

// Configuración de CORS: se crea una política que permite solicitudes desde http://localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Agrega Swagger para la documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD
var configuration = builder.Configuration;
var databaseProvider = configuration["DatabaseProvider"] ?? "PostgreSQL";

// Registración de repositorios y servicios
=======
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
>>>>>>> 76863b5df601062e773c5399a6e91b320bb3576a
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usa la política de CORS antes de la autorización
app.UseCors("AllowFrontend");

app.UseAuthorization();

<<<<<<< HEAD
=======
// Enable CORS
app.UseCors("AllowAllOrigins");

// Map controllers
>>>>>>> 76863b5df601062e773c5399a6e91b320bb3576a
app.MapControllers();

app.Run();
