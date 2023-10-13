using AutoMapper;
using CrudUsuario.Application.Contracts;
using CrudUsuario.Application.DTOs.Usuario;
using CrudUsuario.Application.Notifications;
using CrudUsuario.Domain.Contracts.Repositories;
using CrudUsuario.Domain.Entities;
using CrudUsuarios.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CrudUsuario.Application.Services;

public class UsuarioService : BaseServices, IUsuarioService
{
    private readonly IFileService _fileService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(IMapper mapper, INotificator notificator, IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher, IFileService fileService) : base(mapper, notificator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _fileService = fileService;
    }

    public async Task<UsuarioDto?> Adicionar(AdicionarUsuarioDto usuarioDto)
    {
        var usuario = Mapper.Map<Usuario>(usuarioDto);
        if (!await Validar(usuario))
        {
            return null;
        }

        if (usuarioDto.Foto is { Length: > 0 })
        {
            usuario.Foto = await _fileService.Upload(usuarioDto.Foto, EUploadPath.FotoUsuarios);
        }


        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);
        _usuarioRepository.Adicionar(usuario);
        if (await _usuarioRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<UsuarioDto>(usuario);
        }

        Notificator.Handle("Não foi possível cadastrar o usuário");
        return null;
    }

    public async Task<UsuarioDto?> ObterPorId(int id)
    {
        var usuario = await _usuarioRepository.ObterPorId(id);
        if (usuario != null)
            return Mapper.Map<UsuarioDto>(usuario);

        Notificator.HandleNotFoundResourse();
        return null;
    }

    public async Task<List<UsuarioDto>?> ObterTodos()
    {
        var usuarios = await _usuarioRepository.ObterTodos();
        if (usuarios != null)
            return Mapper.Map<List<UsuarioDto>>(usuarios);

        Notificator.Handle("Não existe usuário cadastrado");
        return null;
    }

    public async Task<UsuarioDto?> Atualizar(int id, AtualizarUsuarioDto usuarioDto)
    {
        if (id != usuarioDto.Id)
        {
            Notificator.Handle("Os ids não conferem!");
            return null;
        }

        var usuario = await _usuarioRepository.ObterPorId(id);
        if (usuario == null)
        {
            Notificator.HandleNotFoundResourse();
            return null;
        }
        
        Mapper.Map(usuarioDto, usuario);
        if (!await Validar(usuario))
        {
            return null;
        }
        
        if (usuarioDto.Fotos is { Length: > 0 } && !await ManterFoto(usuarioDto.Fotos, usuario))
        {
            usuario.Foto = await _fileService.Upload(usuarioDto.Fotos, EUploadPath.FotoUsuarios);
        }
        
        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);
        _usuarioRepository.Atualizar(usuario);
        if (await _usuarioRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<UsuarioDto>(usuario);
        }

        Notificator.Handle("Não foi possível alterar o usuário");
        return null;
    }

    public async Task Remover(int id)
    {
        var usuario = await _usuarioRepository.ObterPorId(id);
        if (usuario == null)
        {
            Notificator.HandleNotFoundResourse();
            return;
        }

        _usuarioRepository.Remover(usuario);
        if (!await _usuarioRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível remover o usuário");
        }
    }

    private async Task<bool> Validar(Usuario usuario)
    {
        if (!usuario.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var usuarioExistente = await _usuarioRepository.FirstOrDefault(c =>
            c.Id != usuario.Id && (c.Cpf == usuario.Cpf || c.Email == usuario.Email));
        if (usuarioExistente != null)
        {
            Notificator.Handle("Já existe um usuário cadastrado com essas idenficações");
        }

        return !Notificator.HasNotification;
    }
    
    private async Task<bool> ManterFoto(IFormFile foto, Usuario usuario)
    {
        if (!string.IsNullOrWhiteSpace(usuario.Foto) && !_fileService.Apagar(new Uri(usuario.Foto)))
        {
            Notificator.Handle("Não foi possível remover a foto anterior.");
            return false;
        }

        usuario.Foto = await _fileService.Upload(foto, EUploadPath.FotoUsuarios);
        return true;
    }
}