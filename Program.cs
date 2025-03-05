using MyMusicApp.Repositories;
using MyMusicApp.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Asegúrate de que es la URL de tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Si usas autenticación con cookies o headers personalizados
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

// 🔹 Configurar Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Habilitar CORS antes de cualquier otro middleware
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
