using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AppTasques
{
    /// <summary>
    /// Lógica de interacción para EditarTasca.xaml
    /// </summary>
    public partial class EditarTasca : Window
    {
        private Tasca tascaSeleccionada;

        public EditarTasca(Tasca tasca)
        {
            InitializeComponent();
            tascaSeleccionada = tasca;
            OmplirCamps();
        }

        private void Guardar(object sender, RoutedEventArgs e)
        {
            tascaSeleccionada.nom = edNom.Text;
            tascaSeleccionada.etiqueta = edEtiqueta.Text;
            tascaSeleccionada.descripcio = edDescripcio.Text;
            tascaSeleccionada.estat = (edColorEstat.SelectedItem as ComboBoxItem)?.Content.ToString();
            tascaSeleccionada.colorEtiqueta = (edColorEtiqueta.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            tascaSeleccionada.dataInici = edInici.SelectedDate?.ToString("yyyy-MM-dd"); // formato MySQL
            tascaSeleccionada.dataFinal = edFinal.SelectedDate?.ToString("yyyy-MM-dd");

            
            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

            using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
                conexion.Open();

                string sql = @"UPDATE Tasca 
                               SET nom=@nom, descripcio=@descripcio, etiqueta=@etiqueta, 
                                   colorEtiqueta=@colorEtiqueta, dataInici=@dataInici, 
                                   dataFinal=@dataFinal, estat=@estat
                               WHERE id=@id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@nom", tascaSeleccionada.nom);
                    cmd.Parameters.AddWithValue("@descripcio", tascaSeleccionada.descripcio);
                    cmd.Parameters.AddWithValue("@etiqueta", tascaSeleccionada.etiqueta);
                    cmd.Parameters.AddWithValue("@colorEtiqueta", tascaSeleccionada.colorEtiqueta);
                    cmd.Parameters.AddWithValue("@dataInici", tascaSeleccionada.dataInici);
                    cmd.Parameters.AddWithValue("@dataFinal", tascaSeleccionada.dataFinal);
                    cmd.Parameters.AddWithValue("@estat", tascaSeleccionada.estat);
                    cmd.Parameters.AddWithValue("@id", 1);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Tasca actualitzada correctament.");
            this.Close();
        }

        private void OmplirCamps()
        {
            edNom.Text = tascaSeleccionada.nom;
            edEtiqueta.Text = tascaSeleccionada.etiqueta;
            edDescripcio.Text = tascaSeleccionada.descripcio;

            if (DateTime.TryParse(tascaSeleccionada.dataInici, out DateTime inici))
            {
                edInici.SelectedDate = inici;
            }

            if (DateTime.TryParse(tascaSeleccionada.dataFinal, out DateTime final))
            {
                edFinal.SelectedDate = final; // corregido: antes ponías edInici
            }

            foreach (ComboBoxItem item in edColorEstat.Items)
            {
                if (item.Content.ToString() == tascaSeleccionada.estat)
                {
                    edColorEstat.SelectedItem = item;
                }
            }

            foreach (ComboBoxItem item in edColorEtiqueta.Items)
            {
                if (item.Tag.ToString() == tascaSeleccionada.colorEtiqueta)
                {
                    edColorEtiqueta.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
