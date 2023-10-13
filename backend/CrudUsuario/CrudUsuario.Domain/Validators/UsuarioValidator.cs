using CrudUsuario.Domain.Entities;
using FluentValidation;

namespace CrudUsuario.Domain.Validators;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nome)
            .NotEmpty()
            .WithMessage("Nome não pode ser vazio")
            .Length(3, 150)
            .WithMessage("Nome deve ter no mínimo 3 e no máximo 150 caracteres");

        RuleFor(u => u.Senha)
            .NotEmpty()
            .WithMessage("Senha não pode ser vazio")
            .Length(8, 20)
            .WithMessage("Senha deve ter no mínimo 8 e no máximo 20 caracteres");

        RuleFor(u => u.Email)
            .EmailAddress();

        RuleFor(u => u.Cpf)
            .NotEmpty()
            .WithMessage("Cpf não pode ser vazio")
            .Length(11, 14)
            .WithMessage("Cpf deve ter no mínimo 11 e no máximo 14 caracteres");
    }
}