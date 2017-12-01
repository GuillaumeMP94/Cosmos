using Cosmos.metier;
using Cosmos.accesBD;
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
    /// Logique d'interaction pour SupprimerDeck.xaml
    /// </summary>
    public partial class SupprimerDeck : UserControl
    {
        public GestionCartes GestionCartes { get; set; }
        private string DeckASupprimer { get; set; }
        public SupprimerDeck(GestionCartes gestionCartes, string deckASupprimer)
        {
            InitializeComponent();

            GestionCartes = gestionCartes;

            DeckASupprimer = deckASupprimer;

            CreerMessageSuppression();
        }

        private void CreerMessageSuppression()
        {
            StringBuilder message = new StringBuilder();

            message.Append(" Êtes vous sur de vouloir supprimer votre deck '").Append(DeckASupprimer).Append("'?");
            txblMessage.Text = message.ToString();
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            GestionCartes.Main.grdMain.Children.Remove(this);
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            MySqlDeckService.Delete(GestionCartes.Main.UtilisateurConnecte.IdUtilisateur, DeckASupprimer);
            GestionCartes.RefreshAll();

            for (int i = 3; i > GestionCartes.Main.UtilisateurConnecte.DecksUtilisateurs.Count; i--)
            {
                ((TabItem)GestionCartes.tbcDecksUtilisateurs.Items[i-1]).Header = "Emplacement vide";
            }

            GestionCartes.Main.grdMain.Children.Remove(this);
        }
    }
}
