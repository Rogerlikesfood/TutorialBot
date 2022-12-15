using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot.Models
{
    public class MatchLobby
    {
        public int currentCount;

        public Player Monster;

        public Player Assault;

        public Player Medic;

        public Player Trapper;

        public Player Support;

        public int LobbyId;

        public MatchLobby()
        {
        }
    }
}
