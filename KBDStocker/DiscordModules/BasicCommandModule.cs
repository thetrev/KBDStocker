using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace KBDStocker.DiscordModules
{
    public class BasicCommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Eachoes a message")]
        public Task SayAsync([Remainder] [Summary("Hello you bearded fucko")]
            string echo)
            => ReplyAsync(echo);
    }
}
