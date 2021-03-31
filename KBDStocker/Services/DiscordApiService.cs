using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RestSharp;

namespace KBDStocker.Services
{
    public class DiscordApiService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        
        
        private readonly string _apiToken;

        public DiscordApiService(string token)
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Error,
                MessageCacheSize = 50
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Error,
                CaseSensitiveCommands = false
            });
            
            _client.Log += Log;
            _commands.Log += Log;

            _apiToken = token;
        }

        

        public async Task Start()
        {
            await _client.LoginAsync(TokenType.Bot, _apiToken);
            await _client.StartAsync();

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                services: null);
        }
        
        public async Task InStock(string item, string url)
        {
            await _client
                .GetGuild(817332933329289218)
                .GetTextChannel(826423111791411200)
                .SendMessageAsync($"{item} is in stock! {url}");
        }

        public async Task OutOfStock(string item)
        {
            await _client
                .GetGuild(817332933329289218)
                .GetTextChannel(826423111791411200)
                .SendMessageAsync($"{item} not currently in stock");
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }

       
    }
}
