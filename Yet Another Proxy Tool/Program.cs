using Spectre.Console;
using Proxy_Scraper_and_Checker;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Yet Another Proxy Tool";
        Menu();
    }

    public static void Menu()
    {
        Logo();

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select options:")
                .AddChoices(new[] {
            "Scrape proxy", "Verify proxy", "View verified proxies",
                }));

        if (options == "Scrape proxy")
            ScrapeProxyLib.ScrapeProxy();
        else if (options == "Verify proxy")
            CheckProxyLib.CheckProxy();
        else if (options == "View verified proxies")
            ViewVerifiedProxy();
    }

    private static void ViewVerifiedProxy()
    {
        string RunTimePath = Environment.CurrentDirectory;
        string ProxyFolder = Path.Combine(RunTimePath, "Proxies");
        string VerfiedProxyFolder = Path.Combine(ProxyFolder, "Verified Proxy");

        if (!Directory.Exists(VerfiedProxyFolder)) 
        {
            Console.WriteLine($"Directory not exists");
            Console.WriteLine("");
            Console.WriteLine("Press enter to back to menu");
            Console.ReadLine();
            Program.Menu();
        }
        else
        {
            var ProxyListFiles = new List<string>();
            var files = Directory.GetFiles(VerfiedProxyFolder, "*.txt");
            foreach (var file in files)
            {
                ProxyListFiles.Add(Path.GetFileName(file.Replace("|", "|").Replace('꞉', ':')));
            }

            if (ProxyListFiles.Count == 0)
            {
                Console.WriteLine($"Proxy file not found");
                Console.WriteLine("");
                Console.WriteLine("Press enter to back to menu");
                Console.ReadLine();
                Program.Menu();
            }
            else
            {
                ProxyListFiles.Add("Back to menu");
                var proxyFile = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                 .Title("Select file:")
                 .PageSize(10)
                 .MoreChoicesText("Move up and down to reveal option")
                 .AddChoices(ProxyListFiles)
                 );

                if (proxyFile == "Back to menu")
                    Program.Menu();
                else
                {
                    var path = @$"{Environment.CurrentDirectory}\Proxies\Verified Proxy\";
                    proxyFile = proxyFile.Replace(':', '꞉');
                    var proxies = File.ReadLines(@$"{path}{proxyFile}");
                    Console.WriteLine("Verified proxies:");
                    Console.WriteLine("");
                    proxies.ToList().ForEach(proxy => AnsiConsole.MarkupLine($"{proxy}"));
                    Console.WriteLine("");
                    Console.WriteLine("Press enter to back to menu");
                    Console.ReadLine();
                    Program.Menu();
                }
            }
        }
       
    }

    public static void Logo()
    {
        Console.Clear();
        Console.WriteLine(@"         _       _               _   _                ");
        Console.WriteLine(@"/\_/\___| |_    /_\  _ __   ___ | |_| |__   ___ _ __  ");
        Console.WriteLine(@"\_ _/ _ \ __|  //_\\| '_ \ / _ \| __| '_ \ / _ \ '__| ");
        Console.WriteLine(@" / \  __/ |_  /  _  \ | | | (_) | |_| | | |  __/ |    ");
        Console.WriteLine(@" \_/\___|\__| \_/ \_/_| |_|\___/ \__|_| |_|\___|_|    ");
        Console.WriteLine(@"    ___                       _____            _      ");
        Console.WriteLine(@"   / _ \_ __ _____  ___   _  /__   \___   ___ | |     ");
        Console.WriteLine(@"  / /_)/ '__/ _ \ \/ / | | |   / /\/ _ \ / _ \| |     ");
        Console.WriteLine(@" / ___/| | | (_) >  <| |_| |  / / | (_) | (_) | |     ");
        Console.WriteLine(@" \/    |_|  \___/_/\_\\__, |  \/   \___/ \___/|_|     ");
        Console.WriteLine(@"                      |___/                           ");
        Console.WriteLine("");
        AnsiConsole.MarkupLine("Source: [mediumorchid3]github.com/pearlxcore/YetAnotherProxyTool[/]");
        AnsiConsole.MarkupLine("Credit: [darkslategray1]miticyber[/] and [darkslategray1]TheC0mpany[/]");
        Console.WriteLine("");
    }


}

