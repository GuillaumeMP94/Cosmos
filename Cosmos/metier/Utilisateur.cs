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
        public string Nom { get; set; }
        //TODO: Pertinence des champs
        public int NiveauDebloque { get; set; }
        public string Courriel { get; set; }
        public string MotDePasse { get; set; }
        public string Salt { get; set; }
        //public list<Deck> DecksUtilisateurs
        public List<Carte> CartesUtilisateurs;
        #endregion
        #region Constructeur
        public Utilisateur(string nom)
            :base()
        {
            Nom = nom;
            NiveauDebloque = 1;
        }
        public Utilisateur(string nom, int niveau)
            :base()
		{
            Nom = nom;
            NiveauDebloque = niveau;
		}
        public Utilisateur(string nom, int niveau, string courriel): this(nom,niveau)
        {
            Courriel = courriel;
        }
        public Utilisateur(string nom, int niveau, string courriel,string motDePasse) : this(nom, niveau, courriel)
        {
            MotDePasse = motDePasse;
        }
        public Utilisateur(string nom, int niveau, string courriel, string motDePasse, string salt) : this(nom, niveau, courriel, motDePasse)
        {
            Salt = salt;
        }
        #endregion
    }
}
