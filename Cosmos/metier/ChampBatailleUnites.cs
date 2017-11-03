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
        public Unite Champ1 { get; set; }
        public Unite Champ2 { get; set; }
        public Unite Champ3 { get; set; }
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

        public void AjouterAuChamp(Carte carteAjouter, int emplacement)
        {
            switch (emplacement)
            {
                case 1: Champ1 = (Unite)carteAjouter;
                    break;
                case 2: Champ2 = (Unite)carteAjouter;
                    break;
                case 3: Champ3 = (Unite)carteAjouter;
                    break;
            }
        }
    }
}
