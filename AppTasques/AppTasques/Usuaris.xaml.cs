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
    /// Lógica de interacción para Usuaris.xaml
    /// </summary>
    public partial class Usuaris : Window
    {
        public Usuaris()
        {
            InitializeComponent();
        }

        private void AfegirUsuari(object sender, RoutedEventArgs e)
        {
            if (nouNom.Text == "" || nouContrasenya.Password == "" || nouConfirmarContrasenya.Password == "")
            {
                MessageBox.Show("Falten camps per omplir.");
            } 
            
            else
            {
                if (nouContrasenya.Password == nouConfirmarContrasenya.Password)
                {
                    llistaUsuaris.Items.Add(nouNom.Text);

                    nouNom.Clear();
                    nouContrasenya.Clear();
                    nouConfirmarContrasenya.Clear();
                }

                else
                {
                    MessageBox.Show("Les contrasenyes no coincideixen.");
                }
            }     
        }
    }
}
