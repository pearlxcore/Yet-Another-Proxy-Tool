using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Proxy_Scraper_and_Checker
{
    public class CheckProxyLib
    {
        internal class Helper
        {
            internal static string[] proxies;
            internal static int goodProxy;
            internal static int badProxy;
            internal static List<string> goodProxtList = new List<string>();
        }

        public static void CheckProxy()
        {
            Console.Clear();
            Program.Logo();
            Helper.goodProxtList.Clear();
            Helper.goodProxy = 0;
            Helper.badProxy = 0;
            Helper.proxies = null;
            string RunTimePath = Environment.CurrentDirectory;
            string ProxyFolder = Path.Combine(RunTimePath, "Proxies");
            if (!Directory.Exists(ProxyFolder)) Directory.CreateDirectory(ProxyFolder);
            var ProxyListFiles = new List<string>();
            var files = Directory.GetFiles(ProxyFolder, "*.txt");
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
                    var proxies = File.ReadLines(@$"{Environment.CurrentDirectory}\Proxies\{proxyFile.Replace(':', '꞉')}");
                    var threads = new List<Thread>();
                    foreach (var proxy in proxies)
                    {
                        if (proxy != "" || proxy != String.Empty)
                            threads.Add(new Thread(() => Checker(proxy)));
                    }
                    AnsiConsole.MarkupLine($"Found [springgreen2]{threads.Count}[/] proxies");
                    Console.WriteLine("");
                    foreach (Thread t in threads)
                        t.Start();
                    foreach (Thread t in threads)
                        t.Join();

                    Console.WriteLine("");
                    AnsiConsole.MarkupLine($"[lightgoldenrod2_2]Total proxies[/]: [springgreen2]{(Helper.badProxy + Helper.goodProxy).ToString()}[/]");
                    AnsiConsole.MarkupLine($"[green]OK[/]: [springgreen2]{Helper.goodProxy.ToString()}[/]");
                    AnsiConsole.MarkupLine($"[red]FAILED[/]: [springgreen2]{Helper.badProxy.ToString()}[/]");
                    Console.WriteLine("");

                    if (Helper.goodProxtList.Count > 0)
                        WriteToFile(Helper.goodProxtList);
                    Console.WriteLine("");
                    var options = AnsiConsole.Prompt(
                      new SelectionPrompt<string>()
                          .Title("Select options:")
                          .AddChoices(new[] {
            "Back to menu",
                          }));

                    Program.Menu();
                }
            }
        }

        private static void Checker(string proxy)
        {
            if(proxy != "" || proxy != String.Empty)
            {
                string testUrl = "https://google.com";
                string proxyType;
                HttpRequest request = new HttpRequest();

                try
                {
                    //http
                    request.Proxy = HttpProxyClient.Parse(proxy);
                    proxyType = "HTTP";

                    ////socks4
                    //request.Proxy = Socks4ProxyClient.Parse(proxy);
                    //proxyType = "Socks4";

                    ////socks5
                    //request.Proxy = Socks5ProxyClient.Parse(proxy);
                    //proxyType = "Socks5";


                    var response = request.Get(testUrl).ToString();
                    AnsiConsole.MarkupLine($"{proxy} : [green]OK[/]");
                    Helper.goodProxy++;
                    Helper.goodProxtList.Add(proxy);
                }
                catch (FormatException)
                {
                    //proxy format error, dont care
                }
                catch (HttpException)
                {
                    AnsiConsole.MarkupLine($"{proxy} : [red]FAILED[/]");
                    Helper.badProxy++;
                }
            }
        }

        private static void WriteToFile(List<string> proxies)
        {
            var text = "";
            proxies.ForEach(proxy => text += proxy + "\n");
            string resultFolder = @$"{Environment.CurrentDirectory}\Proxies\Verified Proxy\";
            if (!Directory.Exists(resultFolder))
                Directory.CreateDirectory(resultFolder);

            string currentTime = DateTime.Now.ToString("dd-MM-yyyy@h꞉mm꞉sstt");
            string fileName = @$"{resultFolder}Verified_{currentTime}.txt";

            using (StreamWriter writer = File.CreateText(fileName))
            {
                writer.Write(text);
            }
            AnsiConsole.MarkupLine($"Verified proxies saved to: \n[cyan]{fileName.Replace('꞉',':')}[/]");
        }
    }
}
