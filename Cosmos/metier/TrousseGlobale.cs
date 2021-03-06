﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class TrousseGlobale
    {
        public class JouerSonEventArgs : EventArgs
        {
            public System.IO.Stream URI { get; set; }
            public JouerSonEventArgs(System.IO.Stream uri)
            {
                URI = uri;
            }
        }
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
        public class FinPartieEventArgs : EventArgs
        {
            public FinPartieEventArgs()
            {

            }
        }
        // Prototypes que doit respecter la méthode qui s'occupera de gérer les évènements
        public delegate void JouerSonEventHandler(object sender, JouerSonEventArgs e);
        public delegate void ChoisirCibleEventHandler(object sender, ChoisirCibleEventArgs e);
        public delegate void PhaseChangeEventHandler(object sender, PhaseChangeEventArgs e);
        public delegate void RefreshAllEventHandler(object sender, RefreshAllEventArgs e);
        public delegate void FinPartieEventHandler(object sender, FinPartieEventArgs e);

        static public event JouerSonEventHandler JouerSon;
        static public event ChoisirCibleEventHandler ChoisirCible;
        static public event PhaseChangeEventHandler PhaseChange;
        static public event RefreshAllEventHandler RefreshAll;
        static public event FinPartieEventHandler FinPartie;

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
        public virtual void OnFinPartie(FinPartieEventArgs e)
        {
            if (FinPartie != null)
                FinPartie(this, e);
        }
        public virtual void OnJouerSon(JouerSonEventArgs e)
        {
            if (JouerSon != null)
                JouerSon(this, e);
        }
    }
}
