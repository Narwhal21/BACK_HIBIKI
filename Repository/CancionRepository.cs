using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class CancionRepository : ICancionRepository
    {
        private readonly string _connectionString;

        public CancionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Cancion>> GetAllAsync()
        {
            var canciones = new List<Cancion>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Incluimos todos los campos: videoURL, Letra y Videoclip
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\"";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        canciones.Add(MapCancion(reader));
                    }
                }
            }

            return canciones;
        }

        public async Task<Cancion> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Incluimos todos los campos: videoURL, Letra y Videoclip
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\" WHERE \"CancionId\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync() ? MapCancion(reader) : null;
                    }
                }
            }
        }

        public async Task<List<Cancion>> GetCancionesByAlbumIdAsync(int albumId)
        {
            var canciones = new List<Cancion>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Incluimos todos los campos: videoURL, Letra y Videoclip
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\" WHERE \"AlbumId\" = @AlbumId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AlbumId", albumId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            canciones.Add(MapCancion(reader));
                        }
                    }
                }
            }

            return canciones;
        }

        public async Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId)
        {
            var canciones = new List<Cancion>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Incluimos todos los campos: videoURL, Letra y Videoclip
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\" WHERE \"CantanteId\" = @CantanteId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CantanteId", cantanteId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            canciones.Add(MapCancion(reader));
                        }
                    }
                }
            }

            return canciones;
        }

        public async Task AddAsync(Cancion cancion)
        {
            if (cancion == null) throw new ArgumentNullException(nameof(cancion));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Incluimos todos los campos: videoURL, Letra y Videoclip
                string query = "INSERT INTO \"Cancion\" (\"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\") VALUES (@AlbumId, @CantanteId, @Nombre, @Duracion, @Ruta, @Image, @VideoUrl, @Letra, @Videoclip)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    SetCancionParameters(command, cancion);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Cancion cancion)
        {
            if (cancion == null) throw new ArgumentNullException(nameof(cancion));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // CORREGIDO: Eliminada la coma extra después de "Letra"
                string query = "UPDATE \"Cancion\" SET \"AlbumId\" = @AlbumId, \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta, \"Image\" = @Image, \"videoURL\" = @VideoUrl, \"Letra\" = @Letra, \"Videoclip\" = @Videoclip WHERE \"CancionId\" = @CancionId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    SetCancionParameters(command, cancion);
                    command.Parameters.AddWithValue("@CancionId", cancion.CancionId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM \"Cancion\" WHERE \"CancionId\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        // Mapear con videoURL (MP4), Letra y Videoclip (YouTube)
        private Cancion MapCancion(NpgsqlDataReader reader)
        {
            return new Cancion
            {
                CancionId = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                CantanteId = reader.GetInt32(2),
                Nombre = reader.GetString(3),
                Duracion = reader.GetTimeSpan(4),
                Ruta = reader.GetString(5),
                Image = reader.GetString(6),
                VideoUrl = reader.IsDBNull(7) ? null : reader.GetString(7), // videoURL de DB
                Letra = reader.IsDBNull(8) ? string.Empty : reader.GetString(8), // Letra de DB
                Videoclip = reader.IsDBNull(9) ? string.Empty : reader.GetString(9) // Videoclip de DB
            };
        }

        // Parámetros con videoURL, Letra y Videoclip
        private void SetCancionParameters(NpgsqlCommand command, Cancion cancion)
        {
            command.Parameters.AddWithValue("@AlbumId", cancion.AlbumId);
            command.Parameters.AddWithValue("@CantanteId", cancion.CantanteId);
            command.Parameters.AddWithValue("@Nombre", cancion.Nombre);
            command.Parameters.AddWithValue("@Duracion", cancion.Duracion);
            command.Parameters.AddWithValue("@Ruta", cancion.Ruta);
            command.Parameters.AddWithValue("@Image", cancion.Image);
            // videoURL se mapea a VideoUrl (MP4 para reproductor)
            command.Parameters.AddWithValue("@VideoUrl", cancion.VideoUrl ?? (object)DBNull.Value);
            // Letra de la canción
            command.Parameters.AddWithValue("@Letra", cancion.Letra ?? (object)DBNull.Value);
            // Videoclip para YouTube
            command.Parameters.AddWithValue("@Videoclip", cancion.Videoclip ?? (object)DBNull.Value);
        }
    }
}