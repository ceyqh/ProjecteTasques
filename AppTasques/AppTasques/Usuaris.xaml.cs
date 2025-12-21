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

                string sql = "SELECT id, nom, rol FROM Usuari";
                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        llistaUsuaris.Items.Add(new Usuari
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NomUsuari = reader["nom"].ToString(),
                            RolUsuari = reader["rol"].ToString()
                        });
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

                            string sql = "INSERT INTO Usuari (nom, contrasenya, rol) VALUES (@nom, @pass, @rol)";
                            using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                            {
                                cmd.Parameters.AddWithValue("@nom", nouNom.Text);
                                cmd.Parameters.AddWithValue("@pass", nouContrasenya.Password);
                                cmd.Parameters.AddWithValue("@rol", ((ComboBoxItem)cbColorEtiqueta.SelectedItem).Tag.ToString());

                                cmd.ExecuteNonQuery();
                            }
                        }

                        MostrarUsuaris();

                        nouNom.Clear();
                        nouContrasenya.Clear();
                        nouConfirmarContrasenya.Clear();
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

        private void EsborrarUsuari(object sender, RoutedEventArgs e)
        {
            if (llistaUsuaris.SelectedItem == null)
            {                
                MessageBox.Show("Selecciona un usuari per esborrar.");
            }
            else
            {
                Usuari usuariSeleccionat = (Usuari)llistaUsuaris.SelectedItem;

                try
                {
                    string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";
                    using (MySqlConnection conexion = new MySqlConnection(cadena))
                    {
                        conexion.Open();

                        string sqlTasques = "DELETE FROM Tasca WHERE usuariId = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sqlTasques, conexion))
                        {
                            cmd.Parameters.AddWithValue("@id", usuariSeleccionat.Id);
                            cmd.ExecuteNonQuery();
                        }

                        string sqlUsuari = "DELETE FROM Usuari WHERE id = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sqlUsuari, conexion))
                        {
                            cmd.Parameters.AddWithValue("@id", usuariSeleccionat.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MostrarUsuaris();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en esborrar l'usuari: " + ex.Message);
                }
            }                
        }
    }
}
