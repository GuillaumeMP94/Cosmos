using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
	public class Effet
	{
		#region Propriétés
		public string Type { get; set; }
		#endregion
		#region Constructeurs
		public Effet(string type)
		{
			Type = type;
		}
		#endregion
		/// <summary>
		/// Fonction qui permet la deep copy d'un effet.
		/// </summary>
		/// <returns></returns>
		public virtual Effet Clone()
		{
		    return new Effet(this.Type);
		}
        public virtual Ressource GetRessourceJoueur()
        {
            return new Ressource(0, 0, 0);
        }
        public virtual Ressource GetRessourceAdversaire()
        {
            return new Ressource(0, 0, 0);
        }
    }
}
