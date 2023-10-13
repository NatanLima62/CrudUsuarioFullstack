using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using CrudUsuario.Application.DTOs.Auth;
using CrudUsuario.Application.DTOs.Usuario;
using CrudUsuario.Application.Notifications;
using CrudUsuario.Domain.Contracts.Repositories;
using CrudUsuario.Domain.Entities;
using CrudUsuarios.Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace CrudUsuario.Application.Services;

public class AuthService : BaseServices, IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    public AuthService(IMapper mapper, INotificator notificator, IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher, IOptions<JwtSettings> jwtSettings, IJwtService jwtService) : base(mapper, notificator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(UsuarioLoginDto usuarioLoginDto)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(usuarioLoginDto.Email);
        if (usuario == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, usuarioLoginDto.Senha);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new TokenDto
        {
            Token = await GerarToken(usuario)
        };
    }

    private async Task<string> GerarToken(Usuario usuario)
    {
        var tokenHandle = new JwtSecurityTokenHandler();

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));

        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandle.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Emissor,
            Audience = _jwtSettings.ComumValidoEm,
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = key
        });

        return tokenHandle.WriteToken(token);
    }
}