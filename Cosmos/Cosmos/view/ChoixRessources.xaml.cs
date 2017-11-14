using Cosmos.metier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour Ressource.xaml
    /// </summary>
    public partial class ChoixRessources : UserControl, INotifyPropertyChanged
    {
        public Partie Partie { get; set; }
        private int points = 0;
        public metier.Ressource RessourceInitial { get; set; }
        public int MaxRessourceLevel { get; set; }
        public int Points
        {
            get { return points; }
            set
            {
                points = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Points"));
                }

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public int PointsInitial { get; set; }
        public Joueur Joueur { get; set; }
        public ChoixRessources(Joueur joueur, int nbPoints, int maxRessourceLevel, Partie partie)
        {
            InitializeComponent();
            Partie = partie;
            MaxRessourceLevel = maxRessourceLevel;
            Joueur = joueur;
            points = nbPoints;
            PointsInitial = nbPoints;
            RessourceInitial = new metier.Ressource(joueur.LevelRessource.Charronite, joueur.LevelRessource.BarilNucleaire, joueur.LevelRessource.AlainDollars);
            // Pour binder les texts box
            this.DataContext = this;


        }

        private void btnPlusCharronite_Click(object sender, RoutedEventArgs e)
        {
            if (Joueur.LevelRessource.Charronite < MaxRessourceLevel && Points > 0)
            {
                Joueur.LevelRessource.Charronite += 1;
                Points--;
                btnMoinsCharronite.Opacity = 1;
                btnMoinsCharronite.IsEnabled = true;
            }
            if (Joueur.LevelRessource.Charronite == 3)
            {
                btnPlusCharronite.Opacity = 0.25;
                btnPlusCharronite.IsEnabled = false;
            }
            verifierTerminer();
        }

        private void btnMoinsCharronite_Click(object sender, RoutedEventArgs e)
        {
            if (Joueur.LevelRessource.Charronite > RessourceInitial.Charronite && Points < PointsInitial)
            {
                Joueur.LevelRessource.Charronite -= 1;
                Points += 1;
                btnPlusCharronite.Opacity = 1;
                btnPlusCharronite.IsEnabled = true;
            }
            if (Joueur.LevelRessource.Charronite == RessourceInitial.Charronite)
            {
                btnMoinsCharronite.Opacity = 0.25;
                btnMoinsCharronite.IsEnabled = false;
            }
            verifierTerminer();
        }

        private void btnPlusBaril_Click(object sender, RoutedEventArgs e)
        {
            if (Joueur.LevelRessource.BarilNucleaire < MaxRessourceLevel && Points > 0)
            {
                Joueur.LevelRessource.BarilNucleaire += 1;
                Points -= 1;
                btnMoinsBaril.Opacity = 1;
                btnMoinsBaril.IsEnabled = true;
            }
            if (Joueur.LevelRessource.BarilNucleaire == 3)
            {
                btnPlusBaril.Opacity = 0.25;
                btnPlusBaril.IsEnabled = false;
            }
            verifierTerminer();
        }

        private void btnMoinsBaril_Click(object sender, RoutedEventArgs e)
        {
            if (Joueur.LevelRessource.BarilNucleaire > RessourceInitial.BarilNucleaire && Points < PointsInitial)
            {
                Joueur.LevelRessource.BarilNucleaire -= 1;
                Points += 1;
                btnPlusBaril.Opacity = 1;
                btnPlusBaril.IsEnabled = true;
            }
            if (Joueur.LevelRessource.BarilNucleaire == RessourceInitial.BarilNucleaire)
            {
                btnMoinsBaril.Opacity = 0.25;
                btnMoinsBaril.IsEnabled = false;
            }
            verifierTerminer();
        }
        private void btnPlusAlain_Click(object sender, RoutedEventArgs e)
        {
            if (Joueur.LevelRessource.AlainDollars < MaxRessourceLevel && Points > 0)
            {
                Joueur.LevelRessource.AlainDollars += 1;
                Points -= 1;
                btnMoinsAlain.Opacity = 1;
                btnMoinsAlain.IsEnabled = true;
            }
            if (Joueur.LevelRessource.AlainDollars == 3)
            {
                btnPlusAlain.Opacity = 0.25;
                btnPlusAlain.IsEnabled = false;
            }
            verifierTerminer();
        }
        private void btnMoinsAlain_Click(object sender, RoutedEventArgs e)
        {
            if (Joueur.LevelRessource.AlainDollars > RessourceInitial.AlainDollars && Points < PointsInitial)
            {
                Joueur.LevelRessource.AlainDollars -= 1;
                Points += 1;
                btnPlusAlain.Opacity = 1;
                btnPlusAlain.IsEnabled = true;
            }
            if (Joueur.LevelRessource.AlainDollars == RessourceInitial.AlainDollars)
            {
                btnMoinsAlain.Opacity = 0.25;
                btnMoinsAlain.IsEnabled = false;
            }
            verifierTerminer();
        }

        private void verifierTerminer()
        {
            if (Points == 0)
            {
                btnOk.IsEnabled = true;
                btnOk.Opacity = 1;
                btnPlusAlain.IsEnabled = false;
                btnPlusBaril.IsEnabled = false;
                btnPlusCharronite.IsEnabled = false;
                btnPlusAlain.Opacity = 0.25;
                btnPlusBaril.Opacity = 0.25;
                btnPlusCharronite.Opacity = 0.25;
            }
            else
            {
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.25;
                if (Joueur.LevelRessource.AlainDollars < MaxRessourceLevel)
                {
                    btnPlusAlain.IsEnabled = true;
                    btnPlusAlain.Opacity = 1;
                }
                if (Joueur.LevelRessource.BarilNucleaire < MaxRessourceLevel)
                {
                    btnPlusBaril.IsEnabled = true;
                    btnPlusBaril.Opacity = 1;
                }
                if (Joueur.LevelRessource.Charronite < MaxRessourceLevel)
                {
                    btnPlusCharronite.IsEnabled = true;
                    btnPlusCharronite.Opacity = 1;
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Partie.FermerEcranRessource();
        }
    }
}
