﻿using Cosmos.metier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.accesBD
{
    /// <summary>
    /// Accès à la BD pour la classe Utilisateur.
    /// </summary>
    class MySqlUtilisateurService
    {
        private static MySqlConnexion ConnectionBD { get; set; }
        /// <summary>
        /// Fonction qui retourne tous les utilisateurs.
        /// </summary>
        /// <returns>Liste de tous les utilisateurs sans leur decks et leur cartes.</returns>
        public static List<Utilisateur> RetrieveAll()
        {
            //List<Carte>
            //List<Deck>
            List<Utilisateur> lstResultat = new List<Utilisateur>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query("SELECT * FROM Utilisateurs");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Utilisateur((int)dr["idUtilisateur"]
                                               , (string)dr["nom"]
                                               , (string)dr["courriel"]
                                               )
                               );
                // Pas besoin des cartes.
            }
            return lstResultat;
        }

        

        /// <summary>
        /// Fonction qui retourne un utilisateur.
        /// </summary>
        /// <param name="query">Requête à effectuer sur la BD</param>
        /// <returns>Un utilisateur avec ses decks et ses cartes.</returns>
        private static Utilisateur Retrieve(string query)
        {
            Utilisateur resultat = null;
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow drResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];
            if (dtResultat.Rows.Count > 0)
            {
                drResultat = dtResultat.Rows[0];

                resultat = new Utilisateur((int)drResultat["idUtilisateur"]
                                         , (string)drResultat["nom"]
                                         , (string)drResultat["courriel"]
                                         , (int)drResultat["idNiveau"]
                                         , (string)drResultat["motDePasse"]
                                         , (string)drResultat["salt"]
                                         );

                // On va chercher les cartes
                resultat.ExemplairesUtilisateurs = MySqlCarteService.RetrieveExemplairesUser((int)drResultat["idUtilisateur"]);
                // On va chercher les decks
                resultat.DecksUtilisateurs = MySqlDeckService.RetrieveAllUserDeck((int)drResultat["idUtilisateur"]);
            }
            return resultat;
        }
        /// <summary>
        /// Fonction qui construit la commande SQL pour la requête par ID et qui la passe ensuite à Retrieve
        /// </summary>
        /// <param name="pIdUtilisateur"></param>
        /// <returns>Retourne l'utilisateur associé au id en paramêtre.</returns>
        public static Utilisateur RetrieveById(int pIdUtilisateur)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Utilisateurs WHERE idUtilisateur = ").Append(pIdUtilisateur.ToString());

            return Retrieve(query.ToString());

        }
        /// <summary>
        /// Fonction qui construit la commande SQL pour la requête par nom et qui la passe ensuite à Retrieve
        /// </summary>
        /// <param name="pNom"></param>
        /// <returns>Retourne l'utilisateur associé au nom en paramêtre.</returns>
        public static Utilisateur RetrieveByNom(string pNom)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Utilisateurs WHERE nom = '").Append(pNom).Append("'");

            return Retrieve(query.ToString());
            
        }
        /// <summary>
        /// Fonction qui construit la commande SQL pour la requête par courriel et qui la passe ensuite à Retrieve
        /// </summary>
        /// <param name="pCourriel"></param>
        /// <returns>Retourne l'utilisateur associé au courriel en paramêtre.</returns>
        public static Utilisateur RetrieveByCourriel(string pCourriel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Utilisateurs WHERE courriel = '").Append(pCourriel).Append("'");

            return Retrieve(query.ToString());

        }
        
        /// <summary>
        /// Fonction qui insert un utilisateur dans la base de données. L'utilisateur reçu en paramètre est valide.
        /// </summary>
        /// <param name="utilisateur">Utilisateur qui vient d'être créer</param>
        public static void Insert(Utilisateur utilisateur)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("INSERT INTO Utilisateurs (nom, motDePasse, salt, courriel) VALUES ('")
                 .Append(utilisateur.Nom).Append("',")
                 .Append("'").Append(utilisateur.MotDePasse).Append("',")
                 .Append("'").Append(utilisateur.Salt).Append("',")
                 .Append("'").Append(utilisateur.Courriel).Append("')");

            ConnectionBD.NonQuery(nonquery.ToString());
        }

        public static List<Utilisateur> RetrieveAmis(int pIdUtilisateur)
        {
            List<Utilisateur> lstResultat = new List<Utilisateur>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT idUtilisateurAmi FROM Amis WHERE idUtilisateurProprietaire = ").Append(pIdUtilisateur);

            dsResultat = ConnectionBD.Query(query.ToString());
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(RetrieveById((int)dr["idUtilisateurAmi"]));
            }

            return lstResultat;
        }

        public static string RetrieveNoteAmiByID(int pIdUtilisateurProprietaire, int pIdUtilisateurAmi)
        {
            string resultat = "";
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow drResultat;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT note FROM Amis WHERE idUtilisateurProprietaire = ").Append(pIdUtilisateurProprietaire)
                 .Append(" AND idUtilisateurAmi = ").Append(pIdUtilisateurAmi);

            dsResultat = ConnectionBD.Query(query.ToString());
            dtResultat = dsResultat.Tables[0];

            if (dtResultat.Rows.Count > 0)
            {
                drResultat = dtResultat.Rows[0];

                if (!drResultat.IsNull(0) )
                    resultat = (string)drResultat["note"];   
            }

            return resultat;
        }

        public static void UpdateNoteAmi(int pIdUtilisateurProprietaire, int pIdUtilisateurAmi, string note)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("UPDATE Amis SET note = '").Append(note).Append("' WHERE idUtilisateurProprietaire = ")
                .Append(pIdUtilisateurProprietaire).Append(" AND idUtilisateurAmi = ").Append(pIdUtilisateurAmi);

            ConnectionBD.NonQuery(nonquery.ToString());
        }

        public static void InsertAmi(int pIdUtilisateurProprietaire, int pIdUtilisateurAmi)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("INSERT INTO Amis (idUtilisateurProprietaire, idUtilisateurAmi) VALUES (")
                 .Append(pIdUtilisateurProprietaire)
                 .Append(",").Append(pIdUtilisateurAmi).Append(")");

            ConnectionBD.NonQuery(nonquery.ToString());
        }

        public static void DeleteAmi(int pIdUtilisateurProprietaire, int pIdUtilisateurAmi)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("DELETE FROM Amis WHERE idUtilisateurProprietaire = ").Append(pIdUtilisateurProprietaire)
                 .Append(" AND idUtilisateurAmi = ").Append(pIdUtilisateurAmi);

            ConnectionBD.NonQuery(nonquery.ToString());
        }

    }
}
