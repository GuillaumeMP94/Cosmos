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
        public MainWindow Main;
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

        public void GenererDecks()
        {
            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count > 0 )
            {
                btnRenommer.Opacity = 1;
                btnRenommer.IsEnabled = true;
                btnRenommer.Cursor = Cursors.Hand;

                btnSupprimer.Opacity = 1;
                btnSupprimer.IsEnabled = true;
                btnSupprimer.Cursor = Cursors.Hand;

                if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 3)
                {
                    btnCreerDeck.Opacity = 0.6;
                    btnCreerDeck.IsEnabled = false;
                    btnCreerDeck.Cursor = Cursors.Arrow;
                }
                
                if (Main.UtilisateurConnecte.DecksUtilisateurs[0].CartesDuDeck.Count == 50)
                {
                    btnAjouterCarte.Opacity = 0.6;
                    btnAjouterCarte.IsEnabled = false;
                    btnAjouterCarte.Cursor = Cursors.Arrow;
                }

                RefreshOnglets();
            }
        }
        #region TrierCartes
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
        #endregion 

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

        /// <summary>
        /// Fonction crée et qui affiche les labels contenant les informations d'un deck, soit les exemplaires et leurs quantités
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="posOnglet"></param>
        private void CreerLabels(Deck deck, int posOnglet)
        {
            int compteurRow = 1;
            List<Exemplaire> lstExemplairesDeck = MySqlCarteService.RetrieveExemplairesDeckUser(deck.Nom, Main.UtilisateurConnecte.IdUtilisateur);

            foreach (Exemplaire e in lstExemplairesDeck)
            {
                RowDefinition rwdRangee = new RowDefinition();
                rwdRangee.Height = GridLength.Auto;

                Label lblCarte = new Label();
                lblCarte.Content = e.Carte.Nom;
                lblCarte.FontWeight = FontWeights.Bold;
                lblCarte.FontSize = 15;
                lblCarte.HorizontalAlignment = HorizontalAlignment.Center;

                Label lblQuantite = new Label();
                lblQuantite.Content = e.Quantite;
                lblQuantite.FontWeight = FontWeights.Bold;
                lblQuantite.FontSize = 15;
                lblQuantite.HorizontalAlignment = HorizontalAlignment.Center;

                switch (posOnglet)
                {
                    case 0:
                        grdDeck1.RowDefinitions.Add(rwdRangee);
                        grdDeck1.Children.Add(lblCarte);
                        grdDeck1.Children.Add(lblQuantite);
                        break;
                    case 1:
                        grdDeck2.RowDefinitions.Add(rwdRangee);
                        grdDeck2.Children.Add(lblCarte);
                        grdDeck2.Children.Add(lblQuantite);
                        break;
                    case 2:
                        grdDeck3.RowDefinitions.Add(rwdRangee);
                        grdDeck3.Children.Add(lblCarte);
                        grdDeck3.Children.Add(lblQuantite);
                        break;
                }

                Grid.SetRow(lblCarte, compteurRow);
                Grid.SetColumn(lblCarte, 0);

                Grid.SetRow(lblQuantite, compteurRow);
                Grid.SetColumn(lblQuantite, 1);
                compteurRow++;

            }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
		{
            Main.ContenuAddModifSupp = new SupprimerDeck(this, ((TabItem)tbcDecksUtilisateurs.SelectedItem).Header.ToString());

            Main.grdMain.Children.Add(Main.ContenuAddModifSupp);

		}

		private void btnRenommer_Click(object sender, RoutedEventArgs e)
		{
            Main.ContenuAddModifSupp = new RenommerDeck(this, ((TabItem)tbcDecksUtilisateurs.SelectedItem).Header.ToString());

            Main.grdMain.Children.Add(Main.ContenuAddModifSupp);

		}

		private bool ValiderSuppression() {

			if (MessageBox.Show("Êtes vous sur de vouloir supprimer votre deck?", "Suppression de deck", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				return true;
			return false;
		}

        private void btnCreerDeck_Click(object sender, RoutedEventArgs e)
        {
            string nomDefaut = TrouverNomParDefaut();
            bool estChoisi = false;

            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 0)
            {
                estChoisi = true;
            }

            MySqlDeckService.Insert(nomDefaut, Main.UtilisateurConnecte.IdUtilisateur, estChoisi);

            RefreshAll();

        }

        private string TrouverNomParDefaut()
        {
            string nomDeck = "Deck A";

            foreach (Deck leDeck in Main.UtilisateurConnecte.DecksUtilisateurs)
            {
                if (leDeck.Nom.ToLower() == "deck c" && nomDeck.ToLower() != "deck b")
                {
                    nomDeck = "Deck A";
                }
                else if (leDeck.Nom.ToLower() == "deck b" && nomDeck.ToLower() != "deck a")
                {
                    nomDeck = "Deck C";
                }
                else if (leDeck.Nom.ToLower() == "deck a" && nomDeck.ToLower() != "deck c")
                {
                    nomDeck = "Deck B";
                }
            }

            return nomDeck;
        }

        private void btnAjouterCarte_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbcDecksUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAll();
        }


        public void RefreshAll()
        {
            Main.UtilisateurConnecte.DecksUtilisateurs = MySqlDeckService.RetrieveAllUserDeck(Main.UtilisateurConnecte.IdUtilisateur);

            RefreshOnglets();
            RefreshBtnSupprimer();
            RefreshBtnCreer();
            RefreshBtnAjouter();
            RefreshBtnRenommer();
        }

        private void RefreshBtnRenommer()
        {
            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 0 || tbcDecksUtilisateurs.SelectedIndex > Main.UtilisateurConnecte.DecksUtilisateurs.Count - 1)
            {
                btnRenommer.Opacity = 0.6;
                btnRenommer.IsEnabled = false;
                btnRenommer.Cursor = Cursors.Arrow;
            }
            else
            {
                btnRenommer.Opacity = 1;
                btnRenommer.IsEnabled = true;
                btnRenommer.Cursor = Cursors.Hand;
            }
        }

        private void RefreshBtnAjouter()
        {
            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 0 || tbcDecksUtilisateurs.SelectedIndex > Main.UtilisateurConnecte.DecksUtilisateurs.Count - 1)
            {
                btnAjouterCarte.Opacity = 0.6;
                btnAjouterCarte.IsEnabled = false;
                btnAjouterCarte.Cursor = Cursors.Arrow;
            }
            else
            {
                btnAjouterCarte.Opacity = 1;
                btnAjouterCarte.IsEnabled = true;
                btnAjouterCarte.Cursor = Cursors.Hand;
            }
        }

        private void RefreshBtnCreer()
        {
            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 3)
            {
                btnCreerDeck.Opacity = 0.6;
                btnCreerDeck.IsEnabled = false;
                btnCreerDeck.Cursor = Cursors.Arrow;
            }
            else
            {
                btnCreerDeck.Opacity = 1;
                btnCreerDeck.IsEnabled = true;
                btnCreerDeck.Cursor = Cursors.Hand;
            }
        }

        private void RefreshBtnSupprimer()
        {
            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 0 || tbcDecksUtilisateurs.SelectedIndex > Main.UtilisateurConnecte.DecksUtilisateurs.Count - 1)
            {
                btnSupprimer.Opacity = 0.6;
                btnSupprimer.IsEnabled = false;
                btnSupprimer.Cursor = Cursors.Arrow;
            }
            else
            {
                btnSupprimer.Opacity = 1;
                btnSupprimer.IsEnabled = true;
                btnSupprimer.Cursor = Cursors.Hand;
            }
        }

        private void RefreshOnglets()
        {
            for (int i = 0; i < Main.UtilisateurConnecte.DecksUtilisateurs.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        tbiEmplacement1.Header = Main.UtilisateurConnecte.DecksUtilisateurs[i].Nom;
                        CreerLabels(Main.UtilisateurConnecte.DecksUtilisateurs[i], i);
                        break;
                    case 1:
                        tbiEmplacement2.Header = Main.UtilisateurConnecte.DecksUtilisateurs[i].Nom;
                        CreerLabels(Main.UtilisateurConnecte.DecksUtilisateurs[i], i);
                        break;
                    case 2:
                        tbiEmplacement3.Header = Main.UtilisateurConnecte.DecksUtilisateurs[i].Nom;
                        CreerLabels(Main.UtilisateurConnecte.DecksUtilisateurs[i], i);
                        break;
                }
            }
        }

    }
}
