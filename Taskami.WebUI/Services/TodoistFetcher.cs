using System.Net.Http.Headers;
using System.Text.Json;
using System.Globalization;
using Taskami.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

public class TodoistFetcher
{
    private readonly HttpClient _httpClient;

    public TodoistFetcher(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> FetchTodaysTasksAsync()
    {
        var url = "https://api.todoist.com/api/v1/tasks";
        var response = await _httpClient.GetAsync(url);
        return await response.Content.ReadAsStringAsync(); ;
    }
    [HttpPost]
    public async Task CompleteTaskAsync(string TaskId)
    {
        var url = $"https://api.todoist.com/api/v1/tasks/{TaskId}/close";
        var response = await _httpClient.PostAsync(url, null);
        Console.WriteLine(response);
    }
}
