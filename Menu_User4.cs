using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoCsharp
{
    public partial class Menu_User : Form
    {

        private ClientWebSocket _webSocket;
        private Utente user;
        public Menu_User(ClientWebSocket webSocket, Utente utente)
        {
            InitializeComponent();
            _webSocket = webSocket;
            user= utente;
            label7.Text = utente.GetUser_lol();
            label6.Text = utente.GetUser_tft();
            label8.Text = utente.GetUser_csgo();
        }

        public Menu_User()
        {
        }

        private async void lol_Click(object sender, EventArgs e)//Al click del bottone viene fatta una request che viene gestita dalla ReciveREsponseLolTft dove viene passato il nome del gioco come parametro
        {
            if (user.GetUser_lol() != "")
            {
                rimuoviBottoni();
                try
                {

                    var message = $"{{\"message\":\"lolstats\",\"nome_ev\":\"{user.GetUser_lol()}\"}}";
                    await SendRequest(message);
                    await ReceiveResponseLolTft("lol");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server Python Offline");//Caso in cui il server python è offline non può riportare i dati reali di gioco del utente
                }
            }
            else
                MessageBox.Show("Account Lol non Presente");
        }

        private async void tft_Click(object sender, EventArgs e)
        {
            if (user.GetUser_tft() != "")
            {
                rimuoviBottoni();
                try
                {
                    var message = $"{{\"message\":\"tftstats\",\"nome_ev\":\"{user.GetUser_tft()}\"}}";
                    await SendRequest(message);
                    await ReceiveResponseLolTft("tft");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server Python Offline");

                }
            }else
                    MessageBox.Show("Account tft non Presente");
            }

        private async void cs_Click(object sender, EventArgs e)//Viene gestita la response da un altra funzione adoperata per cs_go
        {
            if (user.GetUser_csgo() != "")
            {
                rimuoviBottoni();
                try
                {
                    var message = $"{{\"message\":\"csstats\",\"nome_ev\":\"{user.GetUser_csgo()}\"}}";
                    await SendRequest(message);
                    await ReceiveResponseCS();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server Python Offline");
                }
            }
            else
                MessageBox.Show("Account Cs:go non Presente");
        }

        private void rimuoviBottoni()
        {
            while (buttonPanel.Controls.Count > 0)
            {
                Control control = buttonPanel.Controls[0];
                buttonPanel.Controls.Remove(control);
                control.Dispose();
            }
        }


        private async Task SendRequest(string message)
        {
            byte[] requestBuffer = System.Text.Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(requestBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        

        private async Task ReceiveResponseCS()
        {

            byte[] receiveBuffer = new byte[10000 * 10000]; 
            WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var jsonString = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<CSDeserializzato>(jsonString, options);//Viene ricevuta una stringa serializzata in json che successivamente viene deserializzata in un oggetto CSDeserializzato che contiene le varie statistice di gioco dell'utente relative a cs_go
            string labelContent = "";
            labelContent += "Uccisioni totali: " + result.total_kills + "\n";
            labelContent += "KD Ratio: " + result.kd_ratio + "\n";
            labelContent += "Danno totale inflitto: " + result.total_damage_done + "\n";
            labelContent += "Uccisioni totali con colpo alla testa: " + result.total_kills_headshot + "\n";
            labelContent += "Precisione percentuale: " + result.accuracy_perc + "%\n";
            labelContent += "Vittorie totali in percentuale: " + result.total_wins_perc + "%\n";
            Button button = new Button();
            button.Text = "Ultima partita";
            button.Click += (sender, e) => ButtonClickCs(sender, e, result);//Viene creata una lambda function che permette di effettuare delle operazione descritte successivamente
            button.Size = new Size(10, 30);
            button.Dock = DockStyle.Left;
            button.AutoSize = true;
            buttonPanel.Controls.Add(button);
            infoGamesLabel.Text = labelContent;
        }

        private async Task ReceiveResponseLolTft(string tipo)//Esegue la stessa tipologia di codice effettuata con Cs 
        {


            byte[] receiveBuffer = new byte[10000 * 10000];
            WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var jsonString = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<LolTftDeserializzato>(jsonString, options);//In questo caso viene utilizzato un oggetto LolTftDeserializzato che è un oggetto che riesce a gestire i dati sia di lol che di tft
            string labelContent = "";
            labelContent += "Nome: " + result.infoplayer.nome_evocatore + "\n";
            labelContent += "Vittorie in classificata: " + result.infoplayer.win_ranked + "\n";
            labelContent += "Tier: " + result.infoplayer.tier + "\n";
            labelContent += "Rank: " + result.infoplayer.rank + "\n";
            labelContent += "Numero partite: " + result.matchs.Count + "\n";
            labelContent += "Percentuale vittorie: " + result.winrate + "%\n";

            for (int i = 0; i < result.matchs.Count; i++)
            {
                Button button = new Button();
                button.Text = "Partita " + (i + 1);
                button.Name = "Partita" + i;
                button.Tag = i;
                button.Size = new Size(10, 30);
                button.Dock = DockStyle.Left;
                button.AutoSize = true;
                button.Click += (sender, e) => ButtonClickLolTft(sender, e, result.matchs, tipo);//Vengono creati x bottoni in base al numero di match trovati anche qui viene effettuata una labda function
                buttonPanel.Controls.Add(button);

            }
            infoGamesLabel.Text = labelContent;
        }
        private void ButtonClickCs(object sender, EventArgs e, CSDeserializzato result)//Buttone che esegue la lambda function di riferimento di cs che permette l'apertura di un nuovo form statistiche 
        {
            Statistiche form = new Statistiche(result);
            form.Show();
        }
        private void ButtonClickLolTft(object sender, EventArgs e, List<Match> match, string tipo)//Funzionamento similare a cs solo che relativo a lol e tft
        {
            Button btn = sender as Button;
            int index = (int)btn.Tag;

            Match singoloMatch = match[index];

            Match singolaPartita = singoloMatch;

            Statistiche form = new Statistiche(singolaPartita, tipo);
            form.Show();
        }

        private void infoAccountLabel_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
