using UserService.Models;

namespace UserService.Domain_Service
{
    public interface IUserServiceDomain
    {
        Task<bool> CreateUser(User? user, CancellationToken cancellationToken);
        Task<bool> DeleteUser(int userId, CancellationToken cancellationToken);
        Task<bool> UpdateUser(User user, CancellationToken cancellationToken);
        Task<User> GetUser(int userId, CancellationToken cancellationToken);
        Task<User> GetUserByName(string name, string surname, CancellationToken cancellationToken);
    }
}
