using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe qui contient les cartes batiments en jeu pendant la partie
    /// </summary>
    public class ChampConstructions
    {
        #region Propriétés
        public Batiment Champ1 { get; set; }
        public Batiment Champ2 { get; set; }
        public Batiment Champ3 { get; set; }
        public Batiment Champ4 { get; set; }
        #endregion

        #region Constructeurs
        public ChampConstructions()
        {
            Champ1 = null;
            Champ2 = null;
            Champ3 = null;
            Champ4 = null;
        }
        #endregion
        private bool EspaceDisponible()
        {
            if (Champ1 == null || Champ2 == null || Champ3 == null || Champ4 == null)
            {
                return true;
            }
            return false;
        }
        private int EmplacementDisponible()
        {
            // Retourne le numéro du champs disponible
            if (Champ1 == null)
                return 1;
            if (Champ2 == null)
                return 2;
            if (Champ3 == null)
                return 3;
            if (Champ4 == null)
                return 4;
            return 0; // Ici c'est une erreur.
        }
        public void AjouterAuChamp(Carte carteAjouter)
        {
            if (EspaceDisponible())
            {
                switch (EmplacementDisponible())
                {
                    case 1:
                        Champ1 = (Batiment)carteAjouter;
                        break;
                    case 2:
                        Champ2 = (Batiment)carteAjouter;
                        break;
                    case 3:
                        Champ3 = (Batiment)carteAjouter;
                        break;
                    case 4:
                        Champ4 = (Batiment)carteAjouter;
                        break;
                }
            }
            
        }
    }
}
