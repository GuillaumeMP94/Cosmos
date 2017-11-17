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
        public AI Robot { get; set; }
        public Image imgZoom { get; set; }
        public List<Image> ImgMainJoueur { get; set; }
        public List<Border> ListBorderImgMainJoueur { get; set; }
        public int IndexCarteZoomer { get; set; }
        public DispatcherTimer Temps { get; set; }
        public DispatcherTimer TempsAI { get; set; }
        public bool Unite1J1Attack { get; set; }
        public bool Unite2J1Attack { get; set; }
        public bool Unite3J1Attack { get; set; }

        public int Phase
        {
            get { return laTableDeJeu.Phase; }
            set
            {
                laTableDeJeu.Phase = value;
            }
        }

        public Partie(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            //Timer pour changer automatiquement la phase de fin et la phase de ressource.
            Temps = new DispatcherTimer();
            Temps.Interval = TimeSpan.FromMilliseconds(1000);
            Temps.Tick += timer_Tick;
            //Timer pour avertir le AI;
            TempsAI = new DispatcherTimer();
            TempsAI.Interval = TimeSpan.FromMilliseconds(1000);
            TempsAI.Tick += NotifyAi;
            TempsAI.Start();
            //TODO: Utilisateur Connecter
            Utilisateur utilisateur1 = MySqlUtilisateurService.RetrieveByNom("Semesis");
            //Utilisateur utilisateur1 = Main.UtilisateurConnecte;
            // TODO: AI
            //Utilisateur utilisateur2 = MySqlUtilisateurService.RetrieveByNom("Guillaume");
            // TODO: Reinitialiser les utilisateurs à la fin de la partie.
            utilisateur1.Reinitialiser();

            Robot = new AI("Robot Turenne", 1, new Ressource(2, 2, 2), MySqlDeckService.RetrieveById(1));

            //utilisateur2.Reinitialiser();
            laTableDeJeu = new TableDeJeu(utilisateur1, Robot);
            // Permet de lier l'AI avec la table de jeu
            laTableDeJeu.Subscribe(Robot);

            // Demander à l'utilisateur de distribuer ses ressources.
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

            // Binding pour les points d'attauqe et de vie des unités en jeu
            txblEmplacementUnite1J1Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite2J1Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite3J1Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite1J2Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite2J2Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite3J2Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite1J1Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite2J1Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite3J1Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite1J2Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite2J2Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite3J2Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            // Compteur pour afficher le nombre de cartes dans le deck des joueurs
            txBLnbCarteJ1.DataContext = laTableDeJeu.Joueur1.DeckAJouer;
            txBLnbCarteJ2.DataContext = laTableDeJeu.Joueur2.DeckAJouer;
            // Listener des events PhaseChange et RefreshAll
            TrousseGlobale.PhaseChange += changerPhase;
            TrousseGlobale.RefreshAll += RefreshAllEvent;
        }
        /// <summary>
        /// Fonction qui averti l'ai de jouer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyAi(object sender, EventArgs e)
        {
            laTableDeJeu.NotifyAi();
        }
        /// <summary>
        /// Code lorsque l'evenement RefreshAll est lancé quelque part dans l'application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshAllEvent(object sender, RefreshAllEventArgs e)
        {
            RefreshAll();
        }
        /// <summary>
        /// Fonction qui est executer pour automatisé les phases de fin et de ressource.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Fonction qui execute le code pour la fin d'une phase.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Fonction qui change l'affichage pendant la phase de ressource.
        /// </summary>
        private void AffichagePhaseRessource()
        {
            txBlphaseRessource.Background = Brushes.Transparent;
            txBlphasePrincipale.Background = Brushes.DarkGoldenrod;
            txBlphaseRessource.Foreground = Brushes.DarkGoldenrod;
            txBlphasePrincipale.Foreground = Brushes.Black;
            imgFinTour.Visibility = Visibility.Hidden;
            if (laTableDeJeu.JoueurActifEst1)
            {
                btnTerminerPhase.IsEnabled = true;
                btnTerminerPhase.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Actions qui se produit lors de la phase de ressource
        /// </summary>
        private void PhaseRessource()
        {
            // on ajoute les ressources au joueur actif
            laTableDeJeu.AttribuerRessourceLevel();
            laTableDeJeu.EffetBatiments();
            laTableDeJeu.PigerCarte();
            AffichagePhaseRessource();
            RefreshAll();
            Temps.Stop();
        }
        /// <summary>
        /// Actions qui se produit lors de la phase principale.
        /// </summary>
        private void PhasePrincipale()
        {
            if (txblSlash1J1.Dispatcher.CheckAccess() == true)
            {
                AffichagePhasePrincipale();
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    AffichagePhasePrincipale();
                });
            }
            
        }
        /// <summary>
        /// Fonction qui change l'affichage de la phase principale.
        /// </summary>
        private void AffichagePhasePrincipale()
        {
            txBlphasePrincipale.Background = Brushes.Transparent;
            txBlphaseAttaque.Background = Brushes.DarkGoldenrod;
            txBlphasePrincipale.Foreground = Brushes.DarkGoldenrod;
            txBlphaseAttaque.Foreground = Brushes.Black;
        }
        /// <summary>
        /// Actions qui se produit lors de la phase d'attaque
        /// </summary>
        private void PhaseAttaque()
        {
            if (txblSlash1J1.Dispatcher.CheckAccess() == true)
            {
                AffichagePhaseAttaque();
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    AffichagePhaseAttaque();
                });
            }
            
            RefreshAll();
            if (laTableDeJeu.JoueurActifEst1)
                laTableDeJeu.ExecuterAttaque(Unite1J1Attack, Unite2J1Attack, Unite3J1Attack);
            else
                laTableDeJeu.ExecuterAttaque(Robot.AttaqueChamp1, Robot.AttaqueChamp2, Robot.AttaqueChamp3);
            Temps.Start();
        }
        /// <summary>
        /// Affichange de la phase d'attaque.
        /// </summary>
        private void AffichagePhaseAttaque()
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
        }
        /// <summary>
        /// Actions qui se produit dans la phase de fin
        /// </summary>
        private void PhaseFin()
        {
            if (txblSlash1J1.Dispatcher.CheckAccess() == true)
            {
                AffichagePhaseFin();
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    AffichagePhaseFin();
                });
            }
            laTableDeJeu.PreparerTroupes();
            laTableDeJeu.DetruireUnite();
            laTableDeJeu.DetruireBatiment();
            
            VerifierVictoire();
            
            // On remet les trois Bools pour l'attaque a False
            Unite1J1Attack = false;
            Unite2J1Attack = false;
            Unite3J1Attack = false;

            RefreshAll();
        }
        /// <summary>
        /// Affichage de la phase de fin
        /// </summary>
        private void AffichagePhaseFin()
        {
            txBlphaseFin.Background = Brushes.Transparent;
            txBlphaseRessource.Background = Brushes.DarkGoldenrod;
            txBlphaseFin.Foreground = Brushes.DarkGoldenrod;
            txBlphaseRessource.Foreground = Brushes.Black;
        }
        /// <summary>
        /// Fonction qui vérifie si le jeu est terminé.
        /// </summary>
        private void VerifierVictoire()
        {
            if (laTableDeJeu.Joueur2.PointDeBlindage <= 0)
            {
                // Victoire
                TrousseGlobale.PhaseChange -= changerPhase;
                TrousseGlobale.RefreshAll -= RefreshAllEvent;
                Temps.Stop();
                TempsAI.Stop();
                MessageBox.Show("Vous avez gagné!","Victoire", MessageBoxButton.OK);
                Main.EcranMenuPrincipal();
            }
            if (laTableDeJeu.Joueur1.PointDeBlindage <= 0)
            {
                // Défaite
                TrousseGlobale.RefreshAll -= RefreshAllEvent;
                TrousseGlobale.PhaseChange -= changerPhase;
                Temps.Stop();
                TempsAI.Stop();
                MessageBox.Show("Vous avez perdu!", "Défaite", MessageBoxButton.OK);
                Main.EcranMenuPrincipal();
            }
        }
        /// <summary>
        /// Fonction qui réaffiche les éléments du xaml et les mets à jour.
        /// </summary>
        private void RefreshAll()
        {
            if (txblSlash1J1.Dispatcher.CheckAccess() == true)
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

            }
            else
            {
                this.Dispatcher.Invoke(() =>
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
                });
            }

        }
        /// <summary>
        /// Evenement qui se produit quand on clique sur le bouton abandonner.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbandonner_Click(object sender, RoutedEventArgs e)
        {
            TrousseGlobale.PhaseChange -= changerPhase;
            TrousseGlobale.RefreshAll -= RefreshAllEvent;
            Temps.Stop();
            TempsAI.Stop();
            Main.EcranMenuPrincipal();
        }
        /// <summary>
        /// Fonction pour le mouse-over de la carte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarteMain_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            Border border = (Border)img.Parent;

            Thickness margin = border.Margin;

            border.Margin = new Thickness(margin.Left, 0, 0, 0);
        }
        /// <summary>
        /// Fonction pour le mouse over de la carte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarteMain_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            Border border = (Border)img.Parent;

            Thickness margin = border.Margin;

            border.Margin = new Thickness(margin.Left, 40, 0, 0);
        }
        /// <summary>
        /// Construit les images pour la main du joueur.
        /// </summary>
        /// <param name="lstMain"></param>
        private void PeuplerImgMainJoueur(List<Carte> lstMain)
        {
            int i = 1;
            foreach(Carte element in lstMain)
            {
                ImgMainJoueur.Add(CreerImageCarte(element.Nom, i));
                i++;
            }
        }
        /// <summary>
        /// Construit une liste de border pour souligner les cartes jouables.
        /// </summary>
        private void PeuplerListBorderMainJoueur()
        {
            foreach (Image element in ImgMainJoueur)
            {
                CreerBorderDansList(element, ImgMainJoueur.IndexOf(element));
            }
        }
        /// <summary>
        /// Fonction qui créer les border
        /// </summary>
        /// <param name="img"></param>
        /// <param name="position"></param>
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
        /// <summary>
        /// Fonction pour afficher la main du joueur
        /// </summary>
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
        /// <summary>
        /// Fonction pour afficher les batiments
        /// </summary>
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
        /// <summary>
        /// Fonction pour afficher les unites.
        /// </summary>
        private void AfficherChampUnites()
        {
            // Insérer les img des cartes Unités en jeu du joueur 2 s'il y en a
            if(laTableDeJeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                txblSlash1J2.Visibility = Visibility.Visible;
                txblEmplacementUnite1J2Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite1J2Vie.Visibility = Visibility.Visible;
                imgUnite1J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ1.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp1)
                    imgUnite1J2.Opacity = 0.5;
                else
                    imgUnite1J2.Opacity = 1;
            }
            else
            {
                txblSlash1J2.Visibility = Visibility.Hidden;
                txblEmplacementUnite1J2Attaque.Visibility = Visibility.Hidden;
                txblEmplacementUnite1J2Vie.Visibility = Visibility.Hidden;
                imgUnite1J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ2 != null)
            {
                txblSlash2J2.Visibility = Visibility.Visible;
                txblEmplacementUnite2J2Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite2J2Vie.Visibility = Visibility.Visible;
                imgUnite2J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ2.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp2)
                    imgUnite2J2.Opacity = 0.5;
                else
                    imgUnite2J2.Opacity = 1;
            }
            else
            {
                txblSlash2J2.Visibility = Visibility.Hidden;
                txblEmplacementUnite2J2Attaque.Visibility = Visibility.Hidden;
                txblEmplacementUnite2J2Vie.Visibility = Visibility.Hidden;
                imgUnite2J2.Source = null;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                txblSlash3J2.Visibility = Visibility.Visible;
                txblEmplacementUnite3J2Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite3J2Vie.Visibility = Visibility.Visible;
                imgUnite3J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ3.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp3)
                    imgUnite3J2.Opacity = 0.5;
                else
                    imgUnite3J2.Opacity = 1;
            }
            else
            {
                txblSlash3J2.Visibility = Visibility.Hidden;
                txblEmplacementUnite3J2Attaque.Visibility = Visibility.Hidden;
                txblEmplacementUnite3J2Vie.Visibility = Visibility.Hidden;
                imgUnite3J2.Source = null;
            }
            // Insérer les img des cartes Unités en jeu du joueur 1 s'il y en a
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ1 != null)
            {
                txblSlash1J1.Visibility = Visibility.Visible;
                txblEmplacementUnite1J1Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite1J1Vie.Visibility = Visibility.Visible;
                imgUnite1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ1.Nom + ".jpg"));
                //imgUnite1J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
                if (laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp1)
                    imgUnite1J1.Opacity = 0.5;
                else
                    imgUnite1J1.Opacity = 1;
            }
            else
            {
                txblSlash1J1.Visibility = Visibility.Hidden;
                txblEmplacementUnite1J1Attaque.Visibility = Visibility.Hidden;
                txblEmplacementUnite1J1Vie.Visibility = Visibility.Hidden;
                imgUnite1J1.Source = null;
                imgUnite1J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;                
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ2 != null)
            {
                txblSlash2J1.Visibility = Visibility.Visible;
                txblEmplacementUnite2J1Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite2J1Vie.Visibility = Visibility.Visible;
                imgUnite2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ2.Nom + ".jpg"));
                //imgUnite2J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
                if (laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp2)
                    imgUnite2J1.Opacity = 0.5;
                else
                    imgUnite2J1.Opacity = 1;
            }
            else
            {
                txblSlash2J1.Visibility = Visibility.Hidden;
                txblEmplacementUnite2J1Attaque.Visibility = Visibility.Hidden;
                txblEmplacementUnite2J1Vie.Visibility = Visibility.Hidden;
                imgUnite2J1.Source = null;
                imgUnite2J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                txblSlash3J1.Visibility = Visibility.Visible;
                txblEmplacementUnite3J1Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite3J1Vie.Visibility = Visibility.Visible;
                imgUnite3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ1.Champ3.Nom + ".jpg"));
                //imgUnite3J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
                if (laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp3)
                    imgUnite3J1.Opacity = 0.5;
                else
                    imgUnite3J1.Opacity = 1;
            }
            else
            {
                txblSlash3J1.Visibility = Visibility.Hidden;
                txblEmplacementUnite3J1Attaque.Visibility = Visibility.Hidden;
                txblEmplacementUnite3J1Vie.Visibility = Visibility.Hidden;
                imgUnite3J1.Source = null;                
                imgUnite3J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            imgUnite1J1.PreviewMouseLeftButtonUp -= ChoisirEmplacementUnite;            
            imgUnite2J1.PreviewMouseLeftButtonUp -= ChoisirEmplacementUnite;
            imgUnite3J1.PreviewMouseLeftButtonUp -= ChoisirEmplacementUnite;

            emplacementUnite1J1.BorderBrush = Brushes.Transparent;
            emplacementUnite2J1.BorderBrush = Brushes.Transparent;
            emplacementUnite3J1.BorderBrush = Brushes.Transparent;

        }
        /// <summary>
        /// Fonction pour créer des images.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Fonction pour le zoom sur une carte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Carte_Zoom(object sender, MouseEventArgs e)
        {
            imgZoom = (Image)sender;
            AfficherCarteZoom(imgZoom, true);          
            
        }
        /// <summary>
        /// Fonction qui permet le zoom sur une carte en jeu, ou de la selectionner pour attaquer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Carte_CarteEnJeu_Zoom(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            if(Phase == 3 && img.Opacity == 1)
            {
                switch(img.Name.Substring(8, 1))
                {
                    case "1":
                        Unite1J1Attack = ChangerBoolAttaque(Unite1J1Attack);
                        ChangerBorderAttaque(emplacementUnite1J1, Unite1J1Attack);
                        break;
                    case "2":
                        Unite2J1Attack = ChangerBoolAttaque(Unite2J1Attack);
                        ChangerBorderAttaque(emplacementUnite2J1, Unite2J1Attack);
                        break;
                    case "3":
                        Unite3J1Attack = ChangerBoolAttaque(Unite3J1Attack);
                        ChangerBorderAttaque(emplacementUnite3J1, Unite3J1Attack);
                        break;
                }
            }
            else
            {
                AfficherCarteZoom(img, false);
            }
        }
        /// <summary>
        /// Change le booléen associé pour l'attaque.
        /// </summary>
        /// <param name="attaque"></param>
        /// <returns></returns>
        private bool ChangerBoolAttaque(bool attaque)
        {
            if(attaque)
            {
                attaque = false;
            }
            else
            {
                attaque = true;
            }
            return attaque;
        }
        /// <summary>
        /// Change le contour de la carte pour démontré qu'elle va attaquer.
        /// </summary>
        /// <param name="border"></param>
        /// <param name="attaque"></param>
        private void ChangerBorderAttaque(Border border, bool attaque)
        {
            if (attaque)
            {
                border.BorderBrush = Brushes.Yellow;
            }
            else
            {
                attaque = true;
                border.BorderBrush = Brushes.Transparent;
            }
        }
        /// <summary>
        /// Affiche la carte zoomer.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="carteMain"></param>
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
        /// <summary>
        /// Fonction lors de la fermeture de l'écran de ressource.
        /// </summary>
        public void FermerEcranRessource()
        {
            grd1.Children.Remove(ContenuEcran);
            recRessource.Visibility = Visibility.Hidden;
            laTableDeJeu.AvancerPhase();
            AfficherMain();
        }
        /// <summary>
        /// Fonction lors de la création de l'écran ressource.
        /// </summary>
        /// <param name="joueur"></param>
        /// <param name="points"></param>
        /// <param name="maxRessourceLevel"></param>
        /// <param name="partie"></param>
        public void EcranRessource(Joueur joueur, int points, int maxRessourceLevel, Partie partie)
        {
            ContenuEcran = new ChoixRessources(joueur, points, maxRessourceLevel, partie);
            recRessource.Visibility = Visibility.Visible;

            grd1.Children.Add(ContenuEcran);
        }
        /// <summary>
        /// Affiche les coups possible du joueur.
        /// </summary>
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
        /// <summary>
        /// Permet de choisir l'emplacement de l'unité déployer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Action lorsque la carte est zoomer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Action lorsqu'on click sur le rectangle noir à l'entour du zoom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

