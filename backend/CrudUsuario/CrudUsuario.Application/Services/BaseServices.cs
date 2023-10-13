using AutoMapper;
using CrudUsuario.Application.Notifications;

namespace CrudUsuario.Application.Services;

public abstract class BaseServices
{
    protected readonly IMapper Mapper;
    protected readonly INotificator Notificator;

    protected BaseServices(IMapper mapper, INotificator notificator)
    {
        Mapper = mapper;
        Notificator = notificator;
    }
}