using CrudUsuario.Application.DTOs.Usuario;

namespace CrudUsuario.Application.Contracts;

public interface IUsuarioService
{
    Task<UsuarioDto?> Adicionar(AdicionarUsuarioDto usuarioDto);
    Task<UsuarioDto?> ObterPorId(int id);
    Task<List<UsuarioDto>?> ObterTodos();
    Task<UsuarioDto?> Atualizar(int id, AtualizarUsuarioDto usuarioDto);
    Task Remover(int id);
}