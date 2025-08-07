using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Taskami.WebUI.Models;

namespace Taskami.WebUI.Services
{
    public class ApiKeyHandler : DelegatingHandler
    {
        private readonly ApplicationDbContext _context;

        public ApiKeyHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsApiKeyMissingAsync()
        {
            var existingKey = await _context.ApiKey.FirstOrDefaultAsync();
            return string.IsNullOrEmpty(existingKey?.ApiKey);
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKeyEntity = await _context.ApiKey.FirstOrDefaultAsync(cancellationToken);
            if (apiKeyEntity != null && !string.IsNullOrEmpty(apiKeyEntity.ApiKey))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKeyEntity.ApiKey);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
