using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taskami.WebUI.Models;
using Taskami.WebUI.Services;

namespace Taskami.WebUI.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // The settings page where users can configure their preferences
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var taskamiKey = user.TaskamiApiKey ?? string.Empty;
            var model = new ApplicationUser
            {
                TaskamiApiKey = user.TaskamiApiKey ?? string.Empty,
                TodoistApiKey = string.IsNullOrWhiteSpace(user.TodoistApiKey) ? "YOUR_API_KEY" : user.TodoistApiKey,
                PrimaryColor = user.PrimaryColor ?? "#222222",
                SecondaryColor = user.SecondaryColor ?? "#191919",
                PomodoroDuration = user.PomodoroDuration,
                PomodoroBreakDuration = user.PomodoroBreakDuration,
                PomodoroLongBreakDuration = user.PomodoroLongBreakDuration
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAPIKey(string apiKey, string formName)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var keyToSave = string.IsNullOrWhiteSpace(apiKey) ? "YOUR_API_KEY" : apiKey;

            if (formName == "taskami")
            {
                user.TaskamiApiKey = keyToSave;
            }
            else if (formName == "todoist")
            {
                user.TodoistApiKey = keyToSave;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Nepodařilo se uložit API klíč.";
            }

            return RedirectToAction("Today", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetColors(string primaryColor, string secondaryColor)
        {
            Console.WriteLine(primaryColor + " " + secondaryColor);
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            user.PrimaryColor = primaryColor;
            user.SecondaryColor = secondaryColor;
            Console.WriteLine(primaryColor + " " + secondaryColor);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Nepodařilo se uložit barvy.";
            }
            return RedirectToAction("Index", "Settings");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPomodoro(int pomodoroDuration, int pomodoroBreakDuration, int pomodoroLongBreakDuration)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            user.PomodoroDuration = pomodoroDuration;
            user.PomodoroBreakDuration = pomodoroBreakDuration;
            user.PomodoroLongBreakDuration = pomodoroLongBreakDuration;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Nepodařilo se uložit nastavení Pomodoro.";
            }

            return RedirectToAction("Index", "Settings");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetBackends(string backendType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            user.BackendType = backendType;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Nepodařilo se uložit backend.";
            }
            return RedirectToAction("Index", "Settings");
        }
    }
}
