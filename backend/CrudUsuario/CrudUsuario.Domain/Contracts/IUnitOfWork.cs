namespace CrudUsuario.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}