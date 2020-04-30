using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.ViewModel.Validation
{
    public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.Username).Length(3, 250);
            RuleFor(user => user.Password).Length(3, 250);
            RuleFor(user => user.PasswordConfirmation).Equal(user => user.Password);
        }
    }
}
