using CrudUsuario.Application.DTOs.Auth;

namespace CrudUsuario.Application.DTOs.Usuario;

public interface IAuthService
{
    Task<TokenDto?> Login(UsuarioLoginDto usuarioLoginDto);
}