using System.Linq.Expressions;
using CrudUsuario.Domain.Entities;

namespace CrudUsuario.Domain.Contracts.Repositories;

public interface IRepository<T> : IDisposable where T : Entity, IAggregateRoot
{
    public IUnitOfWork UnitOfWork { get; }
    Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression);
}