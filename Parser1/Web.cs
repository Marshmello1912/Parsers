using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Web
    {
        HttpClientHandler clientHandler;
        HttpClient client;


        public Web()
        {
            clientHandler = new HttpClientHandler { UseCookies = false };
            client = new HttpClient(clientHandler);
        }

        public string GetPage(int PageNum)
        {
            String body = default(String);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.wienerborse.at/en/bonds/?=&c7928-page={PageNum}&per-page=50&cHash=8a64e6f2193d28657a2297b3ccfd2a22"),
                Headers =
{
        { "cookie", "_csrf=9967585ee7fb9c36150c3ed449f2046b9b6fd0af69e34b072d1aa1bbccb6efaea%253A2%253A%257Bi%253A0%253Bs%253A5%253A%2522_csrf%2522%253Bi%253A1%253Bs%253A32%253A%2522xgYWSTpEVYWKAJg2CmtlTDftRkPgJF6v%2522%253B%257D; PHPSESSID=6382fb376080529622e5e0dfad7cfe8a" },
        { "User-Agent", "insomnia/9.3.1" },
    },
            };

            Task<HttpResponseMessage> response = client.SendAsync(request);
            body = response.Result.Content.ReadAsStringAsync().Result;

            return body;
        }

    }
}
