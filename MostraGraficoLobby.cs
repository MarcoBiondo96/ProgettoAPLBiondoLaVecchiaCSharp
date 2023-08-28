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
    public partial class MostraGraficoLobby : Form
    {
        private ClientWebSocket _webSocket;
        private string gioco_nome;
        public MostraGraficoLobby(ClientWebSocket webSocket, string gioco)//Form utilizzato per mostrare i grafici del relativo gioco selezionato precedentemente
        {
            InitializeComponent();
            gioco_nome= gioco;
            _webSocket = webSocket;
            label1.Text = "Grafico del Gioco :" + gioco_nome;
        }

        private void MostraGraficoLobby_Load(object sender, EventArgs e)
        {

        }

        private async Task ReceiveResponse()
        {
            byte[] receiveBuffer = new byte[1024 * 1024]; 
            try
            {
                WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                
                if (receiveResult.MessageType == WebSocketMessageType.Binary)
                {
                    try
                    {
                        // Converti il byte array in un'immagine
                        using (MemoryStream mas = new MemoryStream(receiveBuffer, 0, receiveResult.Count))
                        {
                            Image image = Image.FromStream(mas);
                            Image resizedImage = ResizeImage(image, pictureBox1.Width, pictureBox1.Height);
                            pictureBox1.Image = resizedImage;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Server Python Offline");
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Errore durante la ricezione dati: " + ex.Message);
            }
        }

        private Image ResizeImage(Image image, int width, int height)//Funzione che permette di adattare l'img alle dimensioni del picturebox
        {
            Bitmap resizedBitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedBitmap;
        }
        private async void button1_Click(object sender, EventArgs e)//Mostra grafico del numero di utenti nelle ore del giorno corrente
        {
            await SendRequest("dailyuser");
            await ReceiveResponse();
        }
        private async Task SendRequest(String tipologia)//Viene introdotta la tipologia che permette di visualizzare i grafici relativi alla tipologia di bottone premuto
        {
            DateTime currentDate = DateTime.Now;
            var message = $"{{\"message\":\"grafico\",\"gioco\":\"{gioco_nome}\",\"anno\":\"{currentDate.Year}\",\"mese\":\"{currentDate.Month}\",\"giorno\":\"{currentDate.Day}\",\"tipologia\":\"{tipologia}\"}}";
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            
        }

        private async void button3_Click(object sender, EventArgs e)//Mostra grafico del numero di utenti nei giorni del mese corrente
        {
            await SendRequest("monthuser");
            await ReceiveResponse();
        }

        private async void button2_Click(object sender, EventArgs e)//Mostra grafico del numero di utenti nei mesi dell'anno corrente
        {
            await SendRequest("yearuser");
            await ReceiveResponse();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
