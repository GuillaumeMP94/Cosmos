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

namespace Cosmos
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserControl ContenuEcran { get; set; }
        private Connexion Connexion { get; set; }
        private MenuPrincipal MenuPrincipal { get; set; }
        public CreationCompte Creation { get; set; }
        public RecuperationCompte Recuperation { get; set; }
        public OptionCompte OptionCompte { get; set; }
        public Campagne Campagne { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/bg1.png")));

            ContenuEcran = new Connexion(this);
            grdMain.Children.Add(ContenuEcran);

            //TODO: Enlever la prochaine ligne avant remise
            // EcranMenuPrincipal();


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
            ContenuEcran = new CreationCompte(this);


            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranConnexion()
        {
            grdMain.Children.Remove(ContenuEcran);
            ContenuEcran = new Connexion(this);

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/bg1.png")));

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranRecuperation()
        {
            grdMain.Children.Remove(ContenuEcran);
            ContenuEcran = new RecuperationCompte(this);

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranOptionCompte()
        {
            grdMain.Children.Remove(ContenuEcran);
            ContenuEcran = new OptionCompte(this);

            grdMain.Children.Add(ContenuEcran);
        }

        public void EcranCampagne()
        {
            grdMain.Children.Remove(ContenuEcran);
            ContenuEcran = new Campagne(this);

            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/images/campagne/bgSS2.jpg")));

            grdMain.Children.Add(ContenuEcran);
        }
    }
}
