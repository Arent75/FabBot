using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using System.Reflection;

namespace SexyBot
{
    public class Program
    {
        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        private CommandService commands;
        private DiscordSocketClient client;
        private DependencyMap map;

        public async Task RunAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();

            string token = "MjUxNDAzODcyMDM4Mjg5NDA5.CxvZLg.D4qdH8YZnhqOoW5z-kWnYUPLpLo";

            map = new DependencyMap();

            await InstallCommands();

            client.MessageReceived += async (message) =>
            {
                if (message.MentionedUsers.Any(u => u.Id == client.CurrentUser.Id) && !message.Content.Contains("Who's the sexiest of them all?") && message.Author.Id != client.CurrentUser.Id)
                    await message.Channel.SendMessageAsync("Stop poking me!");

                if (message.MentionedUsers.Any(u => u.Id == client.CurrentUser.Id) && message.Content.Contains("Who's the sexiest of them all?"))
                    await message.Channel.SendMessageAsync($"{PickSexiestUser(client.Guilds.FirstOrDefault()).Mention} is the sexist of them all, of course.");
            };

            await client.LoginAsync(TokenType.Bot, token);

            await client.ConnectAsync();

            StatusService.SetMinecraftStatusAsync(client);

            await Task.Delay(-1);
        }

        private SocketUser PickSexiestUser(SocketGuild guild)
        {
            Random random = new Random();

            var count = guild.MemberCount;
            var index = random.Next(0, count - 1);
            return guild.Users.ElementAtOrDefault(index);
        }

        public async Task InstallCommands()
        {
            // Hook the MessageReceived Event into our Command Handler
            client.MessageReceived += HandleCommand;
            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed succesfully)
            var result = await commands.ExecuteAsync(context, argPos, map);
            if (!result.IsSuccess)
                await message.Channel.SendMessageAsync(result.ErrorReason);
        }

    }
}