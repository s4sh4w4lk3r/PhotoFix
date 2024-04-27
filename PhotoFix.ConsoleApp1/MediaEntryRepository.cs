using System.Text.Json;

namespace PhotoFix.ConsoleApp1
{
    public static class MediaEntryRepository
    {
        private static readonly Lazy<List<MediaEntry>> mediaEntries = new([]);
        public static List<MediaEntry> MediaEntries => mediaEntries.Value;

        static MediaEntryRepository()
        {
            Load();
        }

        public static void Load()
        {
            string fileName = "data.json";
            if (!File.Exists(fileName))
            {
                using StreamWriter sw = File.CreateText(fileName);
                JsonSerializer.Serialize(sw.BaseStream, MediaEntries);
            }

            using StreamReader streamReader = new(fileName);
            var list = JsonSerializer.Deserialize<List<MediaEntry>>(streamReader.BaseStream) ?? [];
            MediaEntries.AddRange(list);
        }

        public static void Save()
        {
            string fileName = "data.json";
            if (File.Exists(fileName))
            {
                using StreamWriter sw = new(fileName);
                JsonSerializer.Serialize(sw.BaseStream, MediaEntries);
            }

            if (MediaEntries.Count != MediaEntries.DistinctBy(x => x.Path).Count())
            {
                MediaEntries.Clear();
                Console.WriteLine("Обнаружены дубликаты путей в истории. Выход из программы.");
                Environment.Exit(-1);
            }
        }
    }
}
