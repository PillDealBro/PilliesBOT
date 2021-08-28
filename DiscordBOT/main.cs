using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;


namespace PilliesBOT
{
    class main

    {   
        //You can only await something that returns Task
        //Creating a instance of main class and awaiting RunBotAsync()
        static async Task Main() => await new main().RunBotAsync();

        //Creating my necessery fields

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
         
        public async Task RunBotAsync()
        {
            
            //Assign my fields 
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            //If someone asks for either of these types then provide these instaces
            _services = new ServiceCollection()
                //Use the object that is provided if someone asks for it
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            

           


            _client.Log += _client_Log;

            //Executing 

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
            //Store the token in a env 
            
            await _client.StartAsync();
            await Task.Delay(-1);
           
        }
    

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        // If bot receives a message fire HandleCommandAsync
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        //Check conditions before executing commands! 
        public async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;
           
            // argPos integer that holds and is being check if the prefix is present! 

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
          
        }
    }
}
