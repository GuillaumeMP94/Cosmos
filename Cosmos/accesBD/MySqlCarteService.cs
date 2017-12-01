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
    /// Accès à la BD pour la classe Carte
    /// </summary>
    class MySqlCarteService
    {
        private static MySqlConnexion ConnectionBD { get; set; }
        /// <summary>
        /// Fonction qui retourne toutes les cartes.
        /// </summary>
        /// <returns>Liste de toutes les cartes avec leurs effets.</returns>
        private static List<Carte> RetrieveAll(string query)
        {
            List<Carte> lstResultat = new List<Carte>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                Effet effetCarte = null;
                if (dr["idEffet"] != DBNull.Value)
                {
                    effetCarte = MySqlEffetService.RetrieveById((int)dr["idEffet"]);
                }
                if (dr["typeUnite"] != DBNull.Value) // TODO: vérifié si le if fonctionne sinon changé pour "null"
                {
                    lstResultat.Add(new Unite((int)dr["idCarte"]
                                             , (string)dr["nom"]
                                             , effetCarte
                                             , new Ressource((int)dr["coutCharronite"],(int)dr["coutBarilNucleaire"],(int)dr["coutAlainDollars"])
                                             , (int)dr["pointsAttaque"]
                                             , (int)dr["pointsDefense"]
                                             )
                                   );
                }
                else if (dr["pointsDefense"] != DBNull.Value) // TODO: vérifié si le if fonctionne sinon changé pour "null"
                {
                    lstResultat.Add(new Batiment((int)dr["idCarte"]
                                             , (string)dr["nom"]
                                             , effetCarte
                                             , new Ressource((int)dr["coutCharronite"], (int)dr["coutBarilNucleaire"], (int)dr["coutAlainDollars"])
                                             , (int)dr["pointsDefense"]
                                             )
                                   );

                }
                else
                {
                    lstResultat.Add(new Gadget((int)dr["idCarte"]
                                             , (string)dr["nom"]
                                             , effetCarte
                                             , new Ressource((int)dr["coutCharronite"], (int)dr["coutBarilNucleaire"], (int)dr["coutAlainDollars"])
                                             )
                                   );
                }
            }
            return lstResultat;
        }
        /// <summary>
        /// Fonction qui retourne une carte.
        /// </summary>
        /// <param name="query">Requête à effectuer sur la BD</param>
        /// <returns>Une carte avec son effet.</returns>
        private static Carte Retrieve(string query)
        {

            Carte resultat;
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow drResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];
            drResultat = dtResultat.Rows[0];
            Effet effetCarte = null;
            if (drResultat["idEffet"] != DBNull.Value)
                effetCarte = MySqlEffetService.RetrieveById((int)drResultat["idEffet"]);
            if (drResultat["typeUnite"] != DBNull.Value) // TODO: vérifié si le if fonctionne sinon changé pour "null"
            {
                resultat = new Unite((int)drResultat["idCarte"]
                                         , (string)drResultat["nom"]
                                         , effetCarte
                                         , new Ressource((int)drResultat["coutCharronite"], (int)drResultat["coutBarilNucleaire"], (int)drResultat["coutAlainDollars"])
                                         , (int)drResultat["pointsAttaque"]
                                         , (int)drResultat["pointsDefense"]
                                         );
            }
            else if (drResultat["pointsDefense"] != DBNull.Value) // TODO: vérifié si le if fonctionne sinon changé pour "null"
            {
                resultat = new Batiment((int)drResultat["idCarte"]
                                         , (string)drResultat["nom"]
                                         , effetCarte
                                         , new Ressource((int)drResultat["coutCharronite"], (int)drResultat["coutBarilNucleaire"], (int)drResultat["coutAlainDollars"])
                                         , (int)drResultat["pointsDefense"]
                                         );

            }
            else
            {
                resultat = new Gadget((int)drResultat["idCarte"]
                                         , (string)drResultat["nom"]
                                         , effetCarte
                                         , new Ressource((int)drResultat["coutCharronite"], (int)drResultat["coutBarilNucleaire"], (int)drResultat["coutAlainDollars"])
                                         );
            }

            return resultat;
        }

        /// <summary>
        /// Retrouve tous les exemplaires avec leur quantité
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static List<Carte> RetrieveAllExemplaire(string query)
        {
            List<Carte> lstResultat = new List<Carte>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                
                Effet effetCarte = null;
                if (dr["idEffet"] != DBNull.Value)
                {
                    effetCarte = MySqlEffetService.RetrieveById((int)dr["idEffet"]);
                }
                if (dr["typeUnite"] != DBNull.Value) // TODO: vérifié si le if fonctionne sinon changé pour "null"
                {
                    for (int i = 0; i < (int)dr["quantite"]; i++)
                    {
                        lstResultat.Add(new Unite((int)dr["idCarte"]
                                                    , (string)dr["nom"]
                                                    , effetCarte
                                                    , new Ressource((int)dr["coutCharronite"], (int)dr["coutBarilNucleaire"], (int)dr["coutAlainDollars"])
                                                    , (int)dr["pointsAttaque"]
                                                    , (int)dr["pointsDefense"]
                                                    )
                                        );
                    }
                }
                else if (dr["pointsDefense"] != DBNull.Value) // TODO: vérifié si le if fonctionne sinon changé pour "null"
                {
                    for (int i = 0; i < (int)dr["quantite"]; i++)
                    {
                        lstResultat.Add(new Batiment((int)dr["idCarte"]
                                                    , (string)dr["nom"]
                                                    , effetCarte
                                                    , new Ressource((int)dr["coutCharronite"], (int)dr["coutBarilNucleaire"], (int)dr["coutAlainDollars"])
                                                    , (int)dr["pointsDefense"]
                                                    )
                                        );
                    }
                }
                else
                {
                    for (int i = 0; i < (int)dr["quantite"]; i++)
                    {
                        lstResultat.Add(new Gadget((int)dr["idCarte"]
                                                    , (string)dr["nom"]
                                                    , effetCarte
                                                    , new Ressource((int)dr["coutCharronite"], (int)dr["coutBarilNucleaire"], (int)dr["coutAlainDollars"])
                                                    )
                                        );
                    }
                }
            }
                

            return lstResultat;
        }

        
        /// <summary>
        /// Fonction qui construit la commande SQL pour la requête par ID et qui la passe ensuite à Retrieve
        /// </summary>
        /// <param name="pIdUtilisateur"></param>
        /// <returns>Retourne l'utilisateur associé au id en paramêtre.</returns>
        public static Carte RetrieveById(int pIdCarte)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Cartes WHERE idCarte = ").Append(pIdCarte.ToString());

            return Retrieve(query.ToString());

        }
        public static List<Carte> RetrieveAllCard()
        {
            return RetrieveAll("SELECT * FROM Cartes");
        }


        public static List<Carte> RetrieveAllUserExemplaires(int pIdUtilisateur)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.*, e.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("INNER JOIN Utilisateurs u ON u.idUtilisateur = e.idUtilisateur ")
                 .Append("WHERE e.idUtilisateur =")
                 .Append(pIdUtilisateur.ToString());

            return RetrieveAllExemplaire(query.ToString());
        }

        public static List<Carte> RetrieveAllDeckCard(int pIdDeck)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.*, d.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("INNER JOIN DecksExemplaires d ON d.idExemplaire = e.idExemplaire ")
                 .Append("WHERE d.idDeck =")
                 .Append(pIdDeck.ToString());

            return RetrieveAllExemplaire(query.ToString());
        }

        #region NeoUtilisateur
        public static List<Carte> RetrieveNewUtilisateurCard()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.*, dE.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("INNER JOIN DecksExemplaires dE ON dE.idExemplaire = e.idExemplaire ")
                 .Append("INNER JOIN Decks d ON d.idDeck = dE.idDeck ")
                 .Append("WHERE d.nom = 'defaut' AND d.idUtilisateur IS NULL");

            return RetrieveAllExemplaire(query.ToString());
        }

        public static void InsertNewJoueurCard(Utilisateur utilisateur)
        {
            List<Carte> lstCarteAAjouter = RetrieveNewUtilisateurCard();

            // Faire le insert ici selon chaque carte dans la liste.
            foreach (Carte c in lstCarteAAjouter)
            {
                if (ExemplaireExist(utilisateur, c))
                    UpdateExemplaire(c, utilisateur);
                else
                    InsertExemplaire(c, utilisateur);
                
            }
        }
        #endregion
        public static void InsertExemplaire(Carte carte, Utilisateur utilisateur)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("INSERT INTO Exemplaires (idCarte, idUtilisateur, quantite) VALUES ('")
                .Append(carte.IdCarte).Append("',")
                .Append("'").Append(utilisateur.IdUtilisateur).Append("',")
                .Append(" 1 )");

            ConnectionBD.NonQuery(nonquery.ToString());
        }

        public static void UpdateExemplaire(Carte carte, Utilisateur utilisateur)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            nonquery.Append("UPDATE Exemplaires SET quantite = quantite + 1")
                .Append(" WHERE idCarte = ").Append(carte.IdCarte)
                .Append(" AND idUtilisateur = ").Append(utilisateur.IdUtilisateur);

            ConnectionBD.NonQuery(nonquery.ToString());
        }

        private static bool ExemplaireExist(Utilisateur utilisateur, Carte carte)
        {
            List<Carte> lesCartes = RetrieveExemplaire(utilisateur, carte);
            return lesCartes.Count > 0;
        }

        private static List<Carte> RetrieveExemplaire(Utilisateur utilisateur, Carte carte)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.*, e.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("WHERE e.idCarte = ").Append(carte.IdCarte)
                 .Append(" AND e.idUtilisateur = ").Append(utilisateur.IdUtilisateur);

            return RetrieveAllExemplaire(query.ToString());
        }


        /// <summary>
        /// Fonction qui fait une requête pour aller chercher les exemplaires du deck voulu
        /// </summary>
        /// <param name="nomDeck"></param>
        /// <param name="pIdUtilisateur"></param>
        /// <returns></returns>
        public static List<Exemplaire> RetrieveExemplairesDeckUser(string nomDeck, int pIdUtilisateur)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.idCarte, e.idExemplaire, dE.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("INNER JOIN DecksExemplaires dE ON dE.idExemplaire = e.idExemplaire ")
                 .Append("INNER JOIN Decks d ON d.idDeck = dE.idDeck ")
                 .Append("WHERE d.nom = '").Append(nomDeck).Append("'").Append(" AND d.idUtilisateur =").Append(pIdUtilisateur);

            return RetrieveExemplaires(query.ToString());
        }

        /// <summary>
        /// Fonction qui va chercher la liste complète des exemplaires de l'utilisateur
        /// </summary>
        /// <param name="pIdUtilisateur"></param>
        /// <returns></returns>
        public static List<Exemplaire> RetrieveExemplairesUser(int pIdUtilisateur)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.idCarte, e.idExemplaire, e.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("INNER JOIN Utilisateurs u ON u.idUtilisateur = e.idUtilisateur ")
                 .Append("WHERE e.idUtilisateur =")
                 .Append(pIdUtilisateur.ToString());

            return RetrieveExemplaires(query.ToString());
        }

        /// <summary>
        /// Fonction qui va chercher en BD les exemplaires selon la requête voulue
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static List<Exemplaire> RetrieveExemplaires(string query)
        {
            List<Exemplaire> lstResultat = new List<Exemplaire>();
            Carte laCarte = null;
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                laCarte = MySqlCarteService.RetrieveById((int)dr["idCarte"]);
                lstResultat.Add(new Exemplaire(laCarte, (int)dr["quantite"], (int)dr["idExemplaire"]));
            }

            return lstResultat;
        }

        public static void DeleteAllExemplairesDeck(int pIdDeck)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            // Delete le deck de la table DecksExemplaires
            nonquery.Append("DELETE FROM DecksExemplaires WHERE idDeck = ").Append(pIdDeck);
            ConnectionBD.NonQuery(nonquery.ToString());
        }

    }


}
