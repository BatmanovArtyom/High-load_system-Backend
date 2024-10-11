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

    public async Task<User> CreateUser(User user)
    {
        var userId = await _dbConnection.ExecuteScalarAsync<int>(
            "SELECT create_user (@Login, @Password, @Name, @Surname, @Age)", new
            {
                user.Login,
                user.Password,
                user.Name,
                user.Surname,
                user.Age

            });
        
        //валидировать создание, если получилось, то вкрнем true
        // if ()
        // {
        //     
        // }
        return user;
    }

    public async Task<User> GetUserById(int id)
    {
        //по идее надо как-то проверять айдишник,  но я хз
        var user = await _dbConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM get_user_by_id(@Id)", new { id = id });
        
        return user;
    }

    public async Task<User> GetUserByName(string name, string surname)
    {
        var user = await _dbConnection.QuerySingleOrDefaultAsync<User>( "SELECT * FROM get_user_by_name(@Name, @Surname)", new { Name = name, Surname = surname });
        
        return user;
    }
    
    public async Task<bool> UpdateUser(User user)
    {
        var success = await _dbConnection.ExecuteScalarAsync<bool>(
            "SELECT update_user(@Id, @Name, @Surname, @Age)",
            new
            {
                user.Id,
                user.Name,
                user.Surname,
                user.Age
            });

        return success;
    }
    
    public async Task<bool> DeleteUser(int id)
    {
        var success = await _dbConnection.ExecuteScalarAsync<bool>(
            "SELECT delete_user(@Id)", new { Id = id });

        return success;
    }


}