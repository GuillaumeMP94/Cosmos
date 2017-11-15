using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
	public class Ressource : INotifyPropertyChanged
	{

        #region Propriétés

        private int charronite;
        private int barilNucleaire;
        private int alainDollars;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Charronite
        {
            get { return charronite; }
            set
            {
                charronite = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Charronite"));
                }

            }
        }
        public int BarilNucleaire
        {
            get { return barilNucleaire; }
            set
            {
                barilNucleaire = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BarilNucleaire"));
                }

            }
        }
        public int AlainDollars
        {
            get { return alainDollars; }
            set
            {
                alainDollars = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AlainDollars"));
                }

            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Constructeurs
        public Ressource()
		{
			Charronite = 0;
			BarilNucleaire = 0;
			AlainDollars = 0;
		}
		public Ressource(int charronite, int baril, int alain)
		{
			Charronite = charronite;
			BarilNucleaire = baril;
			AlainDollars = alain;
		}

		public Ressource(Ressource a)
		{
			Charronite = a.Charronite;
			BarilNucleaire = a.BarilNucleaire;
			AlainDollars = a.AlainDollars;
		}
		#endregion
		#region Operateurs Surchargés
		public static bool operator ==(Ressource a, Ressource b)
		{
			if (a.Charronite == b.Charronite && a.BarilNucleaire == b.BarilNucleaire && a.AlainDollars == b.AlainDollars)
				return true;
			return false;
		}

		public static bool operator !=(Ressource a, Ressource b)
		{
			if (a.Charronite != b.Charronite || a.BarilNucleaire != b.BarilNucleaire || a.AlainDollars != b.AlainDollars)
				return true;
			return false;
		}

		public static Ressource operator +(Ressource a, Ressource b)
		{
			Ressource total = new Ressource();
			total.Charronite = a.Charronite + b.Charronite;
			total.BarilNucleaire = a.BarilNucleaire + b.BarilNucleaire;
			total.AlainDollars = a.AlainDollars + b.AlainDollars;
			return total;
		}
        public static Ressource operator -(Ressource a, Ressource b)
        {
            Ressource total = new Ressource();
            total.Charronite = a.Charronite - b.Charronite;
            total.BarilNucleaire = a.BarilNucleaire - b.BarilNucleaire;
            total.AlainDollars = a.AlainDollars - b.AlainDollars;
            return total;
        }

        public static bool operator <(Ressource a, Ressource b)
        {
            if (a.Charronite < b.Charronite && a.BarilNucleaire < b.BarilNucleaire && a.AlainDollars < b.AlainDollars)
                return true;
            return false;
        }

        public static bool operator >(Ressource a, Ressource b)
        {
            if (a.Charronite > b.Charronite && a.BarilNucleaire > b.BarilNucleaire && a.AlainDollars > b.AlainDollars)
                return true;
            return false;
        }

        public override string ToString()
		{
			StringBuilder ressources = new StringBuilder();
			ressources.Append(this.Charronite.ToString());
			if (this.Charronite > 1)
			{
				ressources.Append(" Charronites, ");
			}
			else
			{
				ressources.Append(" Charronite, ");
			}
			ressources.Append(this.BarilNucleaire.ToString());
			if (this.Charronite > 1)
			{
				ressources.Append(" Barils Nucléaire et ");
			}
			else
			{
				ressources.Append(" Baril Nucléaire et ");
			}
			ressources.Append(this.AlainDollars.ToString())
					  .Append(" Alain Dollars");
			return ressources.ToString();
		}
		#endregion
	}
}
