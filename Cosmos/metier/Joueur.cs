using System;
using System.Collections.Generic;
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

        #region Propriétés
        //TODO: Avatar path? ou Image?
        public string Avatar { get; set; }
        public int PointDeBlindage { get; set; }
        public string NomJoueur { get; set; }
        public Deck DeckAJouer { get; set;}
        public Ressource Active { get; set; }
        public Ressource Level { get; set; }
        #endregion
        #region Constructeur
        public Joueur()
        {
        }

        #endregion
        public virtual void Reinitialiser()
        {
            PointDeBlindage = 25;
            Active = new Ressource(0, 0, 0);
            Level = new Ressource(1, 1, 1);
        }
    }
}
