using Cosmos.metier;
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
    /// Logique d'interaction pour ChoixCible.xaml
    /// </summary>
    public partial class ChoixCible : UserControl
    {
        public Partie Partie { get; set; }
        public TableDeJeu TableEnCours { get; set; }
        public List<int> Choix { get; set; }
        public int Cible { get; set; }
        public int NbCible { get; set; }

        public ChoixCible(List<int> choix, int cible , int nbCible, TableDeJeu tableDeJeu , Partie laPartie)
        {
            InitializeComponent();
            Choix = choix;
            Cible = cible;
            NbCible = nbCible;
            TableEnCours = tableDeJeu;
            Partie = laPartie;
            AfficherChoix();
        }

        private void AfficherChoix()
        {
            if (Cible == 0 || Cible == 1 || Cible == 10 || Cible == 9 || Cible == 12 || Cible == 13 || Cible == 18 || Cible == 19)
                recChoixJoueur2.Visibility = Visibility.Visible;
            if (Cible == 11 || Cible == 9 || Cible == 0 || Cible == 2 || Cible == 12 || Cible == 14 || Cible == 18 || Cible == 20)
                recChoixJoueur1.Visibility = Visibility.Visible;
            if (Cible == 0 || Cible == 1 || Cible == 3 || Cible == 4 || Cible == 15 || Cible == 16 || Cible == 18 || Cible == 19)
            {
                // TODO: Changer .Defense
                if (TableEnCours.ChampConstructionsJ2.Champ1 != null)
                    recBatiment1J2.Visibility = Visibility.Visible;
                if (TableEnCours.ChampConstructionsJ2.Champ2 != null)
                    recBatiment2J2.Visibility = Visibility.Visible;
                if (TableEnCours.ChampConstructionsJ2.Champ3 != null)
                    recBatiment3J2.Visibility = Visibility.Visible;
                if (TableEnCours.ChampConstructionsJ2.Champ4 != null)
                    recBatiment4J2.Visibility = Visibility.Visible;
            }
            if (Cible == 0 || Cible == 2 || Cible == 3 || Cible == 5 || Cible == 15 || Cible == 17 || Cible == 18 || Cible == 20)
            {
                // TODO: Changer Defense
                if (TableEnCours.ChampConstructionsJ1.Champ1 != null)
                    recBatiment1J1.Visibility = Visibility.Visible;
                if (TableEnCours.ChampConstructionsJ1.Champ2 != null)
                    recBatiment2J1.Visibility = Visibility.Visible;
                if (TableEnCours.ChampConstructionsJ1.Champ3 != null)
                    recBatiment3J1.Visibility = Visibility.Visible;
                if (TableEnCours.ChampConstructionsJ1.Champ4 != null)
                    recBatiment4J1.Visibility = Visibility.Visible;
            }
            if (Cible == 0 || Cible == 1 || Cible == 6 || Cible == 7 || Cible == 12 || Cible == 13 || Cible == 15 || Cible == 16)
            {
                if (TableEnCours.ChampBatailleUnitesJ2.Champ1 != null)
                    recUnite1J2.Visibility = Visibility.Visible;
                if (TableEnCours.ChampBatailleUnitesJ2.Champ2 != null)
                    recUnite2J2.Visibility = Visibility.Visible;
                if (TableEnCours.ChampBatailleUnitesJ2.Champ3 != null)
                    recUnite3J2.Visibility = Visibility.Visible;
            }
            if (Cible == 0 || Cible == 2 || Cible == 6 || Cible == 8 || Cible == 12 || Cible == 14 || Cible == 15 || Cible == 17)
            {
                if (TableEnCours.ChampBatailleUnitesJ1.Champ1 != null)
                    recUnite1J1.Visibility = Visibility.Visible;
                if (TableEnCours.ChampBatailleUnitesJ1.Champ2 != null)
                    recUnite2J1.Visibility = Visibility.Visible;
                if (TableEnCours.ChampBatailleUnitesJ1.Champ3 != null)
                    recUnite3J1.Visibility = Visibility.Visible;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Partie.FermerEcranChoixCible();
        }

        private void recChoixJoueur2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(200))
            {
                Choix.Remove(200);
                imgChoixJoueur2.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(200);
                    imgChoixJoueur2.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recChoixJoueur1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(100))
            {
                Choix.Remove(100);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(100);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recMainJoueur1_Click(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void recDeckJoueur1_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void recRecyclageJoueur1_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void recMainJoueur2_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void recDeckJoueur2_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void recRecyclageJoueur2_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void recBatiment1J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(211))
            {
                Choix.Remove(211);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(211);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment2J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(212))
            {
                Choix.Remove(212);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(212);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment3J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(213))
            {
                Choix.Remove(213);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(213);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment4J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(214))
            {
                Choix.Remove(214);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(214);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment1J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(111))
            {
                Choix.Remove(111);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(111);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment2J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(112))
            {
                Choix.Remove(112);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(112);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment3J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(113))
            {
                Choix.Remove(113);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(113);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recBatiment4J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(114))
            {
                Choix.Remove(114);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(114);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recUnite1J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(221))
            {
                Choix.Remove(221);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(221);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recUnite2J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(222))
            {
                Choix.Remove(222);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(222);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recUnite3J2_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(223))
            {
                Choix.Remove(223);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(223);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recUnite1J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(121))
            {
                Choix.Remove(121);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(121);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recUnite2J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(122))
            {
                Choix.Remove(122);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(122);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }

        private void recUnite3J1_Click(object sender, MouseButtonEventArgs e)
        {
            if (Choix.Contains(123))
            {
                Choix.Remove(123);
                imgChoixJoueur1.Visibility = Visibility.Hidden;
                btnOk.IsEnabled = false;
                btnOk.Opacity = 0.5;
            }
            else
            {
                if (Choix.Count < NbCible)
                {
                    Choix.Add(123);
                    imgChoixJoueur1.Visibility = Visibility.Visible;
                    if (Choix.Count == NbCible)
                    {
                        btnOk.IsEnabled = true;
                        btnOk.Opacity = 1;
                    }
                }
            }
        }
    }
}
