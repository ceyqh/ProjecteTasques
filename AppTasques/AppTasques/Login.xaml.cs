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

        private void Entrar(object sender, RoutedEventArgs e)
        {
            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadena))
                {
                    conexion.Open();

                    string sql = "SELECT COUNT(*) FROM Usuari WHERE nom=@nom AND contrasenya=@pass";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nom", txtNom.Text);          // TextBox del nombre
                        cmd.Parameters.AddWithValue("@pass", txtContrasenya.Password); // PasswordBox de la contraseña


                        int idUsuari = 1; // valor per defecte

                        int existe = Convert.ToInt32(cmd.ExecuteScalar());

                        if (existe > 0)
                        {
                            MainWindow mw = new MainWindow();
                            mw.UsuariId = idUsuari;
                            mw.Show();
                            this.Close();
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
