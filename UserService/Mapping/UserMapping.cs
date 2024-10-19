using UserService.Models;

namespace UserService.Mapping;

public class UserMapping : IUserMapping
{
    public User? MapToUserFromDbModel(UserDbModel userDbModel)
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

    public  UserDbModel MapToDbModelFromUser(User user)
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