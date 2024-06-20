using GofileDownloader.Utils;

namespace GofileDownloader.Helpers
{
    public class UrlHelper
    {
        public static List<string>? LoadUrlsFromFile()
        {
            if (IsUrlFileExists())
            {
                var urlList = new List<string>(File.ReadAllLines(Constants.Paths.URLS_PATH));
                if (urlList.Count <= 0)
                {
                    return null;
                }
                return urlList;
            }
            else
            {
                GenerateUrlFile();
                return null;
            }
        }

        private static bool IsUrlFileExists()
        {
            return File.Exists(Constants.Paths.URLS_PATH);
        }

        private static void GenerateUrlFile()
        {
            using FileStream fs = File.Create(Constants.Paths.URLS_PATH);
        }
    }
}
