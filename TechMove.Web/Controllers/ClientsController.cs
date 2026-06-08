using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechMove.Shared.Clients;

namespace TechMove.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _http;

        public ClientsController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("api");
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JWT"));
        }

        private void AddJwt()
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                throw new Exception("Not logged in");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // ✅ LIST
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();

            var clients = await _http.GetFromJsonAsync<List<ClientDto>>("api/clients");
            return View(clients);
        }

        // ✅ CREATE
        public IActionResult Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateDto dto)
        {
            AddJwt();

            await _http.PostAsJsonAsync("api/clients", dto);
            return RedirectToAction(nameof(Index));
        }

        // ✅ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            AddJwt();

            var client = await _http.GetFromJsonAsync<ClientDto>($"api/clients/{id}");
            return View(client);
        }

        // ✅ EDIT
        public async Task<IActionResult> Edit(int id)
        {
            AddJwt();

            var client = await _http.GetFromJsonAsync<ClientDto>($"api/clients/{id}");
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientDto dto)
        {
            AddJwt();

            await _http.PutAsJsonAsync($"api/clients/{dto.Id}", dto);
            return RedirectToAction(nameof(Index));
        }

        // ✅ DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            AddJwt();

            var client = await _http.GetFromJsonAsync<ClientDto>($"api/clients/{id}");
            return View(client);
        }

        // ✅ DELETE (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AddJwt();

            await _http.DeleteAsync($"api/clients/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}