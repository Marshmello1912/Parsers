using System;
using System.Net.Http;
using System.Threading.Tasks;
using PuppeteerSharp;
namespace parserJSE
{
    internal class Web
    {
        LaunchOptions launchOptions; 


        public Web()
        {
            launchOptions = new LaunchOptions
            {
                Headless = true, // = false for testing
            };
        }

        public async Task<string> GetPage()
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(launchOptions))
            using (var page = await browser.NewPageAsync())
            {
                page.DefaultTimeout = 0; 
                await page.GoToAsync("https://clientportal.jse.co.za/reports/delta-option-and-structured-option-trades", WaitUntilNavigation.Networkidle2);
                var content = await page.GetContentAsync();
                return content;
            }
        }

    }
}
