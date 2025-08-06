using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Taskami.WebUI.Models;

namespace TaskamiUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly TodoistFetcher _fetcher;

        public HomeController(TodoistFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        // The main landing page of the application
        public async Task<IActionResult> Inbox()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            var resultsElement = JsonDocument.Parse(rawJson).RootElement.GetProperty("results");

            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.GetRawText());

            ViewBag.Tasks = tasks ?? new List<TodoistTask>();

            return View();
        }
        // The main dashboard where users can see their tasks and activities
        public async Task<IActionResult> Today()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            var resultsElement = JsonDocument.Parse(rawJson).RootElement.GetProperty("results");

            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.GetRawText());

            ViewBag.Tasks = tasks ?? new List<TodoistTask>();

            return View();
        }
        // The page where users can view and manage their tasks - calendar mode
        public async Task<IActionResult> Calendar()
        {
            var rawJson = await _fetcher.FetchTodaysTasksAsync();
            var resultsElement = JsonDocument.Parse(rawJson).RootElement.GetProperty("results");

            var tasks = JsonSerializer.Deserialize<List<TodoistTask>>(resultsElement.GetRawText());

            ViewBag.Tasks = tasks ?? new List<TodoistTask>();
            return View();
        }
        // Filters and labels are used to categorize tasks
        public IActionResult Filters()
        {
            return View();
        }
        // Pomodoro is a time management technique that uses a timer to break work into intervals
        public IActionResult Pomodoro()
        {
            return View();
        }
        // The settings page where users can configure their preferences
        public IActionResult Settings()
        {
            return View();
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

    }
}
