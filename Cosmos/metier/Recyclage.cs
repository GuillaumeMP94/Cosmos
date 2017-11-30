using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class Recyclage : Effet
    {
        #region Propriétés
        public int Valeur { get; set; } // NbCartes
        public int Cible { get; set; } // 1 Unite, 2 Gadget, 3 Batiment

        #endregion
        #region Constructeurs
        public Recyclage(string type, int valeur, int cible)
			: base(type)
		{
            Valeur = valeur;
            Cible = cible;
        }
        #endregion
        /// <summary>
        /// Fonction qui permet la deep copy d'un Impact.
        /// </summary>
        /// <returns></returns>
        public override Effet Clone()
        {
            return new Recyclage(this.Type, this.Valeur, this.Cible);
        }

        public override int getValeur()
        {
            return Valeur;
        }
        public override int getCible()
        {
            return Cible;
        }
    }
}
