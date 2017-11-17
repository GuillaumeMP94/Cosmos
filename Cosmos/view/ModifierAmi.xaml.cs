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
    /// Logique d'interaction pour ModifierAmi.xaml
    /// </summary>
    public partial class ModifierAmi : UserControl
    {
        public ListeAmis ListeAmis { get; set; }
        private Utilisateur Ami { get; set; }
        public ModifierAmi(ListeAmis listeAmi, Utilisateur ami)
        {
            InitializeComponent();

            ListeAmis = listeAmi;

            Ami = ami;

            lblAmi.Content = Ami.Nom;
            txbNote.Text = MySqlUtilisateurService.RetrieveNoteAmiByID(ListeAmis.Main.UtilisateurConnecte.IdUtilisateur, Ami.IdUtilisateur);
        }



        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ListeAmis.Main.grdMain.Children.Remove(this);
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            string note = VerifierNote(txbNote.Text);

            MySqlUtilisateurService.UpdateNoteAmi(ListeAmis.Main.UtilisateurConnecte.IdUtilisateur, Ami.IdUtilisateur, note);

            ListeAmis.Main.grdMain.Children.Remove(this);

        }

        private string VerifierNote(string note)
        {
            string temp = "";

            for (int i = 0; i < note.Length; i++)
            {    
                if (note[i] == 92 || note[i] == (char)39)
                    temp += (char)92; 

                temp += note[i];
            }

            return temp;
        }

        private void txbNote_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                btnModifier_Click(sender, e);
            }

            if(e.Key.ToString() == "Escape")
            {
                btnAnnuler_Click(sender, e);
            }
        }
    }
}
