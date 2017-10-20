fuckusing System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe pour les informations d'un batiment qui est un type de carte.
    /// </summary>
	public class Batiment : Carte
	{
		#region Propriétés
		public int Defense { get; set; }
		#endregion
		#region Constructeur
		public Batiment(string nom, Effet effet, Ressource cout, int defense)
			: base(nom, effet, cout)
		{
			Defense = defense;
		}
        #endregion
        /// <summary>
        /// Fonction qui fait une deep copy d'un Batiment.
        /// </summary>
        /// <returns></returns>
        public override Carte Clone()
        {
            return new Batiment(this.Nom, this.EffetCarte.Clone(), new Ressource(this.Cout), this.Defense);
        }
    }
}
