using FluentValidation;
using Grpc.Core;
using UserService.Models;
using UserService.Repositories;
using UserService.Validators;

namespace UserService.Domain_Service;

public class UserServiceDomain : IUserServiceDomain
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User?> _createUserValidator;
    private readonly IValidator<User> _updateUserValidator;
    private readonly ILogger<UserServiceDomain> _logger;

    public UserServiceDomain(IUserRepository userRepository,IValidator<User?> createUserValidator,
        IValidator<User> updateUserValidator,
        ILogger<UserServiceDomain> logger )
    {
        _userRepository = userRepository; 
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
        _logger = logger;
    }

    public async Task<bool> CreateUser(User? user, CancellationToken cancellationToken)
    {
        var validationResult = await _createUserValidator.ValidateAsync(user, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }
        
        var existingUser = await _userRepository.GetUserById(user.Id, cancellationToken);
        if (existingUser != null)
        {
            _logger.LogWarning("User creation failed. Login {Login} already exists.", user.Login);
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Login already exists."));
        }
        
        var isCreated = await _userRepository.CreateUser(user, cancellationToken);
        if (isCreated)
        {
            _logger.LogInformation("User {Login} create", user.Login);
        }
        else
        {
            _logger.LogError("Error with creating user {Login}", user.Login);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid login or password" ));
        }
        
        return isCreated;
    }

    public async Task<bool> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var isDeleted = await _userRepository.DeleteUser(userId, cancellationToken);
        if (isDeleted)
        {
            _logger.LogInformation("User with ID {Id} deleted successfully.", userId);
        }
        else
        {
            _logger.LogError("Failed to delete user with ID {Id}.", userId);
        }
        
        return isDeleted;
    }

    public async Task<bool> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(user, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validation failed for user update: {Errors}", errors);
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }
        var existingUser = await _userRepository.GetUserById(user.Id, cancellationToken);
            
        if (existingUser == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {user.Id} not found."));

        existingUser.Name = user.Name;
        existingUser.Surname = user.Surname;
        existingUser.Age = user.Age;

        var isUpdated = await _userRepository.UpdateUser(existingUser, cancellationToken);
        if (isUpdated)
        {
            _logger.LogInformation("User id {Id} update", user.Id);
        }
        else
        {
            _logger.LogError("Error update user {Id}", user.Id);
        }
        
        return isUpdated;
    }

    public async Task<User> GetUser(int userId, CancellationToken cancellationToken)
    {
        if (userId < 0 )
        {
            _logger.LogError("Error get user {Id}", userId);
            throw new RpcException(new Status(StatusCode.NotFound, $"Id less 0"));
        }
        var user = await _userRepository.GetUserById(userId, cancellationToken);
        if (user == null)
        {
            _logger.LogError("Error get user {Id}", userId);
            throw new RpcException(new Status(StatusCode.NotFound, $"User not found."));
        }

        return user;
    }

    public async Task<User> GetUserByName(string name, string surname, CancellationToken cancellationToken)
    {
        if (name == null )
        {
            _logger.LogError("Error get user {Name}", name);
            throw new RpcException(new Status(StatusCode.NotFound, $"Not name"));
        }
        var user = await _userRepository.GetUserByName(name, surname, cancellationToken);

        if (user == null)
        {
            _logger.LogError("Error get user {Name}", name);
            throw new RpcException(new Status(StatusCode.NotFound,
                $"User with Name {name} and Surname {surname} not found."));
        }
        
        return user; 
    }
    
}