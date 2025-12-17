using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace AppTasques
{
    /// <summary>
    /// Lógica de interacción para Usuaris.xaml
    /// </summary>
    public partial class Usuaris : Window
    {
        public Usuaris()
        {
            InitializeComponent();
            MostrarUsuaris();
        }

        private void MostrarUsuaris()
        {
            llistaUsuaris.Items.Clear();

            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";
            using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
                conexion.Open();
                    
                string sql = "SELECT id, nom FROM Usuari";
                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string usuari = $"{reader["id"]} - {reader["nom"]}";
                        llistaUsuaris.Items.Add(usuari);
                    }
                }
            }
        }
        private void AfegirUsuari(object sender, RoutedEventArgs e)
        {
            if (nouNom.Text == "" || nouContrasenya.Password == "" || nouConfirmarContrasenya.Password == "")
            {
                MessageBox.Show("Falten camps per omplir.");
            } 
            
            else
            {
                if (nouContrasenya.Password == nouConfirmarContrasenya.Password)
                {
                    try
                    {

                        string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

                        using (MySqlConnection conexion = new MySqlConnection(cadena))
                        {
                            conexion.Open();

                            string sql = "INSERT INTO Usuari (nom, contrasenya) VALUES (@nom, @pass)";
                            using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                            {
                                cmd.Parameters.AddWithValue("@nom", nouNom.Text);
                                cmd.Parameters.AddWithValue("@pass", nouContrasenya.Password);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        llistaUsuaris.Items.Add(nouNom.Text);

                        nouNom.Clear();
                        nouContrasenya.Clear();
                        nouConfirmarContrasenya.Clear();
                        MessageBox.Show("Usuari afegit correctament.");
                    }
                    catch (Exception ex) 
                    { 
                        MessageBox.Show("Error al inserir usuari: " + ex.Message); 
                    }
                }

                else
                {
                    MessageBox.Show("Les contrasenyes no coincideixen.");
                }
            }     
        }
    }
}
