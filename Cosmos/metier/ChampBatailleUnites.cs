using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe qui contient les cartes unités en jeu pendant la partie
    /// </summary>
    public class ChampBatailleUnites : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Propriétés
        public Unite Champ1 { get; set; }
        public bool EstEnPreparationChamp1; // Pour savoir si l'unité est en preparation et donc ne peut pas attaquer.
        public Unite Champ2 { get; set; }
        public bool EstEnPreparationChamp2;
        public Unite Champ3 { get; set; }
        public bool EstEnPreparationChamp3;
        private int vieChamp1;
        public int VieChamp1
        {
            get { return vieChamp1; }
            set
            {
                vieChamp1 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VieChamp1"));
                }

            }
        }
        private int vieChamp2;
        public int VieChamp2
        {
            get { return vieChamp2; }
            set
            {
                vieChamp2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VieChamp2"));
                }

            }
        }
        private int vieChamp3;
        public int VieChamp3
        {
            get { return vieChamp3; }
            set
            {
                vieChamp3 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VieChamp3"));
                }

            }
        }
        private int attChamp1;
        public int AttChamp1
        {
            get { return attChamp1; }
            set
            {
                attChamp1 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AttChamp1"));
                }

            }
        }
        private int attChamp2;
        public int AttChamp2
        {
            get { return attChamp2; }
            set
            {
                attChamp2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AttChamp2"));
                }

            }
        }
        private int attChamp3;
        public int AttChamp3
        {
            get { return attChamp3; }
            set
            {
                attChamp3 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AttChamp3"));
                }

            }
        }

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
        public int EspaceOccupe()
        {
            int resultat = 0;
            if (Champ1 != null)
                resultat++;
            if (Champ2 != null)
                resultat++;
            if (Champ3 != null)
                resultat++;
            return resultat;
        }

        public void AjouterAuChamp(Carte carteAjouter, int emplacement)
        {
            switch (emplacement)
            {
                case 1: Champ1 = (Unite)carteAjouter;
                    EstEnPreparationChamp1 = true;
                    VieChamp1 = carteAjouter.getDefense();
                    AttChamp1 = carteAjouter.getAttaque();
                    break;
                case 2: Champ2 = (Unite)carteAjouter;
                    EstEnPreparationChamp2 = true;
                    VieChamp2 = carteAjouter.getDefense();
                    AttChamp2 = carteAjouter.getAttaque();
                    break;
                case 3: Champ3 = (Unite)carteAjouter;
                    EstEnPreparationChamp3 = true;
                    VieChamp3 = carteAjouter.getDefense();
                    AttChamp3 = carteAjouter.getAttaque();
                    break;
            }
        }

        public void Preparer()
        {
            EstEnPreparationChamp1 = false;
            EstEnPreparationChamp2 = false;
            EstEnPreparationChamp3 = false;
        }
        /// <summary>
        /// Détruit les unités si elles sont présentes et que leur vie est sous 0
        /// </summary>
        /// <returns>Une liste d'unité détruite.</returns>
        public List<Unite> DetruireUnite()
        {
            List<Unite> temp = new List<Unite>();
            
            if (Champ1 != null && VieChamp1 <= 0)
            {
                temp.Add(Champ1);
                Champ1 = null;
                VieChamp1 = 0;
                AttChamp1 = 0;
            }
            if (Champ2 != null && VieChamp2 <= 0)
            {
                temp.Add(Champ2);
                Champ2 = null;
                VieChamp2 = 0;
                AttChamp2 = 0;
            }
            if (Champ3 != null && VieChamp3 <= 0)
            {
                temp.Add(Champ3);
                Champ3 = null;
                VieChamp3 = 0;
                AttChamp3 = 0;
            }
            return temp;
        }
    }
}
