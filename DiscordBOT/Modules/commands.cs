
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace PilliesBOT.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {


        [Command("Who")]
        public async Task Question()
        {
            await ReplyAsync("Asked!");
        }

        [Command("echo")]
        public async Task Say([Remainder] string text)
        {
            await ReplyAsync(text);
        }



        [RequireContext(ContextType.Guild)]
        public class Set : ModuleBase
        {
            [Command("mention")]
            public async Task Mention(SocketGuildUser user)
            {
                await ReplyAsync($"Hello {user.Mention}");
            }
        }
        [Command("kck")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick([Remainder] SocketGuildUser user)
        {

            await ReplyAsync($"Wow this {user.Mention} actually managed to get kicked :clap: :clap:");
            await user.KickAsync();
        }

        [Command("swing")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban([Remainder] SocketGuildUser user)
        {

            await ReplyAsync($"THIS PERSON -> {user.Mention} GOT BANNED :dizzy_face: :dizzy_face: ");
            await user.BanAsync();
        }
        [Command("whois")]
        public async Task UserInfoAsync(SocketUser user = null)
        {
            var info = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{info.Username}#{info.Discriminator}");
        }


    }
}