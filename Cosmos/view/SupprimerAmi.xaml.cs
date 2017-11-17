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
    /// Logique d'interaction pour SupprimerAmi.xaml
    /// </summary>
    public partial class SupprimerAmi : UserControl
    {
        public ListeAmis ListeAmis { get; set; }
        private Utilisateur AmiASupprimer { get; set; }
        public SupprimerAmi(ListeAmis listeAmis, Utilisateur amiASupprimer )
        {
            InitializeComponent();

            ListeAmis = listeAmis;

            AmiASupprimer = amiASupprimer;

            CreerMessageAvertissement();
            
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            List<Utilisateur> lstARemove = new List<Utilisateur>();
            foreach (Utilisateur ami in ListeAmis.Main.LstAmis)
            {
                if (ami.IdUtilisateur == AmiASupprimer.IdUtilisateur)
                {
                    lstARemove.Add(ami);
                }
            }

            foreach (Utilisateur ami in lstARemove)
            {
                ListeAmis.Main.LstAmis.Remove(ami);
            }

            ListeAmis.AfficherListeAmis();

            MySqlUtilisateurService.DeleteAmi(ListeAmis.Main.UtilisateurConnecte.IdUtilisateur, AmiASupprimer.IdUtilisateur);

            ListeAmis.Main.grdMain.Children.Remove(this);
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ListeAmis.Main.grdMain.Children.Remove(this);
        }

        private void CreerMessageAvertissement()
        {
            StringBuilder message = new StringBuilder();

            message.Append("Êtes-vous sûr de vouloir supprimer '").Append(AmiASupprimer.Nom).Append("' de votre liste d'amis?");

            txblAvertissement.Text = message.ToString();
        }
    }
}
