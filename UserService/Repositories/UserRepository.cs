using Dapper;
using Npgsql;
using UserService.Models;
using UserService.Services;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connection;

    public UserRepository(string connection)
    {
       _connection = connection;
    }

    public async Task<bool> CreateUser(User user, CancellationToken cancellationToken)
    {
        var userDbModel = UserMapping.MapToDbModelFromUser(user);
        
        var parameters = new DynamicParameters();
        parameters.Add("@Login", userDbModel.Login);
        parameters.Add("@Password", userDbModel.Password);
        parameters.Add("@Name", userDbModel.Name);
        parameters.Add("@Surname", userDbModel.Surname);
        parameters.Add("@Age", userDbModel.Age);
        
        await using var connection = new NpgsqlConnection(_connection);
        var command = new CommandDefinition("SELECT create_user (@Login, @Password, @Name, @Surname, @Age)", parameters, cancellationToken: cancellationToken);
        var userId = await connection.QuerySingleAsync<int>(command);

        return userId != 0;
    }
    
    public async Task<User> GetUserById(int id, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        
        await using var connection = new NpgsqlConnection(_connection);
        var command = new CommandDefinition("SELECT * FROM get_user_by_id(@Id)", parameters, cancellationToken: cancellationToken);
        var userDbModel = await connection.QuerySingleOrDefaultAsync<UserDbModel>(command);
        
        return userDbModel == null ? null : UserMapping.MapToUserFromDbModel(userDbModel);
    }

    public async Task<User> GetUserByName(string name, string surname, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", name);
        parameters.Add("@Surname", surname);
        
        await using var connection = new NpgsqlConnection(_connection);
        var command = new CommandDefinition("SELECT * FROM get_user_by_name(@Name)", parameters, cancellationToken: cancellationToken);
        var userDbModel = await connection.QuerySingleOrDefaultAsync<UserDbModel>(command);
        
        var user = UserMapping.MapToUserFromDbModel(userDbModel);
        return user;
    }
    
    public async Task<bool> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var userDbModel = UserMapping.MapToDbModelFromUser(user);
        
        var parameters = new DynamicParameters();
        parameters.Add("@Id", userDbModel.Id);
        parameters.Add("@Name", userDbModel.Name);
        parameters.Add("@Surname", userDbModel.Surname);
        parameters.Add("@Age", userDbModel.Age);
        
        await using var connection = new NpgsqlConnection(_connection);
        var command = new CommandDefinition("SELECT update_user(@Id, @Name, @Surname, @Age)", parameters, cancellationToken: cancellationToken);
        var success = await connection.ExecuteScalarAsync<bool>(command);

        return success;
    }
    
    public async Task<bool> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        
        
        await using var connection = new NpgsqlConnection(_connection);
        var command = new CommandDefinition("SELECT delete_user(@Id)", parameters, cancellationToken: cancellationToken);
        var success = await connection.ExecuteScalarAsync<bool>(command);

        return success;
    }
}