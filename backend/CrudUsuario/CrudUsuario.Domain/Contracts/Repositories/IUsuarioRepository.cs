using CrudUsuario.Domain.Entities;

namespace CrudUsuario.Domain.Contracts.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    void Adicionar(Usuario usuario);
    Task<Usuario?> ObterPorId(int id);
    Task<Usuario?> ObterPorEmail(string email);
    Task<List<Usuario>?> ObterTodos();
    void Atualizar(Usuario usuario);
    void Remover(Usuario usuario);
}