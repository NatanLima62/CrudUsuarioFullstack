CRUDUsuario - Api

Onjetivo:
Criar uma API Rest usando .NET 6 e EF Core para realizar o gerenciamento de usuário em uma aplicação.

Descrição de Entidade:
* Usuario
  * Id (int)
  * Nome (Required, varchar(150))
  * Cpf (Required, varchar(11))
  * Email (Required, varchar(100))
  * Senha (Required, varchar(255))
  * Foto (varchar(255))
  * CriadoEm (Required, DateTime)
  * AtualizadoEm (Required, DateTime)
 
Registro do usuário
  * Pedir Nome, Email, Cpf, Senha, Foto
  * Deve ser verificado se Email já esta em uso
  * O password deve ser armazenado utilizando algum algoritmo de hash (Argon2).

Dependências:
* .NET 6
*  Entity Framework 6
*  MySql
*  AutoMapper
*  FluentValidation
*  ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher

Dependencia por Camada:
  * Api:
    * Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
    *  Microsoft.EntityFrameworkCore.Design
    * Swashbuckle.AspNetCore
    * Swashbuckle.AspNetCore.Annotations
      
  * Application:
    * AutoMapper
    * AutoMapper.Extensions.Microsoft.DependencyInjection
    * Microsoft.Extensions.DependencyInjection.Abstractions
    * ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher
    * Microsoft.Extensions.Options.ConfigurationExtensions
    * Microsoft.AspNetCore.StaticFiles
    * Microsoft.Extensions.FileProviders.Physical
      
  * Domain:
    * FluentValidation
      
  * Infra:
    * Microsoft.AspNetCore.Http.Abstractions
    * Microsoft.EntityFrameworkCore
    * Microsoft.EntityFrameworkCore.Design
    * Microsoft.EntityFrameworkCore.Tools
    * Pomelo.EntityFrameworkCore.MySql
    * Pomelo.EntityFrameworkCore.MySql.Design
   
  * .Test
    * FluentAssertions
    * Microsoft.Extensions.DependencyInjection
    * Microsoft.NET.Test.Sdk
    * Moq
    * xunit
    * xunit.runner.visualstudio
    * coverlet.collector
