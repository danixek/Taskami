namespace Taskami.WebUI.Models
{
    public class BackgroundColorsViewModel<TModel>
    {
        public string PrimaryColor { get; set; } = "#222222";
        public string SecondaryColor { get; set; } = "#191919";
        public TModel Tasks { get; set; } = default!;
    }

}