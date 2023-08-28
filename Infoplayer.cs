using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCsharp
{
    public class Infoplayer//Utilizzata per gestire i dati generici di un utente lol tft
    {
        public int lose_ranked { get; set; }
        public string nome_evocatore { get; set; }
        public string punti { get; set; }
        public string rank { get; set; }
        public string tier { get; set; }
        public int win_ranked { get; set; }
    }
}
