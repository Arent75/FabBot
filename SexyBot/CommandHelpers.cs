using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyBot
{
    public class CommandHelpers
    {
        //public static async Task RetrievingDataAsync()
        //{

        //}

        public static async Task<IGuildUser> PickSexiestUserAsync(IGuild guild)
        {
            Random random = new Random();

            var users = await guild.GetUsersAsync();

            return users.ElementAt(random.Next(users.Count));
        }
    }
}
