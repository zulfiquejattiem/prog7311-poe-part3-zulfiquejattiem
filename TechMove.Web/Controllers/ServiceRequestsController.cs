using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechMove.Shared.ServiceRequests;
using TechMove.Shared.Contracts;

namespace TechMove.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly HttpClient _http;

        public ServiceRequestsController(IHttpClientFactory factory)
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
            var data = await _http.GetFromJsonAsync<List<ServiceRequestDto>>("api/servicerequests");

            return View(data ?? new List<ServiceRequestDto>());
        }

        // ✅ CREATE
        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();
            
            try
            {
                var response = await _http.GetAsync("api/contracts");
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Contracts = await response.Content.ReadFromJsonAsync<List<ContractDto>>();
                }
                else
                {
                    ViewBag.Contracts = new List<ContractDto>();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Contracts = new List<ContractDto>();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequestCreateDto dto)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();

            try
            {
                if (dto.ContractId <= 0)
                {
                    ModelState.AddModelError("", "Please select a valid contract");
                    ViewBag.Contracts = await GetContractsList();
                    return View(dto);
                }

                var response = await _http.PostAsJsonAsync("api/servicerequests", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", "Invalid contract status or server error: " + errorContent);
                    ViewBag.Contracts = await GetContractsList();
                    return View(dto);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                ViewBag.Contracts = await GetContractsList();
                return View(dto);
            }
        }

        private async Task<List<ContractDto>> GetContractsList()
        {
            try
            {
                var response = await _http.GetAsync("api/contracts");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ContractDto>>() ?? new List<ContractDto>();
                }
            }
            catch { }
            return new List<ContractDto>();
        }

        // ✅ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();
            try
            {
                var response = await _http.GetAsync($"api/servicerequests/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var item = await response.Content.ReadFromJsonAsync<ServiceRequestDto>();
                return View(item);
            }
            catch
            {
                return NotFound();
            }
        }

        // ✅ EDIT
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();
            try
            {
                var response = await _http.GetAsync($"api/servicerequests/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var item = await response.Content.ReadFromJsonAsync<ServiceRequestDto>();
                return View(item);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceRequestDto dto)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();
            try
            {
                var response = await _http.PutAsJsonAsync($"api/servicerequests/{dto.Id}", dto);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Update failed");
                    return View(dto);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while updating");
                return View(dto);
            }
        }

        // ✅ DELETE
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();
            try
            {
                var response = await _http.GetAsync($"api/servicerequests/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var item = await response.Content.ReadFromJsonAsync<ServiceRequestDto>();
                return View(item);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            AddJwt();
            var response = await _http.DeleteAsync($"api/servicerequests/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Delete failed");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
