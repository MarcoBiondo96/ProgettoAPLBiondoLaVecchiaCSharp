using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoCsharp
{
    

    public partial class Inserisci_Lobby : Form
    {
        private ClientWebSocket _webSocket;
        private Utente user;
        public Inserisci_Lobby(ClientWebSocket webSocket,Utente utente)//Form che permette di inserire una lobby da parte del utente
        {
            InitializeComponent();
            _webSocket = webSocket;
            user= utente;
            inicombobox(user);
        }
        private void inicombobox(Utente utente)//vengono visualizzate unicamente i giochi dove si possiede un nickname associato
        {
            if (utente.GetUser_csgo() != "")
                comboBox1.Items.Add("Counter Strike Go");
            if (utente.GetUser_lol() != "")
                comboBox1.Items.Add("League of Legends");
            if (utente.GetUser_tft() != "")
                comboBox1.Items.Add("Teamfight Tactics");
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Inserire il gioco ");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Inserire il Nome ");
                return;
            }
            
            var message = $"{{\"message\":\"inserisci_lobby\",\"gioco\":\"{comboBox1.Text}\",\"nome\":\"{textBox1.Text}\",\"utente_id\":\"{user.GetId()}\",\"orario\":\"{dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")}\",\"nota\":\"{textBox2.Text}\"}}";
            await SendRequest(message);
            var resp = await ReceiveResponse();
            MessageBox.Show(resp.ToString());
            this.Close();
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
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
