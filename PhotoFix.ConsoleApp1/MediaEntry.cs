using System.Text.Json.Serialization;

namespace PhotoFix.ConsoleApp1
{
    public class MediaEntry(string path, DateTime dateTime, MediaEntryStatus status)
    {
        public string Path { get; init; } = path;
        public MediaEntryStatus Status { get; set; } = status;
        [JsonIgnore] public DateTime DateTime { get; set; } = dateTime;
    }
}
