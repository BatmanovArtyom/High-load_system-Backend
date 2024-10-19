using UserService.Models;

namespace UserService.Services;

public class UserMapping
{
    public static User? MappingToUserFromDbModel(UserDbModel userDbModel)
    {
        
        return new User
        {
            Id = userDbModel.Id,
            Name = userDbModel.Name,
            Surname = userDbModel.Surname,
            Age = userDbModel.Age,
            Password = userDbModel.Password,
            Login = userDbModel.Login,
            
        };
    }

    public static UserDbModel MappingToDbModelFromUser(User user)
    {
        return new UserDbModel
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age,
            Password = user.Password,
            Login = user.Login,
        };
    }
}