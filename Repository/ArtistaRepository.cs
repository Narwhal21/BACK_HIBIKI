using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly string _connectionString;

        public ArtistaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Artista>> GetAllAsync()
        {
            var artistas = new List<Artista>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"CantanteId\", \"Nombre\", \"OyentesMensuales\", \"Descripcion\", \"Image\" FROM \"Artista\"";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var artista = new Artista
                            {
                                CantanteId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                OyentesMensuales = reader.GetInt32(2),
                                Descripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Image = reader.GetString(4)
                            };

                            artistas.Add(artista);
                        }
                    }
                }
            }

            return artistas;
        }

        public async Task<Artista> GetByIdAsync(int id)
        {
            Artista artista = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"CantanteId\", \"Nombre\", \"OyentesMensuales\", \"Descripcion\", \"Image\" FROM \"Artista\" WHERE \"CantanteId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            artista = new Artista
                            {
                                CantanteId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                OyentesMensuales = reader.GetInt32(2),
                                Descripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Image = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return artista;
        }

        public async Task AddAsync(Artista artista)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO \"Artista\" (\"Nombre\", \"OyentesMensuales\", \"Descripcion\",\"Image\") VALUES (@Nombre, @OyentesMensuales, @Descripcion, @Image)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", artista.Nombre);
                    command.Parameters.AddWithValue("@OyentesMensuales", artista.OyentesMensuales);
                    command.Parameters.AddWithValue("@Descripcion", artista.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Image", artista.Image);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Artista artista)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

               string query = "UPDATE \"Artista\" SET \"Nombre\" = @Nombre, \"OyentesMensuales\" = @OyentesMensuales, \"Descripcion\" = @Descripcion, \"Image\" = @Image WHERE \"CantanteId\" = @CantanteId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CantanteId", artista.CantanteId);
                    command.Parameters.AddWithValue("@Nombre", artista.Nombre);
                    command.Parameters.AddWithValue("@OyentesMensuales", artista.OyentesMensuales);
                    command.Parameters.AddWithValue("@Descripcion", artista.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Image", artista.Image);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM \"Artista\" WHERE \"CantanteId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task InitializeDataAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO ""Artista"" (""Nombre"", ""OyentesMensuales"", ""Descripcion"")
                    VALUES 
                    (@Nombre1, @OyentesMensuales1, @Descripcion1),
                    (@Nombre2, @OyentesMensuales2, @Descripcion2)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre1", "Artista 1");
                    command.Parameters.AddWithValue("@OyentesMensuales1", 1000);
                    command.Parameters.AddWithValue("@Descripcion1", "Descripción del Artista 1");

                    command.Parameters.AddWithValue("@Nombre2", "Artista 2");
                    command.Parameters.AddWithValue("@OyentesMensuales2", 2000);
                    command.Parameters.AddWithValue("@Descripcion2", "Descripción del Artista 2");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
