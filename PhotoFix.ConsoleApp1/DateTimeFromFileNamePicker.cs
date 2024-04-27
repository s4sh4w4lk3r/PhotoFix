using System.Text.RegularExpressions;

namespace PhotoFix.ConsoleApp1
{
    public class DateTimeFromFileNamePicker(string fileName)
    {
        public static DateTime GetDateTimeOrDefault(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            var picker = new DateTimeFromFileNamePicker(fileName);

            DateTime pattern1 = picker.Pattern1();
            if (pattern1 != default)
            {
                return pattern1;
            }

            DateTime pattern2 = picker.Pattern2();
            if (pattern2 != default)
            {
                return pattern2;
            }

            DateTime pattern3 = picker.Pattern3();
            if (pattern3 != default)
            {
                return pattern3;
            }


            return default;
        }
        private DateTime Pattern1()
        {
            // IMG_20200130_135321;
            Regex regex = new(@"(IMG|VID)_20[0-9]{6}_[0-9]{6}");
            if (!regex.IsMatch(fileName))
            {
                return default;
            }

            string[] parts = fileName.Split('_');
            if (parts.Length < 3)
            {
                return default;
            }

            string? dateStr = parts.ElementAtOrDefault(1);
            string? timeStr = parts.ElementAtOrDefault(2);

            if (dateStr is null || dateStr.Length != 8
                || timeStr is null || timeStr.Length != 6)
            {
                return default;
            }

            try
            {
                int year = int.Parse(dateStr.Substring(0, 4));
                int month = int.Parse(dateStr.Substring(4, 2));
                int day = int.Parse(dateStr.Substring(6, 2));
                int hour = int.Parse(timeStr.Substring(0, 2));
                int minute = int.Parse(timeStr.Substring(2, 2));
                int second = int.Parse(timeStr.Substring(4, 2));

                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (FormatException)
            {
                return default;
            }
            catch (ArgumentOutOfRangeException)
            {
                return default;
            }
        }

        private DateTime Pattern2()
        {
            // 2020-02-23_13.21.13
            Regex regex = new(@"[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}\.[0-9]{2}\.[0-9]{2}");

            if (!regex.IsMatch(fileName))
            {
                return default;
            }

            string[] parts = fileName.Split('_');
            if (parts.Length != 2)
            {
                return default;
            }

            string? dateStr = parts.ElementAtOrDefault(0);
            string? timeStr = parts.ElementAtOrDefault(1);

            if (dateStr is null || dateStr.Length != 10
                || timeStr is null || timeStr.Length != 8)
            {
                return default;
            }

            string[] dateParts = dateStr.Split('-');
            string[] timeParts = dateStr.Split('.');

            if (dateParts.Length != 3 || timeParts.Length < 4)
            {
                return default;
            }

            try
            {
                int year = int.Parse(dateParts.ElementAt(0));
                int month = int.Parse(dateParts.ElementAt(1));
                int day = int.Parse(dateParts.ElementAt(2));
                int hour = int.Parse(timeParts.ElementAt(0));
                int minute = int.Parse(timeParts.ElementAt(1));
                int second = int.Parse(timeParts.ElementAt(2));

                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (FormatException)
            {
                return default;
            }
            catch (ArgumentOutOfRangeException)
            {
                return default;
            }
        }

        private DateTime Pattern3()
        {
            // 2020-09-09_12.17.20
            Regex regex = new(@"[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}\.[0-9]{2}\.[0-9]+");

            if (!regex.IsMatch(fileName))
            {
                return default;
            }

            string[] parts = fileName.Split('_');
            if (parts.Length > 3)
            {
                return default;
            }

            string? dateStr = parts.ElementAtOrDefault(0);
            string? timeStr = parts.ElementAtOrDefault(1);

            if (dateStr is null || timeStr is null)
            {
                return default;
            }

            string[] dateParts = dateStr.Split('-');
            string[] timeParts = timeStr.Split('.');

            if (dateParts.Length != 3 || timeParts.Length > 4)
            {
                return default;
            }

            try
            {
                int year = int.Parse(dateParts.ElementAt(0));
                int month = int.Parse(dateParts.ElementAt(1));
                int day = int.Parse(dateParts.ElementAt(2));
                int hour = int.Parse(timeParts.ElementAt(0));
                int minute = int.Parse(timeParts.ElementAt(1));
                int second = int.Parse(timeParts.ElementAt(2));

                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (FormatException)
            {
                return default;
            }
            catch (ArgumentOutOfRangeException)
            {
                return default;
            }
        }
    }
}
