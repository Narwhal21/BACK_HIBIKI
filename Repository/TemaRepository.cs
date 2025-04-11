using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class TemaRepository : ITemaRepository
    {
        private readonly string _connectionString;

        public TemaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Tema>> GetAllAsync()
        {
            var temas = new List<Tema>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT \"TemaId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Tema\"";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        temas.Add(MapTema(reader));
                    }
                }
            }

            return temas;
        }

        public async Task<List<Tema>> GetTemasByCantanteIdAsync(int cantanteId)
        {
            var temas = new List<Tema>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT \"TemaId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Tema\" WHERE \"CantanteId\" = @CantanteId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CantanteId", cantanteId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            temas.Add(MapTema(reader));
                        }
                    }
                }
            }

            return temas;
        }

        public async Task<Tema> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT \"TemaId\", \"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\" FROM \"Tema\" WHERE \"TemaId\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync() ? MapTema(reader) : null;
                    }
                }
            }
        }

        public async Task AddAsync(Tema tema)
        {
            if (tema == null) throw new ArgumentNullException(nameof(tema));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO \"Tema\" (\"CantanteId\", \"Nombre\", \"Duracion\", \"Ruta\", \"Image\") VALUES (@CantanteId, @Nombre, @Duracion, @Ruta, @Image)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    SetTemaParameters(command, tema);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Tema tema)
        {
            if (tema == null) throw new ArgumentNullException(nameof(tema));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE \"Tema\" SET \"CantanteId\" = @CantanteId, \"Nombre\" = @Nombre, \"Duracion\" = @Duracion, \"Ruta\" = @Ruta, \"Image\" = @Image WHERE \"TemaId\" = @TemaId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    SetTemaParameters(command, tema);
                    command.Parameters.AddWithValue("@TemaId", tema.TemaId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM \"Tema\" WHERE \"TemaId\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        // 🔹 Método para mapear un tema desde un DataReader
        private Tema MapTema(NpgsqlDataReader reader)
        {
            return new Tema
            {
                TemaId = reader.GetInt32(0),
                CantanteId = reader.GetInt32(1),
                Nombre = reader.GetString(2),
                Duracion = reader.GetTimeSpan(3),
                Ruta = reader.GetString(4),
                Image = reader.GetString(5)
            };
        }

        // 🔹 Método para establecer los parámetros de un tema en un comando SQL
        private void SetTemaParameters(NpgsqlCommand command, Tema tema)
        {
            command.Parameters.AddWithValue("@CantanteId", tema.CantanteId);
            command.Parameters.AddWithValue("@Nombre", tema.Nombre);
            command.Parameters.AddWithValue("@Duracion", tema.Duracion);
            command.Parameters.AddWithValue("@Ruta", tema.Ruta);
            command.Parameters.AddWithValue("@Image", tema.Image);
        }
    }
}
