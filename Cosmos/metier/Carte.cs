using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
	public abstract class Carte
	{
        //Manque Image

		#region Propriétés
		public string Nom { get; set; }
		public Effet EffetCarte { get; set; }
		public Ressource Cout { get; set; }
		#endregion
		#region Constructeur
		public Carte(string n, Effet effet, Ressource cout)
		{
			Nom = n;
			EffetCarte = effet;
			Cout = cout;
		}
		#endregion
	}
}
