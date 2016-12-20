using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SexyBot
{
    [Group("test")]
    public class TestCommands : ModuleBase
    {
        [Command("test"), Summary("A test")]
        public async Task TestAsync()
        {
            await Context.Channel.SendMessageAsync("test");
        }
    }

    [Group("minecraft")]
    public class MinecraftCommands : ModuleBase
    {
        [Command("panel"), Summary("Returns status of the Minecraft server panel")]
        public async Task PanelStatusAsync()
        {
            var message = await Context.Channel.SendMessageAsync("`Retrieving Data, please wait...`");
            var panelStatus = await MinecraftHelpers.GetPanelStatusAsync();
            await message.ModifyAsync(m => m.Content = $"Panel 78 is {panelStatus}");
        }

        [Command("server"), Summary("Returns status of the Minecraft Server")]
        public async Task ServerStatusAsync()
        {
            var message = await Context.Channel.SendMessageAsync("`Retrieving Data, please wait...`");
            var serverStatus = await MinecraftHelpers.GetServerStatusAsync();
            await message.ModifyAsync(m => m.Content = $"Server is {serverStatus}");
        }

        [Command("status"), Summary("Returns status of the Minecraft System")]
        public async Task SystemStatusAsync()
        {
            var message = await Context.Channel.SendMessageAsync("`Retrieving Data, please wait...`");
            var panelStatus = await MinecraftHelpers.GetPanelStatusAsync();
            var serverStatus = await MinecraftHelpers.GetServerStatusAsync();
            await message.ModifyAsync(m => m.Content = $"Panel 78 is {panelStatus}, Server is {serverStatus}");
        }
    }

    static class MinecraftHelpers
    {
        public static async Task<string> GetPanelStatusAsync()
        {
            string panelStatus = "offline";
            using (var client = new HttpClient())
            {
                GGServerStatus status = new GGServerStatus();
                HttpResponseMessage response = await client.GetAsync("https://status.ggservers.com/pull/index.php?url=78");
                if(response.IsSuccessStatusCode)
                {
                    string statusString = await response.Content.ReadAsStringAsync();
                    status = JsonConvert.DeserializeObject<GGServerStatus>(statusString);

                    if(!status.Load.Contains("Down"))
                    {
                        panelStatus = "online";
                    }
                }

                return panelStatus;
            }
        }

        public static async Task<string> GetServerStatusAsync()
        {

            string serverStatus = "offline";

            using (var client = new HttpClient())
            {
                MinecraftServerStatus status = new MinecraftServerStatus();
                HttpResponseMessage response = await client.GetAsync("https://mcapi.us/server/status?ip=192.99.20.208&port=29871");
                if(response.IsSuccessStatusCode)
                {
                    string statusString = await response.Content.ReadAsStringAsync();
                    status = JsonConvert.DeserializeObject<MinecraftServerStatus>(statusString);

                    if(status.Online)
                    {
                        serverStatus = "online";
                    }
                }
            }

            return serverStatus;
        }
    }

    class GGServerStatus
    {
        public string Hdd { get; set; }
        public string Load { get; set; }
        public string Memory { get; set; }
        public string Online { get; set; }
        public string Uptime { get; set; }
    }

    public class MinecraftServerStatus
    {
        public string Status { get; set; }
        public bool Online { get; set; }
        public string Motd { get; set; }
        public string Error { get; set; }
        public Players Players { get; set; }
        public Server Server { get; set; }
        public string Last_online { get; set; }
        public string Last_updated { get; set; }
        public int Duration { get; set; }
    }

    public class Players
    {
        public int Max { get; set; }
        public int Now { get; set; }
    }

    public class Server
    {
        public string Name { get; set; }
        public int Protocol { get; set; }
    }

    
}
