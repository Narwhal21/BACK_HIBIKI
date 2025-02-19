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

                string query = "SELECT \"PlaylistId\", \"Nombre\", \"Descripcion\", \"FechaCreacion\", \"CreadorId\" FROM \"Playlist\"";
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
                                FechaCreacion = reader.GetDateTime(3),
                                Creador = new Usuario { UserId = reader.GetInt32(4) }
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

                string query = "SELECT \"PlaylistId\", \"Nombre\", \"Descripcion\", \"FechaCreacion\", \"CreadorId\" FROM \"Playlist\" WHERE \"PlaylistId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            playlist = new Playlist
                            {
                                PlaylistId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                FechaCreacion = reader.GetDateTime(3),
                                Creador = new Usuario { UserId = reader.GetInt32(4) } 
                            };
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

                string query = "INSERT INTO \"Playlist\" (\"Nombre\", \"Descripcion\", \"FechaCreacion\", \"CreadorId\") VALUES (@Nombre, @Descripcion, @FechaCreacion, @CreadorId)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", playlist.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", playlist.Descripcion);
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

                string query = "UPDATE \"Playlist\" SET \"Nombre\" = @Nombre, \"Descripcion\" = @Descripcion, \"FechaCreacion\" = @FechaCreacion, \"CreadorId\" = @CreadorId WHERE \"PlaylistId\" = @PlaylistId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistId", playlist.PlaylistId);
                    command.Parameters.AddWithValue("@Nombre", playlist.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", playlist.Descripcion);
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
