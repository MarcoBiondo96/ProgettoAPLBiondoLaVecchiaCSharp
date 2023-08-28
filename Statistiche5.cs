using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoCsharp
{
    public partial class Statistiche : Menu_User
    {
        public Statistiche(Match match, string tipo)//Viene passato come parametro il tipo di gioco e il match di riferimento e in base a questo vengono mostrate le statistiche del match di riferimento
        {
            InitializeComponent();
            if (tipo == "lol")
            {
                statsForLol(match);
            }
            else if (tipo == "tft")
            {
                statsForTft(match);
            }
        }
        public Statistiche(CSDeserializzato result)
        {
            InitializeComponent();
            statsForCs(result);
        }
        private void statsForCs(CSDeserializzato result)
        {
            labelMatch.Text += "Vittorie : " + result.last_match_wins + "\n";
            labelMatch.Text += "Numero round : " + result.last_match_rounds + "\n";
            labelMatch.Text += "Uccisioni: " + result.last_match_kills + "\n";
            labelMatch.Text += "KD Ratio: " + result.last_match_kd_ratio + "\n";
            labelMatch.Text += "Morti: " + result.last_match_deaths + "\n";
            labelMatch.Text += "Danni inflitti: " + result.last_match_damage + "\n";

        }
        private void statsForLol(Match match)
        {
            labelMatch.Text += "Risultato: " + match.risultato + "\n";
            labelMatch.Text += "ID Partita: " + match.match_id + "\n";
            labelMatch.Text += "Modalità: " + match.mode + "\n";
            labelMatch.Text += "Campione: " + match.campione + "\n";
            labelMatch.Text += "KDA: " + match.kda + "\n";
            labelMatch.Text += "Uccisioni: " + match.kills + "\n";
            labelMatch.Text += "Assist: " + match.assists + "\n";
            labelMatch.Text += "Morti: " + match.deaths + "\n";
            labelMatch.Text += "Durata della partita: " + match.durata_partita + "\n";
        }

        private void statsForTft(Match match)
        {
            labelMatch.Text += "Giocatori eliminati: " + match.giocatoriEliminati + "\n";
            labelMatch.Text += "Livello: " + match.livello + "\n";
            labelMatch.Text += "Durata partita: " + match.durata_partita + "\n";
            labelMatch.Text += "Piazzamento: " + match.piazzamento + "\n";


        }


    }
}
