using Microsoft.Data.Sqlite;
using Microsoft.Maui.Storage;

namespace journal2.Services
{
    public interface IDatabaseService
    {
        Task<int> GetOrCreateFileId(string filename);
        Task InsertKeywordEntry(int fileId, string keyword, string fullText);
         Task<List<string>> FindFilesByKeyword(string keyword);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "textdata.db");

        private SqliteConnection GetConnection()
            => new SqliteConnection($"Data Source={_dbPath}");

        public async Task<int> GetOrCreateFileId(string filename)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            // check if exists
            var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = "SELECT id FROM files WHERE filename = $name";
            checkCmd.Parameters.AddWithValue("$name", filename);

            var result = await checkCmd.ExecuteScalarAsync();
            if (result != null)
                return Convert.ToInt32(result);

            // create new
            var insertCmd = conn.CreateCommand();
            insertCmd.CommandText =
            """
            INSERT INTO files (filename)
            VALUES ($name);
            SELECT last_insert_rowid();
            """;
            insertCmd.Parameters.AddWithValue("$name", filename);

            var newId = await insertCmd.ExecuteScalarAsync();
            return Convert.ToInt32(newId);
        }

        public async Task InsertKeywordEntry(int fileId, string keyword, string fullText)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            INSERT INTO entries (file_id, text, wordset, timestamp)
            VALUES ($file, $text, $keyword, $time)
            """;
            cmd.Parameters.AddWithValue("$file", fileId);
            cmd.Parameters.AddWithValue("$text", fullText);
            cmd.Parameters.AddWithValue("$keyword", keyword);
            cmd.Parameters.AddWithValue("$time", DateTime.Now.ToString("o"));

            await cmd.ExecuteNonQueryAsync();
        }
    
        public async Task<List<string>> FindFilesByKeyword(string keyword)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            // This finds all file_ids where entries contain the keyword
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            """
            SELECT DISTINCT f.filename
            FROM entries e
            JOIN files f ON e.file_id = f.id
            WHERE e.wordset = $kw;
            """;

            cmd.Parameters.AddWithValue("$kw", keyword.ToLower());

            var result = new List<string>();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(reader.GetString(0)); // filename
                }
            }

            return result;
        }
    }
}
