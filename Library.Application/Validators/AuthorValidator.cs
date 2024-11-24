using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.Application.DTOs;

namespace Library.Application.Validators
{
    public class AuthorValidator:AbstractValidator<AuthorDTO>
    {
        public AuthorValidator()
        {
            RuleFor(a=>a.Name).NotEmpty()
                .WithMessage("Author's name can't be empty");

            RuleFor(a => a.LastName).NotEmpty()
                .WithMessage("Author's lastname can't be empty");

            RuleFor(a => a.Country).NotEmpty()
                .WithMessage("Author's country can't be empty");

            RuleFor(a => a.BirthDay).NotEmpty().
                LessThan(DateTime.UtcNow.AddYears(-18))
                .WithMessage("Birthday must be valid");
        }

    }
}
