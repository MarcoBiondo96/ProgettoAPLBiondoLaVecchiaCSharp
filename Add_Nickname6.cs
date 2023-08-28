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

namespace ProgettoCsharp
{
    public partial class Add_Nickname : Form
    {
        private ClientWebSocket _webSocket;
        private Utente user;
        private string tipo;
        public Add_Nickname(ClientWebSocket webSocket,Utente utente, string tipo)//Form utilizzato per inserire il nickname di gioco in base al gioco di riferimento passato per parametro
        {
            _webSocket = webSocket;
            user = utente;
            InitializeComponent();
            this.tipo = tipo;
        }

        private async Task SendRequest(string message)
        {
            byte[] requestBuffer = System.Text.Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(requestBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private async Task<string> ReceiveResponse()
        {
            using (var ms = new MemoryStream())
            {
                var receiveBuffer = new ArraySegment<byte>(new byte[1024]);
                var receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                while (!receiveResult.EndOfMessage)
                {
                    ms.Write(receiveBuffer.Array, receiveBuffer.Offset, receiveResult.Count);
                    receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                }
                ms.Write(receiveBuffer.Array, receiveBuffer.Offset, receiveResult.Count);
                var messageBytes = ms.ToArray();
                var messageString = Encoding.UTF8.GetString(messageBytes);
                return messageString;
            }
        }
        

        private async void button1_Click(object sender, EventArgs e)//AL click del bottone viene inviata una richiesta di inserimento del nickname sul gioco e il server c++ farà delle verifiche se il nickname di gioco è presente realmente tramite chiamata a python e successivamente andra a verificare se all'iterno della piattaforma qualche altro utente ha già associato tale nickname al suo account della piattaforma
        {
            var message = $"{{\"message\":\"{tipo}\",\"check\":\"verifica\",\"nome_ev\":\"{textBox1.Text}\",\"Username\":\"{user.GetUsername()}\"}}";
            await SendRequest(message);
            var resp=await ReceiveResponse();
            if (resp.ToString() == "OK")//Qui viene settato nel Utente passato con la creazione del form il nickname nel caso si successo
            {
                if(tipo=="lolstats")
                    user.SetUser_lol(textBox1.Text);
                else if(tipo=="tftstats")
                    user.SetUser_tft(textBox1.Text);
                else if(tipo=="csstats")
                    user.SetUser_csgo(textBox1.Text);
                MessageBox.Show("Account inserito");
                this.Close();
            }
            else if(resp.ToString() == "Errore_Account_esistente")
            {
                MessageBox.Show("Account gia posseduto");
                this.Close();
            }
            else if(resp.ToString() == "Errore_account_non_esistente")
            {
                MessageBox.Show("Account non esistente");
                this.Close();
            }else if(resp.ToString() == "errore")
            {
                MessageBox.Show("Servizio momentaneamente non disponibile");//Caso in cui il server python è offiline
                this.Close();
            }


        }
    }
}
