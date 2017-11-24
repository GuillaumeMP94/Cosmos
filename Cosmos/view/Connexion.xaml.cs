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
using System.Threading;
using Cosmos.metier;

namespace Cosmos.view
{
    /// <summary>
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : UserControl
    {
        public MainWindow Main { get; set; }
        public Connexion(MainWindow main)
        {
            InitializeComponent();

            Main = main;
            FocusManager.SetFocusedElement(Main, txbPseudo);
            Keyboard.Focus(txbPseudo);
        }

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (estInformationValide())
            {
                Utilisateur unUtilsateur = MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text);
                Main.UtilisateurConnecte = unUtilsateur;
                Main.LstAmis = MySqlUtilisateurService.RetrieveAmis(Main.UtilisateurConnecte.IdUtilisateur);
                Main.EcranMenuPrincipal();
            }
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Main.QuitterMain();
        }

        private void btnCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranCreerCompte();
        }

        private void btnPasswordOublie_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranRecuperation();
        }

        #region ValidationInfoUtilisateur
        private void AfficherMessageErreur(string typeErreur)
        {
            txbPseudo.Text = "";
            passbPassword.Password = "";

            switch (typeErreur)
            {
                case "infoInvalide":
                    txblErreur.Text = "Le nom d'utilisateur ou le mot de passe est invalide.";
                    break;

                case "charInvalide":
                    txblErreur.Text = "Le nom d'utilisateur ou le mot de passe contient des caractères invalides.";
                    break;
                case "tropCourt":
                    txblErreur.Text = "Le nom d'utilisateur ou le mot de passe est trop court.";
                    break;
                case "aucuneSaisie":
                    txblErreur.Text = "Veuillez saisir toutes les informations.";
                    break;
            }

            txblErreur.Visibility = Visibility.Visible;
        }

        private bool estInformationValide()
        {
            if (Main.ValiderChampSaisi(txbPseudo.Text) == txbPseudo.Text && Main.ValiderChampSaisi(passbPassword.Password) == passbPassword.Password)
            {
                if (txbPseudo.Text.Length >= 3 && passbPassword.Password.Length >= 5)
                {
                    Utilisateur unUtilsateur = MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text);
                    if (unUtilsateur != null)
                    {
                        if (unUtilsateur.MotDePasse == passbPassword.Password)
                        {
                            return true;
                        }
                        else
                        {
                            AfficherMessageErreur("infoInvalide");
                            return false;
                        }
                    }
                    else
                    {
                        AfficherMessageErreur("infoInvalide");
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

        #region ActiverBoutonConnexion
        private void ActiverBoutonConnexion()
        {
            if (estBoutonActif())
            {
                btnConnexion.Opacity = 1;
                btnConnexion.IsEnabled = true;
            }
            else
            {
                btnConnexion.Opacity = 0.25;
                btnConnexion.IsEnabled = false;
            }
        }

        private void txbPseudo_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiverBoutonConnexion();
        }

        private void passbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ActiverBoutonConnexion();
        }

        private bool estBoutonActif()
        {
            return (txbPseudo.Text != "" || txbPseudo.IsVisible == false) && (passbPassword.Password != "" || passbPassword.IsVisible == false);
        }
        #endregion

        #region BindingEnterBouton
        private void passbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (estBoutonActif() && e.Key.ToString() == "Return")
            {
                btnConnexion_Click(sender, e);
            }
        }

        private void txbPseudo_KeyUp(object sender, KeyEventArgs e)
        {
            if (estBoutonActif() && e.Key.ToString() == "Return")
            {
                btnConnexion_Click(sender, e);
            }
        }
        #endregion
    }
}
