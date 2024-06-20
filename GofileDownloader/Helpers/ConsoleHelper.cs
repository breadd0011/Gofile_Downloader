using GofileDownloader.Exceptions;
using GofileDownloader.Utils;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace GofileDownloader.Helpers
{
    public class ConsoleHelper
    {
        public static void WriteError(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
            AnsiConsole.Console.Input.ReadKey(false);
        }
        public static string PromptUserForMenuChoice()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(4)
                    .AddChoices(
                        "[yellow]1.| Download from single url[/]",
                        "[yellow]2.| Download from multiple urls[/]",
                        "[yellow]3.| Change Token[/]",
                        "[yellow]4.| Exit[/]"
                    )
            );

            return choice;
        }
        public static string PromptUserForUrl()
        {
            var url = AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "[yellow]Paste your gofile url: [/]"
            ));

            if (Regex.Match(url, Constants.Regex.GOFILE_REGEX).Success)
            {
                return url;
            }
            else
            {
                throw new InvalidInputException("Url is invalid");
            }
        }
        public static string PromptUserForToken()
        {
            return 
                AnsiConsole.Prompt(
                    new TextPrompt<string>(
                        "[yellow]Enter your API token: [/]")
                );
        }
    }
}
