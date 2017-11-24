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
    /// Logique d'interaction pour RecuperationCompte.xaml
    /// </summary>
    public partial class RecuperationCompte : UserControl
    {
        public MainWindow Main { get; set; }
        public RecuperationCompte(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            FocusManager.SetFocusedElement(Main, txbPseudo);
            Keyboard.Focus(txbPseudo);
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranConnexion();
        }

        private void btnRecuperer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
