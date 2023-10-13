using System.Linq.Expressions;
using CrudUsuario.Application.Contracts;
using CrudUsuario.Application.DTOs.Usuario;
using CrudUsuario.Application.Services;
using CrudUsuario.Application.Test.Fixtures;
using CrudUsuario.Domain.Contracts.Repositories;
using CrudUsuario.Domain.Entities;
using CrudUsuarios.Core.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CrudUsuario.Application.Test.ServiceTest;

public class UsuarioServiceTeste : BaseServiceTest, IClassFixture<ServicesFixture>
{
    private readonly UsuarioService _usuarioService;

    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();

    public UsuarioServiceTeste(ServicesFixture fixture)
    {
        _usuarioService = new UsuarioService(fixture.Mapper, NotificatorMock.Object, _usuarioRepositoryMock.Object,
            _passwordHasherMock.Object, _fileServiceMock.Object);
    }

    #region Adicionar

    [Fact]
    public async Task Adicionar_UsuarioInvalido_HandleErros()
    {
        SetupMocks();
        
        var dto = new AdicionarUsuarioDto
        {
            Cpf = "",
            Email = "",
            Nome = "",
            Senha = ""
        };

        var usuario = await _usuarioService.Adicionar(dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            
            _usuarioRepositoryMock.Verify(c => c.Adicionar(It.IsAny<Usuario>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Never);
        }
    }
    
    [Fact]
    public async Task Adicionar_UsuarioExistente_Handle()
    {
        SetupMocks(true);
        
        var dto = new AdicionarUsuarioDto
        {
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };

        var usuario = await _usuarioService.Adicionar(dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            
            _usuarioRepositoryMock.Verify(c => c.Adicionar(It.IsAny<Usuario>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Never);
        }
    }
    
    [Fact]
    public async Task Adicionar_ErroAoSalvar_Handle()
    {
        SetupMocks();
        
        var dto = new AdicionarUsuarioDto
        {
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };

        var usuario = await _usuarioService.Adicionar(dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            
            _usuarioRepositoryMock.Verify(c => c.Adicionar(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }
    
    [Fact]
    public async Task Adicionar_SucessoComFoto_UsuarioDto()
    {
        SetupMocks(false, true);

        var file = new Mock<IFormFile>();
        file.Setup(c => c.Length).Returns(56);
        var dto = new AdicionarUsuarioDto
        {
            Foto = file.Object,
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };

        var usuario = await _usuarioService.Adicionar(dto);

        using (new AssertionScope())
        {
            usuario.Should().NotBeNull();
            
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            
            _usuarioRepositoryMock.Verify(c => c.Adicionar(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }
    
    [Fact]
    public async Task Adicionar_SucessoSemFoto_UsuarioDto()
    {
        SetupMocks(false, true);

        var dto = new AdicionarUsuarioDto
        {
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };

        var usuario = await _usuarioService.Adicionar(dto);

        using (new AssertionScope())
        {
            usuario.Should().NotBeNull();
            
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            NotificatorMock
                .Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            
            _usuarioRepositoryMock.Verify(c => c.Adicionar(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }

    #endregion

    #region ObterPorId

    [Fact]
    public async Task ObterPorId_UsuarioInexistente_HandleNotFounrResourse()
    {
        SetupMocks();

        const int id = 2;
        var usuario = await _usuarioService.ObterPorId(id);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            NotificatorMock.Verify(c => c.HandleNotFoundResourse(), Times.Once);
        }
    }
    
    [Fact]
    public async Task ObterPorId_UsuarioExistente_Sucesso()
    {
        SetupMocks();

        const int id = 1;
        var usuario = await _usuarioService.ObterPorId(id);

        using (new AssertionScope())
        {
            usuario.Should().NotBeNull();
            usuario.Should().BeOfType<UsuarioDto>();
            NotificatorMock.Verify(c => c.HandleNotFoundResourse(), Times.Never);
        }
    }
    
    #endregion
    
    #region ObterTodos

    [Fact]
    public async Task ObterTodos_UsuariosInexistente_Handle()
    {
        SetupMocks(possuiUsuarios: false);
        
        var usuario = await _usuarioService.ObterTodos();

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
        }
    }
    
    [Fact]
    public async Task ObterTodos_UsuariosExistente_Sucesso()
    {
        SetupMocks(possuiUsuarios: true);

        var usuario = await _usuarioService.ObterTodos();

        using (new AssertionScope())
        {
            usuario.Should().NotBeNull();
            usuario.Should().BeOfType<List<UsuarioDto>>();
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
        }
    }
    
    #endregion

    #region Atualizar

    [Fact]
    public async Task Atualizar_IdsNaoConferem_Handle()
    {
        SetupMocks();
        
        const int id = 2;
        var dto = new AtualizarUsuarioDto
        {
            Id = 1
        };
        
        var usuario = await _usuarioService.Atualizar(id, dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.Atualizar(It.IsAny<Usuario>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Never);
        }
    }
    
    [Fact]
    public async Task Atualizar_UsuarioInexistente_HandleNotFoundResourse()
    {
        SetupMocks();
        
        const int id = 2;
        var dto = new AtualizarUsuarioDto
        {
            Id = 2
        };
        
        var usuario = await _usuarioService.Atualizar(id, dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            NotificatorMock.Verify(c => c.HandleNotFoundResourse(), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.Atualizar(It.IsAny<Usuario>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Never);
        }
    }
    
    [Fact]
    public async Task Atualizar_UsuarioInvalido_HandleErros()
    {
        SetupMocks(usuarioExistente: true);
        
        const int id = 1;
        var dto = new AtualizarUsuarioDto
        {
            Id = 1
        };
        
        var usuario = await _usuarioService.Atualizar(id, dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.Atualizar(It.IsAny<Usuario>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Never);
        }
    }
    
    
    
    [Fact]
    public async Task Atualizar_ErroAoComitar_Handle()
    {
        SetupMocks();
        
        const int id = 1;
        var dto = new AtualizarUsuarioDto
        {
            Id = 1,
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };
        
        var usuario = await _usuarioService.Atualizar(id, dto);

        using (new AssertionScope())
        {
            usuario.Should().BeNull();
            
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.Atualizar(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }
    
    [Fact]
    public async Task Atualizar_UsuarioComFoto_Sucesso()
    {
        SetupMocks(commit: true);
        
        var file = new Mock<IFormFile>();
        file.Setup(c => c.Length).Returns(56);
        const int id = 1;
        var dto = new AtualizarUsuarioDto
        {
            Id = 1,
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste",
            Fotos = file.Object
        };
        
        var usuario = await _usuarioService.Atualizar(id, dto);

        using (new AssertionScope())
        {
            usuario.Should().NotBeNull();
            
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.Atualizar(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }
    
    [Fact]
    public async Task Atualizar_UsuarioSemFoto_Sucesso()
    {
        SetupMocks(commit: true);
        
        const int id = 1;
        var dto = new AtualizarUsuarioDto
        {
            Id = 1,
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };
        
        var usuario = await _usuarioService.Atualizar(id, dto);

        using (new AssertionScope())
        {
            usuario.Should().NotBeNull();
            
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.Atualizar(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }

    #endregion

    #region Remover

    [Fact]
    public async Task Remover_UsuarioInexistente_HandleNotFoundResourse()
    {
        SetupMocks();
        
        const int id = 2;

        await _usuarioService.Remover(id);

        using (new AssertionScope())
        {
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.HandleNotFoundResourse(), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.Remover(It.IsAny<Usuario>()), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Never);
        }
    }
    
    [Fact]
    public async Task Remover_ErroAoComitar_Handle()
    {
        SetupMocks(commit: false);
        
        const int id = 1;

        await _usuarioService.Remover(id);

        using (new AssertionScope())
        {
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.HandleNotFoundResourse(), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.Remover(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }
    
    [Fact]
    public async Task Remover_Sucesso()
    {
        SetupMocks(commit: true);
        
        const int id = 1;

        await _usuarioService.Remover(id);

        using (new AssertionScope())
        {
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.HandleNotFoundResourse(), Times.Never);
            _usuarioRepositoryMock.Verify(c => c.Remover(It.IsAny<Usuario>()), Times.Once);
            _usuarioRepositoryMock.Verify(c => c.UnitOfWork.Commit(), Times.Once);
        }
    }

    #endregion

    private void SetupMocks(bool usuarioExistente = false, bool commit = false, bool retornaComFoto = false, bool possuiUsuarios = true)
    {
        var usuario = new Usuario
        {
            Foto = retornaComFoto ? "https://img.com/photo.jpg" : null,
            Cpf = "358.782.730-39",
            Email = "email@teste.com",
            Nome = "UsuarioTeste",
            Senha = "SenhaTeste"
        };

        #region UsuarioRepository

        _usuarioRepositoryMock
            .Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Usuario, bool>>>()))
            .ReturnsAsync(usuarioExistente ? usuario : null);

        _usuarioRepositoryMock
            .Setup(c => c.UnitOfWork.Commit())
            .ReturnsAsync(commit);

        _usuarioRepositoryMock
            .Setup(c => c.ObterPorId(It.Is<int>(id => id == 1)))
            .ReturnsAsync(usuario);
        
        _usuarioRepositoryMock
            .Setup(c => c.ObterPorId(It.Is<int>(id => id != 1)))
            .ReturnsAsync(null as Usuario);

        _usuarioRepositoryMock
            .Setup(c => c.ObterTodos())
            .ReturnsAsync(possuiUsuarios ? new List<Usuario> { usuario } : null);

        #endregion

        #region FileService

        _fileServiceMock
            .Setup(c => c.Upload(It.IsAny<IFormFile>(), It.IsAny<EUploadPath>(), It.IsAny<EPathAccess>(),
                It.IsAny<int>()))
            .ReturnsAsync("path");

        #endregion

        #region PasswordHasher

        _passwordHasherMock
            .Setup(c => c.HashPassword(It.IsAny<Usuario>(), It.IsAny<string>()))
            .Returns("senha-hasheada");

        #endregion
    }
}