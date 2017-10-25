using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
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
		/// <summary>
		/// Fonction qui permet la deep copy d'un gain.
		/// </summary>
		/// <returns></returns>
		public override Effet Clone()
		{
		    return new Gain(this.Type,new Ressource(this.RessourceJoueur),new Ressource(this.RessourceAdversaire));
		}
	}
}
