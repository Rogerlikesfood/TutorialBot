using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorialBot.Models;

namespace TutorialBot.Managers
{
    public class PlayerManager
    {
        public enum roles
        {
           Assault,
           Trapper,
           Support,
           Medic
        }


        List<Player> _allPlayers;

        public PlayerManager()
        {
            _allPlayers = new List<Player>();
        }


        public bool Register(string name)
        {

            var freshEloHunter = new Dictionary<roles, int> {
                { roles.Assault, 1000 }, { roles.Trapper, 1000 }, { roles.Support, 1000 }, { roles.Medic, 1000 }
            };

            var newPlayer = new Player(name, 1000, freshEloHunter);


            foreach (var player in _allPlayers)
            {
                if (player.Name == newPlayer.Name)
                {
                    return false;
                }
            }

            _allPlayers.Add(newPlayer);

            return true;
        }

        public string ShowRegisteredPlayers()
        {
            StringBuilder output = new StringBuilder();
            foreach (var player in _allPlayers)
            {
                output.AppendLine(player.Name);
            }
            return output.ToString();
        }

        public string FindPlayerInfo(string name)
        {
            string output = "";
            foreach (var player in _allPlayers)
            {
                if (player.Name == name)
                {
                    output = $"{player.Name} \n Monster Elo: {player.MonsterElo} \n" +
                        $" Hunter Elo: \n " +
                        $"Assault: {player.HunterElo[roles.Assault]} \n " +
                        $"Trapper: {player.HunterElo[roles.Trapper]} \n" +
                        $"Support: {player.HunterElo[roles.Support]} \n" +
                        $"Medic: {player.HunterElo[roles.Medic]}";
                }
            }
            return output;
        }

        public Player GetPlayerForQueue(string name)
        {
            foreach (var player in _allPlayers)
            {
                if (player.Name == name)
                {
                    return player;
                }
            }

            return null;
        }

    }
}
