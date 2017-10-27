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
            if (txbPseudo.Text.Length > 0 && passbPassword.Password.Length > 0 && passbConfirmPassword.Password.Length > 0 && txbCourriel.Text.Length > 0)
            {
                if (ValiderInformations())
                {
                    if (txbPseudo.Text.Length >=3 && passbPassword.Password.Length >= 5)
                    {
                        if (MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text) == null && MySqlUtilisateurService.RetrieveByCourriel(txbCourriel.Text) == null)
                        {
                            if (passbPassword.Password == passbConfirmPassword.Password)
                            {
                                Utilisateur neoUtilisateur = new Utilisateur(txbPseudo.Text, txbCourriel.Text, passbPassword.Password, "qwerty");
                                MySqlUtilisateurService.Insert(neoUtilisateur);
                                Main.UtilisateurConnecte = MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text);
                                Main.EcranMenuPrincipal();
                            }
                            else
                            {
                                AfficherMessageErreur("motsPasseDifferents");
                            }
                        }
                        else
                        {
                            AfficherMessageErreur("infoExistante");
                        }
                    }
                    else
                    {
                        AfficherMessageErreur("tropCourt");
                    }    
                }
                else
                {
                    AfficherMessageErreur("infoInvalide");
                }
            }
            else
            {
                AfficherMessageErreur("aucuneSaisie");
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Main.EcranConnexion();
        }

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
                case "charInvalide":
                    txblErreur.Text = "Le nom d'utilisateur, le mot de passe ou le courriel contient des caractères invalides.";
                    break;
                case "tropCourt":
                    txblErreur.Text = "Le nom d'utilisateur ou le mot de passe est trop court.";
                    break;
                case "aucuneSaisie":
                    txblErreur.Text = "Veuillez saisir toutes les informations.";
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
            if (Main.ValiderChampSaisi(txbPseudo.Text) == txbPseudo.Text 
                && Main.ValiderChampSaisi(passbPassword.Password) == passbPassword.Password 
                && Main.ValiderChampSaisi(passbConfirmPassword.Password) == passbConfirmPassword.Password
                && Main.estCourrielValide(txbCourriel.Text))
            {
                return true;
            }
            else
            {
                return false;
            }


            
        }
    }
}
