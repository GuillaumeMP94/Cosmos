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

        public Partie(MainWindow main)
        {
            InitializeComponent();

            Joueur joueur1 = new Joueur();
            Joueur joueur2 = new Joueur();

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

            // Récuperer les deck TODO            

            // Brasser les deck            
            //laTableDeJeu.BrasserDeck(laTableDeJeu.DeckJ1);
            //laTableDeJeu.BrasserDeck(laTableDeJeu.DeckJ2);

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
                    txBlphaseRessource.Background = Brushes.Transparent;
                    txBlphasePrincipale.Background = Brushes.DarkGoldenrod;
                    txBlphaseRessource.Foreground = Brushes.DarkGoldenrod;
                    txBlphasePrincipale.Foreground = Brushes.Black;
                    break;
                case 2:
                    phase++;
                    txBlphasePrincipale.Background = Brushes.Transparent;
                    txBlphaseAttaque.Background = Brushes.DarkGoldenrod;
                    txBlphasePrincipale.Foreground = Brushes.DarkGoldenrod;
                    txBlphaseAttaque.Foreground = Brushes.Black;
                    break;
                case 3:
                    phase++;
                    txBlphaseAttaque.Background = Brushes.Transparent;
                    txBlphaseFin.Background = Brushes.DarkGoldenrod;
                    txBlphaseAttaque.Foreground = Brushes.DarkGoldenrod;
                    txBlphaseFin.Foreground = Brushes.Black;
                    break;
                case 4:
                    phase = 1; // La phase de fin est terminer, nous retournons à la première phase
                    txBlphaseFin.Background = Brushes.Transparent;
                    txBlphaseRessource.Background = Brushes.DarkGoldenrod;
                    txBlphaseFin.Foreground = Brushes.DarkGoldenrod;
                    txBlphaseRessource.Foreground = Brushes.Black;
                    break;
            }
        }

        private void btnAbandonner_Click(object sender, RoutedEventArgs e)
        {
            Main.QuitterMain();
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.QuitterMain();

            
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            Thickness margin = img.Margin;

            img.Margin = new Thickness(margin.Left, 0, 0, 0);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            Thickness margin = img.Margin;
            
            img.Margin = new Thickness(margin.Left, 40, 0, 0);
        }

        private void InsererCarteMain(String nom, int position)
        {
            Image carte = new Image();
            carte.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + nom + ".jpg"));
            carte.Height = 300;
            carte.Width = 700;
            carte.VerticalAlignment = VerticalAlignment.Top;
            carte.HorizontalAlignment = HorizontalAlignment.Left;
            carte.Name = "carte" + position;
            carte.Margin = new Thickness(position * 50 - 50, 40, 0, 0);
            carte.SetValue(Panel.ZIndexProperty, position);
            carte.Cursor = Cursors.Hand;
            carte.MouseEnter += Image_MouseEnter;
            carte.MouseLeave += Image_MouseLeave;
            grdCartesJoueur.Children.Add(carte);            
        }
    }
}

