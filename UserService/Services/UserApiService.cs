using Dapper;
using Grpc.Core;
using Npgsql;
using UserService;
using UserService.Repositories;
using UserService.Services;


namespace UserService.Services;

public class UserApiService : UserService.UserServiceBase
{ 
    private readonly UserRepository _userRepository;

    public UserApiService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
    {
        return base.GetUser(request, context);
    }

    public override Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return base.UpdateUser(request, context);
    }

    public override Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return base.DeleteUser(request, context);
    }

    public override Task<UserReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        return base.GetUserByName(request, context);
    }

    public override Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return base.CreateUser(request, context);
    }
}