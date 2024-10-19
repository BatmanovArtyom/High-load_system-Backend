using Grpc.Core;
using UserService.Models;
using UserService.Repositories;
using UserService.Validators;


namespace UserService.Services;

public class UserApiService(IUserRepository userRepository, ILogger<UserApiService> logger)
    : UserService.UserServiceBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UserApiService> _logger = logger;

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Try to create user {Login}", request.Login);
        var validator = new CreateUserValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }
        var existingUser = await _userRepository.GetUserById(request.Id, context.CancellationToken);
        if (existingUser != null)
        {
            _logger.LogWarning("User creation failed. Login {Login} already exists.", request.Login);
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Login already exists."));
        }
        
        var user = new User
        {
            Id = request.Id,
            Login = request.Login,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname,
            Age = request.Age
        };

        var isCreated = await userRepository.CreateUser(user, context.CancellationToken);
        if (isCreated)
        {
            _logger.LogInformation("User {Login} create", request.Login);
        }
        else
        {
            _logger.LogError("Error with creating user {Login}", request.Login);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid login or password" ));
        }

        return new CreateUserResponse { Success = isCreated };
    }

    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var isDeleted = await _userRepository.DeleteUser(request.Id, context.CancellationToken);
        if (isDeleted)
        {
            _logger.LogInformation("User {Login} delete", request.Login);
        }
        else
        {
            _logger.LogError("Error with delete user {Login}", request.Login);
        }
    
        return new DeleteUserResponse { Success = isDeleted };
    }
    public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var validator = new UpdateUserValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validation failed for user update: {Errors}", errors);
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }
        var user = await _userRepository.GetUserById(request.Id, context.CancellationToken);
            
        if (user == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found."));

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Age = request.Age;

        var isUpdated = await _userRepository.UpdateUser(user, context.CancellationToken);
        if (isUpdated)
        {
            _logger.LogInformation("User id {Id} update", request.Id);
        }
        else
        {
            _logger.LogError("Error update user {Id}", request.Id);
        }

        return new UpdateUserResponse { Success = isUpdated };
    }
    public override async Task<UserReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        if (request.Name == null )
        {
            _logger.LogError("Error get user {Name}", request.Name);
            throw new RpcException(new Status(StatusCode.NotFound, $"Not name"));
        }
        var user = await _userRepository.GetUserByName(request.Name, request.Surname, context.CancellationToken);

        if (user == null)
        {
            _logger.LogError("Error get user {Name}", request.Name);
            throw new RpcException(new Status(StatusCode.NotFound,
                $"User with Name {request.Name} and Surname {request.Surname} not found."));
        }

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
        if (request.Id < 0 )
        {
            _logger.LogError("Error get user {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.NotFound, $"Id less 0"));
        }
        var user = await _userRepository.GetUserById(request.Id, context.CancellationToken);
        if (user == null)
        {
            _logger.LogError("Error get user {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.NotFound, $"User not found."));
        }

        return new UserReply
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }
}