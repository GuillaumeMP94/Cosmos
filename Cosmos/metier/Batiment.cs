using System;
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
		public Batiment(int idCarte, string nom, Effet effet, Ressource cout, int defense)
			: base(idCarte, nom, effet, cout)
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
            if (this.EffetCarte != null)
                return new Batiment(this.IdCarte, this.Nom, this.EffetCarte.Clone(), new Ressource(this.Cout), this.Defense);
            else
                return new Batiment(this.IdCarte, this.Nom, null, new Ressource(this.Cout), this.Defense);
        }

        public override string Type()
        {
            return "Batiment";
        }
    }
}
