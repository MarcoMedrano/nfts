using FluentValidation;

namespace NftSample.Business;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Id).NotEmpty();
        RuleFor(u => u.Nickname).NotEmpty();
    }
}
