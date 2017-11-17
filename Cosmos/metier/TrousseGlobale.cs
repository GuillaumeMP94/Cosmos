using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class TrousseGlobale
    {
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
        public delegate void PhaseChangeEventHandler(object sender, PhaseChangeEventArgs e);
        public delegate void RefreshAllEventHandler(object sender, RefreshAllEventArgs e);

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

    }
}
