using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SeleniumProxy;

namespace ConsoleApp
{
    // https://stackoverflow.com/questions/60117232/selenium-google-login-block
    // https://stackoverflow.com/questions/59514049/unable-to-sign-into-google-with-selenium-automation-because-of-this-browser-or/60328992#60328992
    // https://gist.github.com/smartdev10/d7323163c8346c97a9625b18d04ebbc0
    class Program
    {
        private const string _gmailUid = "tech.bifrost@gmail.com";
        private const string _gmailPwd = "T7PKJ7vRn26fre";

        static async Task Main(string[] args)
        {
            Console.WriteLine($"TestSeleniumProxyServer at {DateTime.Now}");
            // Create a local proxy server
            var proxyServer = new SeleniumProxyServer();

            // Don't await, have multiple drivers at once using the local proxy server
            StartSeleniumProxyServer(proxyServer, new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP"));
            //StartSeleniumProxyServerFireFox(proxyServer, new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP"));
            //StartSeleniumProxyServer(proxyServer, new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP"));
            while (true) { }
        }

        private static async Task StartSeleniumProxyServer(SeleniumProxyServer proxyServer, ProxyAuth auth)
        {
            // Add a new local proxy server endpoint
            var localPort = proxyServer.AddEndpoint(auth);
            
            ChromeOptions options = new ChromeOptions();

            // Configure the driver's proxy server to the local endpoint port
            //options.AddArguments("headless");
            options.AddArgument($"--proxy-server=127.0.0.1:{localPort}");
            options.AddArgument("--ignore-ssl-errors=yes");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--allow-running-insecure-content");
            //options.AddArgument("--remote-debugging-port=9222");
            //options.AddArgument(@"user-data-dir=C:\users\campi\AppData\Local\Google\Chrome\User Data");
            options.AddArgument(@"user-data-dir=F:\ChromeUserData");

            //options.AddArgument("--disable-popup-blocking");
            //options.AddArgument("--log-level=3"); // to shut the logging

            // disable
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);
            //options.AddExcludedArgument("--remote-debugging-port");
            //options.AddExcludedArgument("--remote-debugging-port=9222");

            //win.loadURL('https://path/to/your/auth/endpoint', { userAgent: 'Chrome' })
            // win.loadURL(`/ url / path`, { userAgent: 'Chrome' });

            /*
             *In case of electron it looks like the issue is caused by the chrome version from user agent

            Electron default user agent is: 
            Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) 
            old-airport-include/1.0.0 Chrome/78.0.3904.130 Electron/7.1.7 Safari/537.36

            If I remove the Chrome version (/78.0.3904.130 ) i’m able to login:
            mainWindow.loadURL('https://accounts.google.com/signin/v2/sl', 
            {userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 
            (KHTML, like Gecko) old-airport-include/1.0.0 Chrome Electron/7.1.7 Safari/537.36'})
             *
             */

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            // Create the driver
            var driver = new ChromeDriver(service, options);

            // login into gmail using stackoverflow
            driver.Navigate().GoToUrl("https://stackoverflow.com/");
            await Task.Delay(1000);
            driver.FindElement(By.XPath("/html/body/header/div/ol[2]/li[2]/a[1]")).Click();
            await Task.Delay(1000);
            driver.FindElement(By.XPath(@"//*[@id='openid-buttons']/button[1]")).Click();
            driver.FindElement(By.XPath(@"//*[@id='identifierId']")).SendKeys(_gmailUid);
            driver.FindElement(By.XPath(@"//*[@id='identifierNext']/div/button/div[2]")).Click();
            driver.FindElement(By.XPath(@"//*[@id='password']/div[1]/div/div[1]/input")).SendKeys(_gmailPwd);
            driver.FindElement(By.XPath(@"//*[@id='passwordNext']/div/button/div[2]")).Click();

            //var js = (IJavaScriptExecutor)driver;
            //string title2 = (string)js.ExecuteScript($"document.title = '{auth.Proxy}:{auth.Port}'");

            //await Task.Delay(5000);
            //driver.Navigate().GoToUrl("https://amibehindaproxy.com/");
            //await Task.Delay(5000);

            // Dispose the driver
            //driver.Dispose();
        }

        private static async Task StartSeleniumProxyServerFireFox(SeleniumProxyServer proxyServer, ProxyAuth auth)
        {
            // Add a new local proxy server endpoint
            var localPort = proxyServer.AddEndpoint(auth);

            FirefoxOptions options = new FirefoxOptions();
            //options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);

            // Configure the driver's proxy server to the local endpoint port
            options.AddArgument($"--proxy-server=127.0.0.1:{localPort}");
            options.AddArgument("--ignore-ssl-errors=yes");
            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--incognito");

            //  --remote-debugging-port=9222 
            // Optional

            //win.loadURL('https://path/to/your/auth/endpoint', { userAgent: 'Chrome' })
            // win.loadURL(`/ url / path`, { userAgent: 'Chrome' });

            /*
             *In case of electron it looks like the issue is caused by the chrome version from user agent

            Electron default user agent is: 
            Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) 
            old-airport-include/1.0.0 Chrome/78.0.3904.130 Electron/7.1.7 Safari/537.36

            If I remove the Chrome version (/78.0.3904.130 ) i’m able to login:
            mainWindow.loadURL('https://accounts.google.com/signin/v2/sl', 
            {userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 
            (KHTML, like Gecko) old-airport-include/1.0.0 Chrome Electron/7.1.7 Safari/537.36'})
             *
             */

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            // Create the driver
            var driver = new FirefoxDriver(options);

            // Test if the driver is working correctly
            driver.Navigate().GoToUrl("https://www.myip.com/");
            await Task.Delay(5000);

            driver.Navigate().GoToUrl("https://stackoverflow.com/");
            await Task.Delay(5000);
            driver.FindElement(By.XPath("/html/body/header/div/ol[2]/li[2]/a[1]")).Click();
            await Task.Delay(5000);
            driver.FindElement(By.XPath(@"//*[@id='openid-buttons']/button[1]")).Click();
            driver.FindElement(By.XPath(@"//*[@id='identifierId']")).SendKeys(_gmailUid);
            driver.FindElement(By.XPath(@"//*[@id='identifierNext']/div/button/div[2]")).Click();
            driver.FindElement(By.XPath(@"//*[@id='password']/div[1]/div/div[1]/input")).SendKeys(_gmailPwd);
            driver.FindElement(By.XPath(@"//*[@id='passwordNext']/div/button/div[2]")).Click();

            //var js = (IJavaScriptExecutor)driver;
            //string title2 = (string)js.ExecuteScript($"document.title = '{auth.Proxy}:{auth.Port}'");

            //await Task.Delay(5000);
            //driver.Navigate().GoToUrl("https://amibehindaproxy.com/");
            //await Task.Delay(5000);

            // Dispose the driver
            //driver.Dispose();
        }


    }
}
