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
    /// Logique d'interaction pour Campagne.xaml
    /// </summary>
    public partial class Campagne : UserControl
    {
        public MainWindow Main { get; set; }
        public double btnHeight;
        public double btnWidth;
        public bool estin = false;
        public Campagne(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;

            Main = main;

            btnNiveau1.MouseEnter += new MouseEventHandler(btnMouseEnter);
            btnNiveau1.MouseLeave += new MouseEventHandler(btnMouseLeave);
            btnNiveau2.MouseEnter += new MouseEventHandler(btnMouseEnter);
            btnNiveau2.MouseLeave += new MouseEventHandler(btnMouseLeave);
            btnNiveau3.MouseEnter += new MouseEventHandler(btnMouseEnter);
            btnNiveau3.MouseLeave += new MouseEventHandler(btnMouseLeave);
            btnNiveau4.MouseEnter += new MouseEventHandler(btnMouseEnter);
            btnNiveau4.MouseLeave += new MouseEventHandler(btnMouseLeave);
            btnNiveau5.MouseEnter += new MouseEventHandler(btnMouseEnter);
            btnNiveau5.MouseLeave += new MouseEventHandler(btnMouseLeave);
        }

        private void btnMenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNiveau1_Click(object sender, RoutedEventArgs e)
        {
           Main.EcranPartie(); //EN COURS
        }

        private void btnMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = ((Button)sender);

            if (!estin)
            {
                btnHeight = btn.Height;
                btnWidth = btn.Width;
            }
            else
            {
                PresentationNiveau(btn);
                brdStory.Visibility = Visibility.Visible;
            }

            btn.Height = 200;
            btn.Width = 200;

            estin = true;
        }

        private void btnMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = ((Button)sender);

            btn.Height = btnHeight;
            btn.Width = btnWidth;

            brdStory.Visibility = Visibility.Hidden;

            estin = false;
        }

        private void btnNiveau2_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void btnNiveau3_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void btnNiveau4_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void btnNiveau5_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void PresentationNiveau(Button btn)
        {
            string titre = "vide";
            string niveau = "autre";
            //TODO: connexion BD pour les nom des niveaux
            switch (btn.Name.ToString())
            {
                case "btnNiveau1":
                    niveau = "Niveau 1";
                    titre = "Robot Turenne";
                    break;
                case "btnNiveau2":
                    niveau = "Niveau 2";
                    titre = "He sells the garden";
                    break;
                case "btnNiveau3":
                    niveau = "Niveau 3";
                    titre = "Charronnitoman";
                    break;
                case "btnNiveau4":
                    niveau = "Niveau 4";
                    titre = "KESH/SLASH/ALL";
                    break;
                case "btnNiveau5":
                    niveau = "Niveau 5";
                    titre = "The preacher";
                    break;
            }
            txBlStory.Text = @"-***- " + niveau + " - Chef de guerre : " + titre;

            if (btn.Margin.Top < 250 && btn.Margin.Left < 600)
            {
                brdStory.Margin = new Thickness(btn.Margin.Left, btn.Margin.Top + btn.Height + 20, 0, 0);
            }
            else
            {
                if (btn.Margin.Left >= 600)
                {
                    brdStory.Margin = new Thickness(btn.Margin.Left - brdStory.Width + btn.Width, btn.Margin.Top - brdStory.Height - 20, 0, 0);
                }
                else
                {
                    brdStory.Margin = new Thickness(btn.Margin.Left, btn.Margin.Top - brdStory.Height - 20, 0, 0);
                }
            }
        }              
    }
}
