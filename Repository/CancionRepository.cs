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
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Cancion\"";

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
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Cancion\" WHERE \"CancionId\" = @Id";

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
                string query = "SELECT \"CancionId\", \"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Cancion\" WHERE \"AlbumId\" = @AlbumId";

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

        public async Task AddAsync(Cancion cancion)
        {
            if (cancion == null) throw new ArgumentNullException(nameof(cancion));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO \"Cancion\" (\"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\") VALUES (@AlbumId, @CantanteId, @Nombre, @Duracion, @Ruta, @Image)";

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
                string query = "UPDATE \"Cancion\" SET \"AlbumId\" = @AlbumId, \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta, \"Image\" = @Image WHERE \"CancionId\" = @CancionId";

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

        // 🔹 Método para mapear una canción desde un DataReader
        private Cancion MapCancion(NpgsqlDataReader reader)
        {
            return new Cancion
            {
                CancionId = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                CantanteId = reader.GetInt32(2),
                Nombre = reader.GetString(3),
                Duracion = reader.GetTimeSpan(4), // Asegúrate que en la DB sea TimeSpan
                Ruta = reader.GetString(5),
                Image = reader.GetString(6)
            };
        }

        // 🔹 Método para establecer los parámetros de una canción en un comando SQL
        private void SetCancionParameters(NpgsqlCommand command, Cancion cancion)
        {
            command.Parameters.AddWithValue("@AlbumId", cancion.AlbumId);
            command.Parameters.AddWithValue("@CantanteId", cancion.CantanteId);
            command.Parameters.AddWithValue("@Nombre", cancion.Nombre);
            command.Parameters.AddWithValue("@Duracion", cancion.Duracion);
            command.Parameters.AddWithValue("@Ruta", cancion.Ruta);
            command.Parameters.AddWithValue("@Image", cancion.Image);
        }
    }
}
