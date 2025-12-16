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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace AppTasques
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //SqlConnection laMevaConnexioSQL;

        public MainWindow()
        {
            InitializeComponent();

            //string laMevaConnexio = ConfigurationManager.ConnectionStrings["ProjecteTasques.Properties.Settings.ProjecteTasquesConnectionString"].ConnectionString;
            //laMevaConnexioSQL = new SqlConnection(laMevaConnexio);

            //MostraTasques();

            Tasca benvinguda = new Tasca()
            {
                nom = "Benvingut/da",
                descripcio = "Aquesta és una primera tasca de prova, la pots modificar tant com vulguis.",
                colorEtiqueta = "#8f8c8d",
                etiqueta = "Tutorial",
                dataInici = "03/12/2025",
                dataFinal = "03/12/2026",
                estat = "Finalitzat",
                colorEstat = "#55eb34"
            };

            llistaTasques.Items.Add(benvinguda);

            for (int i = 0; i < 20; i++)
            {
                Tasca exemple = new Tasca()
                {
                    nom = i + ". Instal·lar el servidor DHCP",
                    descripcio = "Realitzar tots els passos necessaris utilitzant linux",
                    etiqueta = "Servidor",
                    colorEtiqueta = "#fa5f5f",
                    dataInici = "03/12/2025",
                    dataFinal = "03/12/2025",
                    estat = "Per començar",
                    colorEstat = "#fa5f5f"
                };

                llistaTasques.Items.Add(exemple);

            }
        }
        private void llistaTasques_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string color = (llistaTasques.SelectedItem as Tasca).colorEtiqueta;
            SolidColorBrush pinzell = (SolidColorBrush)(new BrushConverter().ConvertFrom(color));

            exNomTasca.Text = (llistaTasques.SelectedItem as Tasca).nom;
            exColorEtiqueta.Background = pinzell;
            exEtiqueta.Text = (llistaTasques.SelectedItem as Tasca).etiqueta;
            exDescripcio.Text = (llistaTasques.SelectedItem as Tasca).descripcio;

            exInici.Text = "Inici:";
            exDataInici.Text = (llistaTasques.SelectedItem as Tasca).dataInici;

            exFinal.Text = "Final:";
            exDataFinal.Text = (llistaTasques.SelectedItem as Tasca).dataFinal;
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

        private void FinestraEditarTasca(object sender, RoutedEventArgs e)
        {
            if (llistaTasques.SelectedItem == null)
            {
                MessageBox.Show("Escull una tasca.");
            }

            else
            {
                Tasca tascaSeleccionada = llistaTasques.SelectedItem as Tasca;

                EditarTasca et = new EditarTasca(tascaSeleccionada);
                et.Show();
            }                
        }

        private void FinestraUsuaris(object sender, RoutedEventArgs e)
        {
            Usuaris usuaris = new Usuaris();
            usuaris.Show();
        }

        //private void MostraTasques()
        //{
        //    string consulta = "SELECT * FROM TASCA";

        //    SqlDataAdapter elMeuAdaptador = new SqlDataAdapter(consulta, laMevaConnexioSQL);

        //    using (elMeuAdaptador)
        //    {
        //        DataTable tasquesTaula = new DataTable();

        //        elMeuAdaptador.Fill(tasquesTaula);

        //        Tasca tascaSQL = new Tasca()
        //        {
        //            nom = "nom",
        //            descripcio = "descripcio",
        //            etiqueta = "etiqueta",
        //            colorEtiqueta = "coloretiqueta",
        //            dataInici = "datainici",
        //            dataFinal = "datafinal",
        //            estat = "estat",
        //            colorEstat = "#fa5f5f"
        //        };

        //        llistaTasques.Items.Add(tascaSQL);
        //    }
        //}
    }
}
