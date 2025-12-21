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
    /// Lógica de interacción para NovaTasca.xaml
    /// </summary>
    public partial class NovaTasca : Window
    {
        public event Action<Tasca> TascaAfegida;
        public int UsuariId { get; set; }

        public NovaTasca()
        {
            InitializeComponent();
        }

        private void AfegirNovaTasca(object sender, RoutedEventArgs e)
        {
            var itemColorEtiqueta = (ComboBoxItem)cbColorEtiqueta.SelectedItem;
            string valorColorEtiqueta = itemColorEtiqueta.Tag.ToString();

            var itemColorEstat = (ComboBoxItem)cbColorEstat.SelectedItem;
            string tagColorEstat = itemColorEstat.Content.ToString();
            string valorColorEstat = "";

            if (tagColorEstat == "Per començar") { valorColorEstat = "#fa5f5f"; }
            else if (tagColorEstat == "Començat") { valorColorEstat = "#eb7d34"; }
            else if (tagColorEstat == "Repassar") { valorColorEstat = "#ebeb34"; }
            else if (tagColorEstat == "Millorar") { valorColorEstat = "#489bfa"; }
            else if (tagColorEstat == "Finalitzat") { valorColorEstat = "#55eb34"; }

            Tasca novaTasca = new Tasca()
            {
                nom = txtNom.Text,
                descripcio = txtDescripcio.Text,
                etiqueta = txtEtiqueta.Text,
                colorEtiqueta = valorColorEtiqueta,
                dataInici = txtInici.Text,
                dataFinal = txtFinal.Text,
                estat = tagColorEstat,
                colorEstat = valorColorEstat,
                usuariId = UsuariId
            };

            try
            {
                string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

                using (MySqlConnection conexion = new MySqlConnection(cadena))
                {
                    conexion.Open();

                    string sql = @"INSERT INTO Tasca 
                        (nom, descripcio, etiqueta, colorEtiqueta, dataInici, dataFinal, estat, usuariId) 
                        VALUES (@nom, @descripcio, @etiqueta, @colorEtiqueta, @dataInici, @dataFinal, @estat, @usuariId)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                    {
                        DateTime inici = DateTime.ParseExact(txtInici.Text, "dd/MM/yyyy", null);
                        DateTime final = DateTime.ParseExact(txtFinal.Text, "dd/MM/yyyy", null);

                        cmd.Parameters.AddWithValue("@nom", novaTasca.nom);
                        cmd.Parameters.AddWithValue("@descripcio", novaTasca.descripcio);
                        cmd.Parameters.AddWithValue("@etiqueta", novaTasca.etiqueta);
                        cmd.Parameters.AddWithValue("@colorEtiqueta", novaTasca.colorEtiqueta);
                        cmd.Parameters.AddWithValue("@dataInici", inici.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@dataFinal", final.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@estat", novaTasca.estat);
                        cmd.Parameters.AddWithValue("@colorEstat", novaTasca.colorEstat);
                        cmd.Parameters.AddWithValue("@usuariId", UsuariId);

                        cmd.ExecuteNonQuery();

                        this.Close();
                    }
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inserir tasca: " + ex.Message);
            }

            TascaAfegida?.Invoke(novaTasca);
        }
    }
}
