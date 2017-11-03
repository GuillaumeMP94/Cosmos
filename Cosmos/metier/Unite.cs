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
		public Unite(int idCarte, string nom, Effet effet, Ressource cout, int attaque, int defense)
			: base(idCarte, nom, effet, cout)
		{
			Attaque = attaque;
			Defense = defense;
		}
        #endregion
        /// <summary>
        /// Fonction qui fait une deep copy d'une unite.
        /// </summary>
        /// <returns></returns>
        public override Carte Clone()
        {
            if (this.EffetCarte != null)
                return new Unite(this.IdCarte, this.Nom, this.EffetCarte.Clone(), new Ressource(this.Cout), this.Attaque, this.Defense);
            else
                return new Unite(this.IdCarte, this.Nom, null, new Ressource(this.Cout), this.Attaque, this.Defense);

        }
        public static Unite operator -(Unite a, Unite b)
        {
            a.Defense = a.Defense - b.Attaque;
            return a;
        }
    }
}