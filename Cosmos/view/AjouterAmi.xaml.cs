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
    /// Logique d'interaction pour AjouterAmi.xaml
    /// </summary>
    public partial class AjouterAmi : UserControl
    {
        public ListeAmis ListeAmis { get; set; }
        private List<Utilisateur> LstAmisPossibles { get; set; }
        public AjouterAmi(ListeAmis listeAmis)
        {
            InitializeComponent();

            ListeAmis = listeAmis;

            LstAmisPossibles = retrouverAmisPossibles();

            AjouterChoix();
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ListeAmis.Main.grdMain.Children.Remove(this);
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            Utilisateur amiAAjouter = MySqlUtilisateurService.RetrieveByNom(cboAjout.SelectedItem.ToString());

            MySqlUtilisateurService.InsertAmi(ListeAmis.Main.UtilisateurConnecte.IdUtilisateur, amiAAjouter.IdUtilisateur);

            ListeAmis.Main.LstAmis.Add(amiAAjouter);

            ListeAmis.AfficherListeAmis();

            ListeAmis.Main.grdMain.Children.Remove(this);
        }

        /// <summary>
        /// Fonction qui retourne tous les utilisateurs qui ne sont pas déjà dans la liste d'amis.
        /// </summary>
        /// <returns>La liste des ajouts possibles dans la liste d'amis</returns>
        private List<Utilisateur> retrouverAmisPossibles()
        {
            List<Utilisateur> lstAmis = MySqlUtilisateurService.RetrieveAmis(ListeAmis.Main.UtilisateurConnecte.IdUtilisateur);
            List<Utilisateur> lstUtilisateurs = MySqlUtilisateurService.RetrieveAll();
            List<Utilisateur> lstARemove = new List<Utilisateur>();

            foreach (Utilisateur utilisateur in lstUtilisateurs)
            {
                foreach (Utilisateur ami in lstAmis)
                {
                    if (ami.IdUtilisateur == utilisateur.IdUtilisateur )
                    {
                        lstARemove.Add(utilisateur);
                    }
                }
                if (ListeAmis.Main.UtilisateurConnecte.IdUtilisateur == utilisateur.IdUtilisateur)
                {
                    lstARemove.Add(utilisateur);
                }
            }

            foreach (Utilisateur utilisateur in lstARemove)
            {
                lstUtilisateurs.Remove(utilisateur);
            }

            return lstUtilisateurs;
        }

        private void AjouterChoix()
        {
            foreach (Utilisateur ami in LstAmisPossibles)
            {
                cboAjout.Items.Add(ami.Nom);
            }
        }

        private void cboAjout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAjouter.Opacity = 1;
            btnAjouter.IsEnabled = true;
        }
    }
}
