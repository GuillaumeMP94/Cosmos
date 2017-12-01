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
using Cosmos.accesBD;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour ListeAmis.xaml
    /// </summary>
    public partial class ListeAmis : UserControl
    {
        public MainWindow Main { get; set; }
        
        public ListeAmis(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            AfficherListeAmis();
        }

        public void AfficherListeAmis()
        {
            stpListeAmis.Children.Clear();
            foreach (Utilisateur ami in Main.LstAmis)
            {
                TextBlock txbAmi = new TextBlock();
                txbAmi.Padding = new Thickness(10, 5, 0, 0);
                txbAmi.Foreground = Brushes.White;
                txbAmi.FontSize = 15;
                txbAmi.FontWeight = FontWeights.Bold;
                txbAmi.Text = ami.Nom;
                txbAmi.Name = "txb" + ami.Nom; 
                txbAmi.MouseLeftButtonUp += txbAmi_ClickLeftMouseButton;

                stpListeAmis.Children.Add(txbAmi);
            }
        }

        private void txbAmi_ClickLeftMouseButton(object sender, MouseButtonEventArgs e)
        {
            RafraichirSurbrillanceListeAmis();
            
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
            Main.ContenuAddModifSupp = new AjouterAmi(this);
            Main.grdMain.Children.Add(Main.ContenuAddModifSupp);
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            string ami = retrouverAmiSelectionne();

            Main.ContenuAddModifSupp = new SupprimerAmi(this, MySqlUtilisateurService.RetrieveByNom(ami));
            Main.grdMain.Children.Add(Main.ContenuAddModifSupp);
        }

        

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            string ami = retrouverAmiSelectionne();

            Main.ContenuAddModifSupp = new ModifierAmi(this, MySqlUtilisateurService.RetrieveByNom(ami));
            Main.grdMain.Children.Add(Main.ContenuAddModifSupp);
        }

        private void RafraichirSurbrillanceListeAmis()
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

        private string retrouverAmiSelectionne()
        {
            string ami = "";
            foreach(object laListe in stpListeAmis.Children)
            {
                if (laListe is TextBlock)
                {
                    TextBlock txbAmi = (TextBlock)laListe;
                    if (txbAmi.Background == Brushes.DarkGoldenrod)
                    {
                        return ami = txbAmi.Text;
                    }
                }
            }
            return ami;
        }

    }
}
