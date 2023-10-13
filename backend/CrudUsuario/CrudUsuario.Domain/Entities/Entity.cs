using CrudUsuario.Domain.Contracts;
using FluentValidation.Results;

namespace CrudUsuario.Domain.Entities
{
    public abstract class Entity : IEntity, ITracking
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }

        public virtual bool Validar(out ValidationResult validationResult)
        {
            validationResult = new ValidationResult();
            return validationResult.IsValid;
        }
    }
}