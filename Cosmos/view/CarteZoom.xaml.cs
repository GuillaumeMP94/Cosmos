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
    /// Logique d'interaction pour CarteZoom.xaml
    /// </summary>
    public partial class CarteZoom : UserControl
    {
        public Partie Partie { get; set; }
        public Image imgZoom { get; set; }
        public CarteZoom(Image imgSource, Partie partie)
        {
            InitializeComponent();

            Partie = partie;            

            imgZoom = new Image();
            imgZoom.Source = imgSource.Source;
            imgZoom.Height = 500;
            imgZoom.VerticalAlignment = VerticalAlignment.Top;
            imgZoom.HorizontalAlignment = HorizontalAlignment.Center;
            imgZoom.Margin = new Thickness(0, 70, 0, 0);

            grdCarteZoom.Children.Add(imgZoom);
        }
    }
}
