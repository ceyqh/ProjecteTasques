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

            tascaSeleccionada.dataInici = edInici.SelectedDate?.ToString("dd/MM/yyyy");
            tascaSeleccionada.dataFinal = edFinal.SelectedDate?.ToString("dd/MM/yyyy");

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
                edInici.SelectedDate = final;
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
