using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCsharp
{
    public class LobbyContainer//Classe per gestire list di lobby di differente tipologia
    {
        public List<Lobby> lobby_possedute { get; set; }
        public List<Lobby> lobby_presente { get; set; }
        public List<Lobby> lobby_disponibili { get; set; }
    }
}
