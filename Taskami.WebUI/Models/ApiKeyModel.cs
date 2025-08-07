using System.ComponentModel.DataAnnotations;

namespace Taskami.WebUI.Models
{
    public class ApiKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string? ApiKey { get; set; }

    }

}
