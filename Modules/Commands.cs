using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TutorialBot.Managers;
using TutorialBot.Managers.QueueManager;
using TutorialBot.Models;

namespace TutorialBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly QueueManager _queueManager;
        private readonly PlayerManager _playerManager;
        

        public Commands(QueueManager queueManager, PlayerManager playerManager)
        {
            _queueManager = queueManager;
            _playerManager = playerManager;
        }

        [Command("SeeQueueStatus")]
        public async Task SeeQueueStatus(params string[] args)
        {
            await ReplyAsync(_queueManager.showAvailablePlayersInQueue());
        }


        [Command("Register")]
        public async Task Register(params string[] args)
        {
            var success = _playerManager.Register(Context.Message.Author.Username);

            if (success)
            {
                await ReplyAsync($"New Player: {Context.Message.Author.Username} added");
            }

            else
            {
                await ReplyAsync($"Failed to register player, please try again");
            }
        }

        [Command("ShowRegisteredPlayers")]
        public async Task ShowRegisteredPlayers(params string[] args)
        {
            await ReplyAsync($"Registered Players: {_playerManager.ShowRegisteredPlayers()}");
        }

        [Command("FindPlayerInfo")]
        public async Task FindPlayerInfo(params string[] args)
        {
            await ReplyAsync(_playerManager.FindPlayerInfo(args[0]));
        }

        

        [Command("queueMonster")]
        //[Alias]
        public async Task QueueMonster(params string[] args)
        {
            var monsterPlayerInfo = _playerManager.GetPlayerForQueue(Context.Message.Author.Username);

            if (monsterPlayerInfo != null)
            {
                if (monsterPlayerInfo.inQueue == false)
                {
                    _queueManager.AddMonsterPlayer(monsterPlayerInfo);
                    await ReplyAsync($"Added player {monsterPlayerInfo.Name} to monster queue");
                }
                else
                {
                    await ReplyAsync($"Already in queue.");
                }
            }

            else
            {
                await ReplyAsync($"Name not found. Please register first with the !Register command.");
            }

        }

        [Command("queueHunter")]
        public async Task QueueHunter(params string[] args)
        {
            string roleString = args[0].ToLower();

            PlayerManager.roles roles = new PlayerManager.roles();
            switch (roleString)
            {
                case "assault":
                    roles = PlayerManager.roles.Assault;
                    break;
                case "trapper":
                    roles = PlayerManager.roles.Trapper;
                    break;
                case "support":
                    roles = PlayerManager.roles.Support;
                    break;
                case "medic":
                    roles = PlayerManager.roles.Medic;
                    break;
                default:
                    await ReplyAsync($"Please enter an appropriate role (Assault, Trapper, Support, Medic) example: !queueHunter Assault");
                    return;
            }

            var hunterPlayerInfo = _playerManager.GetPlayerForQueue(Context.Message.Author.Username);

            if (hunterPlayerInfo != null)
            {
                if (hunterPlayerInfo.inQueue == false)
                {
                    switch (roles)
                    {
                        case PlayerManager.roles.Assault:
                            _queueManager.AddHunterPlayer(hunterPlayerInfo, roles);
                            break;
                        case PlayerManager.roles.Trapper:
                            _queueManager.AddHunterPlayer(hunterPlayerInfo, roles);
                            break;
                        case PlayerManager.roles.Support:
                            _queueManager.AddHunterPlayer(hunterPlayerInfo, roles);
                            break;
                        case PlayerManager.roles.Medic:
                            _queueManager.AddHunterPlayer(hunterPlayerInfo, roles);
                            break;
                    }
                    await ReplyAsync($"Added player {hunterPlayerInfo.Name} to {roles.ToString()} queue");
                }
                else
                {
                    await ReplyAsync($"Player already in queue.");
                }
            }

            else
            {
                await ReplyAsync($"Player not found. Please register first with the !Register command.");
            }
        }


        [Command("unQueueMonster")]
        public async Task UnQueueMonster(params string[] args)
        {
            var monsterPlayerInfo = _playerManager.GetPlayerForQueue(Context.Message.Author.Username);

            _queueManager.RemoveMonsterPlayer(monsterPlayerInfo);
            await ReplyAsync($"Removed from monster queue.");
        }

        [Command("unQueueHunter")]
        public async Task UnQueueHunter(params string[] args)
        {

            string roleString = args[0].ToLower();

            PlayerManager.roles roles = new PlayerManager.roles();
            switch (roleString)
            {
                case "assault":
                    roles = PlayerManager.roles.Assault;
                    break;
                case "trapper":
                    roles = PlayerManager.roles.Trapper;
                    break;
                case "support":
                    roles = PlayerManager.roles.Support;
                    break;
                case "medic":
                    roles = PlayerManager.roles.Medic;
                    break;
                default:
                    await ReplyAsync($"Please enter an appropriate role (Assault, Trapper, Support, Medic) example: !queueHunter Assault");
                    return;
            }

            var playerInfo = _playerManager.GetPlayerForQueue(Context.Message.Author.Username);

            _queueManager.RemoveHunterPlayer(playerInfo, roles);
            await ReplyAsync($"Removed from {roles} queue.");
        }

        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
        [Command("StartMM")]
        public async Task StartMM()
        {
            _queueManager.FindMatches();
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage ="You don't have the permission ``ban_member``!")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify a user!"); 
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Guild.AddBanAsync(user, 1, reason);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} was banned\n**Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(642698444431032330) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
                .WithDescription($"{user.Mention} was banned\n**Reason** {reason}\n**Moderator** {Context.User.Mention}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);

        }

    }
}
