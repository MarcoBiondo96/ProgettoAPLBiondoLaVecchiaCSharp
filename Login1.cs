using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProgettoCsharp
{
    public partial class Login : Form
    {
        private ClientWebSocket _webSocket;//Dichiarazione dell'oggetto ClientWebSocket che permette di effettuare una comunicazione wwebsocket con il server
        public Login()
        {
            InitializeComponent();
            
        }

        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//Al Click del linklabel1 viene nascosto il form attuale e viene creato un form per la registrazione di un nuovo utente
        {
            this.Hide();
            // Crea un'istanza del form di registrazione dell'account
            Registrazione form2 = new Registrazione();
            form2.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
            form2.Show();
            
        }
        private void Formx_FormClosed(object sender, FormClosedEventArgs e)
        {
            // mostra di nuovo il form del login quando il Form della registrazione è chiuso o nel caso si sia già loggati si è usciti dall'applicazione ovvero dal form del menu principale
            UsernameText.Text = "";
            PasswordText.Text = "";
            this.Show();
        }

        private async Task SendRequest(String req)//Viene inviata al websocket una richiesta asyncrona con la richiesta che deve effettuare il server
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(req));
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<Dictionary<string, object>> ReceiveResponse()//Viene ricevuto un dict da parte del server che il contenuto viene gestito successivamente
        {
            using (var ms = new MemoryStream())
            {
                var receiveBuffer = new ArraySegment<byte>(new byte[1024]);
                var receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                while (!receiveResult.EndOfMessage)//Viene letto il reciveResult (dati inviati dal server) fino a quando non viene letto tutto e viene scritto all'interno di ms
                {
                    ms.Write(receiveBuffer.Array, receiveBuffer.Offset, receiveResult.Count);
                    receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                }
                ms.Write(receiveBuffer.Array, receiveBuffer.Offset, receiveResult.Count);
                var messageBytes = ms.ToArray();
                var messageString = Encoding.UTF8.GetString(messageBytes);
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(messageString);//Viene serializzato come un dato di tipo Dictonary usando JsonSerializer in quanto successivamente viene gestito come tale
                return dict;
            }
        }

        private async void button1_Click(object sender, EventArgs e)//Qui viene eseguito il bottone che permette di effettuare il login con il nostro server
        {
            var serverUri = new Uri("ws://localhost:18080/login");//Url del server in c++ con route login

            try
            {
                this.Hide();
                _webSocket = new ClientWebSocket();
                await _webSocket.ConnectAsync(serverUri, CancellationToken.None);
                var message = $"{{\"message\":\"login\",\"username\":\"{UsernameText.Text}\",\"password\":\"{PasswordText.Text}\"}}";//Creazione di un messaggio strutturato come se fosse un file di tipo json dove viene passato username e password
                await SendRequest(message);//Invio messaggio al server
                Dictionary<string, object> dict = await ReceiveResponse();//Ricezione risposta

                if (int.Parse(dict["Presente"].ToString())==1)//Vari controlli per verificare se l'utente è presente se presente viene creato un nuovo oggetto Utente dove verrano assegnati i vari valori prelevati dal dB e restituiti al client da parte del server c++
                {

                    
                    Utente utente = new Utente(int.Parse(dict["ID"].ToString()), dict["Nome"].ToString(), dict["Cognome"].ToString(), dict["Username"].ToString(), dict["Email"].ToString());
                    if (dict.ContainsKey("User_lol"))//Controllo se sono presenti i vari nickname di gioco
                        utente.SetUser_lol(dict["User_lol"].ToString());
                    if (dict.ContainsKey("User_tft"))
                        utente.SetUser_tft(dict["User_tft"].ToString());
                    if (dict.ContainsKey("User_csgo"))
                        utente.SetUser_csgo(dict["User_csgo"].ToString());

                    Menu_Principale form3 = new Menu_Principale(_webSocket,utente);//Creazione del form Menu_Principale passando l'utente prelevato e il websocket di riferimento creato
                    form3.FormClosed += new FormClosedEventHandler(Formx_FormClosed);
                    form3.Show();

                 }
                 else
                 {
                     this.Show();//Caso in cui l'utente non è presente viene chiuso il websocket 
                     await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connessione Chiusa", CancellationToken.None);
                     MessageBox.Show("Account non Presente"); 
                 }
            }
            catch (Exception ex)
            {
                this.Show();
                MessageBox.Show("Errore: " + ex.Message);
            }
            
            
        }

        private void UsernameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


    }
}