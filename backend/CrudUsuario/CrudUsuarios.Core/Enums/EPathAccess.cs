using System.ComponentModel;

namespace CrudUsuarios.Core.Enums;

public enum EPathAccess
{
    [Description("assets/public")]
    Public,
    [Description("assets/private")]
    Private
}