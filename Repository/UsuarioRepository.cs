using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Consulta actualizada para usar "fecha_registro" en minúsculas
                string query = "SELECT \"UserId\", \"Name\", \"Email\", \"Password\", \"IsPremium\", \"fecha_registro\" FROM \"Usuario\"";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuario = new Usuario
                            {
                                UserId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Password = reader.GetString(3),
                                IsPremium = reader.GetBoolean(4),
                                // Se lee la fecha de registro desde "fecha_registro"
                                Fecha_Registro = reader.GetDateTime(5)
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            Usuario usuario = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Consulta actualizada para usar "fecha_registro" en minúsculas
                string query = "SELECT \"UserId\", \"Name\", \"Email\", \"Password\", \"IsPremium\", \"fecha_registro\" FROM \"Usuario\" WHERE \"UserId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                UserId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Password = reader.GetString(3),
                                IsPremium = reader.GetBoolean(4),
                                // Se lee la fecha de registro desde "fecha_registro"
                                Fecha_Registro = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        // Método agregado para obtener un usuario por email y contraseña
        public async Task<Usuario> GetByCredentialsAsync(string email, string password)
        {
            Usuario usuario = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT \"UserId\", \"Name\", \"Email\", \"Password\", \"IsPremium\", \"fecha_registro\" " +
                               "FROM \"Usuario\" WHERE \"Email\" = @Email AND \"Password\" = @Password";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                UserId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Password = reader.GetString(3),
                                IsPremium = reader.GetBoolean(4),
                                Fecha_Registro = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task AddAsync(Usuario usuario)
{
    using (var connection = new NpgsqlConnection(_connectionString))
    {
        await connection.OpenAsync();
        
        // Si no se pasa una fecha desde el frontend, usamos la fecha actual.
        if (usuario.Fecha_Registro == DateTime.MinValue)
        {
            usuario.Fecha_Registro = DateTime.UtcNow;
        }

        string query = "INSERT INTO \"Usuario\" (\"Name\", \"Email\", \"Password\", \"IsPremium\", \"fecha_registro\") " +
                       "VALUES (@Name, @Email, @Password, @IsPremium, @Fecha_Registro) RETURNING \"UserId\"";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Name", usuario.Name);
            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@Password", usuario.Password);
            command.Parameters.AddWithValue("@IsPremium", usuario.IsPremium);
            command.Parameters.AddWithValue("@Fecha_Registro", usuario.Fecha_Registro);
            
            var result = await command.ExecuteScalarAsync();
            if (result == null)
            {
                throw new Exception("No se obtuvo el UserId generado.");
            }

            usuario.UserId = Convert.ToInt32(result);
            Console.WriteLine($"Usuario registrado con ID: {usuario.UserId}");
        }
    }
}


        public async Task UpdateAsync(Usuario usuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Actualización sin modificar "fecha_registro"
                string query = "UPDATE \"Usuario\" SET \"Name\" = @Name, \"Email\" = @Email, \"Password\" = @Password, \"IsPremium\" = @IsPremium WHERE \"UserId\" = @UserId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", usuario.UserId);
                    command.Parameters.AddWithValue("@Name", usuario.Name);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Password", usuario.Password);
                    command.Parameters.AddWithValue("@IsPremium", usuario.IsPremium);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM \"Usuario\" WHERE \"UserId\" = @Id";
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
