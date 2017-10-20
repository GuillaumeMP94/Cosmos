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
        public static List<Carte> RetrieveAll()
        {
            List<Carte> lstResultat = new List<Carte>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query("SELECT * FROM Carte");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                Effet effetCarte = MySqlEffetService.RetrieveById((int)dr["idEffet"]);
                if ((string)dr["typeUnite"] != null) // TODO: vérifié si le if fonctionne
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
                else if ((string)dr["pointsDefense"] != null) // TODO: vérifié si le if fonctionne
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
        }/*
        /// <summary>
        /// Fonction qui retourne une carte.
        /// </summary>
        /// <param name="query">Requête à effectuer sur la BD</param>
        /// <returns>Une carte avec son effet.</returns>
        private static Utilisateur Retrieve(string query)
        {
            List<Carte> lstCartesUtilisateur;
            List<Deck> lstDecksUtilisateur;
            Utilisateur resultat;
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow drResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];
            drResultat = dtResultat.Rows[0];

            resultat = new Utilisateur((int)drResultat["idUtilisateur"]
                                     , (string)drResultat["nom"]
                                     , (string)drResultat["courriel"]
                                     , (int)drResultat["idNiveau"]
                                     , (string)drResultat["motDePasse"]
                                     , (string)drResultat["salt"]
                                     );

            // On va chercher les cartes
            //lstCartesUtilisateur = MySqlCarteService.RetrieveByIdUtilisateur(pIdUtilisateur);
            // On va chercher les decks
            //lstDecksUtilisateur = MySqlDeckService.RetrieveByIdUtilisateur(pIdUtilisateur);
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
            query.Append("SELECT * FROM Visites WHERE nom = ").Append(pNom);

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
            query.Append("SELECT * FROM Visites WHERE courriel = ").Append(pCourriel);

            return Retrieve(query.ToString());

        }*/
    }
}
