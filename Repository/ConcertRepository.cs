using Npgsql;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public class ConcertRepository : IConcertRepository
    {
        private readonly string _connectionString;

        public ConcertRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Concert>> GetAllAsync()
        {
            var concerts = new List<Concert>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT \"ConcertId\", \"ArtistId\", \"Venue\", \"Date\", \"Description\", \"Image\" FROM \"Concert\"";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        concerts.Add(MapConcert(reader));
                    }
                }
            }

            return concerts;
        }

        public async Task<Concert> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT \"ConcertId\", \"ArtistId\", \"Venue\", \"Date\", \"Description\", \"Image\" FROM \"Concert\" WHERE \"ConcertId\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync() ? MapConcert(reader) : null;
                    }
                }
            }
        }

        public async Task AddAsync(Concert concert)
        {
            if (concert == null) throw new ArgumentNullException(nameof(concert));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO ""Concert"" (""ArtistId"", ""Venue"", ""Date"", ""Description"", ""Image"")
                    VALUES (@ArtistId, @Venue, @Date, @Description, @Image)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    SetConcertParameters(command, concert);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Concert concert)
        {
            if (concert == null) throw new ArgumentNullException(nameof(concert));

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE ""Concert""
                    SET ""ArtistId"" = @ArtistId,
                        ""Venue"" = @Venue,
                        ""Date"" = @Date,
                        ""Description"" = @Description,
                        ""Image"" = @Image
                    WHERE ""ConcertId"" = @ConcertId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    SetConcertParameters(command, concert);
                    command.Parameters.AddWithValue("@ConcertId", concert.ConcertId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM \"Concert\" WHERE \"ConcertId\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task InitializeDataAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO ""Concert"" (""ArtistId"", ""Venue"", ""Date"", ""Description"", ""Image"")
                    VALUES 
                    (@ArtistId1, @Venue1, @Date1, @Description1, @Image1),
                    (@ArtistId2, @Venue2, @Date2, @Description2, @Image2)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistId1", 1);
                    command.Parameters.AddWithValue("@Venue1", "Teatro Nacional");
                    command.Parameters.AddWithValue("@Date1", new DateTime(2025, 7, 10));
                    command.Parameters.AddWithValue("@Description1", "Concierto acústico íntimo.");
                    command.Parameters.AddWithValue("@Image1", "https://example.com/concierto1.jpg");

                    command.Parameters.AddWithValue("@ArtistId2", 2);
                    command.Parameters.AddWithValue("@Venue2", "Estadio Central");
                    command.Parameters.AddWithValue("@Date2", new DateTime(2025, 8, 20));
                    command.Parameters.AddWithValue("@Description2", "Gran show con efectos especiales.");
                    command.Parameters.AddWithValue("@Image2", "https://example.com/concierto2.jpg");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Mapea un registro a un objeto Concert
        private Concert MapConcert(NpgsqlDataReader reader)
        {
            return new Concert
            {
                ConcertId = reader.GetInt32(0),
                ArtistId = reader.GetInt32(1),
                Venue = reader.GetString(2),
                Date = reader.GetDateTime(3),
                Description = reader.GetString(4),
                Image = reader.GetString(5)
            };
        }

        // Configura los parámetros para un comando SQL de Concert
        private void SetConcertParameters(NpgsqlCommand command, Concert concert)
        {
            command.Parameters.AddWithValue("@ArtistId", concert.ArtistId);
            command.Parameters.AddWithValue("@Venue", concert.Venue);
            command.Parameters.AddWithValue("@Date", concert.Date);
            command.Parameters.AddWithValue("@Description", concert.Description);
            command.Parameters.AddWithValue("@Image", concert.Image);
        }
    }
}
