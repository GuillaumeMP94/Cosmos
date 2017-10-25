using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe pour les informations de l'utilisateur.
    /// </summary>
    public class Utilisateur : Joueur
    {
        #region Propriétés
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        //TODO: Pertinence des champs
        public int NiveauDebloque { get; set; }
        public string Courriel { get; set; }
        public string MotDePasse { get; set; }
        public string Salt { get; set; }
        public List<Deck> DecksUtilisateurs { get; set; }
        public List<Carte> CartesUtilisateurs { get; set; }
        #endregion
        #region Constructeur
        public Utilisateur(int idUtilisateur, string nom)
            :base()
        {
            IdUtilisateur = idUtilisateur;
            Nom = nom;
            NiveauDebloque = 1;
        }
        public Utilisateur(int idUtilisateur, string nom, string courriel) : this(idUtilisateur,nom)
		{
            Courriel = courriel;
            
		}
        public Utilisateur(int idUtilisateur,string nom, string courriel, int niveau) : this(idUtilisateur, nom, courriel)
        {
            NiveauDebloque = niveau;
        }
        public Utilisateur(int idUtilisateur, string nom, string courriel, int niveau, string motDePasse) : this(idUtilisateur, nom, courriel, niveau)
        {
            MotDePasse = motDePasse;
        }
        public Utilisateur(int idUtilisateur, string nom, string courriel, int niveau, string motDePasse, string salt) : this(idUtilisateur, nom, courriel, niveau, motDePasse)
        {
            Salt = salt;
        }
        #endregion
        public override void Reinitialiser()
        {
            PointDeBlindage = 25;
            Active = new Ressource(0, 0, 0);
            Level = new Ressource(1, 1, 1);
            foreach (Deck unDeck in this.DecksUtilisateurs)
            {
                if (unDeck.EstChoisi == true)
                {
                    DeckAJouer = new Deck(unDeck);
                }
            }
        }
    }
}
