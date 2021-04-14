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
    //
    
    // Options
    // 1. Replace version of chrome driver (remove selenium web driver chrome driver by something separately
    // 2. Fake user agent = chrome OR firefox (mozilla)
    // 3. Use firefox driver (Selenium Firefox driver)
    
    class Program
    {
        private const string _gmailUid = "tech.bifrost@gmail.com";
        private const string _gmailPwd = "T7PKJ7vRn26fre";

        static Task Main(string[] args)
        {
            Console.WriteLine($"TestSeleniumProxyServer at {DateTime.Now}");
            var proxyAuth = new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP");

            // Don't await, have multiple drivers at once using the local proxy server
            //ChromeProxyDriver.Start(proxyAuth, _gmailUid, _gmailPwd);
            GeckoProxyDriver.Start(proxyAuth, _gmailUid, _gmailPwd);
            //StartSeleniumProxyServerFireFox(proxyServer, new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP"));
            //StartSeleniumProxyServer(proxyServer, new ProxyAuth("64.137.72.147", 65233, "proxy", "C4t8TyP"));
            while (true) { }
        }
    }
}
