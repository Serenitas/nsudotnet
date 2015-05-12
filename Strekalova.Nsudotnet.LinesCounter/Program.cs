using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Strekalova.Nsudotnet.LinesCounter
{
    class Program
    {
        private static string _fileExtension;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("В качестве аргумента программы должно быть указано расширение учитываемых файлов");
                return;
            }

            _fileExtension = args[0];

            var totalLinesCount = GetLinesCountInDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()));

            Console.WriteLine(string.Concat("Количество осмысленных строк в файлах формата \"", _fileExtension, "\" в текущей директории и поддиректориях: ", totalLinesCount));
        }

        static long GetLinesCountInDirectory(DirectoryInfo dir)
        {
            long linesCount = 0;

            try
            {
                var subdirs = dir.GetDirectories();
                linesCount += subdirs.Sum(directory => GetLinesCountInDirectory(directory));

                var files = dir.GetFiles(string.Concat("*.", _fileExtension));
                linesCount += files.Sum(file => GetLinesCountInFile(file));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return linesCount;
        }

        static long GetLinesCountInFile(FileInfo file)
        {
            long linesCount = 0;
            var regex = new Regex("\\/\\*[^\\/]*[^\\*]*\\*\\/|\\/\\/[^\n]*");
            using (TextReader reader = new StreamReader(file.OpenRead()))
            {
                string currString;
                bool isComment = false;
                while ((currString = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(currString))
                        continue;
                    if (isComment)
                        currString = string.Concat("/*", currString);
                    currString = regex.Replace(currString, "");
                    if (currString.Contains("/*"))
                    {
                        isComment = true;
                        currString = string.Concat(currString, "*/");
                        currString = regex.Replace(currString, "");
                    }
                    else
                        isComment = false;
                    if (!string.IsNullOrWhiteSpace(currString))
                        linesCount++;
                }
            }
            return linesCount;
        }
    }
}