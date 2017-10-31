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
    public class Joueur : INotifyPropertyChanged
    {
        private int pointDeBlindage;
        private Ressource ressourceActive;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Propriétés
        //TODO: Avatar path? ou Image?
        public string Avatar { get; set; }
        public int PointDeBlindage
        {
            get { return pointDeBlindage; }
            set
            {
                pointDeBlindage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PointDeBlindage"));
                }

            }
        }
        public string NomJoueur { get; set; }
        public Deck DeckAJouer { get; set;}
        public Ressource RessourceActive
        {
            get { return ressourceActive; }
            set
            {
                ressourceActive = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RessourceActive"));
                }

            }
        }
        public Ressource LevelRessource { get; set; }

        #endregion
        #region Constructeur
        public Joueur()
        {
        }

        #endregion
        public virtual void Reinitialiser()
        {
            PointDeBlindage = 25;
            RessourceActive = new Ressource(0, 2, 1);
            LevelRessource = new Ressource(1, 1, 1);
        }

        public void SetRessource(Ressource neoValeur)
        {
            RessourceActive = new Ressource(neoValeur);
        }
        /// <summary>
        /// Brasse le deck du joueur
        /// </summary>
        public void BrasserDeck()
        {
            DeckAJouer.BrasserDeck();
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
                RessourceActive = RessourceActive + valeur;
            }
            else
            {
                RessourceActive = RessourceActive - valeur;
            }
        }
        /// <summary>
        /// Pige une carte dans le deck du joueur.
        /// </summary>
        /// <returns></returns>
        public Carte PigerCarte()
        {
            return DeckAJouer.PigerCarte();
        }
    }
}
