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
        }

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (Main.ValiderChampSaisi(txbPseudo.Text) == txbPseudo.Text && Main.ValiderChampSaisi(passbPassword.Password) == passbPassword.Password)
            {
                if (txbPseudo.Text.Length > 0 && passbPassword.Password.Length > 0)
                {
                    if (txbPseudo.Text.Length >= 3 && passbPassword.Password.Length >=5 )
                    {
                        Utilisateur unUtilsateur = MySqlUtilisateurService.RetrieveByNom(txbPseudo.Text);
                        if (unUtilsateur != null)
                        {
                            if (unUtilsateur.MotDePasse == passbPassword.Password)
                            {
                                Main.UtilisateurConnecte = unUtilsateur;
                                Main.EcranMenuPrincipal();
                            }
                            else
                            {
                                AfficherMessageErreur("infoInvalide");
                            }
                        }
                        else
                        {
                            AfficherMessageErreur("infoInvalide");
                        }
                    }
                    else
                    {
                        AfficherMessageErreur("tropCourt");
                    }
                }
                else
                {
                    AfficherMessageErreur("aucuneSaisie");
                }
            }
            else
            {
                AfficherMessageErreur("charInvalide");
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
    }
}
