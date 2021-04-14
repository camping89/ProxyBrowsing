using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SeleniumProxy;

namespace ConsoleApp
{
    /// <summary>
    /// https://github.com/mozilla/geckodriver/releases
    /// </summary>
    public static class GeckoProxyDriver
    {
        public static async Task Start(ProxyAuth auth, string gmailUid, string gmailPwd)
        {
            // Add a new local proxy server endpoint
            var proxyServer = new SeleniumProxyServer();
            var localPort = proxyServer.AddEndpoint(auth);

            // network.proxy.type
            // 0 - Direct connection, no proxy. (Default in Windows and Mac previous to 1.9.2.4 / Firefox 3.6.4)
            // 1 - Manual proxy configuration.
            // 2 - Proxy auto-configuration(PAC).
            // 4 - Auto - detect proxy settings.
            // 5 - Use system proxy settings. (Default in Linux; default for all platforms, starting in 1.9.2.4 / Firefox 3.6.4)
            var options = new FirefoxOptions();
            options.SetPreference("network.proxy.type", 1);
            options.SetPreference("network.proxy.http", "127.0.0.1");
            options.SetPreference("network.proxy.http_port", localPort);

            //var service = FirefoxDriverService.CreateDefaultService();
            var service = FirefoxDriverService.CreateDefaultService(".", "geckodriver.exe");
            var driver = new FirefoxDriver(service,options);
            try
            {
                driver.Navigate().GoToUrl("https://stackoverflow.com/");
                await Task.Delay(1000);
                driver.FindElement(By.XPath("/html/body/header/div/ol[2]/li[2]/a[1]")).Click();
                await Task.Delay(1000);
                driver.FindElement(By.XPath(@"//*[@id='openid-buttons']/button[1]")).Click();
                driver.FindElement(By.XPath(@"//*[@id='identifierId']")).SendKeys(gmailUid);
                driver.FindElement(By.XPath(@"//*[@id='identifierNext']/div/button/div[2]")).Click();
                await Task.Delay(5000);
                //var cookies = driver.Manage().Cookies.AllCookies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                driver.Dispose();
            }
        }
    }
}