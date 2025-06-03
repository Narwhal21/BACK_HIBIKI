using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class GeneroRepository : IGeneroRepository
    {
        private readonly string _connectionString;

        public GeneroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Genero>> GetAllAsync()
        {
            var generos = new List<Genero>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"GeneroId\", \"Nombre\", \"Descripcion\", \"Color\", \"Icono\" FROM \"Genero\"";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var genero = new Genero
                            {
                                GeneroId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Color = reader.IsDBNull(3) ? "#ff5100" : reader.GetString(3),
                                Icono = reader.IsDBNull(4) ? "🎵" : reader.GetString(4)
                            };

                            generos.Add(genero);
                        }
                    }
                }
            }

            return generos;
        }

        public async Task<Genero> GetByIdAsync(int id)
        {
            Genero genero = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"GeneroId\", \"Nombre\", \"Descripcion\", \"Color\", \"Icono\" FROM \"Genero\" WHERE \"GeneroId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            genero = new Genero
                            {
                                GeneroId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Color = reader.IsDBNull(3) ? "#ff5100" : reader.GetString(3),
                                Icono = reader.IsDBNull(4) ? "🎵" : reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return genero;
        }

        public async Task AddAsync(Genero genero)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO \"Genero\" (\"Nombre\", \"Descripcion\", \"Color\", \"Icono\") VALUES (@Nombre, @Descripcion, @Color, @Icono)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", genero.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", genero.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Color", genero.Color ?? "#ff5100");
                    command.Parameters.AddWithValue("@Icono", genero.Icono ?? "🎵");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Genero genero)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE \"Genero\" SET \"Nombre\" = @Nombre, \"Descripcion\" = @Descripcion, \"Color\" = @Color, \"Icono\" = @Icono WHERE \"GeneroId\" = @GeneroId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GeneroId", genero.GeneroId);
                    command.Parameters.AddWithValue("@Nombre", genero.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", genero.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Color", genero.Color ?? "#ff5100");
                    command.Parameters.AddWithValue("@Icono", genero.Icono ?? "🎵");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM \"Genero\" WHERE \"GeneroId\" = @Id";
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
                    INSERT INTO ""Genero"" (""Nombre"", ""Descripcion"", ""Color"", ""Icono"")
                    VALUES 
                    (@Nombre1, @Descripcion1, @Color1, @Icono1),
                    (@Nombre2, @Descripcion2, @Color2, @Icono2),
                    (@Nombre3, @Descripcion3, @Color3, @Icono3),
                    (@Nombre4, @Descripcion4, @Color4, @Icono4),
                    (@Nombre5, @Descripcion5, @Color5, @Icono5),
                    (@Nombre6, @Descripcion6, @Color6, @Icono6),
                    (@Nombre7, @Descripcion7, @Color7, @Icono7),
                    (@Nombre8, @Descripcion8, @Color8, @Icono8)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Pop
                    command.Parameters.AddWithValue("@Nombre1", "Pop");
                    command.Parameters.AddWithValue("@Descripcion1", "Música popular contemporánea");
                    command.Parameters.AddWithValue("@Color1", "#FF6B9D");
                    command.Parameters.AddWithValue("@Icono1", "🎤");

                    // Rock
                    command.Parameters.AddWithValue("@Nombre2", "Rock");
                    command.Parameters.AddWithValue("@Descripcion2", "Rock clásico y moderno");
                    command.Parameters.AddWithValue("@Color2", "#8B5A3C");
                    command.Parameters.AddWithValue("@Icono2", "🎸");

                    // Hip Hop
                    command.Parameters.AddWithValue("@Nombre3", "Hip Hop");
                    command.Parameters.AddWithValue("@Descripcion3", "Rap y música urbana");
                    command.Parameters.AddWithValue("@Color3", "#FFD700");
                    command.Parameters.AddWithValue("@Icono3", "🎧");

                    // Reggaeton
                    command.Parameters.AddWithValue("@Nombre4", "Reggaetón");
                    command.Parameters.AddWithValue("@Descripcion4", "Música latina urbana");
                    command.Parameters.AddWithValue("@Color4", "#FF5100");
                    command.Parameters.AddWithValue("@Icono4", "🔥");

                    // Electronic
                    command.Parameters.AddWithValue("@Nombre5", "Electrónica");
                    command.Parameters.AddWithValue("@Descripcion5", "Música electrónica y EDM");
                    command.Parameters.AddWithValue("@Color5", "#00BFFF");
                    command.Parameters.AddWithValue("@Icono5", "⚡");

                    // Jazz
                    command.Parameters.AddWithValue("@Nombre6", "Jazz");
                    command.Parameters.AddWithValue("@Descripcion6", "Jazz clásico y moderno");
                    command.Parameters.AddWithValue("@Color6", "#4B0082");
                    command.Parameters.AddWithValue("@Icono6", "🎷");

                    // Classical
                    command.Parameters.AddWithValue("@Nombre7", "Clásica");
                    command.Parameters.AddWithValue("@Descripcion7", "Música clásica y orquestal");
                    command.Parameters.AddWithValue("@Color7", "#8B4513");
                    command.Parameters.AddWithValue("@Icono7", "🎻");

                    // R&B
                    command.Parameters.AddWithValue("@Nombre8", "R&B");
                    command.Parameters.AddWithValue("@Descripcion8", "Rhythm and Blues");
                    command.Parameters.AddWithValue("@Color8", "#9932CC");
                    command.Parameters.AddWithValue("@Icono8", "💜");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}