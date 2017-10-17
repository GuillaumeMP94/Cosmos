using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe contenant les informations d'un gain qui est un type d'effet.
    /// </summary>
	public class Gain : Effet
	{
		#region Propriétés
		public Ressource RessourceJoueur { get; set; }
		public Ressource RessourceAdversaire { get; set; }
		#endregion
		#region Constructeurs
		public Gain(string type, int charroniteJ, int barilJ, int alainJ, int charroniteA, int barilA, int alainA)
			: base(type)
		{
			RessourceJoueur = new Ressource(charroniteJ, barilJ, alainJ);
			RessourceAdversaire = new Ressource(charroniteA, barilA, alainA);
		}

		public Gain(string type, Ressource joueur, Ressource adversaire)
			: base(type)
		{
			RessourceJoueur = joueur;
			RessourceAdversaire = adversaire;
		}
		#endregion
	}
}
