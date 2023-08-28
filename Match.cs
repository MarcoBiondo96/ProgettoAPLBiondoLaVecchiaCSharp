using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCsharp
{
    public class Match//Classe che gestisce i match di lol e tft
    {
        public int assists { get; set; }
        public string campione { get; set; }
        public int deaths { get; set; }
        public string durata_partita { get; set; }
        public double kda { get; set; }
        public int kills { get; set; }
        public string match_id { get; set; }
        public string mode { get; set; }
        public string risultato { get; set; }
        public int giocatoriEliminati { get; set; }
        public int livello { get; set; }
        public int piazzamento { get; set; }
    }
}
