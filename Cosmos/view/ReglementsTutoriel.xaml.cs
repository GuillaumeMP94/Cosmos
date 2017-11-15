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
    /// Logique d'interaction pour ReglementsTutoriel.xaml
    /// </summary>
    public partial class ReglementsTutoriel : UserControl
    {
        public MainWindow Main { get; set; }
        public ReglementsTutoriel(MainWindow main)
        {
            InitializeComponent();

            Main = main;
 
        }

        private void btnTutoriel_Click(object sender, RoutedEventArgs e)
        {
            if (MySqlCarteService.RetrieveAllUserCard(Main.UtilisateurConnecte.IdUtilisateur).Count == 0)
            {
                MySqlCarteService.InsertNewJoueurCard(Main.UtilisateurConnecte);
                MessageBox.Show("Félicitation pour avoir complété le tutoriel! Vous venez de débloquer vos cartes!");
            }
            else
            {
                MessageBox.Show("Vous avez déjà complété le tutoriel.");
            }

        }

        private void btnMenuPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranMenuPrincipal();
        }
    }
}
