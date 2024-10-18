using Npgsql;

namespace UserService.Database;

using System.Data;
using Dapper;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        // Получаем все SQL-скрипты
        var sqlFiles = Directory.GetFiles("Database", "*.sql");

        foreach (var sqlFile in sqlFiles)
        {
            var sql = await File.ReadAllTextAsync(sqlFile);
            await using var command = new NpgsqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }
}
