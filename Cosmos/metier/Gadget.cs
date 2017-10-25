using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe pour les Gadgets qui sont un type de Carte.
    /// </summary>
	public class Gadget : Carte
	{
		#region Constructeur
		public Gadget(int idCarte, string nom, Effet effet, Ressource cout)
			: base(idCarte, nom, effet, cout)
		{

		}
        #endregion
        /// <summary>
        /// Fonction qui fait une deep copy d'un gadget.
        /// </summary>
        /// <returns></returns>
        public override Carte Clone()
        {
            if (this.EffetCarte != null)
                return new Gadget(this.IdCarte, this.Nom, this.EffetCarte.Clone(), new Ressource(this.Cout));
            else
                return new Gadget(this.IdCarte, this.Nom, null, new Ressource(this.Cout));
        }
    }
}