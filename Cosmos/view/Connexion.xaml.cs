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
using System.Text.RegularExpressions;
using Cosmos.accesBD;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : UserControl
    {
        public MainWindow Main { get; set; }
        public Connexion(MainWindow main)
        {
            InitializeComponent();

            Main = main;
        }

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (ValiderChampSaisi(txbPseudo.Text))
            {

                Main.EcranMenuPrincipal();

            }
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Main.QuitterMain();
        }

        private void btnCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranCreerCompte();
        }

        private void btnPasswordOublie_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranRecuperation();
        }

        private bool ValiderChampSaisi(string champ)
        {
            if (champ.Length > 2)
            {
                string pattern = @"([a-zA-Z0-9]+)";
                Match resultat = Regex.Match(champ, pattern);

                if (resultat.Success)
                {
                    if(MySqlUtilisateurService.RetrieveByNom(champ) != null)
                    {
                        Main.EcranMenuPrincipal();
                    }
                }
            }
            else
            {
                
            }

            return false;
        }
    }
}
