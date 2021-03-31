using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace KBDStocker.DiscordModules
{
    [Group("Simple")]
    public class StockCommandHandler : ModuleBase<SocketCommandContext>
    {

        [Command("!stock")]
        [Summary("Immediate stock check")]
        public async Task StockCheck(string id)
        {
            await Context.Channel.SendMessageAsync($"Checking stock status on item {id}");
        }


    }
}
