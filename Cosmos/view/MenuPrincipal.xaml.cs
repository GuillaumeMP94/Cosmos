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

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : UserControl
    {
        public MainWindow Main { get; set; }
        public MenuPrincipal(MainWindow main)
        {
            InitializeComponent();

            Main = main;
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Main.QuitterMain();
        }

        private void btnRegleTuto_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranReglements();
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            Main.QuitterMain();
        }

        private void btnGestionDecks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMultijoueur_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCampagne_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranCampagne();
        }

        private void btnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranConnexion();
        }
    }
}
