using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SexyBot
{
    [Group("sexiest")]
    public class Commands : ModuleBase
    {
        [Command("who"), Summary("Who's the sexiest of them all?")]
        public async Task DetermineSexiestAsync()
        {
            var user = await CommandHelpers.PickSexiestUserAsync(Context.Guild);
            await Context.Channel.SendMessageAsync($"{user.Mention} is the sexist of them all, of course.");
        }
    }
}
