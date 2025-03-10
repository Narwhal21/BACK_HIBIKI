using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly string _connectionString;

        public PerfilRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Perfil>> GetAllAsync()
        {
            var perfiles = new List<Perfil>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"PerfilId\", \"UserId\", \"Nombre\", \"Imagen\", \"FechaCreacion\", \"UltimaActualizacion\", \"ArtistasMasEscuchados\", \"CancionesMasEscuchadas\", \"CantanteId\", \"CancionId\", \"PlaylistId\" FROM \"Perfil\"";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var perfil = new Perfil();
                            
                    
                            perfil.PerfilId = reader.GetInt32(0);
                            perfil.UserId = reader.GetInt32(1);
                            perfil.Nombre = reader.GetString(2);
                            perfil.Imagen = reader.IsDBNull(3) ? null : reader.GetString(3);
                            perfil.FechaCreacion = reader.GetDateTime(4);
                            perfil.UltimaActualizacion = reader.GetDateTime(5);

                     
                            string artistasJson = reader.IsDBNull(6) ? "[]" : reader.GetString(6);
                            string cancionesJson = reader.IsDBNull(7) ? "[]" : reader.GetString(7);

                            perfil.ArtistasMasEscuchados = JsonSerializer.Deserialize<List<Artista>>(artistasJson);
                            perfil.CancionesMasEscuchadas = JsonSerializer.Deserialize<List<Cancion>>(cancionesJson);

              
                            perfil.CantanteId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8);
                            perfil.CancionId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9);
                            perfil.PlaylistId = reader.IsDBNull(10) ? null : (int?)reader.GetInt32(10);

                            perfiles.Add(perfil);
                        }
                    }
                }
            }

            return perfiles;
        }

        public async Task<Perfil> GetByNombreAsync(string nombre)
        {
            Perfil perfil = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"PerfilId\", \"UserId\", \"Nombre\", \"Imagen\", \"FechaCreacion\", \"UltimaActualizacion\", \"ArtistasMasEscuchados\", \"CancionesMasEscuchadas\", \"CantanteId\", \"CancionId\", \"PlaylistId\" FROM \"Perfil\" WHERE \"Nombre\" = @Nombre";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            perfil = new Perfil();
                            
                            perfil.PerfilId = reader.GetInt32(0);
                            perfil.UserId = reader.GetInt32(1);
                            perfil.Nombre = reader.GetString(2);
                            perfil.Imagen = reader.IsDBNull(3) ? null : reader.GetString(3);
                            perfil.FechaCreacion = reader.GetDateTime(4);
                            perfil.UltimaActualizacion = reader.GetDateTime(5);

                            string artistasJson = reader.IsDBNull(6) ? "[]" : reader.GetString(6);
                            string cancionesJson = reader.IsDBNull(7) ? "[]" : reader.GetString(7);

                            perfil.ArtistasMasEscuchados = JsonSerializer.Deserialize<List<Artista>>(artistasJson);
                            perfil.CancionesMasEscuchadas = JsonSerializer.Deserialize<List<Cancion>>(cancionesJson);

                            perfil.CantanteId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8);
                            perfil.CancionId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9);
                            perfil.PlaylistId = reader.IsDBNull(10) ? null : (int?)reader.GetInt32(10);
                        }
                    }
                }
            }

            return perfil;
        }

        public async Task AddAsync(Perfil perfil)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO ""Perfil"" (""UserId"", ""Nombre"", ""Imagen"", ""FechaCreacion"", ""UltimaActualizacion"", 
                                ""ArtistasMasEscuchados"", ""CancionesMasEscuchadas"", ""CantanteId"", ""CancionId"", ""PlaylistId"") 
                                VALUES (@UserId, @Nombre, @Imagen, @FechaCreacion, @UltimaActualizacion, 
                                @ArtistasMasEscuchados, @CancionesMasEscuchadas, @CantanteId, @CancionId, @PlaylistId) 
                                RETURNING ""PerfilId""";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", perfil.UserId);
                    command.Parameters.AddWithValue("@Nombre", perfil.Nombre);
                    command.Parameters.AddWithValue("@Imagen", perfil.Imagen ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FechaCreacion", perfil.FechaCreacion);
                    command.Parameters.AddWithValue("@UltimaActualizacion", perfil.UltimaActualizacion);
                    
                    string artistasJson = JsonSerializer.Serialize(perfil.ArtistasMasEscuchados);
                    string cancionesJson = JsonSerializer.Serialize(perfil.CancionesMasEscuchadas);
                    command.Parameters.AddWithValue("@ArtistasMasEscuchados", artistasJson);
                    command.Parameters.AddWithValue("@CancionesMasEscuchadas", cancionesJson);
                    
                    command.Parameters.AddWithValue("@CantanteId", perfil.CantanteId.HasValue ? (object)perfil.CantanteId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@CancionId", perfil.CancionId.HasValue ? (object)perfil.CancionId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@PlaylistId", perfil.PlaylistId.HasValue ? (object)perfil.PlaylistId.Value : DBNull.Value);

                    var perfilId = (int)await command.ExecuteScalarAsync();
                    perfil.PerfilId = perfilId;
                }
            }
        }

        public async Task UpdateAsync(Perfil perfil)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                perfil.UltimaActualizacion = DateTime.Now;

                string query = @"UPDATE ""Perfil"" SET 
                                ""UserId"" = @UserId, 
                                ""Nombre"" = @Nombre, 
                                ""Imagen"" = @Imagen, 
                                ""UltimaActualizacion"" = @UltimaActualizacion, 
                                ""ArtistasMasEscuchados"" = @ArtistasMasEscuchados, 
                                ""CancionesMasEscuchadas"" = @CancionesMasEscuchadas, 
                                ""CantanteId"" = @CantanteId, 
                                ""CancionId"" = @CancionId, 
                                ""PlaylistId"" = @PlaylistId 
                                WHERE ""PerfilId"" = @PerfilId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PerfilId", perfil.PerfilId);
                    command.Parameters.AddWithValue("@UserId", perfil.UserId);
                    command.Parameters.AddWithValue("@Nombre", perfil.Nombre);
                    command.Parameters.AddWithValue("@Imagen", perfil.Imagen ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UltimaActualizacion", perfil.UltimaActualizacion);
                    
                    string artistasJson = JsonSerializer.Serialize(perfil.ArtistasMasEscuchados);
                    string cancionesJson = JsonSerializer.Serialize(perfil.CancionesMasEscuchadas);
                    command.Parameters.AddWithValue("@ArtistasMasEscuchados", artistasJson);
                    command.Parameters.AddWithValue("@CancionesMasEscuchadas", cancionesJson);
                    
                    command.Parameters.AddWithValue("@CantanteId", perfil.CantanteId.HasValue ? (object)perfil.CantanteId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@CancionId", perfil.CancionId.HasValue ? (object)perfil.CancionId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@PlaylistId", perfil.PlaylistId.HasValue ? (object)perfil.PlaylistId.Value : DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(string nombre)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM \"Perfil\" WHERE \"Nombre\" = @Nombre";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);

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
                    INSERT INTO ""Perfil"" (""UserId"", ""Nombre"", ""Imagen"", ""FechaCreacion"", ""UltimaActualizacion"", 
                    ""ArtistasMasEscuchados"", ""CancionesMasEscuchadas"")
                    VALUES 
                    (@UserId1, @Nombre1, @Imagen1, @FechaCreacion1, @UltimaActualizacion1, @Artistas1, @Canciones1),
                    (@UserId2, @Nombre2, @Imagen2, @FechaCreacion2, @UltimaActualizacion2, @Artistas2, @Canciones2)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    DateTime now = DateTime.Now;
                    
              
                    command.Parameters.AddWithValue("@UserId1", 1); 
                    command.Parameters.AddWithValue("@Nombre1", "Perfil1");
                    command.Parameters.AddWithValue("@Imagen1", DBNull.Value);
                    command.Parameters.AddWithValue("@FechaCreacion1", now);
                    command.Parameters.AddWithValue("@UltimaActualizacion1", now);
                    command.Parameters.AddWithValue("@Artistas1", "[]");
                    command.Parameters.AddWithValue("@Canciones1", "[]");

              
                    command.Parameters.AddWithValue("@UserId2", 2); 
                    command.Parameters.AddWithValue("@Nombre2", "Perfil2");
                    command.Parameters.AddWithValue("@Imagen2", DBNull.Value);
                    command.Parameters.AddWithValue("@FechaCreacion2", now);
                    command.Parameters.AddWithValue("@UltimaActualizacion2", now);
                    command.Parameters.AddWithValue("@Artistas2", "[]");
                    command.Parameters.AddWithValue("@Canciones2", "[]");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}