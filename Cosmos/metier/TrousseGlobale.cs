using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class TrousseGlobale
    {
        public class ChoisirCibleEventArgs : EventArgs
        {
            public int Cible { get; set; }
            public int NbCible { get; set; }
            public ChoisirCibleEventArgs(int cible, int nbCible)
            {
                Cible = cible;
                NbCible = nbCible;
            }
        }
        public class PhaseChangeEventArgs:EventArgs
        {
            public PhaseChangeEventArgs()
            {

            }
        }
        public class RefreshAllEventArgs : EventArgs
        {
            public RefreshAllEventArgs()
            {

            }
        }
        // Prototypes que doit respecter la méthode qui s'occupera de gérer les évènements
        public delegate void ChoisirCibleEventHandler(object sender, ChoisirCibleEventArgs e);
        public delegate void PhaseChangeEventHandler(object sender, PhaseChangeEventArgs e);
        public delegate void RefreshAllEventHandler(object sender, RefreshAllEventArgs e);

        static public event ChoisirCibleEventHandler ChoisirCible;
        static public event PhaseChangeEventHandler PhaseChange;
        static public event RefreshAllEventHandler RefreshAll;

        // Déclenchement de l'évènement
        public virtual void OnPhaseChange(PhaseChangeEventArgs e)
        {
            if (PhaseChange != null)
                PhaseChange(this, e);
        }
        // Déclenchement de l'évènement
        public virtual void OnRefreshAll(RefreshAllEventArgs e)
        {
            if (RefreshAll != null)
                RefreshAll(this, e);
        }
        public virtual void OnChoisirCible(ChoisirCibleEventArgs e)
        {
            if (ChoisirCible != null)
                ChoisirCible(this, e);
        }
    }
}
