using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            // Tasqua de prova
            Tasca exemple = new Tasca()
            {
                nom = "Instal·lar el servidor DHCP",
                descripcio = "Realitzar tots els passos necessaris utilitzant linux",
                etiqueta = "#Servidor",
                dataInici = "03/12/2025",
                dataFinal = "03/12/2025",
                estat = "Pendent"
            };

            llistaTasques.Items.Add(exemple);
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

            nt.TascaAfegida += (tasca) =>
            {
                llistaTasques.Items.Add(tasca);
            };

            nt.Show();
        }
    }
}
