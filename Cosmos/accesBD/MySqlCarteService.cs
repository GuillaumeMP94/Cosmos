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
        
        public static List<Carte> RetrieveAllUserCard(int pIdUtilisateur)
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

        public static List<Carte> RetrieveNewUtilisateurCard()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT c.*, d.quantite FROM Cartes c ")
                 .Append("INNER JOIN Exemplaires e ON c.idCarte = e.idCarte ")
                 .Append("INNER JOIN DecksExemplaires d ON d.idExemplaire = e.idExemplaire ")
                 .Append("WHERE d.nom = defaut " +
                         "AND d.idUtilisateur IS NULL");

            return RetrieveAllExemplaire(query.ToString());
        }

        public static void InsertNewJoueurCard()
        {
            List<Carte> lstCarteAAjouter = RetrieveNewUtilisateurCard();
            
            // Faire le insert ici selon chaque carte dans la liste.
        }

    }
}
