using Cosmos.accesBD;
using Cosmos.metier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour Partie.xaml
    /// </summary>
    public partial class Partie : UserControl
    {
        const int RESSOURCEDEPART = 3;
        public UserControl ContenuEcran { get; set; }
        public MainWindow Main { get; set; }
        public Image imgZoom { get; set; }
        public List<Image> ImgMainJoueur { get; set; }
        public int IndexCarteZoomer { get; set; }

        int phase;
        TableDeJeu laTableDeJeu;

        public event PropertyChangedEventHandler PropertyChanged;

        public Partie(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            //TODO: Utilisateur Connecter
            Utilisateur utilisateur1 = MySqlUtilisateurService.RetrieveByNom("Semesis");
            //Utilisateur utilisateur1 = Main.UtilisateurConnecte;
            // TODO: AI
            Utilisateur utilisateur2 = MySqlUtilisateurService.RetrieveByNom("Guillaume");
            // TODO: Reinitialiser les utilisateurs à la fin de la partie.
            utilisateur1.Reinitialiser();
            utilisateur2.Reinitialiser();

            laTableDeJeu = new TableDeJeu(utilisateur1, utilisateur2);

            // CODE TEMPORAIRE POUR TESTER
            /*AI ordinateur;
            var ressourceAI = new metier.Ressource(2, 2, 2);
            ordinateur = new AI( "Temporaire", 1, ressourceAI, utilisateur2.DeckAJouer, laTableDeJeu);*/
            // FIN CODE TEMPORAIRE

            // Demander à l'utilisateur de distribuer ses ressources.
            //TODO: Enlever les slash pour afficher EcranRessource
            EcranRessource( laTableDeJeu.Joueur1 , RESSOURCEDEPART, RESSOURCEDEPART, this); // Joueur, nbPoints à distribué, levelMaximum de ressource = 3 + nbTour

            // Permet le binding. Le datacontext est la table de jeu puisque c'est elle qui contient les data qui seront modifiées.
            this.DataContext = laTableDeJeu;
            // Les propriétés bindées en ce moment :
                // Ressources des joueurs 
                // Points de blindages

            Main = main;

            // Prendre les avatars des deux joueurs et les mettres dans le XAML 
            //

            // Initialiser la phase à "phase de ressource"            
            phase = laTableDeJeu.Phase;

            // Afficher la main
            ImgMainJoueur = new List<Image>();
            AfficherMain();            

            // Afficher les cartes sur le champ de bataille, les unités et les batiement
            AfficherChampUnites();
            AfficherChampBatiments();



            // Compteur pour afficher le nombre de cartes dans le deck des joueurs
            // txBLnbCarteJ1.DataContext = utilisateur1.DeckAJouer.CartesDuDeck.Count()
            // txBLnbCarteJ2.DataContext = utilisateur2.DeckAJouer.CartesDuDeck.Count()
            // TODO testé ^

        }
        /// <summary>
        /// Ce bouton change la phase pour l'interface et pour la table de jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTerminerPhase_Click(object sender, RoutedEventArgs e)
        {
            changerPhase();
        }

        private void changerPhase()
        {
            laTableDeJeu.AvancerPhase();
            RefreshAll();
            switch (phase)
            {
                case 1:
                    phase++;
                    // on ajoute les ressources au joueur actif
                    laTableDeJeu.AttribuerRessourceLevel();

                    txBlphaseRessource.Background = Brushes.Transparent;
                    txBlphasePrincipale.Background = Brushes.DarkGoldenrod;
                    txBlphaseRessource.Foreground = Brushes.DarkGoldenrod;
                    txBlphasePrincipale.Foreground = Brushes.Black;
                    System.Threading.Thread.Sleep(500);
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
                    laTableDeJeu.ExecuterAttaque(true,true,true);
                    txBlphaseAttaque.Background = Brushes.Transparent;
                    txBlphaseFin.Background = Brushes.DarkGoldenrod;
                    txBlphaseAttaque.Foreground = Brushes.DarkGoldenrod;
                    txBlphaseFin.Foreground = Brushes.Black;
                    RefreshAll();
                    System.Threading.Thread.Sleep(500);
                    changerPhase();
                    break;
                case 4:
                    phase = 1; // La phase de fin est terminer, nous retournons à la première phase
                    txBlphaseFin.Background = Brushes.Transparent;
                    txBlphaseRessource.Background = Brushes.DarkGoldenrod;
                    txBlphaseFin.Foreground = Brushes.DarkGoldenrod;
                    txBlphaseRessource.Foreground = Brushes.Black;
                    RefreshAll();
                    System.Threading.Thread.Sleep(500);
                    changerPhase();
                    break;
            }
            RefreshAll();
        }

        private void RefreshAll()
        {
            txBlphaseRessource.Refresh();
            txBlphasePrincipale.Refresh();
            txBlphaseAttaque.Refresh();
            txBlphaseFin.Refresh();
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

        private void PeuplerImgMainJoueur(List<Carte> lstMain)
        {
            int i = 1;
            foreach(Carte element in lstMain)
            {
                ImgMainJoueur.Add(CreerImageCarte(element.Nom, i));
                i++;
            }
        }

        private void AfficherMain()
        {
            foreach (Image element in ImgMainJoueur)
            {
                grdCartesJoueur.Children.Remove(element);
            }
            ImgMainJoueur.Clear();
            PeuplerImgMainJoueur(laTableDeJeu.LstMainJ1);
            foreach (Image element in ImgMainJoueur)
            {
                grdCartesJoueur.Children.Add(element);
            }
        }

        private void AfficherChampBatiments()
        {
            // Insérer les img des cartes Batiments en jeu du joueur 2 s'il y en a
            if(laTableDeJeu.ChampBatailleBatimentsJ2.Champ1 != null)
            {
                imgBatiment1J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleBatimentsJ2.Champ1.Nom + ".jpg"));
            }
            else
            {
                imgBatiment1J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleBatimentsJ2.Champ2 != null)
            {
                imgBatiment2J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleBatimentsJ2.Champ2.Nom + ".jpg"));
            }
            else
            {
                imgBatiment2J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleBatimentsJ2.Champ3 != null)
            {
                imgBatiment3J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleBatimentsJ2.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgBatiment3J2.Source = null;
            }
            // Insérer les img des cartes Batiments en jeu du joueur 1 s'il y en a
            if (laTableDeJeu.ChampBatailleBatimentsJ1.Champ1 != null)
            {
                imgBatiment1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleBatimentsJ1.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgBatiment1J1.Source = null;
            }
            if (laTableDeJeu.ChampBatailleBatimentsJ1.Champ2 != null)
            {
                imgBatiment2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleBatimentsJ1.Champ2.Nom + ".jpg"));
            }
            else
            {
                imgBatiment2J1.Source = null;
            }
            if (laTableDeJeu.ChampBatailleBatimentsJ1.Champ3 != null)
            {
                imgBatiment3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleBatimentsJ1.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgBatiment3J1.Source = null;
            }
        }

        private void AfficherChampUnites()
        {
            // Insérer les img des cartes Unités en jeu du joueur 2 s'il y en a
            if(laTableDeJeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                imgUnite1J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ1.Nom + ".jpg"));
            }
            else
            {
                imgUnite1J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ2 != null)
            {
                imgUnite2J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ2.Nom + ".jpg"));
            }
            else
            {
                imgUnite2J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                imgUnite3J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgUnite3J2.Source = null;
            }
            // Insérer les img des cartes Unités en jeu du joueur 1 s'il y en a
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ1 != null)
            {
                imgUnite1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgUnite1J1.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ2 != null)
            {
                imgUnite2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ2.Nom + ".jpg"));
            }
            else
            {
                imgUnite2J1.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                imgUnite3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgUnite3J1.Source = null;
            }
        }

        private Image CreerImageCarte(String nom, int position)
        {
            Image carte = new Image();
            carte.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + nom + ".jpg"));
            carte.Height = 300;
            carte.Width = 700;
            carte.VerticalAlignment = VerticalAlignment.Top;
            carte.HorizontalAlignment = HorizontalAlignment.Left;
            carte.Margin = new Thickness(position * 50 - 50, 40, 0, 0);
            carte.SetValue(Panel.ZIndexProperty, position);
            carte.Cursor = Cursors.Hand;

            // Lier la carte avec les events Mouse Enter et Leave
            carte.MouseEnter += CarteMain_MouseEnter;
            carte.MouseLeave += CarteMain_MouseLeave;

            // Lier la carte avec l'event Carte Zoom
            carte.PreviewMouseLeftButtonUp += Carte_Zoom;           
            

            return carte;
        }

        private void Carte_Zoom(object sender, MouseEventArgs e)
        {
            imgZoom = (Image)sender;
            AfficherCarteZoom(imgZoom, true);          
            
        }
        
        private void Carte_CarteEnJeu_Zoom(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            AfficherCarteZoom(img, false);
        }


        public void AfficherCarteZoom(Image img, bool carteMain)
        {
            rectZoom.Visibility = Visibility.Visible;
            IndexCarteZoomer = ImgMainJoueur.IndexOf(img);
            imgZoomCarte.Source = img.Source;
            imgZoomCarte.Visibility = Visibility.Visible;

        }

        public void FermerEcranRessource()
        {
            grd1.Children.Remove(ContenuEcran);
            recRessource.Visibility = Visibility.Hidden;

            changerPhase();
        }
        public void EcranRessource(Joueur joueur, int points, int maxRessourceLevel, Partie partie)
        {
            ContenuEcran = new ChoixRessources(joueur, points, maxRessourceLevel, partie);
            recRessource.Visibility = Visibility.Visible;


            grd1.Children.Add(ContenuEcran);
        }

        private void imgZoomCarte_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // On peut jouer une carte seulement dans la phase 2
            if (phase == 2 && laTableDeJeu.JoueurActifEst1)
            {

                // Fonctionne partiellement. Pour la première carte, c'est toujours bon.
                // Si on joue plusieurs carte, ça ne fonctionne pas.
                // Il faudrait ré-organiser la main après le 0,5
                if (laTableDeJeu.validerCoup(IndexCarteZoomer))
                {
                    // Choisir l'emplacement.
                    // TODO: Enlever le Inserer Carte Creature
                    InsererCarteCreature(laTableDeJeu.LstMainJ1[IndexCarteZoomer].Nom, 4);
                    laTableDeJeu.JouerCarte(IndexCarteZoomer);
                }
                rectZoom.Visibility = Visibility.Hidden;
                imgZoomCarte.Visibility = Visibility.Hidden;
                AfficherMain();

            }
        }

        private void rectZoom_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectZoom.Visibility = Visibility.Hidden;
            imgZoomCarte.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// Fonction pour refresh l'affichage des labels. Source: https://social.msdn.microsoft.com/Forums/vstudio/en-US/878ea631-c76e-49e8-9e25-81e76a3c4fe3/refresh-the-gui-in-a-wpf-application?forum=wpf
    /// </summary>
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}

