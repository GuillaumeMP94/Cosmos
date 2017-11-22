using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
	public class Impact : Effet
	{
		#region Propriétés
		public int Valeur { get; set; }
		public int Cible { get; set; }
		public int NbCible { get; set; }
		#endregion
		#region Constructeurs
		public Impact(string type, int valeur, int cible, int nbCible)
			: base(type)
		{
			Valeur = valeur;
			Cible = cible;
			NbCible = nbCible;
		}
		#endregion
		/// <summary>
		/// Fonction qui permet la deep copy d'un Impact.
		/// </summary>
		/// <returns></returns>
		public override Effet Clone()
		{
		    return new Impact(this.Type,this.Valeur,this.Cible,this.NbCible);
		}

	}
}
