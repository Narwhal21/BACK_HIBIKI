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
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var cancion = new Cancion
                            {
                                CancionId = reader.GetInt32(0),
                                AlbumId = reader.GetInt32(1),
                                CantanteId = reader.GetInt32(2),
                                Nombre = reader.GetString(3),
                                Duracion = reader.GetInt32(4),
                                Ruta = reader.GetString(5),
                                Image = reader.GetString(6)
                            };

                            canciones.Add(cancion);
                        }
                    }
                }
            }

            return canciones;
        }

        public async Task<Cancion> GetByIdAsync(int id)
        {
            Cancion cancion = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"CancionId\", \"AlbumId\",\"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Cancion\" WHERE \"CancionId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            cancion = new Cancion
                            {
                               CancionId = reader.GetInt32(0),
                                AlbumId = reader.GetInt32(1),
                                CantanteId = reader.GetInt32(2),
                                Nombre = reader.GetString(3),
                                Duracion = reader.GetInt32(4),
                                Ruta = reader.GetString(5),
                                Image = reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return cancion;
        }

     public async Task AddAsync(Cancion cancion)
{
    using (var connection = new NpgsqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        string query = "INSERT INTO \"Cancion\" (\"AlbumId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\") VALUES (@AlbumId, @CantanteId, @Nombre, @Duracion, @Ruta, @Image)";
        using (var command = new NpgsqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@AlbumId", cancion.AlbumId);
            command.Parameters.AddWithValue("@CantanteId", cancion.CantanteId);
            command.Parameters.AddWithValue("@Nombre", cancion.Nombre);
            command.Parameters.AddWithValue("@Duracion", cancion.Duracion);
            command.Parameters.AddWithValue("@Ruta", cancion.Ruta);
            command.Parameters.AddWithValue("@Image", cancion.Image);

            await command.ExecuteNonQueryAsync();
        }
    }
}


        public async Task UpdateAsync(Cancion cancion)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE \"Cancion\" SET \"AlbumId\" = @AlbumId, \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta , \"Image\" = @Image WHERE \"CancionId\" = @CancionId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CancionId", cancion.CancionId);
                    command.Parameters.AddWithValue("@AlbumId", cancion.AlbumId);
                    command.Parameters.AddWithValue("@CantanteId", cancion.CantanteId);
                    command.Parameters.AddWithValue("@Nombre", cancion.Nombre);
                    command.Parameters.AddWithValue("@Duracion", cancion.Duracion);
                    command.Parameters.AddWithValue("@Ruta", cancion.Ruta);
                    command.Parameters.AddWithValue("@Image", cancion.Image);

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

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}