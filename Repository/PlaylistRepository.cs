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

                // Solo usar las columnas que sabemos que existen
                string query = "SELECT \"PlaylistId\", \"CreadorId\", \"Nombre\", \"Image\", \"Descripcion\", \"FechaCreacion\" FROM \"Playlist\"";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var playlist = new Playlist
                            {
                                PlaylistId = reader.GetInt32(0),
                                CreadorId = reader.GetInt32(1),
                                UserId = reader.GetInt32(1), // Usar CreadorId como UserId
                                Nombre = reader.GetString(2),
                                Image = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Descripcion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                FechaCreacion = reader.GetDateTime(5),
                                Creador = new Usuario { UserId = reader.GetInt32(1) },
                                Canciones = new List<Cancion>()
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

                string playlistQuery = "SELECT \"PlaylistId\", \"CreadorId\", \"Nombre\", \"Image\", \"Descripcion\", \"FechaCreacion\" FROM \"Playlist\" WHERE \"PlaylistId\" = @Id";

                using (var command = new NpgsqlCommand(playlistQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            playlist = new Playlist
                            {
                                PlaylistId = reader.GetInt32(0),
                                CreadorId = reader.GetInt32(1),
                                UserId = reader.GetInt32(1), // Usar CreadorId como UserId
                                Nombre = reader.GetString(2),
                                Image = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Descripcion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                FechaCreacion = reader.GetDateTime(5),
                                Creador = new Usuario { UserId = reader.GetInt32(1) },
                                Canciones = new List<Cancion>()
                            };
                        }
                    }
                }

                // Obtener las canciones de la playlist
                if (playlist != null)
                {
                    try
                    {
                        string cancionesQuery = @"
                            SELECT c.""CancionId"", c.""Nombre"", c.""Ruta"", c.""Image"", c.""Duracion"", c.""CantanteId"", c.""AlbumId""
                            FROM ""Cancion"" c
                            INNER JOIN ""PlaylistCancion"" pc ON c.""CancionId"" = pc.""CancionId""
                            WHERE pc.""PlaylistId"" = @PlaylistId";

                        using (var cancionCommand = new NpgsqlCommand(cancionesQuery, connection))
                        {
                            cancionCommand.Parameters.AddWithValue("@PlaylistId", id);

                            using (var cancionReader = await cancionCommand.ExecuteReaderAsync())
                            {
                                while (await cancionReader.ReadAsync())
                                {
                                    var cancion = new Cancion
                                    {
                                        CancionId = cancionReader.GetInt32(0),
                                        Nombre = cancionReader.GetString(1),
                                        Ruta = cancionReader.GetString(2),
                                        Image = cancionReader.IsDBNull(3) ? string.Empty : cancionReader.GetString(3),
                                        Duracion = cancionReader.IsDBNull(4) ? TimeSpan.Zero : cancionReader.GetTimeSpan(4),
                                        CantanteId = cancionReader.GetInt32(5),
                                        AlbumId = cancionReader.GetInt32(6)
                                    };
                                    playlist.Canciones.Add(cancion);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al cargar canciones: {ex.Message}");
                        // Las canciones quedan como lista vacía si hay error
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

                string query = @"
                    INSERT INTO ""Playlist"" (""CreadorId"", ""Nombre"", ""Image"", ""Descripcion"", ""FechaCreacion"") 
                    VALUES (@CreadorId, @Nombre, @Image, @Descripcion, @FechaCreacion) 
                    RETURNING ""PlaylistId""";
                
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreadorId", playlist.CreadorId);
                    command.Parameters.AddWithValue("@Nombre", playlist.Nombre);
                    command.Parameters.AddWithValue("@Image", playlist.Image ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Descripcion", playlist.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FechaCreacion", playlist.FechaCreacion);

                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        playlist.PlaylistId = Convert.ToInt32(result);
                        playlist.UserId = playlist.CreadorId; // Sincronizar
                    }
                }
            }
        }

        public async Task UpdateAsync(Playlist playlist)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE ""Playlist"" 
                    SET ""CreadorId"" = @CreadorId, ""Nombre"" = @Nombre, ""Image"" = @Image,
                        ""Descripcion"" = @Descripcion, ""FechaCreacion"" = @FechaCreacion 
                    WHERE ""PlaylistId"" = @PlaylistId";
                
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistId", playlist.PlaylistId);
                    command.Parameters.AddWithValue("@CreadorId", playlist.CreadorId);
                    command.Parameters.AddWithValue("@Nombre", playlist.Nombre);
                    command.Parameters.AddWithValue("@Image", playlist.Image ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Descripcion", playlist.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FechaCreacion", playlist.FechaCreacion);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Primero eliminar relaciones
                try
                {
                    string deleteRelationsQuery = "DELETE FROM \"PlaylistCancion\" WHERE \"PlaylistId\" = @Id";
                    using (var relationCommand = new NpgsqlCommand(deleteRelationsQuery, connection))
                    {
                        relationCommand.Parameters.AddWithValue("@Id", id);
                        await relationCommand.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar relaciones: {ex.Message}");
                }

                // Luego eliminar la playlist
                string deletePlaylistQuery = "DELETE FROM \"Playlist\" WHERE \"PlaylistId\" = @Id";
                using (var command = new NpgsqlCommand(deletePlaylistQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> AddCancionToPlaylistAsync(int playlistId, int cancionId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                try
                {
                    // Verificar si ya existe la relación
                    string checkQuery = "SELECT COUNT(*) FROM \"PlaylistCancion\" WHERE \"PlaylistId\" = @PlaylistId AND \"CancionId\" = @CancionId";
                    using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@PlaylistId", playlistId);
                        checkCommand.Parameters.AddWithValue("@CancionId", cancionId);
                        
                        var count = await checkCommand.ExecuteScalarAsync();
                        if (Convert.ToInt32(count) > 0)
                        {
                            return false; // Ya existe
                        }
                    }

                    // Insertar nueva relación
                    string insertQuery = "INSERT INTO \"PlaylistCancion\" (\"PlaylistId\", \"CancionId\") VALUES (@PlaylistId, @CancionId)";
                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@PlaylistId", playlistId);
                        command.Parameters.AddWithValue("@CancionId", cancionId);
                        
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar canción: {ex.Message}");
                    return false;
                }
            }
        }

        public async Task<bool> RemoveCancionFromPlaylistAsync(int playlistId, int cancionId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                try
                {
                    string deleteQuery = "DELETE FROM \"PlaylistCancion\" WHERE \"PlaylistId\" = @PlaylistId AND \"CancionId\" = @CancionId";
                    using (var command = new NpgsqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@PlaylistId", playlistId);
                        command.Parameters.AddWithValue("@CancionId", cancionId);
                        
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar canción: {ex.Message}");
                    return false;
                }
            }
        }
    }
}