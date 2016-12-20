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
            var autoEvent = new AutoResetEvent(false);

            var timer = new Timer(async (e) =>
            {
                var panelStatus = await MinecraftHelpers.GetPanelStatusAsync();
                var serverStatus = await MinecraftHelpers.GetServerStatusAsync();

                if(panelStatus == "offline")
                {
                    await client.SetGame($"Panel is {panelStatus}");
                }
                else if(panelStatus == "online" && serverStatus == "offline")
                {
                    await client.SetGame($"Minecraft is {serverStatus}");
                }
                else
                {
                    await client.SetGame($"Minecraft is {serverStatus}");
                }
                
            }, autoEvent, 0, 60000);
        }
    }
}
