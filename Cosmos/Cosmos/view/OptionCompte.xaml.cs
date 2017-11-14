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
        public OptionCompte(MainWindow main)
        {
            InitializeComponent();

            Main = main;
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
    }
}
