// Controllers/AccountController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SmartEventPlanningPlatform.Models;
using SmartEventPlanningPlatform.Services;
using System.Security.Claims;
using AuthenticationService = SmartEventPlanningPlatform.Services.AuthenticationService;
namespace SmartEventPlanningPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthenticationService _authService;

        public AccountController(AuthenticationService authService)
        {
            _authService = authService;
        }
        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Register";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string username, string password, string email, string firstName, string lastName,
    string phoneNumber, string location, string interests, DateTime birthDate, string gender, string profilePictureUrl, string Latitude, string Longitude)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı, şifre ve e-posta zorunludur.");
                return View();
            }

            if (_authService.Register(username, password, email, DateTime.UtcNow, firstName, lastName, phoneNumber, location, interests, birthDate, gender, profilePictureUrl,Latitude,Longitude) == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı zaten kullanılıyor.");
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        // Giriş yapma formu
        public IActionResult Login()
        {
            ViewData["ActivePage"] = "Login";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (username=="admin" && password=="admin")
            {


                return RedirectToAction("Index", "Admin");
            }


            var user = _authService.Authenticate(username, password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }

            // Kullanıcı bilgilerini doğrulandı, cookie oluştur
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) // Kullanıcı ID'si
    };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }



        // Çıkış yapma
        
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth"); // Kullanıcı oturumunu kapat
            return RedirectToAction("Login", "Account");
        }

    }
}
