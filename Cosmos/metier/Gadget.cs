using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
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
