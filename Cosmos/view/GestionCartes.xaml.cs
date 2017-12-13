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
using System.Diagnostics;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour GestionCartes.xaml
    /// </summary>
    public partial class GestionCartes : UserControl
    {
        public MainWindow Main;
        private List<Carte> LstCartesCollection = MySqlCarteService.RetrieveAllCard();
        public List<Label> LstNomExemplaire { get; set; }
        public List<Label> LstQteExemplaire { get; set; }
        public List<Button> LstBtnPlus { get; set; }
        public List<Button> LstBtnMoins { get; set; }
        public List<Button> LstBtnEnlever { get; set; }
        public GestionCartes(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            LstCartesCollection = TrierOrdreAlphabetique("croissant");

            GenererListeCartes();

        }

        private void btnMenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            bool deckComplet = false;
            foreach (Deck deckUtilisateur in Main.UtilisateurConnecte.DecksUtilisateurs)
            {
                if (deckUtilisateur.EstChoisi)
                {
                    if (deckUtilisateur.CartesDuDeck.Count == 50)
                    {
                        deckComplet = true;
                    }
                }
            }

            if (deckComplet)
                Main.EcranMenuPrincipal();
            else
                MessageBox.Show("Votre deck choisi n'est pas complet. Assurez-vous d'avoir 50 cartes.", "Deck incomplet", MessageBoxButton.OK);
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
                imgCarte.ToolTip = "Cliquez ici pour agrandir l'image.";
                imgCarte.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/cartes/" + uneCarte.Nom + ".jpg"));
                imgCarte.Width = 160;
                imgCarte.Height = 190;
                imgCarte.Name = "img" + uneCarte.IdCarte.ToString();
                imgCarte.HorizontalAlignment = HorizontalAlignment.Center;
                imgCarte.Cursor = Cursors.Hand;
                imgCarte.PreviewMouseLeftButtonUp += ZoomerCarte;
                imgCarte.Opacity = 0.6;

                foreach (Exemplaire exemplaireUtilisateur in Main.UtilisateurConnecte.ExemplairesUtilisateurs)
                {
                    if (uneCarte.Nom == exemplaireUtilisateur.Carte.Nom)
                    {
                        imgCarte.Opacity = 1;
                        qte = exemplaireUtilisateur.Quantite;
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
            imgZoomCarte.Name = "z" + image.Name;

            if (image.Opacity == 1)
            {
                imgZoomCarte.Cursor = Cursors.Hand;
                imgZoomCarte.PreviewMouseLeftButtonUp += ImgZoomCarte_PreviewMouseLeftButtonUp;
            }
        }

        private void ImgZoomCarte_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Image imageZoom = (Image)sender;
            bool estPresente = false;
            int onglet = tbcDecksUtilisateurs.SelectedIndex;

            Exemplaire exemplaireAAjouter = RetrouverCarte(Convert.ToInt32(imgZoomCarte.Name.Substring(4)));
            int position = RetrouverPositionExemplaire(exemplaireAAjouter);

            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count > 0 && tbcDecksUtilisateurs.SelectedIndex <= Main.UtilisateurConnecte.DecksUtilisateurs.Count - 1 && LstNomExemplaire.Count >= 0)
            {
                if (Main.UtilisateurConnecte.DecksUtilisateurs[onglet].CartesDuDeck.Count < 50)
                {
                    estPresente = TrouverExemplaireDansDeck(exemplaireAAjouter);

                    if (estPresente)
                    {
                        if (exemplaireAAjouter.Quantite - Convert.ToInt32(LstQteExemplaire[position].Content.ToString()) > 0 && Convert.ToInt32(LstQteExemplaire[position].Content.ToString()) < 3 )
                        {
                            if (Convert.ToInt32(LstQteExemplaire[position].Content.ToString()) == 2)
                            {
                                rectZoom.Visibility = Visibility.Hidden;
                                imgZoomCarte.Visibility = Visibility.Hidden;
                                imgZoomCarte.PreviewMouseLeftButtonUp -= ImgZoomCarte_PreviewMouseLeftButtonUp;
                            }
                            MySqlDeckService.UpdateQteExemplaireDeck(Main.UtilisateurConnecte.DecksUtilisateurs[onglet], exemplaireAAjouter, Convert.ToInt32(LstQteExemplaire[position].Content.ToString()) + 1);
                        }
                    }
                    else
                        MySqlDeckService.InsertExemplaireDeck(Main.UtilisateurConnecte.DecksUtilisateurs[onglet], exemplaireAAjouter, 1);
                    RefreshAll();
                }    
            }
        }

        private int RetrouverPositionExemplaire(Exemplaire exemplaireAAjouter)
        {
            if (LstNomExemplaire != null)
            {
                for (int i = 0; i < LstNomExemplaire.Count; i++)
                {
                    if (LstNomExemplaire[i].Content.ToString() == exemplaireAAjouter.Carte.Nom)
                        return i;
                }
            }

            return 0;
        }

        private bool TrouverExemplaireDansDeck(Exemplaire exemplaireAAjouter)
        {
            foreach (Label nomCarte in LstNomExemplaire)
            {
                if (nomCarte.Content.ToString() == exemplaireAAjouter.Carte.Nom)
                    return true;
            }

            return false;
        }

        private Exemplaire RetrouverCarte(int idCarte)
        {
            foreach (Exemplaire exemplaire in Main.UtilisateurConnecte.ExemplairesUtilisateurs)
            {
                if (exemplaire.Carte.IdCarte == idCarte)
                {
                    return exemplaire;
                }
            }
            return null;
        }

        private void rectZoom_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectZoom.Visibility = Visibility.Hidden;
            imgZoomCarte.Visibility = Visibility.Hidden;
            imgZoomCarte.PreviewMouseLeftButtonUp -= ImgZoomCarte_PreviewMouseLeftButtonUp;
        }

        private List<Carte> RetirerCartes()
        {
            List<Carte> lstARemove = new List<Carte>();
            List<Carte> lstTemp = MySqlCarteService.RetrieveAllCard();

            foreach (Carte carteCollection in lstTemp)
            {
                foreach (Exemplaire carteUtilisateur in Main.UtilisateurConnecte.ExemplairesUtilisateurs)
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
            LstNomExemplaire = new List<Label>();
            LstQteExemplaire = new List<Label>();
            LstBtnPlus = new List<Button>();
            LstBtnMoins = new List<Button>();
            LstBtnEnlever = new List<Button>();

            // Pour chaque exemplaire dans les decks, on instancie la grille avec leur nom et leur quantité, ainsi qu'avec des boutons pour incrémenter de 1, soustraire de 1 et enlever complètement l'exemplaire
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
                Binding myBinding = new Binding("Quantite");
                myBinding.Source = e;
                lblQuantite.SetBinding(Label.ContentProperty , myBinding);
                lblQuantite.FontWeight = FontWeights.Bold;
                lblQuantite.FontSize = 15;
                lblQuantite.HorizontalAlignment = HorizontalAlignment.Center;

                #region Boutons
                Button btnPlus = new Button();
                btnPlus.ToolTip = "Ajouter un exemplaire de cette carte au deck.";
                btnPlus.Click += new RoutedEventHandler(btnPlus_Click);
                btnPlus.Width = 30;
                btnPlus.Height = 30;
                btnPlus.Content = "+";
                btnPlus.FontSize = 18;
                btnPlus.FontWeight = FontWeights.Bold;
                btnPlus.HorizontalAlignment = HorizontalAlignment.Left;

                Button btnMoins = new Button();
                btnMoins.ToolTip = "Enlever un exemplaire de cette carte au deck.";
                btnMoins.Click += new RoutedEventHandler(btnMoins_Click);
                btnMoins.Width = 30;
                btnMoins.Height = 30;
                btnMoins.Content = "-";
                btnMoins.FontSize = 18;
                btnMoins.FontWeight = FontWeights.Bold;
                btnMoins.HorizontalAlignment = HorizontalAlignment.Center;

                Button btnEnlever = new Button();
                btnEnlever.ToolTip = "Enlever tous les exemplaires de cette carte.";
                btnEnlever.Click += new RoutedEventHandler(btnEnlever_Click);
                btnEnlever.Width = 30;
                btnEnlever.Height = 30;
                btnEnlever.Content = "x";
                btnEnlever.FontSize = 18;
                btnEnlever.FontWeight = FontWeights.Bold;
                btnEnlever.Foreground = Brushes.Red;
                btnEnlever.HorizontalAlignment = HorizontalAlignment.Right;
                #endregion


                switch (posOnglet)
                {
                    case 0:
                        grdDeck1.RowDefinitions.Add(rwdRangee);
                        grdDeck1.Children.Add(lblCarte);
                        grdDeck1.Children.Add(lblQuantite);
                        grdDeck1.Children.Add(btnPlus);
                        grdDeck1.Children.Add(btnMoins);
                        grdDeck1.Children.Add(btnEnlever);
                        break;
                    case 1:
                        grdDeck2.RowDefinitions.Add(rwdRangee);
                        grdDeck2.Children.Add(lblCarte);
                        grdDeck2.Children.Add(lblQuantite);
                        grdDeck2.Children.Add(btnPlus);
                        grdDeck2.Children.Add(btnMoins);
                        grdDeck2.Children.Add(btnEnlever);
                        break;
                    case 2:
                        grdDeck3.RowDefinitions.Add(rwdRangee);
                        grdDeck3.Children.Add(lblCarte);
                        grdDeck3.Children.Add(lblQuantite);
                        grdDeck3.Children.Add(btnPlus);
                        grdDeck3.Children.Add(btnMoins);
                        grdDeck3.Children.Add(btnEnlever);
                        break;
                }

                // Placer label Nom Carte
                Grid.SetRow(lblCarte, compteurRow);
                Grid.SetColumn(lblCarte, 0);
                // Placer label Quantité
                Grid.SetRow(lblQuantite, compteurRow);
                Grid.SetColumn(lblQuantite, 1);
                // Placer Bouton +
                Grid.SetRow(btnPlus, compteurRow);
                Grid.SetColumn(btnPlus, 2);
                // Placer Bouton -
                Grid.SetRow(btnMoins, compteurRow);
                Grid.SetColumn(btnMoins, 2);
                // Placer Bouton x
                Grid.SetRow(btnEnlever, compteurRow);
                Grid.SetColumn(btnEnlever, 2);

                compteurRow++;

                LstNomExemplaire.Add(lblCarte);
                LstQteExemplaire.Add(lblQuantite);
                LstBtnPlus.Add(btnPlus);
                LstBtnMoins.Add(btnMoins);
                LstBtnEnlever.Add(btnEnlever);
            }
        }

        #region Boutons Modification quantité dans deck
        private void btnEnlever_Click(object sender, RoutedEventArgs e)
        {
            Button BoutonEnlever = sender as Button;

            int index = LstBtnEnlever.IndexOf(BoutonEnlever);
            int onglet = tbcDecksUtilisateurs.SelectedIndex;

            Exemplaire exemplaireAUpdate = RetrouverExemplaireAUpdate(LstNomExemplaire[index].Content.ToString());

            MySqlDeckService.DeleteExemplaireDeck(Main.UtilisateurConnecte.DecksUtilisateurs[onglet].IdDeck, exemplaireAUpdate.IdExemplaire);

            RefreshAll();
        }

        private void btnMoins_Click(object sender, RoutedEventArgs e)
        {
            Button BoutonMoins = sender as Button;

            int index = LstBtnMoins.IndexOf(BoutonMoins);
            int onglet = tbcDecksUtilisateurs.SelectedIndex;
            int qte;

            Exemplaire exemplaireAUpdate = RetrouverExemplaireAUpdate(LstNomExemplaire[index].Content.ToString());

            if ((int)LstQteExemplaire[index].Content <= 3 && exemplaireAUpdate.Quantite - (int)LstQteExemplaire[index].Content >= 0 && Main.UtilisateurConnecte.DecksUtilisateurs[0].CartesDuDeck.Count <= 50)
            {
                qte = (int)LstQteExemplaire[index].Content - 1;

                if (qte > 0)
                    MySqlDeckService.UpdateQteExemplaireDeck(Main.UtilisateurConnecte.DecksUtilisateurs[onglet], exemplaireAUpdate, qte);
                else
                    MySqlDeckService.DeleteExemplaireDeck(Main.UtilisateurConnecte.DecksUtilisateurs[onglet].IdDeck, exemplaireAUpdate.IdExemplaire);                
            }

            RefreshAll();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Button BoutonPlus = sender as Button;

            int index = LstBtnPlus.IndexOf(BoutonPlus);
            int onglet = tbcDecksUtilisateurs.SelectedIndex;
            int qte;

            Exemplaire exemplaireAUpdate = RetrouverExemplaireAUpdate(LstNomExemplaire[index].Content.ToString());

            if ((int)LstQteExemplaire[index].Content < 3 && exemplaireAUpdate.Quantite - (int)LstQteExemplaire[index].Content > 0 && Main.UtilisateurConnecte.DecksUtilisateurs[onglet].CartesDuDeck.Count < 50)
            {
                qte = (int)LstQteExemplaire[index].Content + 1;
                LstQteExemplaire[index].Content = qte;

                MySqlDeckService.UpdateQteExemplaireDeck(Main.UtilisateurConnecte.DecksUtilisateurs[onglet], exemplaireAUpdate, qte );
            }

            RefreshAll();

        }

        private Exemplaire RetrouverExemplaireAUpdate(string nomCarte)
        {
            foreach (Exemplaire exemplaire in Main.UtilisateurConnecte.ExemplairesUtilisateurs)
            {
                if (exemplaire.Carte.Nom == nomCarte)
                    return exemplaire;
            }

            return null;
        }
        #endregion

        #region Boutons en bas des decks
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

        #region Créer nouveau deck
        private void btnCreerDeck_Click(object sender, RoutedEventArgs e)
        {
            string nomDefaut = TrouverNomParDefaut();
            bool estChoisi = false;

            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 0)
                estChoisi = true;
            else
                estChoisi = VerifierDeckChoisi();

            MySqlDeckService.Insert(nomDefaut, Main.UtilisateurConnecte.IdUtilisateur, estChoisi);

            RefreshAll();
        }

        private bool VerifierDeckChoisi()
        {
            foreach (Deck deckUtilisateur in Main.UtilisateurConnecte.DecksUtilisateurs)
            {
                if (deckUtilisateur.EstChoisi)
                    return false;
            }
            return true;
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
        #endregion

        private void btnChoisirDeck_Click(object sender, RoutedEventArgs e)
        {
            foreach (Deck deckUtilisateur in Main.UtilisateurConnecte.DecksUtilisateurs)
            {
                if (deckUtilisateur.IdDeck == Main.UtilisateurConnecte.DecksUtilisateurs[tbcDecksUtilisateurs.SelectedIndex].IdDeck)
                    deckUtilisateur.EstChoisi = true;
                else
                    deckUtilisateur.EstChoisi = false;

                MySqlDeckService.UpdateChoixDeck(Main.UtilisateurConnecte.IdUtilisateur, deckUtilisateur);

            }

            RefreshAll();

        }

        private void tbcDecksUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAll();
        }
        #endregion

        #region Refraichir la fenêtre
        public void RefreshAll()
        {
            ViderGrids();

            Main.UtilisateurConnecte.DecksUtilisateurs = MySqlDeckService.RetrieveAllUserDeck(Main.UtilisateurConnecte.IdUtilisateur);
            
            if (tbcDecksUtilisateurs.SelectedIndex <= Main.UtilisateurConnecte.DecksUtilisateurs.Count - 1)
            {
                int pos = 0;

                if (tbcDecksUtilisateurs.SelectedIndex != -1)
                {
                    pos = tbcDecksUtilisateurs.SelectedIndex;
                }
                CreerLabels(Main.UtilisateurConnecte.DecksUtilisateurs[pos], pos);
                RefreshQteTotal();
            }
            else
                lblQteTotale.Content = "0/50";

            RefreshOnglets();
            RefreshBtnSupprimer();
            RefreshBtnCreer();
            RefreshBtnChoisirDeck();
            RefreshBtnRenommer();
        }

        private void RefreshQteTotal()
        {
            int Qte = 0;
            foreach (Label qte in LstQteExemplaire)
            {
                Qte += Convert.ToInt32(qte.Content.ToString());
            }

            lblQteTotale.Content = Qte.ToString() + "/50";
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

        private void RefreshBtnChoisirDeck()
        {
            if (Main.UtilisateurConnecte.DecksUtilisateurs.Count == 0 || tbcDecksUtilisateurs.SelectedIndex > Main.UtilisateurConnecte.DecksUtilisateurs.Count - 1 || Main.UtilisateurConnecte.DecksUtilisateurs[tbcDecksUtilisateurs.SelectedIndex].EstChoisi || Main.UtilisateurConnecte.DecksUtilisateurs[tbcDecksUtilisateurs.SelectedIndex].CartesDuDeck.Count < 50)
            {
                btnChoisirDeck.Opacity = 0.6;
                btnChoisirDeck.IsEnabled = false;
                btnChoisirDeck.Cursor = Cursors.Arrow;
            }
            else
            {
                btnChoisirDeck.Opacity = 1;
                btnChoisirDeck.IsEnabled = true;
                btnChoisirDeck.Cursor = Cursors.Hand;
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
                        break;
                    case 1:
                        tbiEmplacement2.Header = Main.UtilisateurConnecte.DecksUtilisateurs[i].Nom;
                        break;
                    case 2:
                        tbiEmplacement3.Header = Main.UtilisateurConnecte.DecksUtilisateurs[i].Nom;
                        break;
                } 
            }
        }

        private void ViderGrids()
        {
            List<Label> lstLblARemove = new List<Label>();
            List<Button> lstBtnARemove = new List<Button>();

            foreach (object item in grdDeck1.Children)
            {
                if (item.GetType() == typeof(Label))
                {
                    if (((Label)item).Name != "lblCarte1" && ((Label)item).Name != "lblQte1")
                        lstLblARemove.Add(((Label)item));
                }
                else if (item.GetType() == typeof(Button))
                {
                    lstBtnARemove.Add(((Button)item));
                }  
            }

            foreach (Label aRemove in lstLblARemove)
            {
                grdDeck1.Children.Remove(aRemove);
            }

            foreach (Button aRemove in lstBtnARemove)
            {
                grdDeck1.Children.Remove(aRemove);
            }

            lstLblARemove = new List<Label>();
            lstBtnARemove = new List<Button>();

            foreach (object item in grdDeck2.Children)
            {
                if (item.GetType() == typeof(Label))
                {
                    if (((Label)item).Name != "lblCarte2" && ((Label)item).Name != "lblQte2")
                        lstLblARemove.Add(((Label)item));
                }
                else if (item.GetType() == typeof(Button))
                {
                    lstBtnARemove.Add(((Button)item));
                }
            }

            foreach (Label aRemove in lstLblARemove)
            {
                grdDeck2.Children.Remove(aRemove);
            }

            foreach (Button aRemove in lstBtnARemove)
            {
                grdDeck2.Children.Remove(aRemove);
            }

            lstLblARemove = new List<Label>();
            lstBtnARemove = new List<Button>();

            foreach (object item in grdDeck3.Children)
            {
                if (item.GetType() == typeof(Label))
                {
                    if (((Label)item).Name != "lblCarte3" && ((Label)item).Name != "lblQte3")
                        lstLblARemove.Add(((Label)item));
                }
                else if (item.GetType() == typeof(Button))
                {
                    lstBtnARemove.Add(((Button)item));
                }
            }

            foreach (Label aRemove in lstLblARemove)
            {
                grdDeck3.Children.Remove(aRemove);
            }

            foreach (Button aRemove in lstBtnARemove)
            {
                grdDeck3.Children.Remove(aRemove);
            }
        }

        #endregion

        private void btnAide_Click(object sender, RoutedEventArgs e)
        {
            Main.Topmost = false;
            String fileName = "GuideUtilisateurCosmos.pdf";
            Process.Start(fileName);
            
        }
    }
}
