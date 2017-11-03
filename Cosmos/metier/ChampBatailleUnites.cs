using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe qui contient les cartes unités en jeu pendant la partie
    /// </summary>
    public class ChampBatailleUnites
    {
        #region Propriétés
        public Carte Champ1 { get; set; }
        public Carte Champ2 { get; set; }
        public Carte Champ3 { get; set; }
        #endregion

        #region Constructeurs
        public ChampBatailleUnites()
        {
            Champ1 = null;
            Champ2 = null;
            Champ3 = null;
        }
        #endregion
        public bool EspaceDisponible()
        {
            if(Champ1 == null || Champ2 == null || Champ3 == null)
            {
                return true;
            }
            return false;
        }
    }
}
