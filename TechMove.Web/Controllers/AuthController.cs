using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TechMove.Shared.Auth;

namespace TechMove.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;

        public AuthController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("api");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(dto);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (result == null)
            {
                ModelState.AddModelError("", "Invalid login response");
                return View(dto);
            }

            HttpContext.Session.SetString("JWT", result.Token);
            HttpContext.Session.SetString("Username", dto.Username);

            return RedirectToAction("Index", "Clients");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
