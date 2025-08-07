using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text.Json;
using Taskami.WebUI.Controllers;
using Taskami.WebUI.Models;
using Taskami.WebUI.Services;

namespace TaskamiUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly TodoistFetcher _fetcher;
        private readonly ApplicationDbContext _context;
        private readonly ApiKeyHandler _apiKeyHandler;

        public HomeController(TodoistFetcher fetcher, ApplicationDbContext context, ApiKeyHandler apiKeyHandler)
        {
            _fetcher = fetcher;
            _context = context;
            _apiKeyHandler = apiKeyHandler;
        }

        // The main landing page of the application
        public async Task<IActionResult> Inbox()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            if (await _apiKeyHandler.IsApiKeyMissingAsync() || !JsonDocument.Parse(rawJson).RootElement.TryGetProperty("results", out var resultsElement))
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Settings", "Home");
            }
            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.GetRawText());

            ViewBag.Tasks = tasks ?? new List<TodoistTask>();

            return View();
        }
        // The main dashboard where users can see their tasks and activities
        public async Task<IActionResult> Today()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            if (await _apiKeyHandler.IsApiKeyMissingAsync() || !JsonDocument.Parse(rawJson).RootElement.TryGetProperty("results", out var resultsElement))
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Settings", "Home");
            }
            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.GetRawText());
            ViewBag.Tasks = tasks ?? new List<TodoistTask>();

            TempData["Success"] = "API klíč úspěšně uložen.";


            return View(); // ✅ Tohle je důležité

        }
        // The page where users can view and manage their tasks - calendar mode
        public async Task<IActionResult> Calendar()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            if (await _apiKeyHandler.IsApiKeyMissingAsync() || !JsonDocument.Parse(rawJson).RootElement.TryGetProperty("results", out var resultsElement))
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Settings", "Home");
            }
            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.GetRawText());

            ViewBag.Tasks = tasks ?? new List<TodoistTask>();
            return View();
        }
        // Filters and labels are used to categorize tasks
        public async Task<IActionResult> Filters()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            if (await _apiKeyHandler.IsApiKeyMissingAsync() || !JsonDocument.Parse(rawJson).RootElement.TryGetProperty("results", out var resultsElement))
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Settings", "Home");
            }
            return View();
        }
        // Pomodoro is a time management technique that uses a timer to break work into intervals
        public async Task<IActionResult> Pomodoro()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            if (await _apiKeyHandler.IsApiKeyMissingAsync() || !JsonDocument.Parse(rawJson).RootElement.TryGetProperty("results", out var resultsElement))
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Settings", "Home");
            }
            return View();
        }
        // The settings page where users can configure their preferences
        public async Task<IActionResult> Settings()
        {
            var existingKey = await _context.ApiKey.FirstOrDefaultAsync();
            var apiKey = existingKey?.ApiKey ?? string.Empty;

            // Zkusíme volání Todoist API, abychom ověřili, zda je klíč platný
            try
            {
                var testResponse = await _fetcher.FetchTodaysTasksAsync(); // nebo něco jiného, co vyžaduje platný klíč

                using var jsonDoc = JsonDocument.Parse(testResponse);
                if (jsonDoc.RootElement.TryGetProperty("error_tag", out var errorTag)
                    && errorTag.GetString() == "UNAUTHORIZED")
                {
                    apiKey = "YOUR_API_KEY";
                }
            }
            catch
            {
                apiKey = "YOUR_API_KEY";
            }

            return View(model: apiKey);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Reschedule(string? TaskId)
        {
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(string TaskId)
        {
            await _fetcher.CompleteTaskAsync(TaskId);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAPIKey(string apiKey)
        {
            var existingKey = await _context.ApiKey.OrderBy(k => k.Id).FirstOrDefaultAsync();
            if (existingKey == null)
            {
                existingKey = new ApiKeyModel { ApiKey = "YOUR_API_KEY" };
                await _context.ApiKey.AddAsync(existingKey);
            }

            existingKey.ApiKey = apiKey;
            await _context.SaveChangesAsync();

            return RedirectToAction("Today", "Home");
        }
    }
}
