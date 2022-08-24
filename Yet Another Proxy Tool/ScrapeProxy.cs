using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spectre.Console;

namespace Proxy_Scraper_and_Checker
{
    public class ScrapeProxyLib
    {
        public class Helper
        {
            internal static List<string> Proxies = new List<string>();
            internal static List<string> ProxiesUrls = new List<string>();

            internal static string GetBetween(string strSource, string strStart, string strEnd)
            {
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    int Start, End;
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                }
                return "";
            }
        }

        public class ProxyType
        {
            private class CheckerProxyJSON
            {
                public int id { get; set; }
                public int local_id { get; set; }
                public string report_id { get; set; }
                public string addr { get; set; }
                public int type { get; set; }
                public int kind { get; set; }
                public int timeout { get; set; }
                public bool cookie { get; set; }
                public bool referer { get; set; }
                public bool post { get; set; }
                public string ip { get; set; }
                public string addr_geo_iso { get; set; }
                public string addr_geo_country { get; set; }
                public string addr_geo_city { get; set; }
                public string ip_geo_iso { get; set; }
                public string ip_geo_country { get; set; }
                public string ip_geo_city { get; set; }
                public DateTime created_at { get; set; }
                public DateTime updated_at { get; set; }
                public bool skip { get; set; }
                public bool from_cache { get; set; }
            }

            internal static void OpenProxy()
            {
                var timeStart = DateTime.Now;
                int count = 0;
                MatchCollection mc = Regex.Matches(
                new WebClient().DownloadString("https://openproxy.space/list/http") +
                new WebClient().DownloadString("https://openproxy.space/list/socks4") +
                new WebClient().DownloadString("https://openproxy.space/list/socks5"),
                @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b:\d{1,6}");

                foreach (Match m in mc)
                {
                    Helper.Proxies.Add(m.ToString());
                    count += 1;
                }

                var timespan = timeStart - DateTime.Now;
                AnsiConsole.Markup($"[deepskyblue3]OpenProxy.Space[/], [springgreen2]{count}[/] proxies scraped.");
                AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

            }

            internal static void ProxyNova()
            {
                var timeStart = DateTime.Now;

                int count = 0;
                string url = "https://www.proxynova.com/proxy-server-list/";
                string data = new WebClient().DownloadString(url);
                string[] lines = data.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i].Contains("<script>"))
                    {
                        if (ip.Matches(lines[i]).Count > 0)
                        {
                            try
                            {
                                var engine = new Jurassic.ScriptEngine();
                                string script = lines[i].Replace("<script>", "").Replace(")</script>", "").Replace("document.write(", "");

                                Helper.Proxies.Add(Regex.Replace(engine.Evaluate(script) + ":" + Helper.GetBetween(data.Replace("title=\"Port 8080 proxies\">8080</a>", "").Replace("title=\"Port 80 proxies\">80</a>", "").Replace("title=\"Port 3128 proxies\">3128</a>", ""), lines[i], "<time"), @"[^0-9\s.:]", "").Replace("\n", "").Replace(" ", ""));
                                count += 1;
                            }
                            catch { }
                        }
                    }
                }
                var timespan = timeStart - DateTime.Now;

                AnsiConsole.Markup($"[deepskyblue3]ProxyNova.Com[/], [springgreen2]{count}[/] proxies scraped.");
                AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

            }

            internal static void ProxyScrapeCom()
            {
                var timeStart = DateTime.Now;

                string data = Regex.Replace(new WebClient().DownloadString("https://api.proxyscrape.com/proxytable.php").ToLower().Replace("\":1", "\n").Replace("\":2", "\n").Replace("\":3", "\n").Replace("\"http\":", "").Replace("\"https\":", "").Replace("\"socks4\":", "").Replace("\"socks5\":", ""), @"[^0-9\s.:]", "");
                Helper.Proxies.Add(data);

                var timespan = timeStart - DateTime.Now;
                AnsiConsole.Markup($"[deepskyblue3]CheckerProxy.Net[/], [springgreen2]{data.Split('\n').Count()}[/] proxies scraped.");
                AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");
            }

            internal static void SocksProxyPack()
            {
                var timeStart = DateTime.Now;

                string one = Helper.GetBetween(new WebClient().DownloadString("https://www.socks-proxy.net/"), " UTC.", "</textarea>");
                Helper.Proxies.Add(one);
                var timespan = timeStart - DateTime.Now;

                AnsiConsole.Markup($"[deepskyblue3]Socks-Proxy.Net[/], [springgreen2]{one.Split('\n').Count()}[/] proxies scraped.", ConsoleColor.Green);
                AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

                string two = Helper.GetBetween(new WebClient().DownloadString("https://free-proxy-list.net/"), " UTC.", "</textarea>");
                Helper.Proxies.Add(two);
                var timespan2 = timeStart - DateTime.Now;

                AnsiConsole.Markup($"[deepskyblue3]Free-Proxy-List.Net[/], [springgreen2]{two.Split('\n').Count()}[/] proxies scraped.", ConsoleColor.Green);
                AnsiConsole.MarkupLine($"   ([yellow]{timespan2.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

                string three = Helper.GetBetween(new WebClient().DownloadString("https://www.sslproxies.org/"), " UTC.", "</textarea>");
                Helper.Proxies.Add(three);
                var timespan3 = timeStart - DateTime.Now;

                AnsiConsole.Markup($"[deepskyblue3]SslProxies.Org[/], [springgreen2]{three.Split('\n').Count()}[/] proxies scraped.", ConsoleColor.Green);
                AnsiConsole.MarkupLine($"   ([yellow]{timespan3.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

            }

            internal static void HideMyName()
            {
                var timeStart = DateTime.Now;

                string url = "https://hidemy.name/en/proxy-list/";
                var output = "";
                var data = new WebClient();
                data.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 OPR/71.0.3770.323");
                //{
                //    CharacterSet = Encoding.UTF8,
                //    KeepAlive = true,
                //    KeepAliveTimeout = 1000,
                //    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 OPR/71.0.3770.323"
                //};
                string response = data.DownloadString(url);
                var matches = Regex.Matches(response, @"(\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}</td>)|(\d{2,5}</td>)");
                int matchesCount = matches.Count;
                for (int i = 0; i < matchesCount; i += 2)
                    if (!Regex.Match(matches[i].Value, @"([A-Za-z-])").Success || matches[i].Value.Contains("<"))
                        output += matches[i].Value.Replace("</td>", "") + ":" + matches[i + 1].Value.Replace("</td>", "") + Environment.NewLine;
                Helper.Proxies.Add(output);
                var timespan = timeStart - DateTime.Now;

                AnsiConsole.Markup($"[deepskyblue3]HideMy.Name[/], [springgreen2]{output.Split('\n').Count()}[/] proxies scraped.", ConsoleColor.Green);
                AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

            }

            internal static void InstaBypass()
            {
                string[] instabypass = new WebClient().DownloadString("https://www.instabypass.com/proxy/proxy/?C=M;O=D").Split('\n');
                int x = int.Parse(DateTime.Now.ToString("dd"));
                for (int i = 0; i < x; i++)
                    if (x != 1)
                    {
                        x--;
                        for (int a = 0; a < 50; a++)
                            if (instabypass[a].Contains(DateTime.Now.ToString("yyyy-MM") + "-0" + x))
                            {
                                Helper.ProxiesUrls.Add("https://www.instabypass.com/proxy/proxy/" + Helper.GetBetween(Helper.GetBetween(instabypass[a], "<tr><td valign=\"top\">&nbsp;</td><td><a href=", "</td><td align=\"right\">" + DateTime.Now.ToString("yyyy-MM") + "-0" + x), "\"", "\">"));
                                AnsiConsole.MarkupLine(" URL added: https://www.instabypass.com/proxy/proxy/" + Helper.GetBetween(Helper.GetBetween(instabypass[a], "<tr><td valign=\"top\">&nbsp;</td><td><a href=", "</td><td align=\"right\">" + DateTime.Now.ToString("yyyy-MM") + "-0" + x), "\"", "\">"), ConsoleColor.Green);
                            }
                    }
            }

            internal static void Proxs()
            {
                var timeStart = DateTime.Now;

                string data = Regex.Replace(Helper.GetBetween(new WebClient().DownloadString("https://proxs.ru/freeproxy.php"), "<table><tr><td>", "</td></tr></table></div><br /><br />").Replace("</td><td width=1%><div class=\"proxy-flag\"></div></td><td>&nbsp;", "\n"), @"[^0-9\s.:]", "");
                Helper.Proxies.Add(data);
                var timespan = timeStart - DateTime.Now;

                AnsiConsole.Markup($"[deepskyblue3]Proxs.Ru[/], [springgreen2]{data.Split('\n').Count()}[/] proxies scraped.", ConsoleColor.Green);
                AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

            }

            internal static void CheckerProxy()
            {

                string data = new WebClient().DownloadString("https://checkerproxy.net/getAllProxy");
                try
                {
                    var timeStart = DateTime.Now;

                    int count = 0;
                    string one = new WebClient().DownloadString("https://checkerproxy.net/api" + Helper.GetBetween(data, "</p><ul><li><a href=\"", "\">"));
                    var jsoninfo = JsonConvert.DeserializeObject<List<CheckerProxyJSON>>(one);
                    for (int z = 0; z < jsoninfo.Count; z++)
                    {
                        Helper.Proxies.Add(Regex.Replace(jsoninfo[z].addr, @"[^0-9\s.:]", ""));
                        count += 1;
                    }
                    var timespan = timeStart - DateTime.Now;

                    AnsiConsole.Markup($"[deepskyblue3]CheckerProxy.Net 1#[/], [springgreen2]{count}[/] proxies scraped.", ConsoleColor.Green);
                    AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

                }
                catch { }
                try
                {
                    var timeStart = DateTime.Now;

                    int count = 0;
                    string two = new WebClient().DownloadString("https://checkerproxy.net/api" + Helper.GetBetween(data, "</a></li><li><a href=\"", "\">"));
                    var jsoninfo = JsonConvert.DeserializeObject<List<CheckerProxyJSON>>(two);
                    for (int z = 0; z < jsoninfo.Count; z++)
                    {
                        Helper.Proxies.Add(Regex.Replace(jsoninfo[z].addr, @"[^0-9\s.:]", ""));
                        count += 1;
                    }
                    var timespan = timeStart - DateTime.Now;

                    AnsiConsole.Markup($"[deepskyblue3]CheckerProxy.Net 2#[/], [springgreen2]{count}[/] proxies scraped.", ConsoleColor.Green);
                    AnsiConsole.MarkupLine($"   ([yellow]{timespan.TotalSeconds.ToString("##.##").Replace("-", "")} second[/])");

                }
                catch { }
            }
        }

        public static void ScrapeProxy()
        {
            var proxiesDir = $"{Environment.CurrentDirectory}/Proxies/";
            if (!Directory.Exists(proxiesDir))
                Directory.CreateDirectory(proxiesDir);

            var proxySources = AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
        .Title("Select sources:")
        .NotRequired() // Not required to have a favorite fruit
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        .InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a option, " +
            "[green]<enter>[/] to accept)[/]")
        .AddChoices(new[] {
            "OpenProxy", "ProxyNova", "ProxyScrapeCom",
            "SocksProxyPack", "HideMyName", "InstaBypass",
            "Proxs", "CheckerProxy",
        }));

            List<Thread> Threads = new List<Thread>();
            foreach (string source in proxySources)
            {
                if(source == "OpenProxy")
                    Threads.Add(new Thread(() => ProxyType.OpenProxy()));
                if (source == "ProxyNova")
                    Threads.Add(new Thread(() => ProxyType.ProxyNova())); 
                if (source == "ProxyScrapeCom")
                    Threads.Add(new Thread(() => ProxyType.ProxyScrapeCom())); 
                if (source == "SocksProxyPack")
                    Threads.Add(new Thread(() => ProxyType.SocksProxyPack())); 
                if (source == "HideMyName")
                    Threads.Add(new Thread(() => ProxyType.HideMyName())); 
                if (source == "InstaBypass")
                    Threads.Add(new Thread(() => ProxyType.InstaBypass())); 
                if (source == "Proxs")
                    Threads.Add(new Thread(() => ProxyType.Proxs()));
                if (source == "CheckerProxy")
                    Threads.Add(new Thread(() => ProxyType.CheckerProxy()));
            }

            foreach (Thread t in Threads)
                t.Start();
            foreach (Thread t in Threads)
                t.Join();

            Helper.Proxies.RemoveAll(s => string.IsNullOrEmpty(s));
            string filename = $"proxies({DateTime.Now.ToShortDateString()}@{DateTime.Now.ToShortTimeString()}).txt";
            filename = filename.Replace('/', '-').Replace(" ", "").Replace(':', '꞉');

            Console.WriteLine("");
            AnsiConsole.MarkupLine($"Total of {Helper.Proxies.Count} proxies scraped");
            File.WriteAllText($"{Environment.CurrentDirectory}/Proxies/{filename}", string.Join(Environment.NewLine, Helper.Proxies.Distinct().ToArray()));
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            AnsiConsole.WriteLine($"Scraped proxies saved to:");
            AnsiConsole.MarkupLine($"[cyan]\"{Environment.CurrentDirectory}/Proxies/{filename.Replace('꞉', ':')}\"[/]");

            Console.WriteLine("");

            var options = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                   .Title("Select options:")
                   .AddChoices(new[] {
            "Validate proxy", "Back to menu",
                   }));

            if (options == "Back to menu")
                Program.Menu();
            else if (options == "Validate proxy")
                CheckProxyLib.CheckProxy();
        }

    }
}
