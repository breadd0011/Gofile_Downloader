namespace GofileDownloader.Utils
{
    public class Constants
    {
        public const string GOFILE_API = "https://api.gofile.io";
        public static class Paths
        {
            public static readonly string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory, "config.json");
            public static readonly string URLS_PATH = Path.Combine(Environment.CurrentDirectory, "urls.txt");
            public static readonly string DOWNLOAD_PATH = Path.Combine(Environment.CurrentDirectory, "downloads");
        }

        public static class Regex
        {
            public static readonly string GOFILE_REGEX = "https:\\/\\/gofile\\.io\\/d\\/[a-zA-Z0-9]+";
        }
    }
}
