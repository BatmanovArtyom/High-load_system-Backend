namespace UserService.Database;

using System.Data;
using Dapper;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(IDbConnection dbConnection)
    {
        // Чтение и выполнение SQL-скриптов из файлов
        var createTableQuery = await File.ReadAllTextAsync("Database/create_table_user.sql");
        await dbConnection.ExecuteAsync(createTableQuery);

        var createFunctionQueries = new[]
        {
            await File.ReadAllTextAsync("Database/create_function_create_user.sql"),
            await File.ReadAllTextAsync("Database/create_function_get_user_by_id.sql"),
            await File.ReadAllTextAsync("Database/create_function_get_user_by_name.sql"),
            await File.ReadAllTextAsync("Database/create_function_update_user.sql"),
            await File.ReadAllTextAsync("Database/create_function_delete_user.sql")
        };

        foreach (var query in createFunctionQueries)
        {
            await dbConnection.ExecuteAsync(query);
        }

        // Применение индексов
        // var createIndexesQuery = await File.ReadAllTextAsync("Database/create_indexes.sql");
        // await dbConnection.ExecuteAsync(createIndexesQuery);
    }
}
