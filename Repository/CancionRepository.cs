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
                // JOIN con las tablas Artista y Genero para obtener nombres completos
                string query = @"
                    SELECT c.""CancionId"", c.""AlbumId"", c.""CantanteId"", c.""GeneroId"", c.""Nombre"", c.""Duracion"", 
                           c.""Ruta"", c.""Image"", c.""videoURL"", c.""Letra"", c.""Videoclip"", 
                           a.""Nombre"" as ""ArtistaNombre"", g.""Nombre"" as ""GeneroNombre"",
                           al.""Name"" as ""AlbumNombre""
                    FROM ""Cancion"" c
                    LEFT JOIN ""Artista"" a ON c.""CantanteId"" = a.""CantanteId""
                    LEFT JOIN ""Genero"" g ON c.""GeneroId"" = g.""GeneroId""
                    LEFT JOIN ""Album"" al ON c.""AlbumId"" = al.""AlbumId""";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        canciones.Add(MapCancionComplete(reader));
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
                // JOIN con las tablas Artista, Genero y Album para obtener nombres completos
                string query = @"
                    SELECT c.""CancionId"", c.""AlbumId"", c.""CantanteId"", c.""GeneroId"", c.""Nombre"", c.""Duracion"", 
                           c.""Ruta"", c.""Image"", c.""videoURL"", c.""Letra"", c.""Videoclip"", 
                           a.""Nombre"" as ""ArtistaNombre"", g.""Nombre"" as ""GeneroNombre"",
                           al.""Name"" as ""AlbumNombre""
                    FROM ""Cancion"" c
                    LEFT JOIN ""Artista"" a ON c.""CantanteId"" = a.""CantanteId""
                    LEFT JOIN ""Genero"" g ON c.""GeneroId"" = g.""GeneroId""
                    LEFT JOIN ""Album"" al ON c.""AlbumId"" = al.""AlbumId""
                    WHERE c.""CancionId"" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync() ? MapCancionComplete(reader) : null;
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
                // JOIN con la tabla Artista para obtener el nombre del artista
                string query = @"
                    SELECT c.""CancionId"", c.""AlbumId"", c.""CantanteId"", c.""GeneroId"", c.""Nombre"", c.""Duracion"", 
                           c.""Ruta"", c.""Image"", c.""videoURL"", c.""Letra"", c.""Videoclip"", 
                           a.""Nombre"" as ""ArtistaNombre""
                    FROM ""Cancion"" c
                    LEFT JOIN ""Artista"" a ON c.""CantanteId"" = a.""CantanteId""
                    WHERE c.""AlbumId"" = @AlbumId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AlbumId", albumId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            canciones.Add(MapCancionWithArtist(reader));
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
                // JOIN con la tabla Artista para obtener el nombre del artista
                string query = @"
                    SELECT c.""CancionId"", c.""AlbumId"", c.""CantanteId"", c.""GeneroId"", c.""Nombre"", c.""Duracion"", 
                           c.""Ruta"", c.""Image"", c.""videoURL"", c.""Letra"", c.""Videoclip"", 
                           a.""Nombre"" as ""ArtistaNombre""
                    FROM ""Cancion"" c
                    LEFT JOIN ""Artista"" a ON c.""CantanteId"" = a.""CantanteId""
                    WHERE c.""CantanteId"" = @CantanteId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CantanteId", cantanteId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            canciones.Add(MapCancionWithArtist(reader));
                        }
                    }
                }
            }

            return canciones;
        }

        // IMPLEMENTACIÓN del método para obtener canciones por género
        public async Task<List<Cancion>> GetCancionesByGeneroAsync(int generoId)
        {
            var canciones = new List<Cancion>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // JOIN con las tablas Artista y Genero para obtener nombres completos
                string query = @"
                    SELECT c.""CancionId"", c.""AlbumId"", c.""CantanteId"", c.""GeneroId"", c.""Nombre"", c.""Duracion"", 
                           c.""Ruta"", c.""Image"", c.""videoURL"", c.""Letra"", c.""Videoclip"", 
                           a.""Nombre"" as ""ArtistaNombre""
                    FROM ""Cancion"" c
                    LEFT JOIN ""Artista"" a ON c.""CantanteId"" = a.""CantanteId""
                    WHERE c.""GeneroId"" = @GeneroId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GeneroId", generoId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            canciones.Add(MapCancionWithArtist(reader));
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
                // Incluir GeneroId en la inserción
                string query = "INSERT INTO \"Cancion\" (\"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\", \"videoURL\", \"Letra\", \"Videoclip\", \"GeneroId\") VALUES (@AlbumId, @CantanteId, @Nombre, @Duracion, @Ruta, @Image, @VideoUrl, @Letra, @Videoclip, @GeneroId)";

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
                // Incluir GeneroId en la actualización
                string query = "UPDATE \"Cancion\" SET \"AlbumId\" = @AlbumId, \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta, \"Image\" = @Image, \"videoURL\" = @VideoUrl, \"Letra\" = @Letra, \"Videoclip\" = @Videoclip, \"GeneroId\" = @GeneroId WHERE \"CancionId\" = @CancionId";

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

        // CORREGIDO: Mapear con videoURL (MP4), Letra, Videoclip (YouTube) y nombre del artista
        private Cancion MapCancionWithArtist(NpgsqlDataReader reader)
        {
            return new Cancion
            {
                CancionId = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                CantanteId = reader.GetInt32(2),
                GeneroId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3), // ✅ INCLUIR GeneroId
                Nombre = reader.GetString(4),
                Duracion = reader.GetTimeSpan(5),
                Ruta = reader.GetString(6),
                Image = reader.GetString(7),
                VideoUrl = reader.IsDBNull(8) ? null : reader.GetString(8), // videoURL de DB
                Letra = reader.IsDBNull(9) ? string.Empty : reader.GetString(9), // Letra de DB
                Videoclip = reader.IsDBNull(10) ? string.Empty : reader.GetString(10), // Videoclip de DB
                Artista = reader.IsDBNull(11) ? "Artista desconocido" : reader.GetString(11) // Nombre del artista
            };
        }

        // LEGACY: Mapear sin artista (para compatibilidad) - CORREGIDO
        private Cancion MapCancion(NpgsqlDataReader reader)
        {
            return new Cancion
            {
                CancionId = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                CantanteId = reader.GetInt32(2),
                GeneroId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3), // ✅ INCLUIR GeneroId
                Nombre = reader.GetString(4),
                Duracion = reader.GetTimeSpan(5),
                Ruta = reader.GetString(6),
                Image = reader.GetString(7),
                VideoUrl = reader.IsDBNull(8) ? null : reader.GetString(8), // videoURL de DB
                Letra = reader.IsDBNull(9) ? string.Empty : reader.GetString(9), // Letra de DB
                Videoclip = reader.IsDBNull(10) ? string.Empty : reader.GetString(10), // Videoclip de DB
                Artista = "Artista desconocido" // Valor por defecto
            };
        }

        // CORREGIDO: Mapear con artista, género y álbum completos
        private Cancion MapCancionComplete(NpgsqlDataReader reader)
        {
            return new Cancion
            {
                CancionId = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                CantanteId = reader.GetInt32(2),
                GeneroId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3), // ✅ INCLUIR GeneroId
                Nombre = reader.GetString(4),
                Duracion = reader.GetTimeSpan(5),
                Ruta = reader.GetString(6),
                Image = reader.GetString(7),
                VideoUrl = reader.IsDBNull(8) ? null : reader.GetString(8), // videoURL de DB
                Letra = reader.IsDBNull(9) ? string.Empty : reader.GetString(9), // Letra de DB
                Videoclip = reader.IsDBNull(10) ? string.Empty : reader.GetString(10), // Videoclip de DB
                Artista = reader.IsDBNull(11) ? "Artista desconocido" : reader.GetString(11), // Nombre del artista
                Genero = reader.IsDBNull(12) ? "Sin género" : reader.GetString(12), // Nombre del género
                Album = reader.IsDBNull(13) ? "Álbum desconocido" : reader.GetString(13) // Nombre del álbum
            };
        }

        // Parámetros con videoURL, Letra, Videoclip y GeneroId
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
            // ✅ CORREGIDO: GeneroId
            command.Parameters.AddWithValue("@GeneroId", cancion.GeneroId.HasValue ? (object)cancion.GeneroId.Value : DBNull.Value);
        }
    }
}