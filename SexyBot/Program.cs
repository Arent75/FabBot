using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;

class Program
{
    static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

    private DiscordSocketClient client;

    public async Task RunAsync()
    {
        client = new DiscordSocketClient();

        string token = "MjUxNDAzODcyMDM4Mjg5NDA5.CxvZLg.D4qdH8YZnhqOoW5z-kWnYUPLpLo";

        client.MessageReceived += async (message) =>
        {
            if (message.MentionedUsers.Any(u => u.Id == client.CurrentUser.Id) && !message.Content.Contains("Who's the sexiest of them all?") && message.Author.Id != client.CurrentUser.Id)
                await message.Channel.SendMessageAsync("Stop poking me!");

            if (message.MentionedUsers.Any(u => u.Id == client.CurrentUser.Id) && message.Content.Contains("Who's the sexiest of them all?"))
                await message.Channel.SendMessageAsync($"{PickSexiestUser(client.Guilds.FirstOrDefault()).Mention} is the sexist of them all, of course.");
        };

        await client.LoginAsync(TokenType.Bot, token);

        await client.ConnectAsync();

        await Task.Delay(-1);
    }

    private SocketUser PickSexiestUser(SocketGuild guild)
    {
        Random random = new Random();

        var count = guild.MemberCount;
        var index = random.Next(0, count - 1);
        return guild.Users.ElementAtOrDefault(index);
    }
}