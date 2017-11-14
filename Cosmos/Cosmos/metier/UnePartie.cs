using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class UnePartie
    {

        #region Propriétés
        public Joueur Joueur1 { get; set; }
        public Joueur Joueur2 { get; set; }
        // TODO TableDeJeu
        //public TableDeJeu Table { get; set; }
        #endregion
        #region Constructeur
        public UnePartie(Joueur un, Joueur deux)
        {
            Joueur1 = un;
            Joueur2 = deux;
            //TODO: Table de jeu
            // TableDeJeu Table = new TableDeJeu(paramêtres);
        }
        #endregion
    }
}
