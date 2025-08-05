using System.Text.Json;
using System.Text.Json.Serialization;

namespace Taskami.WebUI.Models
{
    public class TodoistTask
    {
        [JsonPropertyName("id")]
        public required string TaskId { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("project_id")]
        public string? ProjectId { get; set; }

        [JsonPropertyName("section_id")]
        public string? SectionId { get; set; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("labels")]
        public List<string>? Labels { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("assignee_id")]
        public long? AssigneeId { get; set; }

        [JsonPropertyName("duration")]
        public JsonElement? Duration { get; set; }

        [JsonPropertyName("duration_unit")]
        public string? DurationUnit { get; set; }

        [JsonPropertyName("deadline_date")]
        public string? DeadlineDate { get; set; }

        [JsonPropertyName("deadline_lang")]
        public string? DeadlineLang { get; set; }

        [JsonPropertyName("due")]
        public DueInfo? Due { get; set; } // <- PŘIDAT TUTO VLASTNOST

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    public class DueInfo
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("datetime")]
        public DateTime? DateTime { get; set; }

        [JsonPropertyName("string")]
        public string? DueString { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

    }

}
