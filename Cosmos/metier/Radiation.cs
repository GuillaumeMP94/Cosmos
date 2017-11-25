using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class Radiation : Effet
    {
        #region Propriétés
        public int Valeur { get; set; }

        #endregion
        #region Constructeurs
        public Radiation(string type, int valeur)
			: base(type)
		{
            Valeur = valeur;
        }
        #endregion
        /// <summary>
        /// Fonction qui permet la deep copy d'un Impact.
        /// </summary>
        /// <returns></returns>
        public override Effet Clone()
        {
            return new Radiation(this.Type, this.Valeur);
        }

        public override int getValeur()
        {
            return Valeur;
        }
    }
}
