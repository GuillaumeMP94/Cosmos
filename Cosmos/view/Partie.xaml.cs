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
using static Cosmos.metier.TrousseGlobale;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour Partie.xaml
    /// </summary>
    public partial class Partie : UserControl
    {
        const int RESSOURCEDEPART = 3;
        TableDeJeu laTableDeJeu;
        public UserControl ContenuEcran { get; set; }
        public MainWindow Main { get; set; }
        public Image imgZoom { get; set; }
        public List<Image> ImgMainJoueur { get; set; }
        public List<Border> ListBorderImgMainJoueur { get; set; }
        public int IndexCarteZoomer { get; set; }
        public DispatcherTimer Temps { get; set; }
        public int Phase
        {
            get { return laTableDeJeu.Phase; }
            set
            {
                laTableDeJeu.Phase = value;
            }
        }
        //int phase;


        public Partie(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            //Timer pour refresh
            Temps = new DispatcherTimer();
            Temps.Interval = TimeSpan.FromMilliseconds(1000);
            Temps.Tick += timer_Tick;
            //TODO: Utilisateur Connecter
            Utilisateur utilisateur1 = MySqlUtilisateurService.RetrieveByNom("Semesis");
            //Utilisateur utilisateur1 = Main.UtilisateurConnecte;
            // TODO: AI
            //Utilisateur utilisateur2 = MySqlUtilisateurService.RetrieveByNom("Guillaume");
            // TODO: Reinitialiser les utilisateurs à la fin de la partie.
            utilisateur1.Reinitialiser();
            AI Robot = new AI("Robot Turenne", 1, new Ressource(2, 2, 2), MySqlDeckService.RetrieveById(1));
            //utilisateur2.Reinitialiser();
            laTableDeJeu = new TableDeJeu(utilisateur1, Robot);
            // Permet de lier l'AI avec la table de jeu
            laTableDeJeu.Subscribe(Robot);
            // CODE TEMPORAIRE POUR TESTER
            /*AI ordinateur;
            var ressourceAI = new metier.Ressource(2, 2, 2);
            ordinateur = new AI( "Temporaire", 1, ressourceAI, utilisateur2.DeckAJouer, laTableDeJeu);*/
            // FIN CODE TEMPORAIRE

            // Demander à l'utilisateur de distribuer ses ressources.
            //TODO: Enlever les slash pour afficher EcranRessource
            EcranRessource(laTableDeJeu.Joueur1, RESSOURCEDEPART, RESSOURCEDEPART, this); // Joueur, nbPoints à distribué, levelMaximum de ressource = 3 + nbTour

            // Permet le binding. Le datacontext est la table de jeu puisque c'est elle qui contient les data qui seront modifiées.
            this.DataContext = laTableDeJeu;
            // Les propriétés bindées en ce moment :
            // Ressources des joueurs 
            // Points de blindages

            Main = main;

            // Prendre les avatars des deux joueurs et les mettres dans le XAML 
            //

            // Initialiser la phase à "phase de ressource"            


            // Afficher la main
            ListBorderImgMainJoueur = new List<Border>();
            ImgMainJoueur = new List<Image>();


            // Compteur pour afficher le nombre de cartes dans le deck des joueurs
            // txBLnbCarteJ1.DataContext = utilisateur1.DeckAJouer.CartesDuDeck.Count()
            // txBLnbCarteJ2.DataContext = utilisateur2.DeckAJouer.CartesDuDeck.Count()
            // TODO testé ^
            TrousseGlobale.PhaseChange += changerPhase;

        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Phase == 4 || Phase == 1)
            {
                laTableDeJeu.AvancerPhase();
            }
        }
        /// <summary>
        /// Ce bouton change la phase pour l'interface et pour la table de jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTerminerPhase_Click(object sender, RoutedEventArgs e)
        {
            if (Phase == 2 || Phase == 3)
            {
                laTableDeJeu.AvancerPhase();
            }
        }

        private void changerPhase(object sender, PhaseChangeEventArgs e)
        {
            //laTableDeJeu.AvancerPhase();

            switch (Phase)
            {
                case 2:
                    PhaseRessource();
                    break;
                case 3:
                    PhasePrincipale();
                    break;
                case 4:
                    PhaseAttaque();
                    break;
                case 1:
                    PhaseFin();
                    break;
            }
            RefreshAll();
        }

        /// <summary>
        /// Actions qui se produit lors de la phase de ressource
        /// </summary>
        private void PhaseRessource()
        {
            // on ajoute les ressources au joueur actif
            laTableDeJeu.AttribuerRessourceLevel();
            laTableDeJeu.PigerCarte();
            txBlphaseRessource.Background = Brushes.Transparent;
            txBlphasePrincipale.Background = Brushes.DarkGoldenrod;
            txBlphaseRessource.Foreground = Brushes.DarkGoldenrod;
            txBlphasePrincipale.Foreground = Brushes.Black;
            imgFinTour.Visibility = Visibility.Hidden;
            RefreshAll();
            Temps.Stop();
            btnTerminerPhase.IsEnabled = true;
            btnTerminerPhase.Visibility = Visibility.Visible;
        }
        private void PhasePrincipale()
        {
            txBlphasePrincipale.Background = Brushes.Transparent;
            txBlphaseAttaque.Background = Brushes.DarkGoldenrod;
            txBlphasePrincipale.Foreground = Brushes.DarkGoldenrod;
            txBlphaseAttaque.Foreground = Brushes.Black;
        }

        private void PhaseAttaque()
        {
            btnTerminerPhase.IsEnabled = false;
            btnTerminerPhase.Visibility = Visibility.Hidden;
            btnTerminerPhase.Refresh();
            txBlphaseAttaque.Background = Brushes.Transparent;
            txBlphaseFin.Background = Brushes.DarkGoldenrod;
            txBlphaseAttaque.Foreground = Brushes.DarkGoldenrod;
            txBlphaseFin.Foreground = Brushes.Black;
            if (!laTableDeJeu.JoueurActifEst1)
                imgFinTour.Visibility = Visibility.Visible;
            RefreshAll();
            laTableDeJeu.ExecuterAttaque(true, true, true);
            Temps.Start();
        }
        private void PhaseFin()
        {
            laTableDeJeu.PreparerTroupes();
            laTableDeJeu.DetruireUnite();
            laTableDeJeu.DetruireBatiment();
            txBlphaseFin.Background = Brushes.Transparent;
            txBlphaseRessource.Background = Brushes.DarkGoldenrod;
            txBlphaseFin.Foreground = Brushes.DarkGoldenrod;
            txBlphaseRessource.Foreground = Brushes.Black;
            RefreshAll();
        }



        private void RefreshAll()
        {
            txBlphaseRessource.Refresh();
            txBlphasePrincipale.Refresh();
            txBlphaseAttaque.Refresh();
            txBlphaseFin.Refresh();
            imgFinTour.Refresh();
            // Afficher les cartes sur le champ de bataille, les unités et les batiement
            AfficherChampUnites();
            AfficherChampBatiments();
            AfficherMain();
            //TODO: test
            //AfficherCoupPoosible();
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
            Border border = (Border)img.Parent;

            Thickness margin = border.Margin;

            border.Margin = new Thickness(margin.Left, 0, 0, 0);
        }

        private void CarteMain_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            Border border = (Border)img.Parent;

            Thickness margin = border.Margin;

            border.Margin = new Thickness(margin.Left, 40, 0, 0);
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

        private void PeuplerListBorderMainJoueur()
        {
            foreach (Image element in ImgMainJoueur)
            {
                CreerBorderDansList(element, ImgMainJoueur.IndexOf(element));
            }
        }

        private void CreerBorderDansList(Image img, int position)
        {
            Border border = new Border();
            border.Height = 300;
            border.Width = 200;
            border.VerticalAlignment = VerticalAlignment.Top;
            border.HorizontalAlignment = HorizontalAlignment.Left;
            border.Margin = new Thickness((position + 1) * 50 - 50, 40, 0, 0);
            border.SetValue(Panel.ZIndexProperty, position);

            BrushConverter converter = new BrushConverter();
            border.BorderBrush = converter.ConvertFromString("#e1ff00") as Brush;

            // Si la carte peut être jouer, elle sera entouré d'une bordure de couleur
            if (laTableDeJeu.JoueurActifEst1  && Phase == 2 && laTableDeJeu.validerCoup(position))
            {
                border.BorderThickness = new Thickness(5);
                
            }
            // Sinon pas de bordure
            else
            {
                border.BorderThickness = new Thickness(0);
            }            
            // Insérer l'image de la carte dans le Border
            border.Child = img;

            // Insérer la Border dans la ListBorderImgMainJoueur
            ListBorderImgMainJoueur.Add(border);
        }

        private void AfficherMain()
        {
            ImgMainJoueur.Clear();
            foreach (Border element in ListBorderImgMainJoueur)
            {
                grdCartesJoueur.Children.Remove(element);
            }
            ListBorderImgMainJoueur.Clear();
            PeuplerImgMainJoueur(laTableDeJeu.LstMainJ1);
            PeuplerListBorderMainJoueur();
            // Insérer les Borders de la liste dans le XAML
            foreach(Border element in ListBorderImgMainJoueur)
            {
                grdCartesJoueur.Children.Add(element);
            }
        }

        private void AfficherChampBatiments()
        {
            // Insérer les img des cartes Batiments en jeu du joueur 2 s'il y en a
            if(laTableDeJeu.ChampConstructionsJ2.Champ1 != null)
            {
                imgBatiment1J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ1.Nom + ".jpg"));
            }
            else
            {
                imgBatiment1J2.Source = null;
            }
            if (laTableDeJeu.ChampConstructionsJ2.Champ2 != null)
            {
                imgBatiment2J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ2.Nom + ".jpg"));
            }
            else
            {
                imgBatiment2J2.Source = null;
            }
            if (laTableDeJeu.ChampConstructionsJ2.Champ3 != null)
            {
                imgBatiment3J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgBatiment3J2.Source = null;
            }
            if (laTableDeJeu.ChampConstructionsJ2.Champ4 != null)
            {
                imgBatiment4J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ4.Nom + ".jpg"));
            }
            else
            {
                imgBatiment4J2.Source = null;
            }
            // Insérer les img des cartes Batiments en jeu du joueur 1 s'il y en a
            if (laTableDeJeu.ChampConstructionsJ1.Champ1 != null)
            {
                imgBatiment1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ1.Nom + ".jpg"));
            }
            else
            {
                imgBatiment1J1.Source = null;
            }
            if (laTableDeJeu.ChampConstructionsJ1.Champ2 != null)
            {
                imgBatiment2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ2.Nom + ".jpg"));
            }
            else
            {
                imgBatiment2J1.Source = null;
            }
            if (laTableDeJeu.ChampConstructionsJ1.Champ3 != null)
            {
                imgBatiment3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ3.Nom + ".jpg"));
            }
            else
            {
                imgBatiment3J1.Source = null;
            }
            if (laTableDeJeu.ChampConstructionsJ1.Champ4 != null)
            {
                imgBatiment4J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ4.Nom + ".jpg"));
            }
            else
            {
                imgBatiment4J1.Source = null;
            }
        }

        private void AfficherChampUnites()
        {
            // Insérer les img des cartes Unités en jeu du joueur 2 s'il y en a
            if(laTableDeJeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                imgUnite1J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ1.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp1)
                    imgUnite1J2.Opacity = 0.5;
                else
                    imgUnite1J2.Opacity = 1;
            }
            else
            {
                imgUnite1J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ2 != null)
            {
                imgUnite2J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ2.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp2)
                    imgUnite2J2.Opacity = 0.5;
                else
                    imgUnite2J2.Opacity = 1;
            }
            else
            {
                imgUnite2J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                imgUnite3J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ3.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp3)
                    imgUnite3J2.Opacity = 0.5;
                else
                    imgUnite3J2.Opacity = 1;
            }
            else
            {
                imgUnite3J2.Source = null;
            }
            // Insérer les img des cartes Unités en jeu du joueur 1 s'il y en a
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ1 != null)
            {
                imgUnite1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ1.Nom + ".jpg"));
                //imgUnite1J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
                if (laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp1)
                    imgUnite1J1.Opacity = 0.5;
                else
                    imgUnite1J1.Opacity = 1;
            }
            else
            {
                imgUnite1J1.Source = null;
                imgUnite1J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;                
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ2 != null)
            {
                imgUnite2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ2.Nom + ".jpg"));
                //imgUnite2J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
                if (laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp2)
                    imgUnite2J1.Opacity = 0.5;
                else
                    imgUnite2J1.Opacity = 1;
            }
            else
            {
                imgUnite2J1.Source = null;
                imgUnite2J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                imgUnite3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ3.Nom + ".jpg"));
                //imgUnite3J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
                if (laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp3)
                    imgUnite3J1.Opacity = 0.5;
                else
                    imgUnite3J1.Opacity = 1;
            }
            else
            {
                imgUnite3J1.Source = null;                
                imgUnite3J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            imgUnite1J1.PreviewMouseLeftButtonUp -= ChoisirEmplacementUnite;
            imgUnite2J1.PreviewMouseLeftButtonUp -= ChoisirEmplacementUnite;
            imgUnite3J1.PreviewMouseLeftButtonUp -= ChoisirEmplacementUnite;
        }

        private Image CreerImageCarte(String nom, int position)
        {
            Image carte = new Image();
            carte.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + nom + ".jpg"));
            //carte.Height = 300;
            //carte.Width = 700;
            //carte.VerticalAlignment = VerticalAlignment.Top;
            //carte.HorizontalAlignment = HorizontalAlignment.Left;
            //carte.Margin = new Thickness(position * 50 - 50, 40, 0, 0);
            //carte.SetValue(Panel.ZIndexProperty, position);
            carte.Cursor = Cursors.Hand;
            carte.Stretch = Stretch.Fill;

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

            if(carteMain)
            {
                IndexCarteZoomer = ImgMainJoueur.IndexOf(img);
            }
            else
            {
                IndexCarteZoomer = 99; // On met l'index à 99 pour détecter qu'il a clicker sur une carte en jeu.
            }
            imgZoomCarte.Source = img.Source;
            imgZoomCarte.Visibility = Visibility.Visible;

        }

        public void FermerEcranRessource()
        {
            grd1.Children.Remove(ContenuEcran);
            recRessource.Visibility = Visibility.Hidden;
            laTableDeJeu.AvancerPhase();
            AfficherMain();
        }
        public void EcranRessource(Joueur joueur, int points, int maxRessourceLevel, Partie partie)
        {
            ContenuEcran = new ChoixRessources(joueur, points, maxRessourceLevel, partie);
            recRessource.Visibility = Visibility.Visible;

            grd1.Children.Add(ContenuEcran);
        }

        private void AfficherCoupPoosible()
        {
            grdCartesEnjeu.SetValue(Panel.ZIndexProperty, 99);
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ1 is null)
            {
                imgUnite1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/jouer.jpg"));
                imgUnite1J1.Cursor = Cursors.Hand;
                imgUnite1J1.PreviewMouseLeftButtonUp += ChoisirEmplacementUnite;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ2 is null)
            {
                imgUnite2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/jouer.jpg"));
                imgUnite2J1.Cursor = Cursors.Hand;
                imgUnite2J1.PreviewMouseLeftButtonUp += ChoisirEmplacementUnite;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ3 is null)
            {
                imgUnite3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/jouer.jpg"));
                imgUnite3J1.Cursor = Cursors.Hand;
                imgUnite3J1.PreviewMouseLeftButtonUp += ChoisirEmplacementUnite;
            }
        }

        private void ChoisirEmplacementUnite(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            // TODO: Enlever le Inserer Carte Creature
            //InsererCarteCreature(laTableDeJeu.LstMainJ1[IndexCarteZoomer].Nom, 4);
            laTableDeJeu.JouerCarte(IndexCarteZoomer, Convert.ToInt32(img.Name.Substring(8, 1)));

            grdCartesEnjeu.SetValue(Panel.ZIndexProperty, 0);
            rectZoom.Visibility = Visibility.Hidden;
            img.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;

            // Enlever les évènement

            RefreshAll();
        }

        private void imgZoomCarte_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // On peut jouer une carte seulement dans la phase 2
            if (Phase == 2 && laTableDeJeu.JoueurActifEst1 && IndexCarteZoomer != 99) // Si l'index est à 99 c'est qu'il a cliquer une carte en jeu.
            {

                // Fonctionne partiellement. Pour la première carte, c'est toujours bon.
                // Si on joue plusieurs carte, ça ne fonctionne pas.
                // Il faudrait ré-organiser la main après le 0,5
                if (laTableDeJeu.validerCoup(IndexCarteZoomer))
                {
                    if (laTableDeJeu.CarteAJouerEstUnite(IndexCarteZoomer))
                    {
                        // Choisir l'emplacement.
                        AfficherCoupPoosible();                    
                    }
                    else
                    {
                        laTableDeJeu.JouerCarte(IndexCarteZoomer, 0); // La position n'est pas nécessaire.
                        rectZoom.Visibility = Visibility.Hidden;
                        RefreshAll();
                    }
                }
                else
                {
                    rectZoom.Visibility = Visibility.Hidden;
                }
                imgZoomCarte.Visibility = Visibility.Hidden;

            }
        }

        private void rectZoom_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectZoom.Visibility = Visibility.Hidden;
            imgZoomCarte.Visibility = Visibility.Hidden;
            grdCartesEnjeu.SetValue(Panel.ZIndexProperty, 0);
            RefreshAll();
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

