using EVote360Pro.Core.Application.Dtos.Usuario;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Usuario;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUserSession _userSession;

        public LoginController(IUsuarioService usuarioService, IUserSession userSession)
        {
            _usuarioService = usuarioService;
            _userSession = userSession;
        }

        public IActionResult Index()
        {
            if (_userSession.IsAuthenticated())
            {
                UsuarioDto? usuario = _userSession.GetUser();
                if (usuario != null)
                {
                    return usuario.Rol switch
                    {
                        RolUsuario.Administrador => RedirectToAction("Index", "Home"),
                        RolUsuario.DirigentePolitico => RedirectToAction("Index", "DirigentHome"),
                        _ => RedirectToAction("Index", "Login")
                    };
                }
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (_userSession.IsAuthenticated())
            {
                UsuarioDto? usuario = _userSession.GetUser();
                if (usuario != null)
                {
                    return usuario.Rol switch
                    {
                        RolUsuario.Administrador => RedirectToAction("Index", "Home"),
                        RolUsuario.DirigentePolitico => RedirectToAction("Index", "DirigentHome"),
                        _ => RedirectToAction("Index", "Login")
                    };
                }
            }

            if (!ModelState.IsValid)
            {
                vm.Contrasena = string.Empty;
                return View(vm);
            }

            UsuarioDto? usuarioDto = await _usuarioService.LoginAsync(vm.NombreUsuario, vm.Contrasena);

            if (usuarioDto != null)
            {
                _userSession.SetUser(usuarioDto);

                return usuarioDto.Rol switch
                {
                    RolUsuario.Administrador => RedirectToAction("Index", "Home"),
                    RolUsuario.DirigentePolitico => RedirectToAction("Index", "DirigentHome"),
                    _ => RedirectToAction("Index", "Login")
                };
            }

            ModelState.AddModelError("loginValidation", "Usuario o contraseña incorrectos");
            vm.Contrasena = string.Empty;
            return View(vm);
        }

        public IActionResult Logout()
        {
            _userSession.RemoveUser();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult AccessDenied()
        {
            if (_userSession.IsAuthenticated())
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}