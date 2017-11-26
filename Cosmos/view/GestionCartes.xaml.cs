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
        private List<Exemplaire> LstExemplairesUtilisateur;
        public GestionCartes(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            LstExemplairesUtilisateur = MySqlCarteService.RetrieveExemplairesUser(Main.UtilisateurConnecte.IdUtilisateur);


            LstCartesCollection = TrierOrdreAlphabetique("croissant");

            GenererListeCartes();

            GenererDecks();
        }

        private void btnMenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }

        private void GenererListeCartes()
        {
            int compteurRow = 0;
            int compteurColumn = 0;
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
                imgCarte.Opacity = 0.6;

                foreach (Exemplaire carteUtilisateur in LstExemplairesUtilisateur)
                {
                    if (uneCarte.Nom == carteUtilisateur.Carte.Nom)
                    {
                        imgCarte.Opacity = 1;
                        qte = carteUtilisateur.Quantite;
                    }
                    
                }
              
                grdLesCartes.Children.Add(imgCarte);
                Grid.SetRow(imgCarte, compteurRow);
                Grid.SetColumn(imgCarte, compteurColumn);
                #endregion

                #region LabelQte
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
                Grid.SetRow(qteCarte, compteurRow);
                Grid.SetColumn(qteCarte, compteurColumn);
                #endregion

                if (compteurColumn == 0)
                    imgCarte.Margin = new Thickness(5);

                compteurColumn++;

                if (compteurColumn == 5)
                {
                    compteurRow++;
                    compteurColumn = 0;
                }
                qte = 0;
            }
        }

        private void GenererDecks()
        {
            List<Deck> lstDecksUtilisateur = MySqlDeckService.RetrieveAllUserDeck(Main.UtilisateurConnecte.IdUtilisateur);

            if (lstDecksUtilisateur != null)
            {
                for (int i = 0; i < lstDecksUtilisateur.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            tbiEmplacement1.Header = lstDecksUtilisateur[i].Nom;
                            CreerLabels(lstDecksUtilisateur[i], i);
                            break;
                        case 1:
                            tbiEmplacement2.Header = lstDecksUtilisateur[i].Nom;
                            CreerLabels(lstDecksUtilisateur[i], i);
                            break;
                        case 2:
                            tbiEmplacement3.Header = lstDecksUtilisateur[i].Nom;
                            CreerLabels(lstDecksUtilisateur[i], i);
                            break;
                    }
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
            List<Carte> lstARemove = new List<Carte>();
            List<Carte> lstTemp = MySqlCarteService.RetrieveAllCard();

            foreach (Carte carteCollection in lstTemp)
            {
                foreach (Exemplaire carteUtilisateur in LstExemplairesUtilisateur)
                {
                    if (carteCollection.IdCarte == carteUtilisateur.Carte.IdCarte)
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

        /// <summary>
        /// Fonction crée et qui affiche les labels contenant les informations d'un deck, soit les exemplaires et leurs quantités
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="posOnglet"></param>
        private void CreerLabels(Deck deck, int posOnglet)
        {
            int compteurRow = 1;
            List<Exemplaire> lstExemplairesDeck = MySqlCarteService.RetrieveExemplairesDeckUser(deck.Nom, Main.UtilisateurConnecte.IdUtilisateur);
            switch (posOnglet)
            {
                case 0:
                    foreach (Exemplaire exemplaire in lstExemplairesDeck)
                    {
                        RowDefinition rwdRangee = new RowDefinition();
                        rwdRangee.Height = GridLength.Auto;
                        grdDeck1.RowDefinitions.Add(rwdRangee);

                        Label lblCarte = new Label();
                        lblCarte.Content = exemplaire.Carte.Nom;
                        lblCarte.FontWeight = FontWeights.Bold;
                        lblCarte.FontSize = 15;
                        lblCarte.HorizontalAlignment = HorizontalAlignment.Center;

                        grdDeck1.Children.Add(lblCarte);
                        Grid.SetRow(lblCarte, compteurRow);
                        Grid.SetColumn(lblCarte, 0);

                        Label lblQuantite = new Label();
                        lblQuantite.Content = exemplaire.Quantite;
                        lblQuantite.FontWeight = FontWeights.Bold;
                        lblQuantite.FontSize = 15;
                        lblQuantite.HorizontalAlignment = HorizontalAlignment.Center;

                        grdDeck1.Children.Add(lblQuantite);
                        Grid.SetRow(lblQuantite, compteurRow);
                        Grid.SetColumn(lblQuantite, 1);

                        compteurRow++;
                    }
                    break;
                case 1:
                    foreach (Exemplaire exemplaire in lstExemplairesDeck)
                    {
                        RowDefinition rwdRangee = new RowDefinition();
                        rwdRangee.Height = GridLength.Auto;
                        grdDeck2.RowDefinitions.Add(rwdRangee);

                        Label lblCarte = new Label();
                        lblCarte.Content = exemplaire.Carte.Nom;
                        lblCarte.FontWeight = FontWeights.Bold;
                        lblCarte.FontSize = 15;
                        lblCarte.HorizontalAlignment = HorizontalAlignment.Center;

                        grdDeck2.Children.Add(lblCarte);
                        Grid.SetRow(lblCarte, compteurRow);
                        Grid.SetColumn(lblCarte, 0);

                        Label lblQuantite = new Label();
                        lblQuantite.Content = exemplaire.Quantite;
                        lblQuantite.FontWeight = FontWeights.Bold;
                        lblQuantite.FontSize = 15;
                        lblQuantite.HorizontalAlignment = HorizontalAlignment.Center;

                        grdDeck2.Children.Add(lblQuantite);
                        Grid.SetRow(lblQuantite, compteurRow);
                        Grid.SetColumn(lblQuantite, 1);

                        compteurRow++;
                    }
                    break;
                case 2:
                    foreach (Exemplaire exemplaire in lstExemplairesDeck)
                    {
                        RowDefinition rwdRangee = new RowDefinition();
                        rwdRangee.Height = GridLength.Auto;
                        grdDeck3.RowDefinitions.Add(rwdRangee);

                        Label lblCarte = new Label();
                        lblCarte.Content = exemplaire.Carte.Nom;
                        lblCarte.FontWeight = FontWeights.Bold;
                        lblCarte.FontSize = 15;
                        lblCarte.HorizontalAlignment = HorizontalAlignment.Center;

                        grdDeck3.Children.Add(lblCarte);
                        Grid.SetRow(lblCarte, compteurRow);
                        Grid.SetColumn(lblCarte, 0);

                        Label lblQuantite = new Label();
                        lblQuantite.Content = exemplaire.Quantite;
                        lblQuantite.FontWeight = FontWeights.Bold;
                        lblQuantite.FontSize = 15;
                        lblQuantite.HorizontalAlignment = HorizontalAlignment.Center;

                        grdDeck3.Children.Add(lblQuantite);
                        Grid.SetRow(lblQuantite, compteurRow);
                        Grid.SetColumn(lblQuantite, 1);

                        compteurRow++;
                    }
                    break;
            }
        }

		private void btnSupprimer_Click(object sender, RoutedEventArgs e)
		{
			if (ValiderSuppression())
			{
				string nomDeck = ((TabItem)tbcDecksUtilisateurs.SelectedItem).Header.ToString();
				MySqlDeckService.Delete(Main.UtilisateurConnecte.IdUtilisateur, nomDeck);
			}
		}

		private void btnRenommer_Click(object sender, RoutedEventArgs e)
		{
			TabItem tbiTest = (TabItem)tbcDecksUtilisateurs.SelectedItem;
			MessageBox.Show(tbiTest.Header.ToString());
		}

		private bool ValiderSuppression() {

			if (MessageBox.Show("Êtes vous sur de vouloir supprimer votre deck?", "Suppression de deck", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				return true;
			return false;
		}

	}
}
