using Cosmos.accesBD;
using Cosmos.metier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
        public List<Image> ImgMainJ2 { get; set; }
        public List<Image> ImgMainJoueur { get; set; }
        public List<Border> ListBorderImgMainJoueur { get; set; }
        public int IndexCarteZoomer { get; set; }

        public DispatcherTimer Temps { get; set; }
        public bool Unite1J1Attack { get; set; }
        public bool Unite2J1Attack { get; set; }
        public bool Unite3J1Attack { get; set; }
        public List<int> Choix { get; set; }
        public SoundPlayer Player { get; set; }

        public int Phase
        {
            get { return laTableDeJeu.Phase; }
            set
            {
                laTableDeJeu.Phase = value;
            }
        }

        public Partie(MainWindow main, int niveau)
        {
            InitializeComponent();
            Main = main;
            Choix = new List<int>();
            //Timer pour changer automatiquement la phase de fin et la phase de ressource.
            Temps = new DispatcherTimer();
            Temps.Interval = TimeSpan.FromMilliseconds(1000);
            Temps.Tick += timer_Tick;

            //TODO: Utilisateur Connecter
            //Utilisateur utilisateur1 = MySqlUtilisateurService.RetrieveByNom("Damax");
            Utilisateur utilisateur1 = Main.UtilisateurConnecte;
            // TODO: AI
            //Utilisateur utilisateur2 = MySqlUtilisateurService.RetrieveByNom("Guillaume");
            // TODO: Reinitialiser les utilisateurs à la fin de la partie.
            utilisateur1.Reinitialiser();


            Robot = new AI(niveau, new Ressource(2, 2, 2), MySqlDeckService.RetrieveById(1));


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
            ImgMainJ2 = new List<Image>();

            // Binding pour les points d'attaque et de vie des unités en jeu
            txblEmplacementUnite1J1Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite2J1Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite3J1Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite1J2Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite2J2Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite3J2Vie.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;

            // Binding pour les points de vie des unités en jeu
            txblEmplacementUnite1J1Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite2J1Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite3J1Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ1;
            txblEmplacementUnite1J2Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite2J2Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;
            txblEmplacementUnite3J2Attaque.DataContext = laTableDeJeu.ChampBatailleUnitesJ2;

            // binding pour le gain de ressources par tour
            txBlnbCharroniteTourJ1.DataContext = laTableDeJeu.Joueur1.LevelRessource;
            txBlnbBarilTourJ1.DataContext = laTableDeJeu.Joueur1.LevelRessource;
            txBlnbAlainDollarTourJ1.DataContext = laTableDeJeu.Joueur1.LevelRessource;
            txBlnbCharroniteTourJ2.DataContext = laTableDeJeu.Joueur2.LevelRessource;
            txBlnbBarilTourJ2.DataContext = laTableDeJeu.Joueur2.LevelRessource;
            txBlnbAlainDollarTourJ2.DataContext = laTableDeJeu.Joueur2.LevelRessource;

            // Compteur pour afficher le nombre de cartes dans le deck des joueurs
            txBLnbCarteJ1.DataContext = laTableDeJeu.Joueur1.DeckAJouer;
            txBLnbCarteJ2.DataContext = laTableDeJeu.Joueur2.DeckAJouer;

            // Listener des events PhaseChange, RefreshAll et ChoisirCible
            TrousseGlobale.ChoisirCible += ChoixCibleListener;
            TrousseGlobale.PhaseChange += changerPhase;
            TrousseGlobale.RefreshAll += RefreshAllEvent;
            TrousseGlobale.FinPartie += TerminerPartie;
        }

        private void TerminerPartie(object sender, FinPartieEventArgs e)
        {
            VerifierVictoire();
        }

        private void ChoixCibleListener(object sender, ChoisirCibleEventArgs e)
        {
            EcranChoixCible(e.Cible,e.NbCible);
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
            if (txblSlash1J1.Dispatcher.CheckAccess() == true)
            {
                AffichagePhaseRessource();
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    AffichagePhaseRessource();
                });
            }
            RefreshAll();
            Temps.Stop();
            laTableDeJeu.NotifyAi();
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
            laTableDeJeu.NotifyAi();
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
            {
                laTableDeJeu.ExecuterAttaque(Unite1J1Attack, Unite2J1Attack, Unite3J1Attack);
                AfficherAttaqueJ1(Unite1J1Attack, Unite2J1Attack, Unite3J1Attack);
            }
            else
            {
                laTableDeJeu.ExecuterAttaque(Robot.AttaqueChamp1, Robot.AttaqueChamp2, Robot.AttaqueChamp3);
                AfficherAttaqueJ2(Robot.AttaqueChamp1, Robot.AttaqueChamp2, Robot.AttaqueChamp3);
            }
            Temps.Start();
        }


        private void AfficherAttaqueJ1(bool champ1, bool champ2, bool champ3)
        {
            if (champ1 && laTableDeJeu.ChampBatailleUnitesJ1.Champ1 != null && !laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp1 )
            {
                this.Dispatcher.Invoke(() =>
                {
                    imgAttaque1J1.Visibility = Visibility.Visible;
                    if (laTableDeJeu.ChampBatailleUnitesJ2.Champ1 != null)
                        imgCible1J2.Visibility = Visibility.Visible;
                    else
                        imgCibleJ2.Visibility = Visibility.Visible;
                });
            }
            if (champ2 && laTableDeJeu.ChampBatailleUnitesJ1.Champ2 != null && !laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp2)
            {
                this.Dispatcher.Invoke(() =>
                {
                    imgAttaque2J1.Visibility = Visibility.Visible;
                    if (laTableDeJeu.ChampBatailleUnitesJ2.Champ2 != null)
                        imgCible2J2.Visibility = Visibility.Visible;
                    else
                        imgCibleJ2.Visibility = Visibility.Visible;
                    
                });
            }
            if (champ3 && laTableDeJeu.ChampBatailleUnitesJ1.Champ3 != null && !laTableDeJeu.ChampBatailleUnitesJ1.EstEnPreparationChamp3 )
            {
                this.Dispatcher.Invoke(() =>
                {
                    imgAttaque3J1.Visibility = Visibility.Visible;
                    if (laTableDeJeu.ChampBatailleUnitesJ2.Champ3 != null)
                        imgCible3J2.Visibility = Visibility.Visible;
                    else
                        imgCibleJ2.Visibility = Visibility.Visible;

                });
            }
            Task.Delay(1000).ContinueWith(_ =>
            {
                //Effacer les icons
                HideAttaque();

            });
        }
        private void AfficherAttaqueJ2(bool champ1, bool champ2, bool champ3)
        {
            if (champ1 && laTableDeJeu.ChampBatailleUnitesJ2.Champ1 != null && !laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp1)
            {
                this.Dispatcher.Invoke(() =>
                {

                    imgAttaque1J2.Visibility = Visibility.Visible;
                    if (laTableDeJeu.ChampBatailleUnitesJ1.Champ1 != null)
                        imgCible1J1.Visibility = Visibility.Visible;
                    else
                        imgCibleJ1.Visibility = Visibility.Visible;
                });
            }
            if (champ2 && laTableDeJeu.ChampBatailleUnitesJ2.Champ2 != null && !laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp2)
            {
                this.Dispatcher.Invoke(() =>
                {
                    imgAttaque2J2.Visibility = Visibility.Visible;
                    if (laTableDeJeu.ChampBatailleUnitesJ1.Champ2 != null)
                        imgCible2J1.Visibility = Visibility.Visible;
                    else
                        imgCibleJ1.Visibility = Visibility.Visible;
                });
            }
            if (champ3 && laTableDeJeu.ChampBatailleUnitesJ2.Champ3 != null && !laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp3)
            {
                this.Dispatcher.Invoke(() =>
                {

                    imgAttaque3J2.Visibility = Visibility.Visible;
                    if (laTableDeJeu.ChampBatailleUnitesJ1.Champ3 != null)
                        imgCible3J1.Visibility = Visibility.Visible;
                    else
                        imgCibleJ1.Visibility = Visibility.Visible;
                });
            }
            Task.Delay(1000).ContinueWith(_ =>
            {
                //Effacer les icons
                HideAttaque();

            });
        }

        private void HideAttaque()
        {
            if (imgCible1J1.Dispatcher.CheckAccess() == true)
            {
                CacherAttaque();
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    CacherAttaque();
                });
            }
        }

        private void CacherAttaque()
        {
            // Cible
            imgCible1J1.Visibility = Visibility.Hidden;
            imgCible2J1.Visibility = Visibility.Hidden;
            imgCible3J1.Visibility = Visibility.Hidden;
            imgCible1J2.Visibility = Visibility.Hidden;
            imgCible2J2.Visibility = Visibility.Hidden;
            imgCible3J2.Visibility = Visibility.Hidden;
            imgCibleJ1.Visibility = Visibility.Hidden;
            imgCibleJ2.Visibility = Visibility.Hidden;
            // Attaque
            imgAttaque1J1.Visibility = Visibility.Hidden;
            imgAttaque2J1.Visibility = Visibility.Hidden;
            imgAttaque3J1.Visibility = Visibility.Hidden;
            imgAttaque1J2.Visibility = Visibility.Hidden;
            imgAttaque2J2.Visibility = Visibility.Hidden;
            imgAttaque3J2.Visibility = Visibility.Hidden;

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
            laTableDeJeu.ExecuterEffetFinTour();
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
            if (laTableDeJeu.JoueurEstMort(false))
            {
                // Victoire
                imgFinTour.Visibility = Visibility.Hidden;
                TrousseGlobale.PhaseChange -= changerPhase;
                TrousseGlobale.RefreshAll -= RefreshAllEvent;
                TrousseGlobale.FinPartie -= TerminerPartie;
                Temps.Stop();
                PlaySound(Cosmos.Properties.Resources.win);
                EcranFinDePartie(true);
            }
            else if (laTableDeJeu.JoueurEstMort(true))
            {
                // Défaite
                imgFinTour.Visibility = Visibility.Hidden;
                TrousseGlobale.RefreshAll -= RefreshAllEvent;
                TrousseGlobale.PhaseChange -= changerPhase;
                TrousseGlobale.FinPartie -= TerminerPartie;
                Temps.Stop();
                PlaySound(Cosmos.Properties.Resources.lost);
                EcranFinDePartie(false);
            }
        }

        private void PlaySound(System.IO.Stream uri)
        {
            Player = new SoundPlayer(uri);
            Player.Play();
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
                AfficherDerniereUsine();
                AfficherCarteMainFermeJ2();

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
                    AfficherCarteMainFermeJ2();
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
        /// Afficher à l'écran la main fermé de l'adverdsaire
        /// </summary>
        private void AfficherCarteMainFermeJ2()
        {
            
            foreach (Image element in ImgMainJ2)
            {
                grdCartesAdversaire.Children.Remove(element);
            }
            ImgMainJ2.Clear();
            int i = 0;
            foreach(Carte element in laTableDeJeu.LstMainJ2)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/CardBack.png"));
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.Margin = new Thickness(i * 50, 0, 0, 0);
                img.SetValue(Panel.ZIndexProperty, i);
                
                grdCartesAdversaire.Children.Add(img);
                i++;
                ImgMainJ2.Add(img);
            }


            //grdCartesAdversaire
            //< Image Source = "/images/CardBack.png" Grid.Column = "0" HorizontalAlignment = "Left" Panel.ZIndex = "1" Margin = "0,0,0,0" />
        }
        private void AfficherCarteMainOuvertJ2()
        {

            foreach (Image element in ImgMainJ2)
            {
                grdCartesAdversaire.Children.Remove(element);
            }
            ImgMainJ2.Clear();
            int i = 0;
            foreach (Carte element in laTableDeJeu.LstMainJ2)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + element.Nom + ".jpg"));
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.Margin = new Thickness(i * 50, 0, 0, 0);
                img.SetValue(Panel.ZIndexProperty, i);

                grdCartesAdversaire.Children.Add(img);
                i++;
                ImgMainJ2.Add(img);
            }


            //grdCartesAdversaire
            //< Image Source = "/images/CardBack.png" Grid.Column = "0" HorizontalAlignment = "Left" Panel.ZIndex = "1" Margin = "0,0,0,0" />
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
                imgBatiment1J2.Cursor = Cursors.Hand;
                imgBatiment1J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment1J2.Source = null;
                imgBatiment1J2.Cursor = Cursors.Arrow;
                imgBatiment1J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampConstructionsJ2.Champ2 != null)
            {
                imgBatiment2J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ2.Nom + ".jpg"));
                imgBatiment2J2.Cursor = Cursors.Hand;
                imgBatiment2J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment2J2.Source = null;
                imgBatiment2J2.Cursor = Cursors.Arrow;
                imgBatiment2J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampConstructionsJ2.Champ3 != null)
            {
                imgBatiment3J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ3.Nom + ".jpg"));
                imgBatiment3J2.Cursor = Cursors.Hand;
                imgBatiment3J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment3J2.Source = null;
                imgBatiment3J2.Cursor = Cursors.Arrow;
                imgBatiment3J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampConstructionsJ2.Champ4 != null)
            {
                imgBatiment4J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ2.Champ4.Nom + ".jpg"));
                imgBatiment4J2.Cursor = Cursors.Hand;
                imgBatiment4J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment4J2.Source = null;
                imgBatiment4J2.Cursor = Cursors.Arrow;
                imgBatiment4J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            // Insérer les img des cartes Batiments en jeu du joueur 1 s'il y en a
            if (laTableDeJeu.ChampConstructionsJ1.Champ1 != null)
            {
                imgBatiment1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ1.Nom + ".jpg"));
                imgBatiment1J1.Cursor = Cursors.Hand;
                imgBatiment1J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment1J1.Source = null;
                imgBatiment1J1.Cursor = Cursors.Arrow;
                imgBatiment1J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampConstructionsJ1.Champ2 != null)
            {
                imgBatiment2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ2.Nom + ".jpg"));
                imgBatiment2J1.Cursor = Cursors.Hand;
                imgBatiment2J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment2J1.Source = null;
                imgBatiment2J1.Cursor = Cursors.Arrow;
                imgBatiment2J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampConstructionsJ1.Champ3 != null)
            {
                imgBatiment3J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ3.Nom + ".jpg"));
                imgBatiment3J1.Cursor = Cursors.Hand;
                imgBatiment3J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment3J1.Source = null;
                imgBatiment3J1.Cursor = Cursors.Arrow;
                imgBatiment3J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
            if (laTableDeJeu.ChampConstructionsJ1.Champ4 != null)
            {
                imgBatiment4J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampConstructionsJ1.Champ4.Nom + ".jpg"));
                imgBatiment4J1.Cursor = Cursors.Hand;
                imgBatiment4J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            }
            else
            {
                imgBatiment4J1.Source = null;
                imgBatiment4J1.Cursor = Cursors.Arrow;
                imgBatiment4J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            }
        }
        /// <summary>
        /// Fonction pour afficher les unites.
        /// </summary>
        private void AfficherChampUnites()
        {
            // Pour pas stacker les events
            imgUnite1J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite2J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite3J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            // Insérer les img des cartes Unités en jeu du joueur 2 s'il y en a
            if (laTableDeJeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                txblSlash1J2.Visibility = Visibility.Visible;
                txblEmplacementUnite1J2Attaque.Visibility = Visibility.Visible;
                txblEmplacementUnite1J2Vie.Visibility = Visibility.Visible;
                imgUnite1J2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.ChampBatailleUnitesJ2.Champ1.Nom + ".jpg"));
                if (laTableDeJeu.ChampBatailleUnitesJ2.EstEnPreparationChamp1)
                    imgUnite1J2.Opacity = 0.5;
                else
                    imgUnite1J2.Opacity = 1;
                imgUnite1J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
              
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
                imgUnite2J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
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
                imgUnite3J2.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
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
                        if (laTableDeJeu.ChampBatailleUnitesJ1.AttChamp1 > 0)
                        {
                            Unite1J1Attack = ChangerBoolAttaque(Unite1J1Attack);
                            ChangerBorderAttaque(emplacementUnite1J1, Unite1J1Attack);
                        }
                        break;
                    case "2":
                        if (laTableDeJeu.ChampBatailleUnitesJ1.AttChamp2 > 0)
                        {
                            Unite2J1Attack = ChangerBoolAttaque(Unite2J1Attack);
                            ChangerBorderAttaque(emplacementUnite2J1, Unite2J1Attack);
                        }
                        break;
                    case "3":
                        if (laTableDeJeu.ChampBatailleUnitesJ1.AttChamp3 > 0)
                        {
                            Unite3J1Attack = ChangerBoolAttaque(Unite3J1Attack);
                            ChangerBorderAttaque(emplacementUnite3J1, Unite3J1Attack);
                        }
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
        /// Fonction pour afficher la dernière carte arrivée dans l'usine de recyclage
        /// </summary>
        public void AfficherDerniereUsine()
        {
            if(laTableDeJeu.LstUsineRecyclageJ1.Count > 0)
            {
                imgUsineRecyclageJ1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.LstUsineRecyclageJ1[laTableDeJeu.LstUsineRecyclageJ1.Count - 1].Nom + ".jpg"));
            }
            if(laTableDeJeu.LstUsineRecyclageJ2.Count > 0)
            {
                imgUsineRecyclageJ2.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + laTableDeJeu.LstUsineRecyclageJ2[laTableDeJeu.LstUsineRecyclageJ2.Count - 1].Nom + ".jpg"));
            }

        }
        /// <summary>
        /// Fonction lors de la fermeture de l'écran d'options.
        /// </summary>
        public void FermerEcranOptions()
        {
            grd1.Children.Remove(ContenuEcran);
            recRessource.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Fonction lors de la fermeture de l'écran fin de partie.
        /// </summary>
        public void FermerEcranFinDePartie()
        {
            grd1.Children.Remove(ContenuEcran);
            Main.EcranMenuPrincipal();
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
        /// Fonction lors de la création de l'écran fin de partie.
        /// </summary>
        /// <param name="victoire"></param>
        public void EcranFinDePartie(bool victoire)
        {
            ContenuEcran = new FinDePartie(this, victoire);
            recRessource.Visibility = Visibility.Visible;

            grd1.Children.Add(ContenuEcran);
        }
        /// <summary>
        /// Fonction lors de la création de l'écran fin de partie.
        /// </summary>
        /// <param name="victoire"></param>
        public void EcranOptions()
        {
            ContenuEcran = new OptionCompte(this);
            recRessource.Visibility = Visibility.Visible;

            grd1.Children.Add(ContenuEcran);
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
        /// Fonction lors de la fermeture de l'écran de choix cible.
        /// </summary>
        public void FermerEcranChoixCible()
        {
            grd1.Children.Remove(ContenuEcran);
            recRessource.Visibility = Visibility.Hidden;
            // Executer l'impact avec les choix.
            laTableDeJeu.ExecuterImpact(Choix);
            Choix.Clear();
            RefreshAll();
        }
        /// <summary>
        /// Fonction lors de la création de l'écran choix cible.
        /// </summary>
        /// <param name="joueur"></param>
        /// <param name="points"></param>
        /// <param name="maxRessourceLevel"></param>
        /// <param name="partie"></param>
        public void EcranChoixCible(int cible, int nbCible)
        {
            ContenuEcran = new ChoixCible(Choix, cible , nbCible , laTableDeJeu, this);
            recRessource.Visibility = Visibility.Visible;

            grd1.Children.Add(ContenuEcran);
        }
        /// <summary>
        /// Affiche les coups possible du joueur.
        /// </summary>
        private void AfficherCoupPoosible()
        {
            grdCartesEnjeu.SetValue(Panel.ZIndexProperty, 99);
            // Enlever les zooms
            imgUnite1J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite2J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite3J1.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite1J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite2J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            imgUnite3J2.PreviewMouseLeftButtonUp -= Carte_CarteEnJeu_Zoom;
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ1 == null) // TEMP FIX TODO REMOVE
            {
                imgUnite1J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/jouer.jpg"));
                imgUnite1J1.Cursor = Cursors.Hand;
                imgUnite1J1.PreviewMouseLeftButtonUp += ChoisirEmplacementUnite;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ2 == null)
            {
                imgUnite2J1.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/jouer.jpg"));
                imgUnite2J1.Cursor = Cursors.Hand;
                imgUnite2J1.PreviewMouseLeftButtonUp += ChoisirEmplacementUnite;
            }
            if (laTableDeJeu.ChampBatailleUnitesJ1.Champ3 == null)
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
            // Remettre les zoom
            imgUnite1J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            imgUnite2J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;
            imgUnite3J1.PreviewMouseLeftButtonUp += Carte_CarteEnJeu_Zoom;

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

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            EcranOptions();
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

