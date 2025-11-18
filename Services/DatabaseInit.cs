using Microsoft.Data.Sqlite;
namespace journal2.Services
{
    public interface IDataBaseInitializer
    {
        Task DBInitAsync();
    }

    public class DataBaseInitializer : IDataBaseInitializer
    {
        private readonly string dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "textdata.db");

        public async Task DBInitAsync()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            await connection.OpenAsync();

            // Create files table
            var createFiles = connection.CreateCommand();
            createFiles.CommandText =
            """
            CREATE TABLE IF NOT EXISTS files(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                filename TEXT NOT NULL UNIQUE
            );
            """;
            await createFiles.ExecuteNonQueryAsync();

            // Create entries table
            var createEntries = connection.CreateCommand();
            createEntries.CommandText =
            """
            CREATE TABLE IF NOT EXISTS entries(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                file_id INTEGER NOT NULL,
                line_id INTEGER,
                text TEXT,
                wordset TEXT,
                timestamp TEXT,
                FOREIGN KEY(file_id) REFERENCES files(id)
            );
            """;
            await createEntries.ExecuteNonQueryAsync();
        }
    }
}
