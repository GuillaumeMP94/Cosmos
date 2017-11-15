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
        public MainWindow Main { get; set; }
        public ModifierAmi(MainWindow main, Utilisateur ami)
        {
            InitializeComponent();

            Main = main;

            lblAmi.Content = ami.Nom;
            txbNote.Text = MySqlUtilisateurService.RetrieveNoteAmiByID(Main.UtilisateurConnecte.IdUtilisateur, ami.IdUtilisateur);

        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Main.grdMain.Children.Remove(this);
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            string note = VerifierNote(txbNote.Text);

            txbNote.Text = note;

        }

        private string VerifierNote(string note)
        {
            string temp = note;
            char fuckingguillement = '\\';
            string calissdemarde = fuckingguillement + "\'";

            temp = temp.Replace("\\", "\\\\" ).Replace("'", calissdemarde);

            return temp;
        }


    }
}
