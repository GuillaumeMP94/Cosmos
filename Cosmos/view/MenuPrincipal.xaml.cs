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

            ValiderNeoJoueur();
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
            Main.EcranOptionCompte();
        }

        private void btnGestionDecks_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranGestionCartes();
        }

        private void btnMultijoueur_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("En construction");
        }

        private void btnCampagne_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranCampagne();
        }

        private void btnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            Main.UtilisateurConnecte = null;
            Main.EcranConnexion();
        }

        private void btnListeAmis_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranListeAmis();
        }

        private void ValiderNeoJoueur()
        {
            if (Main.UtilisateurConnecte.CartesUtilisateurs.Count == 0)
            {
                btnCampagne.Opacity = 0.6;
                btnCampagne.IsEnabled = false;

                btnMultijoueur.Opacity = 0.6;
                btnMultijoueur.IsEnabled = false;

                btnGestionDecks.Opacity = 0.6;
                btnGestionDecks.IsEnabled = false;

                btnOptions.Opacity = 0.6;
                btnOptions.IsEnabled = false;

                btnListeAmis.Opacity = 0.6;
                btnListeAmis.IsEnabled = false;
            }
            else
            {
                btnCampagne.Opacity = 1;
                btnCampagne.IsEnabled = true;

                btnMultijoueur.Opacity = 1;
                btnMultijoueur.IsEnabled = true;

                btnGestionDecks.Opacity = 1;
                btnGestionDecks.IsEnabled = true;

                btnOptions.Opacity = 1;
                btnOptions.IsEnabled = true;

                btnListeAmis.Opacity = 1;
                btnListeAmis.IsEnabled = true;
            }
            
        }
    }
}
