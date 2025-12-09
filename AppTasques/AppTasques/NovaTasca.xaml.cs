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
            Tasca nova = new Tasca()
            {
                nom = txtNom.Text,
                descripcio = txtDescripcio.Text,
                etiqueta = txtEtiqueta.Text,
                dataInici = txtInici.Text,
                dataFinal = txtFinal.Text,
                estat = "Pendent"
            };

            TascaAfegida?.Invoke(nova);

            this.Close();

        }
    }
}
