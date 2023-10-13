using AutoMapper;
using CrudUsuario.Application.DTOs.Usuario;
using CrudUsuario.Domain.Entities;
using CrudUsuarios.Core.Extensions;

namespace CrudUsuario.Application.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Usuario, UsuarioDto>().ReverseMap()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros()!);
        CreateMap<Usuario, AdicionarUsuarioDto>().ReverseMap()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros()!);
        CreateMap<Usuario, AtualizarUsuarioDto>().ReverseMap()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros()!);
    }
}