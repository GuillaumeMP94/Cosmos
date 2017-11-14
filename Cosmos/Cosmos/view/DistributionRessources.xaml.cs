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
    /// Logique d'interaction pour DistributionRessources.xaml
    /// </summary>
    public partial class DistributionRessources : UserControl
    {
        public DistributionRessources()
        {
            InitializeComponent();
        }




        // À déplacer dans l'écran de partie
        //public DistributionRessources DistRess { get; set; }

        /// <summary>
        /// À déplacer dans l'écran de partie
        /// </summary>
        private void RectangleOmbrage()
        {
            Rectangle rect = new Rectangle();
            rect.Fill = Brushes.Black;
            rect.Opacity = 0.2;
            //TODOL Insérer le bon nom de grid
            //???.Children.Add(rect);
        }
    }
}
