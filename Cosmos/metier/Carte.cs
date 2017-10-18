using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    /// <summary>
    /// Classe abstraite pour les informations de la cartes.
    /// </summary>
	public abstract class Carte
	{
		#region Propriétés
		public string Nom { get; set; }
		public Effet EffetCarte { get; set; }
		public Ressource Cout { get; set; }
        public string NomImage { get; set; }
        #endregion
        #region Constructeur
        public Carte(string n, Effet effet, Ressource cout)
		{
			Nom = n;
			EffetCarte = effet;
			Cout = cout;
            NomImage = n + ".jpg";
		}
        #endregion
        public abstract Carte Clone();

	}
}
