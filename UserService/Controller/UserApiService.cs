using Grpc.Core;
using UserService.Models;
using UserService.Validators;
using UserService.Domain_Service;

namespace UserService.Controller;

public class UserApiService : UserService.UserServiceBase
{
    private readonly UserServiceDomain _userService;
    private readonly ILogger<UserApiService> _logger;
    
    public UserApiService(UserServiceDomain userServiceDomain, ILogger<UserApiService> logger)
    {
        _userService = userServiceDomain;
        _logger = logger;
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Try to create user {Login}", request.Login);
        
        var user = new User
        {
            Id = request.Id,
            Login = request.Login,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname,
            Age = request.Age
        };
        
        var isCreated = await _userService.CreateUser(user, context.CancellationToken);
        return new CreateUserResponse { Success = isCreated };
    }

    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var isDeleted = await _userService.DeleteUser(request.Id, context.CancellationToken);
        return new DeleteUserResponse { Success = isDeleted };
    }
    public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var user = new User
        {
            Id = request.Id,
            Age = request.Age,
            Name = request.Name,
            Surname = request.Surname
        };
        
        var isUpdated = await _userService.UpdateUser(user, context.CancellationToken);
        return new UpdateUserResponse { Success = isUpdated };
    }
    public override async Task<UserReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        var user = await _userService.GetUserByName(request.Name,request.Surname, context.CancellationToken);

        return new UserReply
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }

    public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var user = await _userService.GetUser(request.Id, context.CancellationToken);

        var userReply = new UserReply
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
        return userReply;
    }
}