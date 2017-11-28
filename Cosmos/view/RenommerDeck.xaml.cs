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
using Cosmos.accesBD;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour RenommerDeck.xaml
    /// </summary>
    public partial class RenommerDeck : UserControl
    {
        public GestionCartes GestionCartes { get; set; }
        private string DeckARenommer { get; set; }
        public RenommerDeck(GestionCartes gestionCartes, string nomAncienDeck)
        {
            InitializeComponent();

            GestionCartes = gestionCartes;

            DeckARenommer = nomAncienDeck;
        }

        private void btnRenommer_Click(object sender, RoutedEventArgs e)
        {
            string nomVerifie = GestionCartes.Main.VerifierTexte(txbNomDeck.Text);

            MySqlDeckService.UpdateNomDeck(GestionCartes.Main.UtilisateurConnecte.IdUtilisateur, DeckARenommer, nomVerifie);

            GestionCartes.Main.grdMain.Children.Remove(this);
            
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            GestionCartes.Main.grdMain.Children.Remove(this);
        }
    }
}
