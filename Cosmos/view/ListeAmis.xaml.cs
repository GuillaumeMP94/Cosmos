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
using Cosmos.metier;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour ListeAmis.xaml
    /// </summary>
    public partial class ListeAmis : UserControl
    {
        public MainWindow Main { get; set; }
        public UserControl ContenuEcran { get; set; }
        
        public ListeAmis(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            AfficherListeAmis();
        }

        private void AfficherListeAmis()
        {
            foreach (Utilisateur ami in Main.LstAmis)
            {

                TextBlock txbAmi = new TextBlock();
                txbAmi.Padding = new Thickness(10, 5, 0, 0);
                txbAmi.Foreground = Brushes.White;
                txbAmi.Text = ami.Nom;
                txbAmi.MouseLeftButtonUp += txbAmi_ClickLeftMouseButton; 

            }
        }

        private void txbAmi_ClickLeftMouseButton(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void btnFermer_Click(object sender, RoutedEventArgs e)
        {
            Main.grdMain.Children.Remove(this);
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
