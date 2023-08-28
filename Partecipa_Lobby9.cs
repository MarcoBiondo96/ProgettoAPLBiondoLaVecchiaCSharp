using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProgettoCsharp.Menu_Principale;

namespace ProgettoCsharp
{
    public partial class Partecipa_Lobby : Form
    {
        private ClientWebSocket _webSocket;
        private Utente user;
        private int lobby_id;
        private string gioco;
        public Partecipa_Lobby(ClientWebSocket webSocket, Utente utente, int id,string nome_gioco)//Form dalla doppia funzione utilizzato sia per visualizzare gli utenti accettati di una lobby e sia di inserire una partecipazione nel caso in cui non si è presenti nella lobby
        {
            InitializeComponent();
            _webSocket = webSocket;
            user = utente;
            lobby_id = id;
            gioco= nome_gioco;
            set_users_lobby(lobby_id,gioco);
        }

        private async void set_users_lobby(int lobby_id,string gioco)
        {
            flowLayoutPanel1.Controls.Clear();
            if (gioco == "")//caso in cui l'utente risulta essere già accettato non può inserire un ulteriore prenotazione alla lobby
            {
                button2.Hide();
                textBox2.Hide();
                label2.Hide();
            }
            var message = $"{{\"message\":\"visualizza_lobby_id\",\"lobby_id\":\"{lobby_id}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
            
        }
        private async Task SendRequest(String req)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(req));
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async void linkLabelvalue_LinkClicked_Visualizza_Utente(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var message = $"{{\"message\":\"visualizza_utente\",\"username\":\"{((Valuedlinklabel)sender).Value.ToString()}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }
        private void linkLabelvalue_LinkClicked_Nota(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(((Valuedlinklabel)sender).Value.ToString());
        }
        private async Task ReceiveResponse()
        {
            byte[] receiveBuffer = new byte[10000 * 10000];
            WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var jsonString = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

            if (jsonObject["type"].ToString() == "stato_utenti_lobby")//Serve a visualizzare gli utenti accettati alla lobby
            {
                flowLayoutPanel1.Controls.Clear();

                List<Dictionary<string, object>> utenti_accettati = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonObject["utenti_accettati"].ToString());

                foreach (Dictionary<string, object> utente_accettato in utenti_accettati)
                {
                    Panel panel = new Panel();

                    Valuedlinklabel linkLabelUsername = new Valuedlinklabel();
                    linkLabelUsername.Value = utente_accettato["id_utente"];
                    linkLabelUsername.Text = utente_accettato["username"].ToString();
                    linkLabelUsername.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Visualizza_Utente);

                    Valuedlinklabel linkLabelNota = new Valuedlinklabel();
                    linkLabelNota.Value = utente_accettato["Nota"].ToString();
                    linkLabelNota.Text = "Nota";
                    linkLabelNota.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Nota);

                    
                    linkLabelUsername.Location = new Point(0, 0);
                    linkLabelNota.Location = new Point(linkLabelUsername.Right + 5, 0); 

                    panel.Controls.Add(linkLabelUsername);
                    panel.Controls.Add(linkLabelNota);

                    flowLayoutPanel1.Controls.Add(panel);

                }
            }

            
            else if (jsonObject["type"].ToString() == "utente_visualizzato")//Utilizzato per richiamare il form Menu_Utente per visualizzare le statistiche del giocatore
            {
                Utente user = new Utente();


                user.SetId(int.Parse(jsonObject["id_utente"].ToString()));
                if (jsonObject.ContainsKey("User_lol"))
                    user.SetUser_lol(jsonObject["User_lol"].ToString());
                if (jsonObject.ContainsKey("User_tft"))
                    user.SetUser_tft(jsonObject["User_tft"].ToString());
                if (jsonObject.ContainsKey("User_csgo"))
                    user.SetUser_csgo(jsonObject["User_csgo"].ToString());
                this.Hide();
                Menu_User form4 = new Menu_User(_webSocket, user);
                form4.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form4.Show();
            }
            else if (jsonObject["type"].ToString() == "partecipazione_inviata")//Messaggio di conferma ricevuto da parte del server che la partecipazione è stata inviata
            {
                MessageBox.Show("Partecipazione inviata");
                this.Close();
            }

        }
        private void Formx_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
        private async void button2_Click(object sender, EventArgs e)//Bottone che permette di inviare la partecipazione alla lobby
        {
            var message = $"{{\"message\":\"partecipa_lobby\",\"id_utente\":\"{user.GetId().ToString()}\",\"gioco\":\"{gioco}\",\"nota\":\"{textBox2.Text}\",\"lobby_id\":\"{lobby_id}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }
    }
}
