using Cosmos.accesBD;
using Cosmos.metier;
using Cosmos.view;
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
using System.Media;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace Cosmos
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public UserControl ContenuEcran { get; set; }
        public UserControl ContenuListeAmi { get; set; }
        public UserControl ContenuAddModifSupp { get; set; }
        private Connexion Connexion { get; set; }
        private MenuPrincipal MenuPrincipal { get; set; }
        public CreationCompte Creation { get; set; }
        public RecuperationCompte Recuperation { get; set; }
        public OptionCompte OptionCompte { get; set; }
        public Campagne Campagne { get; set; }
        public Utilisateur UtilisateurConnecte { get; set; }
        public Partie Partie { get; set; }
        public List<Utilisateur> LstAmis { get; set; }
        public SoundPlayer Player { get; set; }
        private bool musicOn = true;
        public bool MusicOn
        {
            get { return musicOn; }
            set
            {
                musicOn = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MusicOn"));
                }

            }
        }
        private bool aideContextuelle = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AideContextuelle
        {
            get { return aideContextuelle; }
            set
            {
                aideContextuelle = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AideContextuelle"));
                }

            }
        }

        public MainWindow()
        {
            InitializeComponent();

            //this.Topmost = true;

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/bg1.png")));
            
            ContenuEcran = new Connexion(this);
            grdMain.Children.Add(ContenuEcran);

            //Thread Son = new Thread(PlayMusic);
            //Son.Start();
            PlayMusic();


            //TODO: Enlever la prochaine ligne avant remise
            //EcranPartie(1);
            //EcranMenuPrincipal();
            //EcranCampagne();

        }

        public void PlayMusic()
        {
            MusicOn = true;
            Player = new SoundPlayer(Cosmos.Properties.Resources.superboy);
            Player.PlayLooping();
        }

        public void QuitterMain()
        {
            this.Close();
        }

        public void EcranMenuPrincipal()
        {            
            grdMain.Children.Remove(ContenuEcran);
            ContenuEcran = new MenuPrincipal(this);

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/backMenuPrincipal.png")));

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranCreerCompte()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            ContenuEcran = new CreationCompte(this);


            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranConnexion()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            ContenuEcran = new Connexion(this);

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/bg1.png")));

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranRecuperation()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            ContenuEcran = new RecuperationCompte(this);

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranOptionCompte()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            ContenuEcran = new OptionCompte(this);

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranCampagne()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            Player.Stop();
            imgMusic.Visibility = Visibility.Hidden;
            ContenuEcran = new Campagne(this);

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/campagne/bgSS2.jpg")));

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranReglements()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            ContenuEcran = new ReglementsTutoriel(this);

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranListeAmis()
        {
            ContenuListeAmi = new ListeAmis(this);

            grdMain.Children.Add(ContenuListeAmi);
        }

        public void EcranPartie(int niveau)
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);

            ContenuEcran = new Partie(this, niveau);

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/partie/partie_BG.jpg")));

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranGestionCartes()
        {
            grdMain.Children.Remove(ContenuEcran);
            grdMain.Children.Remove(ContenuListeAmi);
            ContenuEcran = new GestionCartes(this);

            grdMain.Children.Add(ContenuEcran);
        }

        #region ValidationChamps
        public string ValiderChampSaisi(string champ)
        {
            string pattern = @"([a-zA-Z0-9]*)";
            Match resultat = Regex.Match(champ, pattern);

            return resultat.ToString();
        }

        public bool estCourrielValide(string courriel)
        {
            bool estValide = false;
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (String.IsNullOrEmpty(courriel))
                return estValide;

            if (Regex.IsMatch(courriel,pattern))
            {
                estValide = true;
            }

            return estValide;
        }

        public string VerifierTexte(string texte)
        {
            string temp = "";

            for (int i = 0; i < texte.Length; i++)
            {
                if (texte[i] == 92 || texte[i] == (char)39)
                    temp += (char)92;

                temp += texte[i];
            }

            return temp;
        }
        #endregion

        private void imgMusic_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(imgMusic.Opacity < 1)
            {
                PlayMusic();
                imgMusic.Opacity = 1;
            }
            else
            {
                Player.Stop();
                imgMusic.Opacity = 0.5;
                MusicOn = false;
            }
        }

        private void btnAide_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            String fileName = "GuideUtilisateurCosmos.pdf";
            Process.Start(fileName);

        }

        private void Focus_Main(object sender, MouseButtonEventArgs e)
        {
            Topmost = true;
        }
    }
}
