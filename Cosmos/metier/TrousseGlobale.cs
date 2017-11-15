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
            public int MyProperty { get; set; }
            public PhaseChangeEventArgs(int i)
            {
                MyProperty = i;
            }
        }
        // Le prototype que doit respecter la méthode qui s'occupera de gérer l'évènement
        // Ce sont les noms des méthodes qu'on ajoutera à TrousseGlobale.NouvelleMission += 
        public delegate void PhaseChangeEventHandler(object sender, PhaseChangeEventArgs e);

        static public event PhaseChangeEventHandler PhaseChange;

        // Déclenchement de l'évènement
        public virtual void OnPhaseChange(PhaseChangeEventArgs e)
        {
            if (PhaseChange != null)
                PhaseChange(this, e);
        }

    }
}
