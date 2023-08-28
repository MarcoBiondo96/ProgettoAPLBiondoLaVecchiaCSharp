using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProgettoCsharp
{
    public class Utente
    {
        private int id_utente;
        private string Nome;
        private string Cognome;
        private string Username;
        
        private string Email;
        private string User_lol;
        private string User_tft;
        private string User_csgo;

        
        public Utente()//Classe che permette di gestire un utente 
        {
        }

        
        public Utente(int id,string nome, string cognome, string username,  string email, string user_lol, string user_tft, string user_csgo)
        {
            id_utente= id;
            Nome = nome;
            Cognome = cognome;
            Username = username;
            
            Email = email;
            User_lol = user_lol;
            User_tft = user_tft;
            User_csgo = user_csgo;
        }

        public Utente(int id,string nome, string cognome, string username,  string email)
        {
            id_utente = id;
            Nome = nome;
            Cognome = cognome;
            Username = username;
            
            Email = email;
            User_lol = "";
            User_tft = "";
            User_csgo = "";
        }
        public Utente(int id,string user_lol,string user_tft,string user_csgo)
        {
            id_utente = id;
            User_lol= user_lol;
            User_tft = user_tft;
            User_csgo = user_csgo;
            Nome = "";
            Cognome = "";
            Username = "";

            Email = "";
        }

        
        public string GetNome()
        {
            return Nome;
        }

        public void SetNome(string value)
        {
            Nome = value;
        }

        
        public string GetCognome()
        {
            return Cognome;
        }

        public void SetCognome(string value)
        {
            Cognome = value;
        }

        
        public string GetUsername()
        {
            return Username;
        }


        public void SetUsername(string value)
        {
            Username = value;
        }
        public int GetId()
        {
            return id_utente;
        }


        public void SetId(int value)
        {
            id_utente = value;
        }



        
        public string GetEmail()
        {
            return Email;
        }

        public void SetEmail(string value)
        {
            Email = value;
        }

        
        public string GetUser_lol()
        {
            return User_lol;
        }

        public void SetUser_lol(string value)
        {
            User_lol = value;
        }

        
        public string GetUser_tft()
        {
            return User_tft;
        }

        public void SetUser_tft(string value)
        {
            User_tft = value;
        }

        
        public string GetUser_csgo()
        {
            return User_csgo;
        }

        public void SetUser_csgo(string value)
        {
            User_csgo = value;
        }
    }
}
