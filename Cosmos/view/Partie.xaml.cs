using Cosmos.accesBD;
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
        public CarteZoom Zoom { get; set; }

        int phase;
        TableDeJeu laTableDeJeu;


        public Partie(MainWindow main)
        {
            InitializeComponent();

            Utilisateur utilisateur1 = MySqlUtilisateurService.RetrieveByNom("Damax");
            Utilisateur utilisateur2 = MySqlUtilisateurService.RetrieveByNom("Guillaume");
            utilisateur1.Reinitialiser();
            utilisateur2.Reinitialiser();

            Joueur joueur1 = utilisateur1;
            Joueur joueur2 = utilisateur2;

            laTableDeJeu = new TableDeJeu(utilisateur1.DeckAJouer.CartesDuDeck, utilisateur2.DeckAJouer.CartesDuDeck);

            this.DataContext = this; // TODO changé pour bon binding maybe ?
            

            Main = main;

            InsererCarteMain("Nova", 1);
            InsererCarteMain("Nova", 2);
            InsererCarteMain("Nova", 3);
            InsererCarteMain("Nova", 4);
            InsererCarteMain("Nova", 5);
            InsererCarteMain("Nova", 6);
            InsererCarteMain("Nova", 7);
            InsererCarteMain("Nova", 8);


            // Initialiser les points de blindage
            txBlnbBlindageJ.Text = joueur1.PointDeBlindage.ToString();
            txBlnbBlindageA.Text = joueur2.PointDeBlindage.ToString();

            //Initialiser les points de ressources
            txBlnbCharroniteJ.DataContext = joueur1.RessourceActive.Charronite.ToString();
            txBlnbBarilJ.DataContext = joueur1.RessourceActive.BarilNucleaire.ToString();
            txBlnbAlainDollarJ.DataContext = joueur1.RessourceActive.AlainDollars.ToString();
            txBlnbCharroniteA.DataContext = joueur2.RessourceActive.Charronite.ToString();
            txBlnbBarilA.DataContext = joueur2.RessourceActive.BarilNucleaire.ToString();
            txBlnbAlainDollarA.DataContext = joueur2.RessourceActive.AlainDollars.ToString();


            // Prendre les avatars des deux joueurs et les mettres dans le XAML 
            //

            // Initialiser la phase à "phase de ressource"            
            phase = laTableDeJeu.Phase;

            // Brasser les deck
            //laTableDeJeu.BrasserDeck(laTableDeJeu.DeckJ1);
            //laTableDeJeu.BrasserDeck(laTableDeJeu.DeckJ2);
            utilisateur1.DeckAJouer.BrasserDeck();
            utilisateur2.DeckAJouer.BrasserDeck();

            // Donner une main à chaque joueurs 
            int compteurNbCarte = 0;
            while( compteurNbCarte != 6 )
            {
                laTableDeJeu.LstMainJ1.Add(utilisateur1.DeckAJouer.CartesDuDeck[0]);
                InsererCarteMain(laTableDeJeu.LstMainJ1[compteurNbCarte].Nom , compteurNbCarte+1 ); 
                utilisateur1.DeckAJouer.CartesDuDeck.RemoveAt(0) ;
                compteurNbCarte++;                
            }

            compteurNbCarte = 0;
            while (compteurNbCarte != 6)
            {
                laTableDeJeu.LstMainJ2.Add(utilisateur2.DeckAJouer.CartesDuDeck[0]);
                utilisateur2.DeckAJouer.CartesDuDeck.RemoveAt(0);
                compteurNbCarte++;
            }

            // Compteur pour afficher le nombre de cartes dans le deck des joueurs
            // txBLnbCarteJ1.DataContext = utilisateur1.DeckAJouer.CartesDuDeck.Count()
            // txBLnbCarteJ2.DataContext = utilisateur2.DeckAJouer.CartesDuDeck.Count()
            // TODO testé ^


            // Initialiser les emplacements d'unités 
            // TODO or not
            // Prob not

            // Initialiser les emplacements de bâtiments
            // TODO or not

            // Binding 
            txBlnbBlindageJ.DataContext = joueur1.PointDeBlindage;
            txBlnbBlindageA.DataContext = joueur2.PointDeBlindage;

        }
        /// <summary>
        /// Ce bouton change la phase pour l'interface et pour la table de jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTerminerPhase_Click(object sender, RoutedEventArgs e)
        {
            changerPhase();
            laTableDeJeu.AvancerPhase();
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

        private void CarteMain_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            Thickness margin = img.Margin;

            img.Margin = new Thickness(margin.Left, 0, 0, 0);
        }

        private void CarteMain_MouseLeave(object sender, MouseEventArgs e)
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

            // Lier la carte avec les events Mouse Enter et Leave
            carte.MouseEnter += CarteMain_MouseEnter;
            carte.MouseLeave += CarteMain_MouseLeave;

            // Lier la carte avec l'event Carte Zoom
            carte.PreviewMouseLeftButtonUp += Carte_Zoom;

            grdCartesJoueur.Children.Add(carte);
        }

        private void Carte_Zoom(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            AfficherCarteZoom(img);          
            
        }


        public void AfficherCarteZoom(Image img)
        {
            rectZoom.Visibility = Visibility.Visible;
            Zoom = new CarteZoom(img, this);
            grd1.Children.Add(Zoom);
        }

        public void FermerCarteZoom(Image img)
        {
            grd1.Children.Remove(Zoom);
            rectZoom.Visibility = Visibility.Hidden;
        }

        private void JouerCarte( bool estJoueur1 , Carte laCarte )
        {
                laTableDeJeu.JouerCarte(laCarte , estJoueur1 );            
        }

    }
}

