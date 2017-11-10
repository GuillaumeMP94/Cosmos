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
        public bool EstEnPreparationChamp1; // Pour savoir si l'unité est en preparation et donc ne peut pas attaquer.
        public Unite Champ2 { get; set; }
        public bool EstEnPreparationChamp2;
        public Unite Champ3 { get; set; }
        public bool EstEnPreparationChamp3;
        #endregion

        #region Constructeurs
        public ChampBatailleUnites()
        {
            Champ1 = null;
            Champ2 = null;
            Champ3 = null;
            EstEnPreparationChamp1 = false;
            EstEnPreparationChamp2 = false;
            EstEnPreparationChamp3 = false;
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
                    EstEnPreparationChamp1 = true;
                    break;
                case 2: Champ2 = (Unite)carteAjouter;
                    EstEnPreparationChamp1 = true;
                    break;
                case 3: Champ3 = (Unite)carteAjouter;
                    EstEnPreparationChamp1 = true;
                    break;
            }
        }

        public void Preparer()
        {
            EstEnPreparationChamp1 = false;
            EstEnPreparationChamp2 = false;
            EstEnPreparationChamp3 = false;
        }

        public List<Unite> DetruireUnite()
        {
            List<Unite> temp = new List<Unite>();
            
            if (Champ1 != null && Champ1.Defense <= 0)
            {
                temp.Add(Champ1);
                Champ1 = null;
            }
            if (Champ2 != null && Champ2.Defense <= 0)
            {
                temp.Add(Champ2);
                Champ2 = null;
            }
            if (Champ3 != null && Champ3.Defense <= 0)
            {
                temp.Add(Champ3);
                Champ3 = null;
            }
            return temp;
        }
    }
}
