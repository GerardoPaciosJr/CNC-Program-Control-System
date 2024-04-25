using FluentValidation;
using System.Text.RegularExpressions;

namespace CNC_Program_Control_System
{
    public class DatabaseCredentialValidator : AbstractValidator<DatabaseCredentialModel>, IValidator<DatabaseCredentialModel>
    {
        public DatabaseCredentialValidator()
        {
            RuleFor(credential => credential.ServerHostName)
                .NotEmpty()
                .WithMessage("Please input Host Name.");

            RuleFor(credential => credential.DatabaseName)
                .NotEmpty()
                .WithMessage("Please input Database Name.");

            RuleFor(credential => credential.DatabaseUser)
                .NotEmpty()
                .WithMessage("Please input User Name.");

            RuleFor(credential => credential.DatabasePassword)
                .NotEmpty()
                .WithMessage("Please input Password.");
        }
    }
}
