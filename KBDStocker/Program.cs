using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KBDStocker.Models;
using KBDStocker.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KBDStocker
{
    class Program
    {
        public static WebClientService webClient;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var settings = JObject
                .Parse(File.ReadAllText(@"config.json"))
                .ToObject<Settings>();

            if (string.IsNullOrEmpty(settings.DiscordApiKey))
            {
                Console.WriteLine("No API key provided");
                Console.ReadKey();
                return;
            }

            if (!string.IsNullOrEmpty(settings.UserToPing))
            {
                Console.WriteLine("No user provided");
                Console.ReadKey();
                return;
            }

            webClient = new WebClientService(settings);
            
            var stockTask = Task.Run(async () =>
            {
                await webClient.Init();
            });
            //Start the Import
            stockTask.Wait();

            Console.WriteLine("done, press any key");
            Console.ReadKey();
        }
        
        
    }
}
