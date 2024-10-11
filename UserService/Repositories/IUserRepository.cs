using UserService.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUser(User user);
    Task<User> GetUserById(int id);
    Task<User> GetUserByName(string name, string surname);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(int id);

}