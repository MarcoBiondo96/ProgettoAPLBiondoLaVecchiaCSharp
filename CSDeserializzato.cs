using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCsharp
{
    public class CSDeserializzato//Classe utilizzata per gestire i dati di CS_go
    {
        public int total_kills { get; set; }
        public int total_time_played { get; set; }
        public int total_damage_done { get; set; }
        public int total_kills_headshot { get; set; }
        public float kd_ratio { get; set; }
        public float total_wins_perc { get; set; }
        public float accuracy_perc { get; set; }
        public int last_match_wins { get; set; }
        public int last_match_kills { get; set; }
        public int last_match_deaths { get; set; }
        public int last_match_damage { get; set; }
        public int last_match_rounds { get; set; }
        public float last_match_kd_ratio { get; set; }

    }
}
