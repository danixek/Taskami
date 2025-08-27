using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using Taskami.WebUI.Controllers;
using Taskami.WebUI.Models;
using Taskami.WebUI.Services;

namespace TaskamiUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TodoistFetcher _fetcher;
        private readonly ApplicationDbContext _context;
        private readonly ApiKeyHandler _apiKeyHandler;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(TodoistFetcher fetcher, ApplicationDbContext context, ApiKeyHandler apiKeyHandler, UserManager<ApplicationUser> userManager)
        {
            _fetcher = fetcher;
            _context = context;
            _apiKeyHandler = apiKeyHandler;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Today");
        }
        // The main landing page of the application
        public async Task<IActionResult> Inbox()
        {
            var resultsElement = await CallingApiKey();

            if (resultsElement is null)
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Index", "Settings");
            }
            var allTasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.Value.GetRawText()) ?? new List<TodoistTask>();
            var tasks = allTasks?.Where(t => t.ProjectId == "6F2RRjJg9xmgWH7q").ToList() ?? new List<Taskami.WebUI.Models.TodoistTask>();
            var tasksWithColors = await tasks.WithColorsAsync(_userManager, User);

            return View(tasksWithColors);
        }
        // The main dashboard where users can see their tasks and activities
        public async Task<IActionResult> Today()
        {
            var resultsElement = await CallingApiKey();

            if (resultsElement is null)
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Index", "Settings");
            }
            var allTasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.Value.GetRawText()) ?? new List<TodoistTask>();

            var parentTasksOrdered = allTasks
            .Where(t => t.ParentId == null)
            .OrderBy(t =>
                t.Description != null && t.Description.StartsWith("**Current streak:**") ? 0 :
                (t.Due != null && t.Due.Date.TimeOfDay > TimeSpan.Zero ? 1 : 2))
            .ThenBy(t => t.Due?.Date)
            .ToList();
            var allTasksOrdered = new List<TodoistTask>();

            foreach (var parent in parentTasksOrdered)
            {
                allTasksOrdered.Add(parent);

                var childTasks = allTasks
                    .Where(t => t.ParentId == parent.TaskId)
                    .OrderBy(t => t.Due?.Date) // nebo jak chceš řadit děti
                    .ToList();

                allTasksOrdered.AddRange(childTasks);
            }

            var todayParentIds = allTasksOrdered
                .Where(t => t.Due != null && DateOnly.FromDateTime(t.Due.Date) == DateOnly.FromDateTime(DateTime.Today))
                .Select(t => t.ParentId ?? t.TaskId)
                .Distinct()
                .ToList();

            var todaysTasks = allTasksOrdered
                .Where(t => (t.ParentId == null && todayParentIds.Contains(t.TaskId))
                         || (t.ParentId != null && todayParentIds.Contains(t.ParentId)))
                .ToList();
            var otherTasks = allTasksOrdered.Except(todaysTasks).ToList();

            var tasks = new TodayViewModel
            {
                TodaysTasks = todaysTasks,
                OtherTasks = otherTasks
            };
            var tasksWithColors = await tasks.WithColorsAsync(_userManager, User);


            return View(tasksWithColors);

        }
        // The page where users can view and manage their tasks - calendar mode
        public async Task<IActionResult> Calendar()
        {
            var resultsElement = await CallingApiKey();

            if (resultsElement is null)
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Index", "Settings");
            }

            var allTasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.Value.GetRawText()) ?? new List<TodoistTask>();

            var tasks = new CalendarViewModel
            {
                TodaysTasks = allTasks
                    .Where(t => t.Due != null && DateOnly.FromDateTime(t.Due.Date) == DateOnly.FromDateTime(DateTime.Today))
                    .ToList(),

                TomorrowsTasks = allTasks
                    .Where(t => t.Due != null && DateOnly.FromDateTime(t.Due.Date) == DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
                    .ToList(),

                AllTasks = allTasks // používá se pro 30denní smyčku - zobrazení měsíců
            };
            var tasksWithColors = await tasks.WithColorsAsync(_userManager, User);
            Console.WriteLine("AllTasks:");

            return View(tasksWithColors);
        }
        // Filters and labels are used to categorize tasks
        public async Task<IActionResult> Filters()
        {
            var resultsElement = await CallingApiKey();

            if (resultsElement is null)
            {
                TempData["Error"] = "API klíč je neplatný nebo chybí. Prosím, zadej nový klíč v nastavení.";
                return RedirectToAction("Index", "Settings");
            }

            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.Value.GetRawText()) ?? new List<TodoistTask>();
            var tasksWithColors = await tasks.WithColorsAsync(_userManager, User);


            return View(tasksWithColors);
        }
        // Pomodoro is a time management technique that uses a timer to break work into intervals
        public async Task<IActionResult> Pomodoro()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var taskamiKey = user.TaskamiApiKey ?? string.Empty;
            var pomodoro = new PomodoroViewModel
            {
                PrimaryColor = user.PrimaryColor ?? "#222222",
                SecondaryColor = user.SecondaryColor ?? "#191919",
                PomodoroDuration = user.PomodoroDuration,
                PomodoroBreakDuration = user.PomodoroBreakDuration,
                PomodoroLongBreakDuration = user.PomodoroLongBreakDuration
            };

            return View(pomodoro);
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

        private async Task<JsonElement?> TryApiKey(string rawJson)
        {
            if (await _apiKeyHandler.IsApiKeyMissingAsync())
                return null;

            var root = JsonDocument.Parse(rawJson).RootElement;

            if (!root.TryGetProperty("results", out var resultsElement))
                return null;

            return resultsElement;
        }
        private async Task<JsonElement?> CallingApiKey()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            var todoistKey = string.IsNullOrWhiteSpace(user.TodoistApiKey)
                ? "YOUR_API_KEY"
                : user.TodoistApiKey;

            var rawJson = await _fetcher.FetchTodaysTasksAsync(todoistKey);
            var resultsElement = await TryApiKey(rawJson);

            return resultsElement;
        }
    }
    public static class ViewModelExtensions
    {
        public static async Task<BackgroundColorsViewModel<TModel>> WithColorsAsync<TModel>(
            this TModel model,
            UserManager<ApplicationUser> userManager,
            ClaimsPrincipal userPrincipal)
            where TModel : class
        {
            var user = await userManager.GetUserAsync(userPrincipal);
            if (user == null) throw new InvalidOperationException("User not found");

            return new BackgroundColorsViewModel<TModel>
            {
                PrimaryColor = user.PrimaryColor ?? "#222222",
                SecondaryColor = user.SecondaryColor ?? "#191919",
                Tasks = model
            };
        }
    }

}
