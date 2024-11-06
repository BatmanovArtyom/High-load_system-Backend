namespace UserService.Mapping;

public abstract class UserMappingFactory
{
    public static IUserMapping CreateUserMapping() => new UserMapping();
}