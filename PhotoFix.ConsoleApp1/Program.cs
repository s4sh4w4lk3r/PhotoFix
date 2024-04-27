using System;


namespace PhotoFix.ConsoleApp1
{
    internal class Program
    {
        private const int YEAR = 2020;
        private static void Main(string[] args)
        {
            string folderPath = @"D:\fff";
            var filePaths = System.IO.Directory.GetFiles(folderPath);

            List<MediaDateTime> mediaDateTimeList = [];

            foreach (var item in filePaths.Order())
            {
                var mediaDateTime = new MediaDateTime(item, GetDateTimeOrDefault(item));
                mediaDateTimeList.Add(mediaDateTime);
                Console.WriteLine(mediaDateTime);
            }


            Console.WriteLine($"Total : {mediaDateTimeList.Count}");
            Console.WriteLine($"Fail : {mediaDateTimeList.Count(x => x.DateTime == default)}");

            Console.Write("Continue? y/n ");
            bool goNext = Console.ReadKey().Key == ConsoleKey.Y;
            if (!goNext)
            {
                Environment.Exit(0);
            }
            mediaDateTimeList = mediaDateTimeList.OrderBy(x => x.IsDefault).ToList();
            mediaDateTimeList.ForEach(SetLastWrite);
            Console.WriteLine("Done.");
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
        private static void SetLastWrite(MediaDateTime mediaDateTime)
        {
            if (mediaDateTime.IsDefault)
            {
                DateTime dateTime = SetDateManually(mediaDateTime.Path, YEAR);
                if (dateTime == default)
                {
                    Console.WriteLine("Skipped.");
                    return;
                }
                File.SetLastWriteTime(mediaDateTime.Path, dateTime);
                Console.WriteLine($"{mediaDateTime.Path} - Ok.");
                return;
            }

            if (File.GetLastWriteTime(mediaDateTime.Path) == mediaDateTime.DateTime)
            {
                Console.WriteLine($"{mediaDateTime.Path} - Not required.");
                return;
            }

            File.SetLastWriteTime(mediaDateTime.Path, mediaDateTime.DateTime);
            Console.WriteLine("Ok.");
        }
        private static DateTime SetDateManually(string path, int year)
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

                Console.Write("Часы: ");
                int hours = int.Parse(Console.ReadLine()!);

                Console.Write("Минуты: ");
                int minutes = int.Parse(Console.ReadLine()!);

                Console.Write("Секунды: ");
                int seconds = int.Parse(Console.ReadLine()!);

                return new DateTime(year, month, day, hours, minutes, seconds);
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
    }
    public record class MediaDateTime(string Path, DateTime DateTime)
    {
        public bool IsDefault { get => DateTime == default; }
        public override string ToString()
        {
            string msg = !IsDefault ? "Success" : "Can't recognize.";
            return $"{Path} : {DateTime} : {msg}";
        }
    }
}
