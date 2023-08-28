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
    public partial class Visualizza_Lobby : Form
    {
        private ClientWebSocket _webSocket;
        private Utente user;
        private int lobby_id;
        public Visualizza_Lobby(ClientWebSocket webSocket, Utente utente, int id)//Form che permette la gestione di una lobby da parte del suo creatore
        {
            InitializeComponent();
            _webSocket = webSocket;
            user = utente;
            lobby_id = id;
            set_users_lobby(lobby_id);
        }

        private void Formx_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
        private void linkLabelvalue_LinkClicked_Nota(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(((Valuedlinklabel)sender).Value.ToString());
        }
        private async void linkLabelvalue_LinkClicked_Visualizza_Utente(object sender, LinkLabelLinkClickedEventArgs e)//Linklabel che permette di effettuare una richiesta al server per passare i dati relativi ad un utente specifico (Serve per valutare le capacità di un utente per decidere se aggiungerlo ad una lobby o meno)
        {
            var message = $"{{\"message\":\"visualizza_utente\",\"username\":\"{((Valuedlinklabel)sender).Value.ToString()}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }
        private async void linkLabelvalue_LinkClicked_Elimina(object sender, LinkLabelLinkClickedEventArgs e)//Linklabel che permette di escludere un utente dalla lobby
        {
            var message = $"{{\"message\":\"elimina_utente\",\"username\":\"{((Valuedlinklabel)sender).Value.ToString()}\",\"lobby_id\":\"{lobby_id}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }
        private async void linkLabelvalue_LinkClicked_Aggiungi(object sender, LinkLabelLinkClickedEventArgs e)//Linklabel che permette di aggiungere un utente iscritto alla lobby
        {
            
            var message = $"{{\"message\":\"aggiungi_utente\",\"username\":\"{((Valuedlinklabel)sender).Value.ToString()}\",\"lobby_id\":\"{lobby_id}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }
        private async Task ReceiveResponse()
        {
            byte[] receiveBuffer = new byte[10000 * 10000];
            WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var jsonString = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);//Viene deserializzata una stringa in un oggetto Dictionary
            if (jsonObject["type"].ToString() == "stato_utenti_lobby")//Messaggio ricevuto dal client da parte del server che restituisce dei valori
            {
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel2.Controls.Clear();
                List<Dictionary<string, object>> utenti_attesa = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonObject["utenti_attesa"].ToString());//Viene creata una lista di Dictionary dove al suo interno viene inserita la lista degli utenti in attesa alla lobby quindi l'utente singolo viene rappresentato come un dizionario a sua volta che contiene i dati del utente accettato
                foreach (Dictionary<string,object> utente_attesa in utenti_attesa)
                {
                    Panel panel = new Panel();
                    panel.AutoSize = true;
                    Valuedlinklabel linkLabelUsername = new Valuedlinklabel();
                    linkLabelUsername.Value = utente_attesa["id_utente"];
                    linkLabelUsername.Text = utente_attesa["username"].ToString();
                    linkLabelUsername.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Visualizza_Utente);

                    Valuedlinklabel linkLabelNota = new Valuedlinklabel();
                    linkLabelNota.Value = utente_attesa["Nota"].ToString();
                    linkLabelNota.Text = "Nota";
                    linkLabelNota.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Nota);

                    Valuedlinklabel linkLabelAggiungi = new Valuedlinklabel();
                    linkLabelAggiungi.Value = utente_attesa["id_account"];
                    linkLabelAggiungi.Text = "Aggiungi";
                    linkLabelAggiungi.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Aggiungi);

                    Valuedlinklabel linkLabelElimina = new Valuedlinklabel();
                    linkLabelElimina.Value = utente_attesa["id_account"];
                    linkLabelElimina.Text = "Elimina";
                    linkLabelElimina.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Elimina);
                    
                    
                    linkLabelUsername.Location = new Point(0, 0);
                    linkLabelNota.Location = new Point(linkLabelUsername.Right + 5, 0); 
                    linkLabelAggiungi.Location = new Point(linkLabelNota.Right + 5, 0); 
                    linkLabelElimina.Location = new Point(linkLabelAggiungi.Right + 5, 0); 
                    
                    panel.Controls.Add(linkLabelUsername);
                    panel.Controls.Add(linkLabelNota);
                    panel.Controls.Add(linkLabelAggiungi);
                    panel.Controls.Add(linkLabelElimina);
                    flowLayoutPanel2.Controls.Add(panel);

                }

                List<Dictionary<string, object>> utenti_accettati = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonObject["utenti_accettati"].ToString());//Viene eseguito lo stesso ma con gli utenti accettati 

                foreach (Dictionary<string, object> utente_accettato in utenti_accettati)
                {
                    Panel panel = new Panel();
                    panel.AutoSize = true;
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
                    if(user.GetId() != int.Parse(utente_accettato["id_utente"].ToString())) { //Possibilità di eliminare gli utenti accettati tranne il creatore della lobby
                        Valuedlinklabel linkLabelElimina = new Valuedlinklabel();
                        linkLabelElimina.Value = utente_accettato["id_account"];
                        linkLabelElimina.Text = "Elimina";
                        linkLabelElimina.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelvalue_LinkClicked_Elimina);
                        linkLabelElimina.Location = new Point(linkLabelNota.Right + 5, 0);
                        panel.Controls.Add(linkLabelElimina);
                    }
                    flowLayoutPanel1.Controls.Add(panel);

                }
            }
            
            else if (jsonObject["type"].ToString()== "eliminazione_lobby")
            {
                MessageBox.Show("Lobby Eliminata");
                this.Close();
            }
            else if(jsonObject["type"].ToString() == "utente_aggiunto")
            {
                MessageBox.Show("Utente Aggiunto alla Lobby");
                set_users_lobby(lobby_id);
            }
            else if (jsonObject["type"].ToString() == "utente_eliminato")
            {
                MessageBox.Show("Utente Eliminato dalla Lobby");
                set_users_lobby(lobby_id);
            }
            else if (jsonObject["type"].ToString() == "utente_visualizzato")
            {
                Utente user=new Utente();
                
                
                user.SetId(int.Parse(jsonObject["id_utente"].ToString()));
                if (jsonObject.ContainsKey("User_lol"))
                    user.SetUser_lol(jsonObject["User_lol"].ToString());
                if (jsonObject.ContainsKey("User_tft"))
                    user.SetUser_tft(jsonObject["User_tft"].ToString());
                if (jsonObject.ContainsKey("User_csgo"))
                    user.SetUser_csgo(jsonObject["User_csgo"].ToString());
                this.Hide();
                Menu_User form4 = new Menu_User(_webSocket, user); //Richiamo del Form che si occupa nella visualizzazione delle statistiche del utente
                form4.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form4.Show();
            }


        }
        private async Task SendRequest(String req)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(req));
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private async void set_users_lobby(int lobby_id) {//Utilizzata per caricare i dati della lobby nel form
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            var message = $"{{\"message\":\"visualizza_lobby_id\",\"lobby_id\":\"{lobby_id}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button2_Click(object sender, EventArgs e)//Bottone che ha la funzione di eliminare la lobby visto che vi è una regolazione oncascade tutte le prenotazioni associate alla lobby vengono eliminate
        {
            var message = $"{{\"message\":\"elimina_lobby\",\"lobby_id\":\"{lobby_id}\"}}";
            await SendRequest(message);
            await ReceiveResponse();
        }
    }
}
