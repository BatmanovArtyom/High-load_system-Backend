using UserService.Models;

namespace UserService.Mapping;

public interface IUserMapping
{
    User? MapToUserFromDbModel(UserDbModel userDbModel);
    UserDbModel MapToDbModelFromUser(User? user);
}