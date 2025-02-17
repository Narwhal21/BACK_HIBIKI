using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly string _connectionString;

        public AlbumRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Album>> GetAllAsync()
        {
            var albums = new List<Album>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"AlbumId\", \"ArtistId\", \"Name\", \"ReleaseDate\", \"Image\" FROM \"Album\"";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var album = new Album
                            {
                                AlbumId = reader.GetInt32(0),
                                ArtistId = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                ReleaseDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                Image = reader.GetString(4)
                            };

                            albums.Add(album);
                        }
                    }
                }
            }

            return albums;
        }

        public async Task<Album> GetByIdAsync(int id)
        {
            Album album = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT \"AlbumId\", \"ArtistId\", \"Name\", \"ReleaseDate\", \"Image\" FROM \"Album\" WHERE \"AlbumId\" = @Id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            album = new Album
                            {
                                AlbumId = reader.GetInt32(0),
                                ArtistId = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                ReleaseDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                Image = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return album;
        }

        public async Task AddAsync(Album album)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO \"Album\" (\"ArtistId\", \"Name\", \"ReleaseDate\", \"Image\") VALUES (@ArtistId, @Name, @ReleaseDate, @Image)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistId", album.ArtistId);
                    command.Parameters.AddWithValue("@Name", album.Name);
                    command.Parameters.AddWithValue("@ReleaseDate", album.ReleaseDate.HasValue ? (object)album.ReleaseDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Image", album.Image);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Album album)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE \"Album\" SET \"ArtistId\" = @ArtistId, \"Name\" = @Name, \"ReleaseDate\" = @ReleaseDate, \"Image\" = @Image WHERE \"AlbumId\" = @AlbumId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AlbumId", album.AlbumId);
                    command.Parameters.AddWithValue("@ArtistId", album.ArtistId);
                    command.Parameters.AddWithValue("@Name", album.Name);
                    command.Parameters.AddWithValue("@ReleaseDate", album.ReleaseDate.HasValue ? (object)album.ReleaseDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Image", album.Image);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM \"Album\" WHERE \"AlbumId\" = @Id";
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
                    INSERT INTO ""Album"" (""ArtistId"", ""Name"", ""ReleaseDate"", ""Image"")
                    VALUES 
                    (@ArtistId1, @Name1, @ReleaseDate1, @Image1),
                    (@ArtistId2, @Name2, @ReleaseDate2, @Image2)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistId1", 1);
                    command.Parameters.AddWithValue("@Name1", "Album 1");
                    command.Parameters.AddWithValue("@ReleaseDate1", new DateTime(2020, 1, 1));
                    command.Parameters.AddWithValue("@Image1", "image1.jpg");

                    command.Parameters.AddWithValue("@ArtistId2", 2);
                    command.Parameters.AddWithValue("@Name2", "Album 2");
                    command.Parameters.AddWithValue("@ReleaseDate2", new DateTime(2021, 2, 2));
                    command.Parameters.AddWithValue("@Image2", "image2.jpg");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
