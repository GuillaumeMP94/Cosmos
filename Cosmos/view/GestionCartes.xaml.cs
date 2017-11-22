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
using Cosmos.metier;
using Cosmos.accesBD;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour GestionCartes.xaml
    /// </summary>
    public partial class GestionCartes : UserControl
    {
        private MainWindow Main;
        private List<Carte> LstCartesCollection = MySqlCarteService.RetrieveAllCard();
        private List<Carte> LstCartesNonAcquises;
        public GestionCartes(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            LstCartesNonAcquises = RetirerCartes();

            LstCartesCollection = TrierOrdreAlphabetique("croissant");

            GenererListeCartes();

            

        }

        private void btnMenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void GenererListeCartes()
        {
            int compteurX = 0;
            int compteurY = 0;
            int qte = 0;
            foreach (Carte uneCarte in LstCartesCollection)
            {
                #region ImageCarte
                Image imgCarte = new Image();
                imgCarte.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + uneCarte.Nom + ".jpg"));
                imgCarte.Width = 160;
                imgCarte.Height = 190;
                imgCarte.HorizontalAlignment = HorizontalAlignment.Center;
                imgCarte.Cursor = Cursors.Hand;
                imgCarte.PreviewMouseLeftButtonUp += ZoomerCarte;

                //On veut griser les cartes non acquises
                if (LstCartesNonAcquises != null)
                {
                    foreach (Carte carteAEnlever in LstCartesNonAcquises)
                    {
                        if (carteAEnlever.Nom == uneCarte.Nom)
                        {
                            imgCarte.Opacity = 0.6;
                        }
                    }
                }
                

                grdLesCartes.Children.Add(imgCarte);
                Grid.SetRow(imgCarte, compteurX);
                Grid.SetColumn(imgCarte, compteurY);
                #endregion

                #region LabelQte
                qte = MySqlCarteService.RetrieveQuantiteExemplaire(uneCarte.IdCarte, Main.UtilisateurConnecte.IdUtilisateur);
                Label qteCarte = new Label();
                qteCarte.Content = qte.ToString();
                qteCarte.Background = Brushes.Wheat;
                qteCarte.HorizontalAlignment = HorizontalAlignment.Center;
                qteCarte.VerticalAlignment = VerticalAlignment.Bottom;
                qteCarte.FontSize = 12;
                qteCarte.FontWeight = FontWeights.Bold;
                qteCarte.HorizontalContentAlignment = HorizontalAlignment.Center;
                qteCarte.Width = 30;
                qteCarte.Height = 30;
                qteCarte.Margin = new Thickness(0, 0, 0, 10);

                grdLesCartes.Children.Add(qteCarte);
                Grid.SetRow(qteCarte, compteurX);
                Grid.SetColumn(qteCarte, compteurY);
                #endregion

                if (compteurY == 0)
                    imgCarte.Margin = new Thickness(5);

                compteurY++;

                if (compteurY == 5)
                {
                    compteurX++;
                    compteurY = 0;
                }
            }
        }


        private List<Carte> TrierOrdreAlphabetique(string ordre)
        {
            List<Carte> lstTemp = LstCartesCollection;
            if (ordre == "croissant")
                return lstTemp.OrderBy(carte => carte.Nom).ToList();
            else
                return lstTemp.OrderByDescending(carte => carte.Nom).ToList();
        }

        private List<Carte> TrierTypeCarte()
        {
            List<Carte> lstTemp = LstCartesCollection;

            return lstTemp.OrderBy(carte => carte.Type()).ToList();
        }

        private void ZoomerCarte(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;

            rectZoom.Visibility = Visibility.Visible;

            imgZoomCarte.Source = image.Source;
            imgZoomCarte.Visibility = Visibility.Visible;
        }

        private void rectZoom_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectZoom.Visibility = Visibility.Hidden;
            imgZoomCarte.Visibility = Visibility.Hidden;
        }

        private List<Carte> RetirerCartes()
        {
            List<Carte> lstCarteUtilisateur = MySqlCarteService.RetrieveAllUserCards(Main.UtilisateurConnecte.IdUtilisateur);
            List<Carte> lstARemove = new List<Carte>();
            List<Carte> lstTemp = MySqlCarteService.RetrieveAllCard();

            foreach (Carte carteCollection in lstTemp)
            {
                foreach (Carte carteUtilisateur in lstCarteUtilisateur)
                {
                    if (carteCollection.IdCarte == carteUtilisateur.IdCarte)
                    {
                        lstARemove.Add(carteCollection);
                    }
                }
            }

            foreach (Carte carteUtilisateur in lstARemove)
            {
                lstTemp.Remove(carteUtilisateur);
            }

            return lstTemp;
        }

        private void cboChoixTri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstCartesNonAcquises = RetirerCartes();
            grdLesCartes.Children.Clear();
            switch (cboChoixTri.SelectedIndex)
            {
                case 0:    
                    LstCartesCollection = TrierOrdreAlphabetique("croissant");
                    break;
                case 1:
                    LstCartesCollection = TrierOrdreAlphabetique("décroissant");
                    break;
                case 2:
                    LstCartesCollection = TrierTypeCarte();
                    
                    break;
            }
            GenererListeCartes();
        }
    }
}
