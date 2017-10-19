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
    /// Logique d'interaction pour Partie.xaml
    /// </summary>
    public partial class Partie : UserControl
    {
        public MainWindow Main { get; set; }
        // Variables accessibles partout dans la partie
        int phase = 1;

        public Partie(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this; // TODO changé pour bon binding
            /*      Example année passé
            dtgListePersonnes.ItemsSource = LstPersonnes;
            grdModification.DataContext = LstPersonnes;
            */
            Main = main;



            // Initialiser le nombre de points de blindage de chaque joueurs (25)
            //
            // nbBlindageJ.Text = joueur.nbBlindage
            // nbBlindageA.Text = adversaire.nbBlindage


            // Initialiser les ressources de chaque joueurs (0)
            //
            // nbCharroniteJ = joueur.ressource.charronite
            // nbBarilsJ = joueur.ressource.baril
            // nbAlainDollarJ = joueur.ressource.alainDollar
            // nbCharroniteA = adversaire.ressource.baril
            // nbBarilsA = adversaire.ressource.baril
            // nbAlainDollarJ = adversaire.ressource.alainDollar


            // Prendre les avatars des deux joueurs et les mettres dans le XAML 
            //


            // Initialiser la phase à "phase de ressource"


            // Déterminer au hasard qui débute la partie


            // Donner une main à chaque joueurs ? (Ici ou pas ? )
            // Initialiser le nombre de carte dans chaque paquet pour l'afficher (44)


            // Initialiser les emplacements d'unités et de bâtiements


        }

        private void btnTerminerPhase_Click(object sender, RoutedEventArgs e)
        {
            changerPhase();
        }

        private void changerPhase()
        {
            switch (phase)
            {
                case 1:
                    phase++;
                    txBlphaseRessource.Background = Brushes.Beige;
                    txBlphasePrincipale.Background = Brushes.Crimson;
                    break;
                case 2:
                    phase++;
                    txBlphasePrincipale.Background = Brushes.Beige;
                    txBlphaseAttaque.Background = Brushes.Crimson;
                    break;
                case 3:
                    phase++;
                    txBlphaseAttaque.Background = Brushes.Beige;
                    txBlphaseFin.Background = Brushes.Crimson;
                    break;
                case 4:
                    phase = 1; // La phase de fin est terminer, nous retournons à la première phase
                    txBlphaseFin.Background = Brushes.Beige;
                    txBlphaseRessource.Background = Brushes.Crimson;
                    break;
            }
        }
    }
}

