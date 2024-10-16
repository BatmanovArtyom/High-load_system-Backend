using UserService.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUser(User user, CancellationToken cancellationToken);
    Task<User> GetUserById(int id, CancellationToken cancellationToken);
    Task<User> GetUserByName(string name, string surname, CancellationToken cancellationToken);
    Task<bool> UpdateUser(User user, CancellationToken cancellationToken);
    Task<bool> DeleteUser(int id, CancellationToken cancellationToken);

}