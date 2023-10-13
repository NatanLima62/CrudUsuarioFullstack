using CrudUsuario.Domain.Contracts;
using CrudUsuario.Domain.Validators;
using FluentValidation.Results;

namespace CrudUsuario.Domain.Entities;

public class Usuario : Entity, IAggregateRoot
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string? Foto { get; set; }

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new UsuarioValidator().Validate(this);
        return validationResult.IsValid;
    }
}