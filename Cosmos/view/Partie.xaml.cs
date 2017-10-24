using Cosmos.metier;
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

        int phase;

        public Partie(MainWindow main, Joueur joueur1, Joueur joueur2)
        {
            InitializeComponent();

            TableDeJeu laTableDeJeu = new TableDeJeu(joueur1.DeckAJouer , joueur2.DeckAJouer);

            this.DataContext = this; // TODO changé pour bon binding
            grd1.DataContext = this; // TODO
            grd2.DataContext = this; // TODO

            Main = main;

            // Initialiser les points de blindage
            txBlnbBlindageJ.Text = joueur1.PointDeBlindage.ToString();
            txBlnbBlindageA.Text = joueur2.PointDeBlindage.ToString();

            // Initialiser les points de ressources
            // txBlnbCharroniteJ.Text = joueur1.Active.Charronite
            // txBlnbBarilJ.Text = joueur1.Active.BarilNucleaire
            // txBlnbAlainDollarJ.Text = joueur1.Active.AlainDollar
            // txBlnbCharroniteA.Text = joueur2.Active.Charronite
            // txBlnbBarilA.Text = joueur2.Active.BarilNucleaire
            // txBlnbAlainDollarA.Text = joueur2.Active.AlainDollar

            // Prendre les avatars des deux joueurs et les mettres dans le XAML 
            //

            // Initialiser la phase à "phase de ressource"
            phase = laTableDeJeu.Phase;

            // Brasser les deck
            laTableDeJeu.BrasserDeck(laTableDeJeu.DeckJ1);
            laTableDeJeu.BrasserDeck(laTableDeJeu.DeckJ2);

            // Donner une main à chaque joueurs 
            // Initialiser le nombre de carte dans chaque paquet pour l'afficher (44)
            int compteurNbCarte = 0;
            while( compteurNbCarte != 6 )
            {
                laTableDeJeu.PigerCarte(joueur1.DeckAJouer, true );
                compteurNbCarte++;
            }

            compteurNbCarte = 0;
            while (compteurNbCarte != 6)
            {
                laTableDeJeu.PigerCarte(joueur2.DeckAJouer, false);
                compteurNbCarte++;
            }

            // Initialiser les emplacements d'unités 
            // TODO 

            // Initialiser les emplacements de bâtiements
            //

            // Binding Points de blindage
            txBlnbBlindageJ.DataContext = joueur1.PointDeBlindage; //TODO pas testé
            txBlnbBlindageA.DataContext = joueur2.PointDeBlindage;

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

