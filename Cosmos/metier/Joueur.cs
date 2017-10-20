using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe pour les informations du joueur
    /// </summary>
    public class Joueur
    {
        int pointDeBlindage;

        public event PropertyChangedEventHandler modifPropriete;

        #region Propriétés
        //TODO: Avatar path? ou Image?
        public string Avatar { get; set; }
        public int PointDeBlindage
        {
            get { return pointDeBlindage; }
            set
            {
                pointDeBlindage = value;
                if (modifPropriete != null)
                {
                    modifPropriete(this, new PropertyChangedEventArgs("PointDeBlindage"));
                }

            }
        }
        public string NomJoueur { get; set; }
        public Deck DeckAJouer { get; set;}
        public Ressource Active { get; set; }
        public Ressource Level { get; set; }
        #endregion
        #region Constructeur
        public Joueur()
        {
            Reinitialiser();
        }

        public void Reinitialiser()
        {
            PointDeBlindage = 25;
            Active = new Ressource(0, 0, 0);
            Level = new Ressource(1, 1, 1);
        }
        #endregion

        public void SetRessource(Ressource neoValeur)
        {
            Active = new Ressource(neoValeur);
        }
        /// <summary>
        /// Permet l'addition ou la soustraction de ressource à celle du joueur. 
        /// </summary>
        /// <param name="addition"> Permet l'addition ou la soustraction de ressource à celle du joueur. </param>
        /// <param name="valeur"></param>
        public void ModifierRessource(bool addition, Ressource valeur)
        {
            if (addition)
            {
                Active = Active + new Ressource(valeur);
            }
            else
            {
                Active = Active - new Ressource(valeur);
            }
        }


    }
}
