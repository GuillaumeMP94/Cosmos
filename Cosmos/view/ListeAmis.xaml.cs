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
        public MenuPrincipal MenuPrincipal { get; set; }
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
                txbAmi.Name = "txb" + ami.Nom; 
                txbAmi.MouseLeftButtonUp += txbAmi_ClickLeftMouseButton;

                stpListeAmis.Children.Add(txbAmi);
            }
        }

        private void txbAmi_ClickLeftMouseButton(object sender, MouseButtonEventArgs e)
        {
            RafraichirListeAmis();
            
            TextBlock txbAmi = (TextBlock)sender;
            txbAmi.Background = Brushes.DarkGoldenrod;

            btnModifier.IsEnabled = true;
            btnModifier.Opacity = 1;

            btnSupprimer.IsEnabled = true;
            btnSupprimer.Opacity = 1;

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

        private void RafraichirListeAmis()
        {
            foreach (object ami in stpListeAmis.Children)
            {
                if (ami is TextBlock)
                {
                    TextBlock txbUnAmi = (TextBlock)ami;
                    txbUnAmi.Background = Brushes.Black;
                }
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranModifierAmi();

        }
    }
}
