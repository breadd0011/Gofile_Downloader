using GofileDownloader.Exceptions;
using GofileDownloader.Helpers;
using GofileDownloader.Models;
using GofileDownloader.Services;
using GofileDownloader.Utils;
using Spectre.Console;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GoFileDownloader
{
    public class App
    {
        private static Config _config = new();
        private static readonly DownloadService _downloadService = new();
        public static async Task Main()
        {
            var returnBack = true;
            do
            {
                Initialize();

                var choice = ConsoleHelper.PromptUserForMenuChoice();
                switch (choice)
                {
                    case ("[yellow]1.| Download from single url[/]"):
                        await DownloadSingleUrlOption();
                        break;

                    case ("[yellow]2.| Download from multiple urls[/]"):
                        await DownloadMultipleUrlOption();
                        break;

                    case ("[yellow]3.| Change Token[/]"):
                        ChangeToken();
                        break;

                    case ("[yellow]4.| Exit[/]"):
                        returnBack = false;
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }

            } while (returnBack);
        }
        private static void Initialize()
        {
            AnsiConsole.Clear();
            _config = ConfigHelper.LoadConfig();
        }
        private static async Task DownloadSingleUrlOption()
        {
            try
            {
                // Get URL from user and check if working
                string url = ConsoleHelper.PromptUserForUrl();
                string response = await GetResponse(url);
                GofileDataModel gofileData = JsonSerializer.Deserialize<GofileDataModel>(response);

                // Add files to list
                List<GofileDataModel.Child> fileList = [];
                foreach (var item in gofileData.Data.Children.Values)
                {
                    fileList.Add(item);
                }

                await DownloadFiles(fileList);
            }
            catch (InvalidInputException ex)
            {
                ConsoleHelper.WriteError(ex.Message);
                return;
            }
            catch (InvalidTokenException ex)
            {
                ConsoleHelper.WriteError(ex.Message);
                return;
            }
        }
        private static async Task DownloadMultipleUrlOption()
        {
            try
            {
                // Load Urls from file if theres any
                List<string>? urls = UrlHelper.LoadUrlsFromFile();
                if (urls == null)
                {
                    ConsoleHelper.WriteError("There are no links in your urls.txt file");
                    return;
                }

                // Validate loaded Urls
                List<string> validUrls = [];
                foreach (var url in urls)
                {
                    bool isValid = Regex.Match(url, Constants.Regex.GOFILE_REGEX).Success;
                    if (isValid)
                    {
                        validUrls.Add(url);
                    }
                }

                if (validUrls.Count <= 0)
                {
                    ConsoleHelper.WriteError("There are no valid urls in your urls.txt");
                    return;
                }

                // Ask user if wants to continue if any of the links were bad
                if (urls.Count != validUrls.Count && !AnsiConsole.Confirm($"[red]Only {validUrls.Count} out of {urls.Count} are valid URL's from your urls.txt. Do you wish to continue?[/]"))
                {
                    return;
                }


                List<GofileDataModel.Child> fileList = [];
                foreach (var url in validUrls)
                {
                    // Get API responses from urls
                    GofileDataModel gofileData = JsonSerializer.Deserialize<GofileDataModel>(
                        await GetResponse(url));

                    // Add file download URLs from response
                    foreach (var item in gofileData.Data.Children.Values)
                    {
                        fileList.Add(item);
                    }

                }

                await DownloadFiles(fileList);
            }
            catch (InvalidInputException ex)
            {
                ConsoleHelper.WriteError(ex.Message);
                return;
            }
            catch (InvalidTokenException ex)
            {
                ConsoleHelper.WriteError(ex.Message);
                return;
            }
            
        }
        private static void ChangeToken()
        {
            Config config = new() { Token = ConsoleHelper.PromptUserForToken() };
            ConfigHelper.SaveConfig(config);
        }

        private static async Task DownloadFiles(IEnumerable<GofileDataModel.Child> fileList)
        {
            if (!Directory.Exists(Constants.Paths.DOWNLOAD_PATH))
            {
                Directory.CreateDirectory(Constants.Paths.DOWNLOAD_PATH);
            }

            await AnsiConsole.Progress()
               .AutoClear(false)
               .HideCompleted(false)
               .Columns(new ProgressColumn[]
               {
                   new TaskDescriptionColumn { Alignment = Justify.Left},
                   new ProgressBarColumn(),
                   new PercentageColumn(),
                   new RemainingTimeColumn(),
               })
               .StartAsync(async ctx =>
               {
                   var maxLength = fileList.Max(file => file.Name.Length);

                   var tasks = fileList.Select(file =>
                   {
                       var paddedName = file.Name.PadRight(maxLength);
                       var progressTask = ctx.AddTask($"[yellow]{paddedName}[/]", autoStart: false);

                       return Task.Run(async () =>
                       {
                           var progress = new Progress<double>(value =>
                           {
                               progressTask.Value = value;
                           });

                           progressTask.StartTask();

                           var savePath = Path.Combine(Constants.Paths.DOWNLOAD_PATH, file.ParentFolder);
                           Directory.CreateDirectory(savePath);
                           await _downloadService.DownloadAsync(savePath, file.Link, _config.Token, progress);

                           progressTask.StopTask();
                       });
                   });

                   await Task.WhenAll(tasks);
               });

            AnsiConsole.MarkupLine("[green]All downloads have finished, press any key to go back to the menu.[/]");
            AnsiConsole.Console.Input.ReadKey(false);
        }
        private static async Task<string> GetResponse(string url)
        {
            try
            {
                return await APIHelper.GetDataAsync(url, _config.Token);
            }
             catch (InvalidTokenException)
            {
                throw;
            }
        }
    }
}