using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.accesBD
{
    /// <summary>
    /// Cette classe combine l'utilisation MySqlConnection et MySqlTransaction.
    /// Une création originale de Yannick Charron.
    /// </summary>
    public class MySqlConnexion
    {
        private static readonly string CONNECTION_STRING;

        private MySqlConnection Connection { get; set; }
        private MySqlTransaction Transaction { get; set; }


        static MySqlConnexion()
        {
            // Ici, il faut définir la connexion.
            // Elle est présentée sur plusieurs lignes seulement pour clarifier les différents éléments de celle-ci.
            // On doit évidemment faire attention de ne pas mettre un mot de passe important en texte dans le code.
            CONNECTION_STRING = "server=420.cstj.qc.ca;"
                              + "userid=Magico;"
                              + "password=Inf157486;"
                              + "database=magico";
        }

        public MySqlConnexion()
        {
            Connection = new MySqlConnection(CONNECTION_STRING);
        }

        /// <summary>
        /// Permet d'ouvrir une connexion.
        /// La méthode est privée. Ce n'est pas à l'utilisateur d'ouvrir la connection. La classe s'en charge. 
        /// </summary>
        /// <returns>Vrai si la connection a été ouverte.</returns>
        private bool Open()
        {
            Connection.Open();

            return true;
        }

        /// <summary>
        /// On fonctionne avec une transaction seulement si on a besoin de faire plusieurs instructions avant de faire un commit.
        /// Sans transaction, la fermeture de la connexion fait un commit.
        /// </summary>
        /// <returns>Vrai si la connection a été ouverte.</returns>
        public bool OpenWithTransaction()
        {
            if (Open())
            {
                Transaction = Connection.BeginTransaction();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Permet de fermer une connexion.
        /// Fermer une connexion sans transaction fait un commit.
        /// Pour les transaction, on "termine" avec un commit, qui fait appel à Close à la fin.
        /// </summary>
        private void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Utile dans les cas où on travaille avec une transaction.
        /// Se charge de commit et de fermer la transaction et la connexion.
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
            Transaction = null;
            Connection.Close();
        }

        /// <summary>
        /// Utile dans les cas où on travaille avec une transaction.
        /// Se charge de faire un rollback et de fermer la transaction et la connexion.
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
            Transaction = null;
            Connection.Close();
        }

        /// <summary>
        /// Sert à exécuter tout ce qui n'est pas un SELECT. 
        /// On parle de IUD, mais aussi, techniquement, de tout autre commande utile.
        /// </summary>
        /// <param name="nonquery">Une commande complète et fonctionnelle.</param>
        /// <returns>La valeur de "rows returned".</returns>
        public int NonQuery(string nonquery)
        {
            int nbResultat = 0;
            try
            {
                if (Open() || Connection.State == ConnectionState.Open)
                {
                    MySqlCommand command = new MySqlCommand(nonquery, Connection);
                    nbResultat = command.ExecuteNonQuery();
                }

                return nbResultat;
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                if (Transaction == null)
                {
                    Close();
                }
            }
        }


        public DataSet Query(string query)
        {

            DataSet dataset = new DataSet();

            try
            {
                if (Open() || Transaction != null)
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = new MySqlCommand(query, Connection);
                    adapter.Fill(dataset);


                }
                return dataset;

            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                if (Transaction == null)
                {
                    Close();
                }
            }

        }


        public DataSet StoredProcedure(string query, IList<MySqlParameter> parameters = null)
        {
            DataSet dataset = new DataSet();

            try
            {
                if (Open() || Transaction != null)
                {

                    MySqlCommand commande = new MySqlCommand(query, Connection);
                    commande.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        commande.Parameters.AddRange(parameters.ToArray());
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = commande;
                    adapter.Fill(dataset);


                }
                return dataset;

            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                if (Transaction == null)
                {
                    Close();
                }
            }
        }
    }
}
