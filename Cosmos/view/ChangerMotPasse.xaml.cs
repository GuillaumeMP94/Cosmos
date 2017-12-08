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
    /// Logique d'interaction pour ChangerMotPasse.xaml
    /// </summary>
    public partial class ChangerMotPasse : UserControl
    {
        OptionCompte LesOptions { get; set; }
        public ChangerMotPasse(OptionCompte optionCompte)
        {
            InitializeComponent();
            LesOptions = optionCompte;
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            LesOptions.FermerChangerMotPasse();
        }

        private void btnAccepter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
