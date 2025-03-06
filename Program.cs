using Microsoft.AspNetCore.Rewrite;
using MyMusicApp.Repositories;
using MyMusicApp.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
        // Nota: AllowAnyOrigin() es incompatible con AllowCredentials()
        // Si necesitas cookies/credenciales, deberás especificar orígenes concretos
    });
});

// 🔹 Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

// 🔹 Registración de repositorios y servicios
builder.Services.AddScoped<IAlbumRepository>(provider =>
    new AlbumRepository(configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddScoped<IArtistaRepository>(provider =>
    new ArtistaRepository(configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IArtistaService, ArtistaService>();

builder.Services.AddScoped<ICancionRepository>(provider =>
    new CancionRepository(configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<ICancionService, CancionService>();

builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IPlaylistRepository>(provider =>
    new PlaylistRepository(configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

builder.Services.AddScoped<IPerfilRepository>(provider => 
    new PerfilRepository(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IPerfilService, PerfilService>();

var app = builder.Build();

// 🔹 Configurar Swagger siempre (no solo en desarrollo)
app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Añadir regla de redirección de la raíz a Swagger
app.UseRewriter(new RewriteOptions()
    .AddRedirect("^$", "swagger"));

// 🔹 Habilitar CORS antes de cualquier otro middleware
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 🔹 También añadir endpoint explícito para la raíz que redirige a Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();