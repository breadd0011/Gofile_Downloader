using GofileDownloader.Exceptions;
using GofileDownloader.Utils;
using System.Net.NetworkInformation;

namespace GofileDownloader.Helpers
{
    public class APIHelper
    {
        public static async Task<string> GetDataAsync(string url, string token)
        {
            if (IsInternetAvailable())
            {
                using (HttpClient client = new())
                {
                    if (token.All(char.IsAscii))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                        HttpResponseMessage responseMessage = await client.GetAsync($"{Constants.GOFILE_API}/contents/{GetIdFromUrl(url)}?wt=4fd6sg89d7s6");

                        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new InvalidTokenException("Token is invalid");
                        }
                        else
                        {
                            return await responseMessage.Content.ReadAsStringAsync();
                        }
                    }
                    throw new InvalidTokenException("Token is invalid");
                }
            }
            else
            {
                throw new NoInternetException("No internet connection");
            }
        }
        public static string GetIdFromUrl(string url)
        {
            return url.Replace("https://gofile.io/d/", "");
        }
        private static bool IsInternetAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
