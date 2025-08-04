using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Taskami.WebUI.Models;

namespace TaskamiUI.Controllers
{
    public class HomeController : Controller
    {

        // The main landing page of the application
        public IActionResult Index()
        {
            return View();
        }
        // The main dashboard where users can see their tasks and activities
        public IActionResult Today()
        {
            return View();
        }
        // The page where users can view and manage their tasks - calendar mode
        public IActionResult Calendar()
        {
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
        public IActionResult Reschedule()
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
