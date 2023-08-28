using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCsharp
{
    public class LolTftDeserializzato //Classe che permette di gestire tutti i dati relativi ad un account di lol tft
    {
        public Infoplayer infoplayer { get; set; }
        public List<Match> matchs { get; set; }
        public float winrate { get; set; }
    }
}
