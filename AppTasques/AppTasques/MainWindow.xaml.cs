using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace AppTasques
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int UsuariId { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        { 
            MostraTasques(); 
        }

        private void llistaTasques_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tasca = llistaTasques.SelectedItem as Tasca;
            if (tasca == null) return;

            SolidColorBrush pinzell = (SolidColorBrush)(new BrushConverter().ConvertFrom(tasca.colorEtiqueta));

            exNomTasca.Text = tasca.nom;
            exColorEtiqueta.Background = pinzell;
            exEtiqueta.Text = tasca.etiqueta;
            exDescripcio.Text = tasca.descripcio;

            exInici.Text = "Inici:";
            exDataInici.Text = tasca.dataInici;

            exFinal.Text = "Final:";
            exDataFinal.Text = tasca.dataFinal;
        }

        private void FinestraNovaTasca(object sender, RoutedEventArgs e)
        {
            NovaTasca nt = new NovaTasca();
            nt.UsuariId = this.UsuariId;

            nt.TascaAfegida += (tasca) =>
            {
                llistaTasques.Items.Add(tasca);
            };

            nt.Show();
        }

        private void FinestraEditarTasca(object sender, RoutedEventArgs e)
        {
            if (llistaTasques.SelectedItem == null)
            {
                MessageBox.Show("Escull una tasca.");
                return;
            }

            Tasca tascaSeleccionada = llistaTasques.SelectedItem as Tasca;
            EditarTasca et = new EditarTasca(tascaSeleccionada);
            et.Show();
        }

        private void FinestraUsuaris(object sender, RoutedEventArgs e)
        {
            Usuaris usuaris = new Usuaris();
            usuaris.Show();
        }

        private void MostraTasques()
        {
            llistaTasques.Items.Clear();

            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

            using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
                conexion.Open();

                string sql = @"SELECT nom, descripcio, etiqueta, colorEtiqueta, dataInici, dataFinal, estat 
                               FROM Tasca 
                               WHERE usuariId=@usuariId";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuariId", UsuariId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tasca tascaSQL = new Tasca()
                            {
                                nom = reader["nom"].ToString(),
                                descripcio = reader["descripcio"].ToString(),
                                etiqueta = reader["etiqueta"].ToString(),
                                colorEtiqueta = reader["colorEtiqueta"].ToString(),
                                dataInici = Convert.ToDateTime(reader["dataInici"]).ToString("dd/MM/yyyy"),
                                dataFinal = Convert.ToDateTime(reader["dataFinal"]).ToString("dd/MM/yyyy"),
                                estat = reader["estat"].ToString(),                            
                            };

                            llistaTasques.Items.Add(tascaSQL);
                        }
                    }
                }
            }
        }
    }
}
