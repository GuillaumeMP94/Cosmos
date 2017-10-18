using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe pour une unite qui est un type de carte.
    /// </summary>
	public class Unite : Carte
	{
		#region Propriétés
		public int Attaque { get; set; }
		public int Defense { get; set; }
		#endregion
		#region Constructeur
		public Unite(string nom, Effet effet, Ressource cout, int attaque, int defense)
			: base(nom, effet, cout)
		{
			Attaque = attaque;
			Defense = defense;
		}
		#endregion
	}
}
