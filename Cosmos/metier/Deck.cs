using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    public class Deck
    {
        #region Propriétés
        public List<Carte> CartesDuDeck { get; set; }
        public bool EstChoisi { get; set; }
        #endregion
        #region Constructeurs
        public Deck(bool estChoisi)
        {
            EstChoisi = estChoisi;
        }

        /// <summary>
        /// Constructeur qui permet de faire une Deep copy du deck passé en paramêtre.
        /// </summary>
        /// <param name="aCopier"></param>
        public Deck(Deck aCopier)
        {
            //foreach (Carte uneCarte in aCopier.CartesDuDeck)
            //{
            //    //this.CartesDuDeck.Add(uneCarte.Clone());
            //}
        }
        #endregion


    }
}
