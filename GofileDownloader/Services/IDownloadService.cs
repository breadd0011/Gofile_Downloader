namespace GofileDownloader.Services
{
    public interface IDownloadService
    {
        Task DownloadAsync(string path, string url, string token, IProgress<double> progress);
    }
}
