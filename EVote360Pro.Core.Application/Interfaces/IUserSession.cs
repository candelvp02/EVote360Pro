using EVote360Pro.Core.Application.Dtos.Usuario;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IUserSession
    {
        void SetUser(UsuarioDto usuario);
        UsuarioDto? GetUser();
        void RemoveUser();
        bool IsAuthenticated();
    }
}