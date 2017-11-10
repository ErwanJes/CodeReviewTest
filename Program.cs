namespace ConsoleApplication4
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static void GetLocalNotebook(string inputFile, string outputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
                throw new Exception("Input file not specified.");

            string filePath = Path.GetFullPath(inputFile);

            if (File.Exists(filePath) == false)
                throw new Exception($"Input file not found: \"{filePath}\"");

            File.WriteAllText(outputFile, filePath);

            Console.WriteLine($"Saved local notebook \"{filePath}\" to \"{outputFile}\".");
        }

        private static void GetRemoteNotebook(string url, string outputFile)
        {
            var uri = new Uri(url);

            File.WriteAllText(outputFile, (new WebClient()).DownloadString(uri));

            Console.WriteLine($"Downloaded remote notebook \"{uri.AbsoluteUri}\" to \"{outputFile}\".");
        }

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                    throw new Exception("Notebook [source] and [destination] arguments are required!");

                var source = args[0].Trim();
                var destination = args[1].Trim();

                if (Regex.Match(source.ToUpperInvariant(), "^HTTP://.+$").Success)
                    GetRemoteNotebook(source, destination);
                else
                    GetLocalNotebook(source, destination);
            }
            catch (Exception exn)
            {
                Console.WriteLine(exn.Message);
            }
        }
    }
}
