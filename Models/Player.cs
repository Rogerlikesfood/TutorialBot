using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TutorialBot.Managers.PlayerManager;

namespace TutorialBot.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int MonsterElo { get; set; }
        public Dictionary<roles, int> HunterElo { get; set; }

        public bool inQueue = false;

        public bool inGame = false;

        public Player(string name, int monsterElo, Dictionary<roles, int> hunterElo)
        {
            Name = name;
            MonsterElo = monsterElo;
            HunterElo = hunterElo;
        }
    }
}
