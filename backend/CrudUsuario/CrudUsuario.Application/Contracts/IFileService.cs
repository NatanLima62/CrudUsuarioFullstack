using CrudUsuarios.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace CrudUsuario.Application.Contracts;

public interface IFileService
{
    Task<string> Upload(IFormFile arquivo, EUploadPath uploadPath, EPathAccess pathAccess = EPathAccess.Private,
        int urlLimitLength = 255);
    string ObterPath(Uri uri);
    bool Apagar(Uri uri);
}