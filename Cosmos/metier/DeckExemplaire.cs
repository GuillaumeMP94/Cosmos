using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    public class DeckExemplaire
    {
        public List<Exemplaire> ExemplairesDeck { get; set; }
        public int IdDeck { get; set; }
        public bool EstChoisi { get; set; }
        public string Nom { get; set; }


        public DeckExemplaire()
        {

        }

    }
}
