using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly string _connectionString;

        public PlaylistRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

       public async Task<List<Playlist>> GetAllAsync()
{
    var playlists = new List<Playlist>();

    using (var connection = new NpgsqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        string query = "SELECT \"PlaylistId\", \"Nombre\", \"Descripcion\", \"Image\", \"FechaCreacion\", \"CreadorId\" FROM \"Playlist\"";

        using (var command = new NpgsqlCommand(query, connection))
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var playlist = new Playlist
                    {
                        PlaylistId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Image = reader.GetString(3),
                        FechaCreacion = reader.GetDateTime(4),
                        Creador = new Usuario { UserId = reader.GetInt32(5) }
                    };

                    playlists.Add(playlist);
                }
            }
        }
    }

    return playlists;
}


       public async Task<Playlist> GetByIdAsync(int id)
{
    Playlist playlist = null;

    using (var connection = new NpgsqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        string query = @"
            SELECT p.""PlaylistId"", p.""Nombre"", p.""Descripcion"", p.""Image"", p.""FechaCreacion"", p.""CreadorId"", 
                   c.""CancionId"", c.""Nombre"" AS CancionNombre, c.""Ruta"", c.""Image"" AS CancionImage, c.""Duracion""
            FROM ""Playlist"" p
            LEFT JOIN ""PlaylistCancion"" pc ON p.""PlaylistId"" = pc.""PlaylistId""
            LEFT JOIN ""Cancion"" c ON pc.""CancionId"" = c.""CancionId""
            WHERE p.""PlaylistId"" = @Id";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (playlist == null)
                    {
                        playlist = new Playlist
                        {
                            PlaylistId = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Image = reader.GetString(3),
                            FechaCreacion = reader.GetDateTime(4),
                            Creador = new Usuario { UserId = reader.GetInt32(5) },
                            Canciones = new List<Cancion>() // 🔹 Asegurar que la lista de canciones esté inicializada
                        };
                    }

                    if (!reader.IsDBNull(6)) // 🔹 Verificar si hay canciones asociadas
                    {
                        var cancion = new Cancion
                        {
                            CancionId = reader.GetInt32(6),
                            Nombre = reader.GetString(7),
                            Ruta = reader.GetString(8),
                            Image = reader.GetString(9),
                            Duracion = reader.IsDBNull(10) ? TimeSpan.Zero : reader.GetTimeSpan(10) // Manejo seguro de Duracion
                        };
                        playlist.Canciones.Add(cancion);
                    }
                }
            }
        }
    }

    return playlist;
}





        public async Task AddAsync(Playlist playlist)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO \"Playlist\" (\"Nombre\", \"Descripcion\",\"Image\", \"FechaCreacion\", \"CreadorId\") VALUES (@Nombre, @Descripcion, @Image, @FechaCreacion, @CreadorId)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", playlist.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", playlist.Descripcion);
                    command.Parameters.AddWithValue("@Image", playlist.Image);
                    command.Parameters.AddWithValue("@FechaCreacion", playlist.FechaCreacion);
                    command.Parameters.AddWithValue("@CreadorId", playlist.Creador.UserId); 

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Playlist playlist)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE \"Playlist\" SET \"Nombre\" = @Nombre, \"Descripcion\" = @Descripcion,\"Image\" = @Image, \"FechaCreacion\" = @FechaCreacion, \"CreadorId\" = @CreadorId WHERE \"PlaylistId\" = @PlaylistId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistId", playlist.PlaylistId);
                    command.Parameters.AddWithValue("@Nombre", playlist.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", playlist.Descripcion);
                    command.Parameters.AddWithValue("@Image", playlist.Image);
                    command.Parameters.AddWithValue("@FechaCreacion", playlist.FechaCreacion);
                    command.Parameters.AddWithValue("@CreadorId", playlist.Creador.UserId); 

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM \"Playlist\" WHERE \"PlaylistId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
