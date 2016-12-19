using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SexyBot
{
    [Group("minecraft")]
    public class MinecraftCommands : ModuleBase
    {
        [Command("panel"), Summary("Returns status of the Minecraft server panel")]
        public async Task PanelStatusAsync()
        {
            var panelStatus = await MinecraftHelpers.GetPanelStatusAsync();
            await Context.Channel.SendMessageAsync($"Panel 78 is {panelStatus}");
        }
    }

    static class MinecraftHelpers
    {
        public static async Task<string> GetPanelStatusAsync()
        {
            string serverStatus = "offline";
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
                        serverStatus = "online";
                    }
                }

                return serverStatus;
            }
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
}
