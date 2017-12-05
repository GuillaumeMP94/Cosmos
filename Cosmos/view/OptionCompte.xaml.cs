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
        public OptionCompte(MainWindow main)
        {
            InitializeComponent();

            Main = main;
            btnMenuPrincipal.Visibility = Visibility.Visible;
            btnRetourPartie.Visibility = Visibility.Hidden;
        }
        public OptionCompte(Partie laPartie)
        {
            InitializeComponent();

            LaPartie = laPartie;
            btnRetourPartie.Visibility = Visibility.Visible;
            btnMenuPrincipal.Visibility = Visibility.Hidden;
        }

        private void btnModifPassword_Click(object sender, RoutedEventArgs e)
        {

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

            if(tb.Text == "Mettre en fenêtre mobile")
            {
                tb.Text = "Mettre en plein écran";
                Main.WindowState = WindowState.Normal;
                Main.WindowStyle = WindowStyle.SingleBorderWindow;                
            }
            else
            {                
                tb.Text = "Mettre en fenêtre mobile";
                Main.WindowState = WindowState.Maximized;
                Main.WindowStyle = WindowStyle.None;
                Main.Topmost = true;                
            }

        }

        private void btnRetourPartie_Click(object sender, RoutedEventArgs e)
        {
            LaPartie.FermerEcranOptions();
        }
    }
}
