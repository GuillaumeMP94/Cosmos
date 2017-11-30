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
    /// Accès BD pour les effets.
    /// </summary>
    class MySqlEffetService
    {
        private static MySqlConnexion ConnectionBD { get; set; }
        /// <summary>
        /// Fonction qui retourne tous les effets.
        /// </summary>
        /// <returns>Liste de tous les effets</returns>
        public static List<Effet> RetrieveAll()
        {

            List<Effet> lstResultat = new List<Effet>();
            DataSet dsResultat;
            DataTable dtResultat;


            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query("SELECT * FROM Effets");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                if ((int)dr["champEffetF"] != 99)
                {
                    lstResultat.Add(new Gain((string)dr["typeEffet"]
                                            , (int)dr["champEffetA"]
                                            , (int)dr["champEffetB"]
                                            , (int)dr["champEffetC"]
                                            , (int)dr["champEffetD"]
                                            , (int)dr["champEffetE"]
                                            , (int)dr["champEffetF"]
                                            )
                                   );
                }
                else if ((int)dr["champEffetC"] != 99)
                {
                    lstResultat.Add(new Impact((string)dr["typeEffet"]
                                            , (int)dr["champEffetA"]
                                            , (int)dr["champEffetB"]
                                            , (int)dr["champEffetC"]
                                            )
                                   );
                }
                else if ((int)dr["champEffetB"] != 99)
                {
                    lstResultat.Add(new Recyclage((string)dr["typeEffet"]
                                            , (int)dr["champEffetA"]
                                            , (int)dr["champEffetB"]
                                            )
                                   );
                }
                else if ((int)dr["champEffetA"] != 99)
                {
                    lstResultat.Add(new Radiation((string)dr["typeEffet"]
                                            , (int)dr["champEffetA"]
                                            )
                                   );
                }
                else
                {
                    lstResultat.Add(new Effet((string)dr["typeEffet"]));
                }
            }
            return lstResultat;
        }
        /// <summary>
        /// Fonction qui retourne un Effet.
        /// </summary>
        /// <param name="query">Requête à effectuer sur la BD</param>
        /// <returns>Un Effet</returns>
        private static Effet Retrieve(string query)
        {

            Effet resultat;
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow drResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];
            drResultat = dtResultat.Rows[0];


            if ((int)drResultat["champEffetF"] != 99)
            {
                resultat = new Gain((string)drResultat["typeEffet"]
                                   , (int)drResultat["champEffetA"]
                                   , (int)drResultat["champEffetB"]
                                   , (int)drResultat["champEffetC"]
                                   , (int)drResultat["champEffetD"]
                                   , (int)drResultat["champEffetE"]
                                   , (int)drResultat["champEffetF"]
                                   );
            }
            else if ((int)drResultat["champEffetC"] != 99)
            {
                resultat = new Impact((string)drResultat["typeEffet"]
                                     , (int)drResultat["champEffetA"]
                                     , (int)drResultat["champEffetB"]
                                     , (int)drResultat["champEffetC"]
                                     );
            }
            else if ((int)drResultat["champEffetB"] != 99)
            {
                resultat = new Recyclage((string)drResultat["typeEffet"]
                                        , (int)drResultat["champEffetA"]
                                        , (int)drResultat["champEffetB"]
                                        );
            }
            else if ((int)drResultat["champEffetA"] != 99)
            {
                resultat = new Radiation((string)drResultat["typeEffet"]
                                        , (int)drResultat["champEffetA"]
                                        );
            }
            else
            {
                resultat = new Effet((string)drResultat["typeEffet"]);
            }

            return resultat;

        }
        /// <summary>
        /// Fonction qui construit la commande SQL pour la requête par ID et qui la passe ensuite à Retrieve
        /// </summary>
        /// <param name="pIdEffet">id de l'effet.</param>
        /// <returns>Retourne l'effet associé au id en paramêtre.</returns>
        public static Effet RetrieveById(int pIdEffet)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Effets WHERE idEffet = ").Append(pIdEffet.ToString());

            return Retrieve(query.ToString());

        }
        
    }
}
