using FluentValidation;
using UserService.Models;

namespace UserService.Validators;

public class UpdateUserValidator : AbstractValidator<User>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required");
        RuleFor(x => x.Age).GreaterThan(0).WithMessage("Age cant be 0");
    }
    
}