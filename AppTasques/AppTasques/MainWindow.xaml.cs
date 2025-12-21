using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace AppTasques
{
    public partial class MainWindow : Window
    {
        // Atributs de l'usuari amb el que hem iniciat sessio

        public int UsuariId { get; set; }
        public string UsuariNom { get; set; }
        public string UsuariRol{ get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        // Quan es carregui la pàgina 

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        { 
            // Mostrar les tasques personals si és usuari

            if (UsuariRol == "usuari")
            {
                MostraTasques();
            }

            // Mostrar totes les tasques de tots els usuaris si és administrador o supervisor

            else if (UsuariRol == "administrador" || UsuariRol == "supervisor" || UsuariRol == "" )
            {
                MostraTasquesTotes();
            }
            txtBenvinguda.Text = "Benvingut/da, " + UsuariNom;
        }

        // Quan es fa doble click a una tasca per visualitzar-la

        private void llistaTasques_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tasca = llistaTasques.SelectedItem as Tasca;
            if (tasca == null) return;

            SolidColorBrush pinzell = (SolidColorBrush)(new BrushConverter().ConvertFrom(tasca.colorEtiqueta));

            exNomTasca.Text = tasca.nom;
            exColorEtiqueta.Background = pinzell;
            exEtiqueta.Text = tasca.etiqueta;
            exDescripcio.Text = tasca.descripcio;

            pinzell = (SolidColorBrush)(new BrushConverter().ConvertFrom(tasca.colorEstat));

            exEstat.Text = tasca.estat;
            exColorEstat.Background = pinzell;

            exInici.Text = "Inici:";
            exDataInici.Text = tasca.dataInici;

            exFinal.Text = "Final:";
            exDataFinal.Text = tasca.dataFinal;

            if (UsuariRol == "administrador" || UsuariRol == "supervisor")
            {
                exAutorText.Text = "Autor:";
                exAutor.Text = tasca.usuariNom + " (" + tasca.usuariId + ")";
            }
        }

        // CREAR NOVA TASCA

        private void FinestraNovaTasca(object sender, RoutedEventArgs e)
        {
            if (UsuariRol == "administrador" || UsuariRol == "usuari")
            {
                NovaTasca nt = new NovaTasca();
                nt.UsuariId = this.UsuariId;

                nt.TascaAfegida += (tasca) =>
                {
                    llistaTasques.Items.Add(tasca);
                };

                nt.Show();
            }
            else
            {
                MessageBox.Show("No tens prou permisos.");
            }
            
        }

        // EDITAR LA TASCA

        private void FinestraEditarTasca(object sender, RoutedEventArgs e)
        {
            if (UsuariRol == "administrador" || UsuariRol == "usuari")
            {
                if (llistaTasques.SelectedItem == null)
                {
                    MessageBox.Show("Escull una tasca.");
                }
                else
                {
                    Tasca tascaSeleccionada = llistaTasques.SelectedItem as Tasca;
                    EditarTasca et = new EditarTasca(tascaSeleccionada);

                    et.TascaActualitzada += (tascaEditada) =>
                    {
                        if (UsuariRol == "usuari")
                        {
                            MostraTasques();
                        }
                        else
                        {
                            MostraTasquesTotes();
                        }
                    };

                    et.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No tens prou permisos.");
            }
            
        }

        // GESTIONAR USUARIS

        private void FinestraUsuaris(object sender, RoutedEventArgs e)
        {
            if (UsuariRol == "administrador")
            {
                Usuaris usuaris = new Usuaris();
                usuaris.Show();
            }
            else
            {
                MessageBox.Show("No tens prou permisos.");
            }            
        }

        // MOSTRAR TASQUES DE L'USUARI

        private void MostraTasques()
        {
            llistaTasques.Items.Clear();

            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

            using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
                conexion.Open();

                string sql = @"SELECT t.id, t.nom, t.descripcio, t.etiqueta, t.colorEtiqueta, t.dataInici, t.dataFinal, t.estat, t.usuariId,
                            u.nom AS UsuariNom
                            FROM Tasca t
                            JOIN Usuari u ON t.usuariId = u.id
                            WHERE t.usuariId=@usuariId";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuariId", UsuariId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tasca tascaSQL = new Tasca()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                nom = reader["nom"].ToString(),
                                descripcio = reader["descripcio"].ToString(),
                                etiqueta = reader["etiqueta"].ToString(),
                                colorEtiqueta = reader["colorEtiqueta"].ToString(),
                                dataInici = Convert.ToDateTime(reader["dataInici"]).ToString("dd/MM/yyyy"),
                                dataFinal = Convert.ToDateTime(reader["dataFinal"]).ToString("dd/MM/yyyy"),
                                estat = reader["estat"].ToString(),
                                usuariId = Convert.ToInt32(reader["usuariId"]),
                                usuariNom = reader["UsuariNom"].ToString()
                            };

                            if (tascaSQL.estat == "Per començar") { tascaSQL.colorEstat = "#fa5f5f"; }
                            else if (tascaSQL.estat == "Començat") { tascaSQL.colorEstat = "#eb7d34"; }
                            else if (tascaSQL.estat == "Repassar") { tascaSQL.colorEstat = "#ebeb34"; }
                            else if (tascaSQL.estat == "Millorar") { tascaSQL.colorEstat = "#489bfa"; }
                            else if (tascaSQL.estat == "Finalitzat") { tascaSQL.colorEstat = "#55eb34"; }

                            llistaTasques.Items.Add(tascaSQL);
                        }
                    }
                }
            }
        }

        // MOSTRAR TOTES LES TASQUES
        private void MostraTasquesTotes()
        {
            llistaTasques.Items.Clear();

            string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

            using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
                conexion.Open();

                string sql = @"SELECT t.id, t.nom, t.descripcio, t.etiqueta, t.colorEtiqueta, t.dataInici, t.dataFinal, t.estat, t.usuariId,
                            u.nom AS UsuariNom
                            FROM Tasca t
                            JOIN Usuari u ON t.usuariId = u.id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tasca tascaSQL = new Tasca()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                nom = reader["nom"].ToString(),
                                descripcio = reader["descripcio"].ToString(),
                                etiqueta = reader["etiqueta"].ToString(),
                                colorEtiqueta = reader["colorEtiqueta"].ToString(),
                                dataInici = Convert.ToDateTime(reader["dataInici"]).ToString("dd/MM/yyyy"),
                                dataFinal = Convert.ToDateTime(reader["dataFinal"]).ToString("dd/MM/yyyy"),
                                estat = reader["estat"].ToString(),
                                usuariId = Convert.ToInt32(reader["usuariId"]),
                                usuariNom = reader["UsuariNom"].ToString()
                            };

                            if (tascaSQL.estat == "Per començar") { tascaSQL.colorEstat = "#fa5f5f"; }
                            else if (tascaSQL.estat == "Començat") { tascaSQL.colorEstat = "#eb7d34"; }
                            else if (tascaSQL.estat == "Repassar") { tascaSQL.colorEstat = "#ebeb34"; }
                            else if (tascaSQL.estat == "Millorar") { tascaSQL.colorEstat = "#489bfa"; }
                            else if (tascaSQL.estat == "Finalitzat") { tascaSQL.colorEstat = "#55eb34"; }

                            llistaTasques.Items.Add(tascaSQL);
                        }
                    }
                }
            }
        }

        private void EsborrarTasca(object sender, RoutedEventArgs e)
        {
            if (UsuariRol == "administrador" || UsuariRol == "usuari")
            {
                if (llistaTasques.SelectedItem == null)
                {
                    MessageBox.Show("Selecciona una tasca per esborrar.");
                    return;
                }

                Tasca tascaSeleccionada = llistaTasques.SelectedItem as Tasca;
                
                string cadena = "Server=ellaboratori.cat;Database=alex;Uid=alex;Pwd=1234";

                using (MySqlConnection conexion = new MySqlConnection(cadena))
                {
                    conexion.Open();

                    string sql = "DELETE FROM Tasca WHERE id=@id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", tascaSeleccionada.id);

                        int filesAfactades = cmd.ExecuteNonQuery();

                        if (filesAfactades > 0)
                        {
                            if (UsuariRol == "usuari")
                            {
                                MostraTasques();
                            }

                            else if (UsuariRol == "administrador" || UsuariRol == "supervisor" || UsuariRol == "")
                            {
                                MostraTasquesTotes();
                            }
                            txtBenvinguda.Text = "Benvingut/da, " + UsuariNom;
                        }
                        else
                        {
                            MessageBox.Show("No s'ha pogut esborrar la tasca.");
                        }
                    }
                }                
            }

            else
            {
                MessageBox.Show("No tens prou permisos.");
            }            
        }

        private void TancarSessio(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
