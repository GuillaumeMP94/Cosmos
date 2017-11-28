using Cosmos.metier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.accesBD
{
    class MySqlDeckService
    {
        /// <summary>
        /// Accès à la BD pour la classe Deck
        /// </summary>
        private static MySqlConnexion ConnectionBD { get; set; }
        /// <summary>
        /// Fonction qui retourne un deck.
        /// </summary>
        /// <param name="query">Requête à effectuer sur la BD</param>
        /// <returns>Une carte avec son effet.</returns>
        private static Deck Retrieve(string query)
        {

            Deck resultat = null;
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow drResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];
            drResultat = dtResultat.Rows[0];

            resultat = new Deck((int)drResultat["idDeck"]
                                , (string)drResultat["nom"]
                                , (bool)drResultat["estChoisi"]
                                );
            //On va chercher ses cartes avec le id.
            resultat.CartesDuDeck = MySqlCarteService.RetrieveAllDeckCard((int)drResultat["idDeck"]);

            return resultat;
        }
        private static List<Deck> RetrieveAllDeck(string query)
        {
            List<Deck> lstResultat = new List<Deck>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {

                Deck deckPourAjouter = new Deck((int)dr["idDeck"]
                                                , (string)dr["nom"]
                                                , (bool)dr["estChoisi"]
                                                );
                deckPourAjouter.CartesDuDeck = MySqlCarteService.RetrieveAllDeckCard((int)dr["idDeck"]);
                lstResultat.Add(deckPourAjouter);

            }

            return lstResultat;
        }
        /// <summary>
        /// Fonction qui construit la commande SQL pour la requête par ID et qui la passe ensuite à Retrieve
        /// </summary>
        /// <param name="pIdUtilisateur"></param>
        /// <returns>Retourne le deck associé au id en paramêtre.</returns>
        public static Deck RetrieveById(int pIdDeck)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Decks WHERE idDeck = ").Append(pIdDeck.ToString());

            return Retrieve(query.ToString());

        }

		private static Deck RetrieveByNom(string nomDeck, int pIdUtilisateur)
		{
			StringBuilder query = new StringBuilder();
			query.Append("SELECT * FROM Decks WHERE nom = '").Append(nomDeck.ToString()).Append("' AND idUtilisateur = ").Append(pIdUtilisateur);

			return Retrieve(query.ToString());

		}

		public static List<Deck> RetrieveAllUserDeck(int pIdUtilisateur)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Decks d ")
                    .Append("INNER JOIN Utilisateurs u ON u.idUtilisateur = d.idUtilisateur ")
                    .Append("WHERE d.idUtilisateur =")
                    .Append(pIdUtilisateur.ToString());

            // TODO: Prend pas en charge les quantité
            return RetrieveAllDeck(query.ToString());
        }

		public static void Delete(int pIdUtilisateur, string nomDeck)
		{
			Deck deckASupprimer = RetrieveByNom(nomDeck, pIdUtilisateur);

			StringBuilder nonquery = new StringBuilder();
			ConnectionBD = new MySqlConnexion();

			// Delete le deck de la table DecksExemplaires
			nonquery.Append("DELETE FROM DecksExemplaires WHERE idDeck = ").Append(deckASupprimer.IdDeck);
			ConnectionBD.NonQuery(nonquery.ToString());

			// Delete le deck
			nonquery = new StringBuilder();
			nonquery.Append("DELETE FROM Decks WHERE nom = '").Append(deckASupprimer.Nom).Append("' AND idUtilisateur = ").Append(pIdUtilisateur);

			ConnectionBD.NonQuery(nonquery.ToString());
		}

        public static void UpdateNomDeck(int pIdUtilisateur, string ancienNomDeck, string nouveauNomDeck)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("UPDATE Decks SET nom = '").Append(nouveauNomDeck).Append("' WHERE idUtilisateur = ")
                .Append(pIdUtilisateur).Append(" AND nom = '").Append(ancienNomDeck).Append("'");

            ConnectionBD.NonQuery(nonquery.ToString());
        }
    }
}
