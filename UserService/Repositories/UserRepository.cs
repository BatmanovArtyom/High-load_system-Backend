using System.Data;
using Dapper;
using UserService.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> CreateUser(User user, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Login", user.Login);
        parameters.Add("@Password", user.Password);
        parameters.Add("@Name", user.Name);
        parameters.Add("@Surname", user.Surname);
        parameters.Add("@Age", user.Age);
        
        
        var command = new CommandDefinition("SELECT create_user (@Login, @Password, @Name, @Surname, @Age)", parameters, cancellationToken: cancellationToken);
        var userId = await _dbConnection.QuerySingleAsync<int>(command);

        return userId != 0;
    }
    
    public async Task<User> GetUserById(int id, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        
        var command = new CommandDefinition("SELECT * FROM get_user_by_id(@Id)", parameters, cancellationToken: cancellationToken);
        var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(command);
        
        return user;
    }

    public async Task<User> GetUserByName(string name, string surname, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", name);
        parameters.Add("@Surname", surname);
        
        var command = new CommandDefinition("SELECT * FROM get_user_by_name(@Name)", parameters, cancellationToken: cancellationToken);
        var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(command);
        
        return user;
    }
    
    public async Task<bool> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", user.Id);
        parameters.Add("@Name", user.Name);
        parameters.Add("@Surname", user.Surname);
        parameters.Add("@Age", user.Age);
        
        
        var command = new CommandDefinition("SELECT update_user(@Id, @Name, @Surname, @Age)", parameters, cancellationToken: cancellationToken);
        var success = await _dbConnection.ExecuteScalarAsync<bool>(command);

        return success;
    }
    
    public async Task<bool> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        
        var command = new CommandDefinition("SELECT delete_user(@Id)", parameters, cancellationToken: cancellationToken);
        var success = await _dbConnection.ExecuteScalarAsync<bool>(command);

        return success;
    }


}