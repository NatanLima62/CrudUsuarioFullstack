using Microsoft.EntityFrameworkCore;

namespace CrudUsuario.Infra.Context;

public sealed class ApplicationDbContext : BaseApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}