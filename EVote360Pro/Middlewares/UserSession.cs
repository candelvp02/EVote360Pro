using EVote360Pro.Core.Application.Dtos.Usuario;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Helpers;

namespace EVote360Pro.Middlewares
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUser(UsuarioDto usuario)
        {
            _httpContextAccessor.HttpContext.Session.Set<UsuarioDto>("Usuario", usuario);
        }

        public UsuarioDto? GetUser()
        {
            return _httpContextAccessor.HttpContext.Session.Get<UsuarioDto>("Usuario");
        }

        public void RemoveUser()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Usuario");
        }

        public bool IsAuthenticated()
        {
            return GetUser() != null;
        }
    }
}