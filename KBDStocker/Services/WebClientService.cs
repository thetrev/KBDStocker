using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KBDStocker.Models;

namespace KBDStocker.Services
{
    public class WebClientService
    {
        private readonly Queue<string> _queue;
        private readonly object _synclock;
        private int _waitTime;
        private readonly Settings _settings;

        private readonly DiscordApiService _discordApiService;
        
        public WebClientService(Settings settings)
        {
            _settings = settings;
            _synclock = new object();
            _queue = new Queue<string>();

            _discordApiService = new DiscordApiService(settings.DiscordApiKey);
        }

        public async Task Init()
        {
            //just being super lazy and not putting the other settings through the worker

            if (_settings.WaitTimeInSeconds != null)
            {
                _waitTime = (int)_settings.WaitTimeInSeconds;

            }
            else
            {
                Console.WriteLine("No wait time found, defaulting to 10 minutes");
                _waitTime = 600;
            }

            var urlList = _settings.Urls.ToList();
            urlList.ForEach(x => _queue.Enqueue(x));
            
            //Testing the bot works
            await _discordApiService.Start();

            var workers = new List<Task>();

            for (var i = 0; i < urlList.Count; i++)
            {
                workers.Add(OnDoWork());
            }

            await Task.WhenAll(workers);
        }

        private async Task OnDoWork()
        {
            var currentItem = string.Empty;
            
            lock(_synclock)
            {
                if (_queue.Any())
                {
                    currentItem = _queue.Dequeue();
                }
            }

            while (string.IsNullOrEmpty(currentItem) == false)
            {
                await StockCheck(currentItem);
                lock (_synclock)
                {
                    if (_queue.Any() == false)
                    {
                        break;
                    }

                    currentItem = _queue.Dequeue();
                }
            }
        }

        private async Task StockCheck(string url)
        {
            var inStock = false;

            while (!inStock)
            {
                byte[] htmlCode;

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent: Other");
                    htmlCode = await client.DownloadDataTaskAsync(url);
                }

                var webData = Encoding.UTF8.GetString(htmlCode);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(webData);

                char[] trimChars = { '\\', 'n', ' ' };

                var item = htmlDoc.DocumentNode
                    .SelectSingleNode("//h1[contains(@class, 'product-single__title')]")
                    .InnerHtml.Trim();

                //Console.WriteLine($"Stock item - {item}");

                var stock = htmlDoc.DocumentNode
                    .SelectSingleNode("//span[@data-add-to-cart-text]").InnerHtml.Trim();

                Console.WriteLine($"Page button status - {stock}");

                if (stock.Contains("add To cart"))
                {
                    inStock = false;
                    Console.WriteLine("OMG IT'S IN STOCK OH GOD OH FUCK");
                    await _discordApiService.InStock(item, url);
                }
                else
                {
                    await _discordApiService.OutOfStock(item);
                    Console.WriteLine($"Not in stock at {DateTime.UtcNow}");
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(_waitTime));
            }
        }
    }
}
