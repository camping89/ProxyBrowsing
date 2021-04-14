using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProxy;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"TestSeleniumProxyServer at {DateTime.Now}");
            // Create a local proxy server
            var proxyServer = new SeleniumProxyServer();

            // Don't await, have multiple drivers at once using the local proxy server
            StartSeleniumProxyServer(proxyServer, new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP"));
            //TestSeleniumProxyServer(proxyServer, new ProxyAuth("11.22.33.44", 80, "prox-username2", "proxy-password2"));
            //TestSeleniumProxyServer(proxyServer, new ProxyAuth("111.222.222.111", 80, "prox-username3", "proxy-password3"));

            while (true) { }
        }

        private static async Task StartSeleniumProxyServer(SeleniumProxyServer proxyServer, ProxyAuth auth)
        {
            // Add a new local proxy server endpoint
            var localPort = proxyServer.AddEndpoint(auth);

            ChromeOptions options = new ChromeOptions();
            //options1.AddArguments("headless");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);

            // Configure the driver's proxy server to the local endpoint port
            options.AddArgument($"--proxy-server=127.0.0.1:{localPort}");
            options.AddArgument("--ignore-ssl-errors=yes");
            options.AddArgument("--ignore-certificate-errors");



            // Optional
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            // Create the driver
            var driver = new ChromeDriver(service, options);

            // Test if the driver is working correctly
            driver.Navigate().GoToUrl("https://www.myip.com/");

            var js = (IJavaScriptExecutor)driver;
            string title2 = (string)js.ExecuteScript($"document.title = '{auth.Proxy}:{auth.Port}'");

            //await Task.Delay(5000);
            //driver.Navigate().GoToUrl("https://amibehindaproxy.com/");
            //await Task.Delay(5000);

            // Dispose the driver
            //driver.Dispose();
        }
    }
}
