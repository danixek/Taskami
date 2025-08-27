using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Taskami.WebUI.Models;

namespace Taskami.WebUI.Services
{
    public class ApiKeyHandler : DelegatingHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiKeyHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsApiKeyMissingAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            return string.IsNullOrWhiteSpace(user?.TodoistApiKey);
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if (user != null && !string.IsNullOrWhiteSpace(user.TodoistApiKey))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.TodoistApiKey);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
