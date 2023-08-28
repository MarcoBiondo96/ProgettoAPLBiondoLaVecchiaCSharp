using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProgettoCsharp
{
    public partial class Menu_Principale : Form//Form principale che gestisce l'applicazione (Sarebbe l'Home dell'applicazione)
    {
        private ClientWebSocket _webSocket;
        private Utente user;
        private string gioco_attuale;
        public Menu_Principale(ClientWebSocket webSocket,Utente utente)//Viene passato nel costruttore il websocket creato precedentemente ed inoltre viene passato anche l'utente che è stato ricevuto tramite il login inoltre va a settare delle label che corrispondono ad i nomi di gioco utilizzati nei vari giochi
        {
            InitializeComponent();
            FormClosing += Form3_FormClosing;
            _webSocket = webSocket;
            user= utente;
            label7.Text=utente.GetUser_lol();
            label6.Text = utente.GetUser_tft();
            label8.Text = utente.GetUser_csgo();
            

        }

        private async Task SendRequest(String req)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(req));
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)//Bottone che permette l'uscita del client chiudendo la comunicazione websocket
        {
            try
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connessione Chiusa", CancellationToken.None);
                MessageBox.Show("Uscita Client");
                this.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Errore s: " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        public class Valuedlinklabel : LinkLabel//Oggetto che eredita linklabel ma viene aggiunto un ulteriore parametro interno che viene utilizzato successivamente da altre funzioni in basso
        {
            public object Value { get; set; }
            
        }

        

        private async void Form3_FormClosing(object sender, FormClosingEventArgs e)//Caso in cui vi è la chiusura del client tramite pulsante x vi è la chiusura del websocket creato con il login
        {
            try
            {
                
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connessione Chiusa", CancellationToken.None);
                MessageBox.Show("Uscita Client");
                this.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Errore s: " + ex.Message);
            }
        }
        private async Task ReceiveResponse()//Ricezione di dati da parte del server
        {

            byte[] receiveBuffer = new byte[10000 * 10000];
            WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var jsonString = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<LobbyContainer>(jsonString, options);//I dati del server vengono Deserializzati da una stringa Json in un tipo LobbyContainer che è una classe che abbiamo dichiarato, inoltre, verifica che il JSON fornito sia valido e corrisponda alla struttura dell'oggetto LobbyContainer



            foreach (var item in result.lobby_possedute)//Prelevazione delle lobby possedute all'interno di lobby container
            {
                string labelcontent = "";
                Panel panel = new Panel();
                Label label = new Label();
                labelcontent += "Nome lobby:" + item.nomelobby + "\n";
                labelcontent += "Numero accettati:" + item.Accettati + "\n";
                labelcontent += "Orario:" + item.Orario + "\n";
                label.Text = labelcontent;
                label.Dock = DockStyle.Left;

                label.AutoSize = true;
                Valuedlinklabel linkLabel = new Valuedlinklabel();//LinkLabel Creata precedentemente dove viene associato un valore corrispondente all'id della lobby
                
                linkLabel.Dock = DockStyle.Top;
                label.Dock = DockStyle.Bottom;
                linkLabel.Value = item.id_lobby;
                linkLabel.Text = "Vai alla lobby";
                linkLabel.LinkClicked += (sender, e) => linkLabelvalue_LinkClicked(sender, e, 0);//Viene creata tramite una lambda function una funzione anonima dove viene passato un parametro a 0 che richiamerà una funzione particolare spiegato successivamente
                panel.Controls.Add(linkLabel);
                panel.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(panel);
            }

            foreach (var item in result.lobby_disponibili)
            {
                string labelcontent = "";
                Panel panel = new Panel();
                Label label = new Label();
                labelcontent += "Nome lobby:" + item.nomelobby + "\n";
                labelcontent += "Numero accettati:" + item.Accettati + "\n";
                labelcontent += "Orario:" + item.Orario + "\n";
                label.Text = labelcontent;
                label.Dock = DockStyle.Left;

                label.AutoSize = true;
                Valuedlinklabel linkLabel = new Valuedlinklabel();

                linkLabel.Dock = DockStyle.Top;
                label.Dock = DockStyle.Bottom;
                linkLabel.Value = item.id_lobby;
                linkLabel.Text = "Vai alla lobby";
                linkLabel.LinkClicked += (sender, e) => linkLabelvalue_LinkClicked(sender, e, 2);//Parametro a 2
                panel.Controls.Add(linkLabel);
                panel.Controls.Add(label);
                flowLayoutPanel3.Controls.Add(panel);
            }

            foreach (var item in result.lobby_presente)
            {
                string labelcontent = "";
                Panel panel = new Panel();
                Label label = new Label();
                labelcontent += "Nome lobby:" + item.nomelobby + "\n";
                labelcontent += "Numero accettati:" + item.Accettati + "\n";
                labelcontent += "Orario:" + item.Orario + "\n";
                label.Text = labelcontent;
                label.Dock = DockStyle.Left;

                label.AutoSize = true;
                Valuedlinklabel linkLabel = new Valuedlinklabel();

                linkLabel.Dock = DockStyle.Top;
                label.Dock = DockStyle.Bottom;
                linkLabel.Value = item.id_lobby;
                linkLabel.Text = "Vai alla lobby";
                linkLabel.LinkClicked += (sender, e) => linkLabelvalue_LinkClicked(sender, e, 1);//Parametro a 1
                panel.Controls.Add(linkLabel);
                panel.Controls.Add(label);
                flowLayoutPanel2.Controls.Add(panel);
            }

        }
        private async void button1_Click(object sender, EventArgs e)//Pulsante utilizzato per due funzioni nel caso in cui l'utente del gioco di riferimento del tasto è presente viene fatta una call al server che restituisce le lobby di riferimento dell'utente nel caso contrario aprira un altro form Nickname
        {
            gioco_attuale = "League of Legends";
            label10.Text= gioco_attuale;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            if (user.GetUser_lol() == "")
            {
                MessageBox.Show("Account_Lol Non Presente inserisci il nickname");
                this.Hide();
                Add_Nickname form6 = new Add_Nickname(_webSocket, user,"lolstats");//Creazione di un nuovo form nickname che permette l'inserimento del account relativo a tale gioco
                form6.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form6.Show();
            }
            else
            {
                
                var message = $"{{\"message\":\"visualizza_lobby\",\"gioco\":\"League of Legends\",\"user_id\":\"{user.GetId()}\"}}";//Dichiarazione del messaggio da inviare al server c++ contenente l'id relativo al account del determinato gioco
                await SendRequest(message) ;
                await ReceiveResponse();
                
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabelvalue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e,int i)//Funzione che in base al parametro di passaggio i andra ad eseguire diverse funzionalità
        {
            if (i == 0)//Questa permette di aprire un nuovo form per visualizzare le lobby con l'id relativo associato al linklabel precedentemente e permette la visualizzazione delle lobby create dall'utente
            {
                
                int lob = (int)((Valuedlinklabel)sender).Value;
                this.Hide();
                Visualizza_Lobby form8 = new Visualizza_Lobby(_webSocket, user, lob);
                form8.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form8.Show();
            }
            else if(i == 1)//siamo nelle lobby accettate in questo caso viene aperto un altro form di tipo Partecipa_Lobby in questo caso serve unicamente a visualizzare i partecipanti alla lobby che sono stati accettati
            {
                gioco_attuale = "";//Server per nascondere il bottone che ha la funzionalità di partecipare ad una lobby è presente nel caso del if==2
                int lob = (int)((Valuedlinklabel)sender).Value;
                this.Hide();
                Partecipa_Lobby form9 = new Partecipa_Lobby(_webSocket, user, lob,gioco_attuale);
                form9.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form9.Show();
            }
            else if(i==2)//siamo nelle lobby disponibili in questo caso invece viene creato sempre un form di tipo partecipalobby ma sarà permessa la registrazione da parte del utente alla lobby di riferimento
            {
                int lob = (int)((Valuedlinklabel)sender).Value;
                this.Hide();
                Partecipa_Lobby form9 = new Partecipa_Lobby(_webSocket, user, lob, gioco_attuale);
                form9.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form9.Show();
            }

        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//Linklabel utilizzato per creare un nuovo form di tipo Menu_User che permette di visualizzare le statistiche del utente di riferimento(In questo il proprietario dell'account)
        {
            this.Hide();
            Menu_User form4 = new Menu_User(_webSocket,user);
            form4.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
            form4.Show();
        }
        
        private void Formx_FormClosed(object sender, FormClosedEventArgs e)
        {
            // mostra di nuovo il Form del Menu_Principale quando uno dei form aperti in precedenza viene chiuso
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            label10.Text = "";
            this.Show();

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)//Fa lo stesso del button relativo all'altro gioco
        {
            gioco_attuale = "Teamfight Tactics";
            label10.Text = gioco_attuale;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            if (user.GetUser_tft() == "")
            {
                MessageBox.Show("Account_Tft Non Presente inserisci il nickname");
                this.Hide();
                Add_Nickname form6 = new Add_Nickname(_webSocket, user, "tftstats");
                form6.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form6.Show();
            }
            else
            {
                
                var message = $"{{\"message\":\"visualizza_lobby\",\"gioco\":\"Teamfight Tactics\",\"user_id\":\"{user.GetId()}\"}}";
                await SendRequest(message);
                await ReceiveResponse();
            }
        }

        private async void button3_Click(object sender, EventArgs e)//Fa lo stesso del button relativo all'altro gioco
        {
            gioco_attuale = "Counter Strike Go";
            label10.Text = gioco_attuale;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            if (user.GetUser_csgo() == "")
            {
                MessageBox.Show("Account_Cs_go Non Presente inserisci il nickname");
                this.Hide();
                Add_Nickname form6 = new Add_Nickname(_webSocket, user, "csstats");
                form6.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form6.Show();
            }
            else
            {
                
                var message = $"{{\"message\":\"visualizza_lobby\",\"gioco\":\"Counter Strike Go\",\"user_id\":\"{user.GetId()}\"}}";
                await SendRequest(message);
                await ReceiveResponse();
            }
        }

        private void button5_Click(object sender, EventArgs e)//Bottone che permette la creazione di una nuova lobby se non si possiede un account di gioco associato non è possibile creare una lobby per tale gioco
        {
            if (user.GetUser_csgo() == "" && user.GetUser_lol() == "" && user.GetUser_tft() == "")
            {
                MessageBox.Show("Non hai nessun Account di gioco Inserito non puoi creare nessuna Lobby");
            }
            else
            {
                
                this.Hide();
                Inserisci_Lobby form7 = new Inserisci_Lobby(_webSocket, user);
                form7.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form7.Show();
            }
        }

        

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)//Bottone che permette di creare un form di MostraGrafico che permette di visualizzare la densità degli utenti in base al gioco
        {
            if (label10.Text == "")
                MessageBox.Show("Seleziona un Gioco");
            else
            {
                this.Hide();
                MostraGraficoLobby form10 = new MostraGraficoLobby(_webSocket, gioco_attuale);
                form10.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                form10.Show();
            }
        }
    }
}
