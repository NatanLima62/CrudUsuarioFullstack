using System.Linq.Expressions;
using CrudUsuario.Domain.Contracts;
using CrudUsuario.Domain.Contracts.Repositories;
using CrudUsuario.Domain.Entities;
using CrudUsuario.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuario.Infra.Repositories;

public abstract class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
{
    private bool _isDisposed;
    private readonly DbSet<T> _dbSet;
    protected readonly BaseApplicationDbContext Context;
    
    protected Repository(BaseApplicationDbContext context)
    {
        Context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AsNoTrackingWithIdentityResolution().Where(expression).FirstOrDefaultAsync();
    }

    public IUnitOfWork UnitOfWork => Context;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            // free managed resources
            Context.Dispose();
        }

        _isDisposed = true;
    }
    
    ~Repository()
    {
        Dispose(false);
    }
}