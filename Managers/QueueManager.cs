using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorialBot.Models;

namespace TutorialBot.Managers
{
    using System;
    using System.Collections.Generic;

    namespace QueueManager
    {
        // Define a player class to store information about a player.

        // Define a class to store information about the matchmaking system.
        public class QueueManager
        {
            private List<Player> availableMonsterPlayers;
            private List<Player> availableAssaultPlayers;
            private List<Player> availableTrapperPlayers;
            private List<Player> availableSupportPlayers;
            private List<Player> availableMedicPlayers;
            private List<MatchLobby> availableLobbies;
            public QueueManager()
            {
                availableMonsterPlayers = new List<Player>();
                availableAssaultPlayers = new List<Player>();
                availableTrapperPlayers = new List<Player>();
                availableSupportPlayers = new List<Player>();
                availableMedicPlayers = new List<Player>();
                availableLobbies = new List<MatchLobby>();
            }

            // Add a player to the system.


            public string showAvailablePlayersInQueue()
            {
                StringBuilder output = new StringBuilder();

                output.AppendLine($"Monster Players in queue: {availableMonsterPlayers.Count}");
                int x = 1;
                foreach (var player in availableMonsterPlayers)
                {
                    output.AppendLine($"{x}. {player.Name}");
                    x++;
                }

                output.AppendLine($"Assault Players in queue: {availableAssaultPlayers.Count}");
                x = 1;
                foreach (var player in availableAssaultPlayers)
                {
                    output.AppendLine($"{x}. {player.Name}");
                    x++;
                }

                output.AppendLine($"Trapper Players in queue: {availableTrapperPlayers.Count}");
                x = 1;
                foreach (var player in availableTrapperPlayers)
                {
                    output.AppendLine($"{x}. {player.Name}");
                    x++;
                }

                output.AppendLine($"Support Players in queue: {availableSupportPlayers.Count}");
                x = 1;
                foreach (var player in availableSupportPlayers)
                {
                    output.AppendLine($"{x}. {player.Name}");
                    x++;
                }

                output.AppendLine($"Medic Players in queue: {availableMedicPlayers.Count}");
                x = 1;
                foreach (var player in availableMedicPlayers)
                {
                    output.AppendLine($"{x}. {player.Name}");
                    x++;
                }

                return output.ToString();
            }
            public void AddMonsterPlayer(Player player)
            {
                availableMonsterPlayers.Add(player);
                player.inQueue = true;
            }
            public void AddHunterPlayer(Player player, PlayerManager.roles role)
            {
                switch (role)
                {
                    case PlayerManager.roles.Assault:
                        availableAssaultPlayers.Add(player);
                        player.inQueue = true;
                        break;
                    case PlayerManager.roles.Trapper:
                        availableTrapperPlayers.Add(player);
                        player.inQueue = true;
                        break;
                    case PlayerManager.roles.Support:
                        availableSupportPlayers.Add(player);
                        player.inQueue = true;
                        break;
                    case PlayerManager.roles.Medic:
                        availableMedicPlayers.Add(player);
                        player.inQueue = true;
                        break;
                }
            }

            // Remove a player from the system.
            public void RemoveMonsterPlayer(Player player)
            {
                availableMonsterPlayers.Remove(player);
                player.inQueue = false;
            }

            public void RemoveHunterPlayer(Player player, PlayerManager.roles role)
            {
                switch (role)
                {
                    case PlayerManager.roles.Assault:
                        availableAssaultPlayers.Remove(player);
                        player.inQueue = false;
                        break;
                    case PlayerManager.roles.Trapper:
                        availableTrapperPlayers.Remove(player);
                        player.inQueue = false;
                        break;
                    case PlayerManager.roles.Support:
                        availableSupportPlayers.Remove(player);
                        player.inQueue = false;
                        break;
                    case PlayerManager.roles.Medic:
                        availableMedicPlayers.Remove(player);
                        player.inQueue = false;
                        break;
                }
            }

            public void FindMatches()
            {
                while (true)
                {
                    var x = 1;
                    //create lobby if there is none, start with monster
                    if (availableLobbies.Count == 0)
                    {
                        var tempLobby = new MatchLobby();
                        tempLobby.LobbyId = x;

                        if (availableMonsterPlayers.Count > 0)
                        {
                            availableMonsterPlayers[0].inQueue = true;
                            tempLobby.currentCount++;
                            tempLobby.Monster = availableMonsterPlayers[0];
                            availableLobbies.Add(tempLobby);
                        }
                        else if (availableAssaultPlayers.Count > 0)
                        {

                        }
                        else if (availableTrapperPlayers.Count > 0)
                        {

                        }
                        else if (availableSupportPlayers.Count > 0)
                        {

                        }
                        else if (availableMedicPlayers.Count > 0)
                        {

                        }
                        x++;
                        continue;
                    }

                    if (availableLobbies.Count > 0)
                    {
                        //try to match lobby
                        foreach (var lobby in availableLobbies)
                        {
                            if (lobby.Monster == null)
                            {
                                //if no monster, search for a monster
                                foreach (var player in availableMonsterPlayers)
                                {
                                    lobby.Monster = player;
                                    lobby.currentCount++;
                                    availableMonsterPlayers.Remove(player);
                                }
                            }

                            else
                            {
                                //if there is a monster find hunters
                                if (lobby.Medic == null)
                                {
                                    foreach (var player in availableMedicPlayers)
                                    {
                                        var medicElo = player.HunterElo[PlayerManager.roles.Medic];
                                        if (Math.Abs(medicElo - lobby.Monster.MonsterElo) < 200)
                                        {
                                            lobby.Medic = player;
                                            lobby.currentCount++;
                                            availableMedicPlayers.Remove(player);
                                        }
                                    }
                                }
                                if (lobby.Trapper == null)
                                {
                                    foreach (var player in availableTrapperPlayers)
                                    {
                                        var trapperElo = player.HunterElo[PlayerManager.roles.Trapper];
                                        if (Math.Abs(trapperElo - lobby.Monster.MonsterElo) < 200)
                                        {
                                            lobby.Trapper = player;
                                            lobby.currentCount++;
                                            availableTrapperPlayers.Remove(player);
                                        }
                                    }
                                }
                                if (lobby.Support == null)
                                {
                                    foreach (var player in availableSupportPlayers)
                                    {
                                        var trapperElo = player.HunterElo[PlayerManager.roles.Support];
                                        if (Math.Abs(trapperElo - lobby.Monster.MonsterElo) < 200)
                                        {
                                            lobby.Trapper = player;
                                            lobby.currentCount++;
                                            availableTrapperPlayers.Remove(player);
                                        }
                                    }
                                }
                                if (lobby.Assault == null)
                                {
                                    foreach (var player in availableAssaultPlayers)
                                    {
                                        var assaultElo = player.HunterElo[PlayerManager.roles.Assault];
                                        if (Math.Abs(assaultElo - lobby.Monster.MonsterElo) < 200)
                                        {
                                            lobby.Assault = player;
                                            lobby.currentCount++;
                                            availableAssaultPlayers.Remove(player);
                                        }
                                    }
                                }
                            }
                            //if (lobby.currentCount < 5)
                            //{
                            //    continue;
                            //}
                            if (lobby.currentCount == 1)
                            {
                                availableLobbies.Remove(lobby);
                                continue;
                            }
                        }
                    }

                    else
                    {
                        //sleep for x amount of time
                    }
                }
            }

            // Find matches for all players in the system.
            public void FindMatches2()
            {
                // Create a list of players who have not been matched yet.
                var unmatchedMonsterPlayers = new List<Player>(availableMonsterPlayers);
                //var unmatchedHunterPlayers = new List<Player>(availableHunterPlayers);

                // Continue looping until all players have been matched.
                //while (unmatchedMonsterPlayers.Count > 0 && unmatchedHunterPlayers.Count > 0)
                //{
                //    // Get the first unmatched player.
                //    //var player = unmatchedPlayers[0];

                //    //// Loop through the other unmatched players and try to find a match for the current player.
                //    //for (int i = 1; i < unmatchedPlayers.Count; i++)
                //    //{
                //    //    var otherPlayer = unmatchedPlayers[i];

                //    //    // Check if the other player is a compatible match for the current player.
                //    //    if (IsMatch(player, otherPlayer))
                //    //    {
                //    //        // Assign the players to the A and B teams based on their skill level and preferred game mode.
                //    //        if (player.SkillLevel > otherPlayer.SkillLevel || (player.SkillLevel == otherPlayer.SkillLevel))
                //    //        {
                //    //            player.Team = "A";
                //    //            otherPlayer.Team = "B";
                //    //        }
                //    //        else
                //    //        {
                //    //            player.Team = "B";
                //    //            otherPlayer.Team = "A";
                //    //        }

                //    //        // Remove the other player from the list of unmatched players.
                //    //        unmatchedPlayers.Remove(otherPlayer);
                //    //        break;
                //    //    }
                //    //}

                //    //// Remove the current player from the list of unmatched players.
                //    //unmatchedPlayers.Remove(player);
                //}
            }
            // Determine if two players are a compatible match.
        }
    }
}


