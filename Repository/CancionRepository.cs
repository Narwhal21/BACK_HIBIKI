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
<<<<<<< HEAD
                // Añadido campo Videoclip para YouTube
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Videoclip\" FROM \"Cancion\"";
=======
                // CORREGIDO: videoURL con mayúsculas
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\"";
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

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
<<<<<<< HEAD
                // Añadido campo Videoclip para YouTube
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Videoclip\" FROM \"Cancion\" WHERE \"CancionId\" = @Id";
=======
                // CORREGIDO: videoURL con mayúsculas
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\" WHERE \"CancionId\" = @Id";
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

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
<<<<<<< HEAD
                // Añadido campo Videoclip para YouTube
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Videoclip\" FROM \"Cancion\" WHERE \"AlbumId\" = @AlbumId";
=======
                // CORREGIDO: videoURL con mayúsculas
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\" WHERE \"AlbumId\" = @AlbumId";
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

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

        // NUEVO: Método que faltaba - Obtener canciones por cantante
        public async Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId)
        {
            var canciones = new List<Cancion>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
<<<<<<< HEAD
                // Añadido campo Videoclip para YouTube
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Videoclip\" FROM \"Cancion\" WHERE \"CantanteId\" = @CantanteId";
=======
                // CORREGIDO: videoURL con mayúsculas
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\" FROM \"Cancion\" WHERE \"CantanteId\" = @CantanteId";
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

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
<<<<<<< HEAD
                // Añadido campo Videoclip para YouTube
                string query = "INSERT INTO \"Cancion\" (\"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Videoclip\") VALUES (@AlbumId, @CantanteId, @Nombre, @Duracion, @Ruta, @Image, @VideoUrl, @Videoclip)";
=======
                // CORREGIDO: videoURL con mayúsculas
                string query = "INSERT INTO \"Cancion\" (\"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\") VALUES (@AlbumId, @CantanteId, @Nombre, @Duracion, @Ruta, @Image, @VideoUrl, @Letra, @Videoclip)";
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

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
<<<<<<< HEAD
                // Añadido campo Videoclip para YouTube
                string query = "UPDATE \"Cancion\" SET \"AlbumId\" = @AlbumId, \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta, \"Image\" = @Image, \"videoURL\" = @VideoUrl, \"Videoclip\" = @Videoclip WHERE \"CancionId\" = @CancionId";
=======
                // CORREGIDO: videoURL con mayúsculas
                string query = "UPDATE \"Cancion\" SET \"AlbumId\" = @AlbumId, \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta, \"Image\" = @Image, \"videoURL\" = @VideoUrl, \"Letra\", = @Letra, \"Videoclip\" = @Videoclip WHERE \"CancionId\" = @CancionId";
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

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

        // Mapear con videoURL (MP4) y Videoclip (YouTube)
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
<<<<<<< HEAD
                VideoUrl = reader.IsDBNull(7) ? null : reader.GetString(7),    // MP4 para sincronización
                Videoclip = reader.IsDBNull(8) ? null : reader.GetString(8)    // YouTube para abrir externamente
=======
                VideoUrl = reader.IsDBNull(7) ? null : reader.GetString(7), // Leer videoURL de DB
                Letra = reader.GetString(8),
                Videoclip = reader.GetString(9)
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86
            };
        }

        // Parámetros con videoURL y Videoclip
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
<<<<<<< HEAD
            // Videoclip para YouTube
            command.Parameters.AddWithValue("@Videoclip", cancion.Videoclip ?? (object)DBNull.Value);
=======
            command.Parameters.AddWithValue("@Letra", cancion.Letra);
            command.Parameters.AddWithValue("@Videoclip", cancion.Videoclip);
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86
        }
    }
}