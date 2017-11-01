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
using Cosmos.metier;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour CreationCompte.xaml
    /// </summary>
    public partial class CreationCompte : UserControl
    {
        public MainWindow Main { get; set; }
        public CreationCompte(MainWindow main)
        {
            InitializeComponent();

            Main = main;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            if (estInformationValide())
            {
                Utilisateur neoUtilisateur = new Utilisateur(txbPseudo.Text, txbCourriel.Text, passbPassword.Password, "qwerty");
                MySqlUtilisateurService.Insert(neoUtilisateur);
                Main.UtilisateurConnecte = MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text);
                Main.EcranMenuPrincipal();
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranConnexion();
        }

        #region ValiderInfoSaisies
        private void AfficherMessageErreur(string typeErreur)
        {
            txbPseudo.Text = "";
            passbPassword.Password = "";
            passbConfirmPassword.Password = "";
            txbCourriel.Text = "";

            switch (typeErreur)
            {
                case "infoInvalide":
                    txblErreur.Text = "Le nom d'utilisateur, le mot de passe ou le courriel est invalide.";
                    break;
                case "tropCourt":
                    txblErreur.Text = "Le nom d'utilisateur ou le mot de passe est trop court.";
                    break;
                case "infoExistante":
                    txblErreur.Text = "Le nom d'utilisateur ou le courriel est déjà utilisé.";
                    break;
                case "motsPasseDifferents":
                    txblErreur.Text = "Le mot de passse de confirmation est différent du mot de passe.";
                    break;
            }

            txblErreur.Visibility = Visibility.Visible;
        }

        private bool ValiderInformations()
        {
            return (Main.ValiderChampSaisi(txbPseudo.Text) == txbPseudo.Text
                && Main.ValiderChampSaisi(passbPassword.Password) == passbPassword.Password
                && Main.ValiderChampSaisi(passbConfirmPassword.Password) == passbConfirmPassword.Password
                && Main.estCourrielValide(txbCourriel.Text));
        }

        private bool estInformationValide()
        {
            if (ValiderInformations())
            {
                if (txbPseudo.Text.Length >= 3 && passbPassword.Password.Length >= 5)
                {
                    if (MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text) == null && MySqlUtilisateurService.RetrieveByCourriel(txbCourriel.Text) == null)
                    {
                        if (passbPassword.Password == passbConfirmPassword.Password)
                        {
                            return true;
                        }
                        else
                        {
                            AfficherMessageErreur("motsPasseDifferents");
                            return false;
                        }
                    }
                    else
                    {
                        AfficherMessageErreur("infoExistante");
                        return false;
                    }
                }
                else
                {
                    AfficherMessageErreur("tropCourt");
                    return false;
                }
            }
            else
            {
                AfficherMessageErreur("infoInvalide");
                return false;
            }
        }
        #endregion

        #region ActiverBoutonCreer
        private void ActiverBoutonCreer()
        {
            if (estBoutonActif())
            {
                btnCreer.IsEnabled = true;
                btnCreer.Opacity = 1;
            }
            else
            {
                btnCreer.IsEnabled = false;
                btnCreer.Opacity = 0.25;
            }
        }

        private void txbPseudo_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiverBoutonCreer();
        }

        private void txbCourriel_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiverBoutonCreer();
        }

        private void passbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ActiverBoutonCreer();
        }

        private void passbConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ActiverBoutonCreer();
        }

        private bool estBoutonActif()
        {
            return ((txbPseudo.Text != "" || txbPseudo.IsVisible == false) && (txbCourriel.Text != "" || txbCourriel.IsVisible == false) && (passbPassword.Password != "" || passbPassword.IsVisible == false) && (passbConfirmPassword.Password != "" || passbConfirmPassword.IsVisible == false));
        }
        #endregion

        #region BindingEnter
        private void txbPseudo_KeyUp(object sender, KeyEventArgs e)
        {
            if (estBoutonActif() && e.Key.ToString() == "Return")
            {
                btnCreer_Click(sender, e);
            }
        }

        private void passbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (estBoutonActif() && e.Key.ToString() == "Return")
            {
                btnCreer_Click(sender, e);
            }
        }

        private void passbConfirmPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (estBoutonActif() && e.Key.ToString() == "Return")
            {
                btnCreer_Click(sender, e);
            }
        }

        private void txbCourriel_KeyUp(object sender, KeyEventArgs e)
        {
            if (estBoutonActif() && e.Key.ToString() == "Return")
            {
                btnCreer_Click(sender, e);
            }
        }
        #endregion
    }
}
