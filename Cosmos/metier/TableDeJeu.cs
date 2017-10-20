using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    class TableDeJeu
    {
        private int phase;
        private bool joueurActifEst1;

        public event PropertyChangedEventHandler modifPropriete; 

        // Deck des joueurs
        public Deck DeckJ1 { get; set; }
        public Deck DeckJ2 { get; set; }

        // Mains des joueurs
        List<Carte> LstMainJ1 { get; set; }
        List<Carte> LstMainJ2 { get; set; }

        List<Batiment> LstBatimentJ1 { get; set; } // Bâtiment du joueur 1, celui qui commence la parti
        List<Batiment> LstBatimentJ2 { get; set; }

        List<Unite> LstUniteJ1 { get; set; } // Liste des unités du joueurs 1, maximum de 3.
        List<Unite> LstUniteJ2 { get; set; }

        // Usine de recyclage des joueurs / Défausse
        List<Carte> LstUsineRecyclageJ1 { get; set; }
        List<Carte> LstUsineRecyclageJ2 { get; set; }

        public int Phase
        {
            get { return phase; }
            set
            {
                phase = value;
                if (modifPropriete != null)
                {
                    modifPropriete(this, new PropertyChangedEventArgs("Phase"));
                }

            }
        }
        public bool JoueurActifEst1
        {
            get { return joueurActifEst1; }
            set
            {
                joueurActifEst1 = value;
                if (modifPropriete != null)
                {
                    modifPropriete(this, new PropertyChangedEventArgs("JoueurActifEst1"));
                }

            }
        }


        // Constructeur de la table de jeu.
        // Celle-ci à besoin d'avoir les decks des deux joueurs pour mélanger ceux-ci et distribuer les mains de départs.
        public TableDeJeu( Deck deck1, Deck deck2)
        {
            DeckJ1 = new Deck(deck1);
            DeckJ2 = new Deck(deck2);

            LstBatimentJ1 = new List<Batiment>();
            LstBatimentJ2 = new List<Batiment>();

            LstUniteJ1 = new List<Unite>();
            LstUniteJ2 = new List<Unite>();

            LstUsineRecyclageJ1 = new List<Carte>();
            LstUsineRecyclageJ1 = new List<Carte>();

            Phase = 1; // La partie commence en phase "1", c'est à dire la phase de ressource. Il n'y a pas de phase 0.

            JoueurActifEst1 = true; // Il y a deux joueur : Joueur 1 et joueur 2. Lorsque ce bool est vrai, c'est au joueur 1 et vice-versa.


        }

        public bool validerCoup(Carte carteAJouer, Joueur leJoueur)
        {
            Ressource temp = new Ressource(0, 0, 0);

            // Si suite à la soustraction les ressources du joueurs sont à zéro ou plus, le coup est valide.
            if ( leJoueur.Active - carteAJouer.Cout > temp ) 
            {
                return true;
            }
            return false;
        }

        public void JouerCarte(Carte carteAJouer)
        {
            // Valider si le joueur a les ressources nécessaire
            // Valider ailleur ? La table connait pas les joueur en ce moment

            // Jouer la carte


        }
        /// <summary>
        /// Pige une carte dans le deck spécifié pour l'ajouter à la main du joueur propriétaire du deck
        /// </summary>
        /// <param name="leDeck"></param>
        /// <param name="estJoueur1"></param>
        public void PigerCarte(Deck leDeck, bool estJoueur1)
        {
            if (estJoueur1)
            {
                LstMainJ1.Add(leDeck.CartesDuDeck[0]);
                leDeck.CartesDuDeck.RemoveAt(0);
            }
            else
            {
                LstMainJ2.Add(leDeck.CartesDuDeck[0]);
                leDeck.CartesDuDeck.RemoveAt(0);
            }
        }

        public void AvancerPhase()
        {
            if(Phase != 4)
            {
                Phase++;
            }
            else
            {
                Phase = 1;
            }
        }

        /// <summary>
        /// Mélange le deck de façon aléatoire. L'algo est médiocre, TODO tester.
        /// </summary>
        public Deck BrasserDeck(Deck leDeck)
        {
            Deck aBrasser = new Deck(leDeck);
            var rnd = new Random();
            aBrasser.CartesDuDeck.OrderBy(item => rnd.Next());
            return aBrasser;
            /* Algo alternatif probablement plus efficace
                int n = list.Count;
                Random rnd = new Random();
                while (n > 1) {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;              
            */
        }



    }
}
