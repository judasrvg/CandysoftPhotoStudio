namespace Tattoo.Management.Models.Dialogs
{
    public class DialogHeaders
    {
        public string DialogTitle { get; set; } = "";
        public string DialogHeader { get; set; } = "";
        public Dictionary<string, object> dialogParams { get; set; } = new();
    }
}
