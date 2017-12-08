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
    /// Logique d'interaction pour OptionCompte.xaml
    /// </summary>
    public partial class OptionCompte : UserControl
    {
        public MainWindow Main { get; set; }
        public Partie LaPartie { get; set; }
        public UserControl ContenuEcran { get; set; }
        public OptionCompte(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            chbMusic.Click += PlayStopMusic;
            this.DataContext = Main;
            btnMenuPrincipal.Visibility = Visibility.Visible;
            btnRetourPartie.Visibility = Visibility.Hidden;
        }

        private void PlayStopMusic(object sender, RoutedEventArgs e)
        {
            if (Main.MusicOn == true)
            {
                Main.PlayMusic();
                Main.imgMusic.Opacity = 1;
            }
            else
            {
                Main.Player.Stop();
                Main.imgMusic.Opacity = 0.5;
            }
        }

        public OptionCompte(MainWindow main, Partie laPartie)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = Main;
            LaPartie = laPartie;
            btnRetourPartie.Visibility = Visibility.Visible;
            btnMenuPrincipal.Visibility = Visibility.Hidden;
            btnModifPassword.Opacity = 0.5;
            btnModifPassword.IsEnabled = false;
            btnModifPassword.Cursor = Cursors.Arrow;
        }

        private void btnModifPassword_Click(object sender, RoutedEventArgs e)
        {
            grdOptions.Visibility = Visibility.Hidden;
            EcranChangerMotPasse();
        }

        private void btnMenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void btnModifImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnModifSupprimerCompte_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnScreen_Click(object sender, RoutedEventArgs e)
        {

            TextBlock tb = (TextBlock)btnScreen.Template.FindName("txblScreen", btnScreen);

            if(tb.Text == "Sortir du mode plein écran")
            {
                tb.Text = "Mode plein écran";
                Main.WindowState = WindowState.Normal;
                Main.WindowStyle = WindowStyle.SingleBorderWindow;                
            }
            else
            {                
                tb.Text = "Sortir du mode plein écran";
                Main.WindowState = WindowState.Maximized;
                Main.WindowStyle = WindowStyle.None;
                Main.Topmost = true;                
            }

        }
        /// <summary>
        /// Fonction qui ouvre l'écran de changement de mot de passe.
        /// </summary>
        public void EcranChangerMotPasse()
        {
            ContenuEcran = new ChangerMotPasse(this);

            grdOption.Children.Add(ContenuEcran);
        }
        /// <summary>
        /// Fonction lors de la fermeture de l'écran de changement de mot de passe.
        /// </summary>
        public void FermerChangerMotPasse()
        {
            grdOption.Children.Remove(ContenuEcran);
            grdOptions.Visibility = Visibility.Visible;
        }
        private void btnRetourPartie_Click(object sender, RoutedEventArgs e)
        {
            LaPartie.FermerEcranOptions();
        }
    }
}
