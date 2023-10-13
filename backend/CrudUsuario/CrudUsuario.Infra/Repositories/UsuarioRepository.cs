using CrudUsuario.Domain.Contracts.Repositories;
using CrudUsuario.Domain.Entities;
using CrudUsuario.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuario.Infra.Repositories;

public class UsuarioRepository : Repository<Usuario>,IUsuarioRepository
{
    public UsuarioRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Usuario usuario)
    {
        Context.Add(usuario);
    }

    public async Task<Usuario?> ObterPorId(int id)
    {
        return await Context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Usuario?> ObterPorEmail(string email)
    {
        return await Context.Usuarios.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<List<Usuario>?> ObterTodos()
    {
        return await Context.Usuarios.AsNoTracking().ToListAsync();
    }

    public void Atualizar(Usuario usuario)
    {
        Context.Update(usuario);
    }

    public void Remover(Usuario usuario)
    {
        Context.Remove(usuario);
    }
}