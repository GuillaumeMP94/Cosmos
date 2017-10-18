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
        #endregion
        #region Constructeurs
        public Deck()
        {

        }
        #endregion
        #region Operateurs Surchargés

        public static Deck operator =(Deck a, Deck b)
        {
            
        }
        
        
        
    }
}
