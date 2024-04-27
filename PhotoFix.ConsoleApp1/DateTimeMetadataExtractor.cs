using MetadataExtractor.Formats.Exif;
using MetadataExtractor;

namespace PhotoFix.ConsoleApp1
{
    public class DateTimeMetadataExtractor(string filePath)
    {
        public DateTime GetDateTimeOrDefault()
        {
            string extension = Path.GetExtension(filePath);
            return extension switch
            {
                ".mp4" => GetDateTimeFromMp4(),
                ".jpeg" or ".jpg" => GetDateTimeFromJpeg(),
                ".png" => GetDateTimeFromPng(),
                _ => default
            };
        }

        private DateTime GetDateTimeFromMp4()
        {
            /* using ShellObject shell = ShellObject.FromParsingName(path);

             DateTime dateTime = shell.Properties.System.Media.DateEncoded.Value ?? default;
             return dateTime.Year == 2019 ? dateTime : default;*/
            return default;
        }

        private DateTime GetDateTimeFromJpeg()
        {
            using var stream = File.OpenRead(filePath);
            var dirs = ImageMetadataReader.ReadMetadata(stream);

            foreach (var item in dirs)
            {
                if (item.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dt))
                {
                    return dt;
                }
            }
            return default;
        }

        private DateTime GetDateTimeFromPng()
        {
            return default;
        }
    }
}
