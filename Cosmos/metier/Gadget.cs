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
		public Gadget(string nom, Effet effet, Ressource cout, int defense)
			: base(nom, effet, cout)
		{

		}
		#endregion
	}
}
