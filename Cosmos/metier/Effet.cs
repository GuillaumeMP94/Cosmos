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
	}
}
