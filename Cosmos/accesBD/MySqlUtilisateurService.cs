using Cosmos.metier;
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
                resultat.CartesUtilisateurs = MySqlCarteService.RetrieveAllUserCard((int)drResultat["idUtilisateur"]);
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
    }
}
