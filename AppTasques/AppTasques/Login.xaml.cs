using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace AppTasques
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        // LOGIN

        private void Entrar(object sender, RoutedEventArgs e)
        {
            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadena))
                {
                    conexion.Open();

                    // Comptar els usuaris que coincideixen amb la contrasenya

                    string sql = "SELECT COUNT(*) FROM Usuari WHERE nom=@nom AND contrasenya=@pass";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nom", txtNom.Text);
                        cmd.Parameters.AddWithValue("@pass", txtContrasenya.Password);

                        int existeix = Convert.ToInt32(cmd.ExecuteScalar());

                        // Si el recompte és > 0 vol dir que existeix

                        if (existeix > 0)
                        {
                            string sql2 = "SELECT id, nom, rol FROM Usuari WHERE nom=@nom AND contrasenya=@pass LIMIT 1";

                            using (MySqlCommand cmd2 = new MySqlCommand(sql2, conexion))
                            {
                                cmd2.Parameters.AddWithValue("@nom", txtNom.Text);
                                cmd2.Parameters.AddWithValue("@pass", txtContrasenya.Password);

                                using (MySqlDataReader reader = cmd2.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int idUsuari = reader.GetInt32("id");
                                        string nomUsuari = reader.GetString("nom");
                                        string nomRol= reader.GetString("rol");

                                        MainWindow mw = new MainWindow();
                                        mw.UsuariId = idUsuari;
                                        mw.UsuariNom = nomUsuari;
                                        mw.UsuariRol = nomRol;
                                        mw.Show();
                                        this.Close();
                                    }
                                }
                            }
                        }

                        else
                        {
                            MessageBox.Show("Usuari o contrasenya incorrectes.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al connectar: " + ex.Message);
            }
        }
    }
}
