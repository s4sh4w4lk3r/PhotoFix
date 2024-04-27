namespace PhotoFix.ConsoleApp1
{
    internal class Program
    {
        private const int YEAR = 2023;
        private static void Main(string[] args)
        {
            string folderPath = @"D:\ffff";
            Init(folderPath);

            SetDateTimeFromMetadataOrName();

            SetDateTimeMannualy();

            Console.WriteLine("Харош!");
            MediaEntryRepository.MediaEntries.Clear();
            MediaEntryRepository.Save();
        }

        private static DateTime GetDateTimeOrDefault(string path)
        {
            DateTime dt = new DateTimeMetadataExtractor(path).GetDateTimeOrDefault();
            if (dt != default)
            {
                return dt;
            }

            dt = DateTimeFromFileNamePicker.GetDateTimeOrDefault(path);
            if (dt != default)
            {
                return dt;
            }

            return default;
        }
        private static void SetLastWrite(MediaEntry mediaEntry)
        {
            File.SetLastWriteTime(mediaEntry.Path, mediaEntry.DateTime);
            mediaEntry.Status = MediaEntryStatus.Done;
            Console.WriteLine($"Файл: {Path.GetFileName(mediaEntry.Path)}, DateTime: {mediaEntry.DateTime}. Применено успешно.");
        }

        private static void SetDateTimeMannualy()
        {

            foreach (var entry in MediaEntryRepository.MediaEntries.Where(x=>x.Status != MediaEntryStatus.Done))
            {
                DateTime dt = GetDateTimeFromInput(entry.Path, YEAR);
                if (dt == default)
                {
                    entry.Status = MediaEntryStatus.Skipped;
                }
                else
                {
                    entry.DateTime = dt;
                    SetLastWrite(entry);
                    MediaEntryRepository.Save();
                }
            }
        }
        private static DateTime GetDateTimeFromInput(string path, int year)
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine($"Файл : {Path.GetFileName(path)}");
                Console.WriteLine($"Год: {year}");

                Console.Write($"Месяц: ");
                int month = int.Parse(Console.ReadLine()!);

                Console.Write($"День: ");
                int day = int.Parse(Console.ReadLine()!);

/*                Console.Write("Часы: ");
                int hours = int.Parse(Console.ReadLine()!);

                Console.Write("Минуты: ");
                int minutes = int.Parse(Console.ReadLine()!);

                Console.Write("Секунды: ");
                int seconds = int.Parse(Console.ReadLine()!);*/

                return new DateTime(year, month, day, 11, 11, 11);
                //return new DateTime(year, month, day, hours, minutes, seconds);
            }
            catch (FormatException)
            {
                Console.WriteLine("Неправильный ввод.");
                return default;
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Неправильный ввод.");
                return default;
            }
        }
        private static void Init(string path)
        {
            string[] filePaths = System.IO.Directory.GetFiles(path);

            foreach (string filePath in filePaths)
            {
                if (!MediaEntryRepository.MediaEntries.Any(x => x.Path == filePath))
                {
                    MediaEntryRepository.MediaEntries.Add(new(filePath, default, MediaEntryStatus.None));
                }
            }

            MediaEntryRepository.Save();
        }

        private static void SetDateTimeFromMetadataOrName()
        {
            foreach (var entry in MediaEntryRepository.MediaEntries.Where(x=>x.Status != MediaEntryStatus.Done))
            {
                DateTime dt = GetDateTimeOrDefault(entry.Path);
                if (dt != default)
                {
                    entry.DateTime = dt;
                    entry.Status = MediaEntryStatus.DateTimeFound;
                    Console.WriteLine($"Файл: {Path.GetFileName(entry.Path)}, DateTime: {entry.DateTime}");
                    continue;
                }
                entry.DateTime = default;
                entry.Status = MediaEntryStatus.DateTimeNotFound;
                Console.WriteLine($"Файл: {Path.GetFileName(entry.Path)}, дата и время не получены.");
            }

            MediaEntryRepository.Save();

            Console.Write("Установить даты для файлов, для которых получена дата и время? y/n ");
            if (Console.ReadKey().Key != ConsoleKey.Y)
            {
                Environment.Exit(0);
            }

            foreach (var entry in MediaEntryRepository.MediaEntries.Where(x => x.Status == MediaEntryStatus.DateTimeFound))
            {

                SetLastWrite(entry);
            }

            MediaEntryRepository.Save();
        }
    }
}
