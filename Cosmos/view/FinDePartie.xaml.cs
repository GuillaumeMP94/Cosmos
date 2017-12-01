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
    /// Logique d'interaction pour FinDePartie.xaml
    /// </summary>
    public partial class FinDePartie : UserControl
    {
        public Partie PartieTerminer { get; set; }
        public FinDePartie(Partie laPartie, bool victoire)
        {
            InitializeComponent();
            PartieTerminer = laPartie;
            if (victoire)
            {
                imgFin.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/victoire.png"));
            }
            else
            {
                imgFin.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/partie/defaite.png"));
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            PartieTerminer.FermerEcranFinDePartie();
        }
    }
}
