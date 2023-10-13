using CrudUsuario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudUsuario.Infra.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11);
        
        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(c => c.Senha)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Foto)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}