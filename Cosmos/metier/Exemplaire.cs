using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    public class Exemplaire
    {
        public Carte Carte { get; set; }
        public int Quantite { get; set; }

        #region Constructeur
        public Exemplaire(Carte carte, int quantite)
        {
            Carte = carte;
            Quantite = quantite;
        }
        #endregion
    }
}
