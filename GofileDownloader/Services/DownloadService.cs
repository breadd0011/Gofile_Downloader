namespace GofileDownloader.Services
{
    public class DownloadService : IDownloadService
    {
        public async Task DownloadAsync(string path, string url, string token, IProgress<double> progress)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Cookie", $"accountToken={token}");
                    var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                    var canReportProgress = totalBytes != -1;


                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[8192];
                        var isMoreToRead = true;

                        var fileName = GetNameFromUrl(url);
                        var savePath = Path.Combine(path, fileName);

                        using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true))
                        {
                            do
                            {
                                var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                                if (read == 0)
                                {
                                    isMoreToRead = false;
                                    progress.Report(100);
                                    continue;
                                }

                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;
                                if (canReportProgress)
                                {
                                    progress.Report((double)totalRead / totalBytes * 100);
                                }
                            }
                            while (isMoreToRead);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new NotImplementedException();
            }
            
        }

        private string GetNameFromUrl(string url)
        {
            int lastIndex = url.LastIndexOf("/");
            return url.Substring(lastIndex + 1);
        }
    }
}
