using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.Threading;

namespace SexyBot
{
    public class StatusService
    {
        public static void SetMinecraftStatusAsync(DiscordSocketClient client)
        {
            var timer = new Timer(async (e) =>
            {
                var panelStatus = await MinecraftHelpers.GetPanelStatusAsync();
                await client.SetGame($"Panel 78 is {panelStatus}");
            }, null, 1000, 60000);
            

        }
    }
}
