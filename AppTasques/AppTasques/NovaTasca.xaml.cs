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
    /// Lógica de interacción para NovaTasca.xaml
    /// </summary>
    public partial class NovaTasca : Window
    {
        public event Action<Tasca> TascaAfegida;

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
                colorEstat = valorColorEstat
            };

            TascaAfegida?.Invoke(novaTasca);

            //this.Close();
        }
    }
}
