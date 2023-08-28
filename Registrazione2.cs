using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoCsharp
{
    public partial class Registrazione : Form
    {
        
        public Registrazione()//Form che permette di registrare nuovi utenti 
        {
            InitializeComponent();
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)//Al click di questo tasto viene inviata una richiesta al server per inserire un nuovo utente
        {
            if (string.IsNullOrEmpty(Nome.Text))
            {
                MessageBox.Show("Inserire il nome ");
                return;
            }
            if (string.IsNullOrEmpty(Cognome.Text))
            {
                MessageBox.Show("Inserire il cognome ");
                return;
            }
            if (string.IsNullOrEmpty(Username.Text))
            {
                MessageBox.Show("Inserire Username");
                return;
            }
            if (string.IsNullOrEmpty(Password.Text))
            {
                MessageBox.Show("Inserire Password");
                return;
            }
            if (string.IsNullOrEmpty(Email.Text))
            {
                MessageBox.Show("Inserire email");
                return;
            }
            if (string.IsNullOrEmpty(dateTimePicker1.Text))
            {
                MessageBox.Show("Inserire Data di nascita");
                return;
            }     
            var data = new
            {
                Nome = Nome.Text,
                Cognome = Cognome.Text,
                Username = Username.Text,
                Password = Password.Text,
                Email = Email.Text,
                Data_Nascita = dateTimePicker1.Value.ToString("yyyy-MM-dd")
            };
            var httpClient = new HttpClient();//In questo caso viene fatta una richiesta tramite Http con metodo post e non viene utilizzato il websocket in quanto il websocket l'abbiamo utilizzato per mantenere una connessione continua con l'utente con il server
            httpClient.BaseAddress = new Uri("http://localhost:18080");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/registrazione", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent.ToString() == "OK")
            {
                MessageBox.Show("Account Creato");
                this.Close();
            }else//Account già esistente
                MessageBox.Show(responseContent.ToString());



        }

        

    }
}
