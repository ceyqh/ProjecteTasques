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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppTasques
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Afegir_Click(object sender, RoutedEventArgs e)
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

            // afegir a la taula
            llistaTasques.Items.Add(nova);

            // netejar camps
            txtNom.Clear();
            txtDescripcio.Clear();
            txtEtiqueta.Clear();
            txtInici.Clear();
            txtFinal.Clear();
        }

        private void llistaTasques_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show((llistaTasques.SelectedItem as Tasca).nom);

            if (llistaTasques.SelectedItem as Tasca != null)
            {
                TabItem ti = new TabItem();
                ti.Header = (llistaTasques.SelectedItem as Tasca).nom;
                Pestanyes.Items.Add(ti);
                Pestanyes.SelectedIndex = 2;
            }
        }

        private void FinestraNovaTasca(object sender, RoutedEventArgs e)
        {
            NovaTasca nt = new NovaTasca();
            nt.Show();
        }
    }
}
