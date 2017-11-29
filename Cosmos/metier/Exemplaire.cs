using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    public class Exemplaire: INotifyPropertyChanged
    {
        private int quantite;
        public Carte Carte { get; set; }
        public int Quantite
        {
            get { return quantite; }
            set
            {
                quantite = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Quantite"));
                }

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructeur
        public Exemplaire(Carte carte, int quantite)
        {
            Carte = carte;
            Quantite = quantite;
        }

        #endregion
    }
}
