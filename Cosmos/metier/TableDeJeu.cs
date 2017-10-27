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
        private Joueur joueurActif;

        public event PropertyChangedEventHandler modifPropriete;

        // Les deux joueurs
        private Joueur joueur1;
        private Joueur joueur2;


        // Deck des joueurs
        public List<Carte> DeckJ1 { get; set; }
        public List<Carte> DeckJ2 { get; set; }

        // Mains des joueurs
        public List<Carte> LstMainJ1 { get; set; }
        public List<Carte> LstMainJ2 { get; set; }

        public List<Batiment> LstBatimentJ1 { get; set; } // Bâtiment du joueur 1, celui qui commence la parti
        public List<Batiment> LstBatimentJ2 { get; set; }

        public List<Unite> LstUniteJ1 { get; set; } // Liste des unités du joueurs 1, maximum de 3.
        public List<Unite> LstUniteJ2 { get; set; }

        // Usine de recyclage des joueurs / Défausse
        public List<Carte> LstUsineRecyclageJ1 { get; set; }
        public List<Carte> LstUsineRecyclageJ2 { get; set; }

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
        /*
        public Joueur JoueurActif
        {
            get { return joueurActif; }
            set
            {
                joueurActif = value;
                if (modifPropriete != null)
                {
                    modifPropriete(this, new PropertyChangedEventArgs("JoueurActif"));
                }

            }
        }*/

        public Joueur Joueur1
        {
            get { return joueur1; }
            set
            {
                joueur1 = value;
                if (modifPropriete != null)
                {
                    modifPropriete(this, new PropertyChangedEventArgs("Joueur1"));
                }

            }
        }

        public Joueur Joueur2
        {
            get { return joueur2; }
            set
            {
                joueur2 = value;
                if (modifPropriete != null)
                {
                    modifPropriete(this, new PropertyChangedEventArgs("Joueur2"));
                }

            }
        }

        // Constructeur de la table de jeu.
        // Celle-ci à besoin d'avoir les decks des deux joueurs pour mélanger ceux-ci et distribuer les mains de départs.
        public TableDeJeu(List<Carte> deck1, List<Carte> deck2)
        {
            DeckJ1 = new List<Carte>(deck1);
            DeckJ2 = new List<Carte>(deck2);

            LstMainJ1 = new List<Carte>();
            LstMainJ2 = new List<Carte>();

            LstBatimentJ1 = new List<Batiment>();
            LstBatimentJ2 = new List<Batiment>();

            LstUniteJ1 = new List<Unite>();
            LstUniteJ2 = new List<Unite>();

            LstUsineRecyclageJ1 = new List<Carte>();
            LstUsineRecyclageJ1 = new List<Carte>();

            Phase = 1; // La partie commence en phase "1", c'est à dire la phase de ressource. Il n'y a pas de phase 0.

            //TODO use only 1
            JoueurActifEst1 = true; // Il y a deux joueur : Joueur 1 et joueur 2. Lorsque ce bool est vrai, c'est au joueur 1 et vice-versa.
            //joueurActif = joueur1;



        }
        /// <summary>
        /// Retourne vrai si le coup est valide
        /// </summary>
        /// <param name="carteAJouer"></param>
        /// <param name="leJoueur"></param>
        /// <returns></returns>
        public bool validerCoup(Carte carteAJouer, Joueur leJoueur)
        {
            Ressource temp = new Ressource(0, 0, 0);

            // Si suite à la soustraction les ressources du joueurs sont à zéro ou plus, le coup est valide.
            if ( leJoueur.RessourceActive - carteAJouer.Cout > temp ) 
            {
                return true;
            }
            return false;
        }

        public void JouerCarte(Carte carteAJouer,bool joueurActifEst1 )
        {
            // Le coup à déjà été validé rendu ici                  

            // Enlever la carte de la main du joueur et la mettre à l'endroit qu'elle va
            if (joueurActifEst1)
            {
                // On enleve les ressources au joueurs
                joueur1.RessourceActive -= carteAJouer.Cout;

                if (carteAJouer is Unite)
                {
                    LstMainJ1.Remove(carteAJouer);
                    LstUniteJ1.Add((Unite)carteAJouer);
                }
                if (carteAJouer is Batiment)
                {
                    LstMainJ1.Remove(carteAJouer);
                    LstBatimentJ1.Add((Batiment)carteAJouer);
                }
                if (carteAJouer is Gadget)
                {
                    LstMainJ1.Remove(carteAJouer);
                    LstUsineRecyclageJ1.Add(carteAJouer);
                }
            }
            else
            {
                // On enleve les ressources au joueurs
                joueur2.RessourceActive -= carteAJouer.Cout;

                if (carteAJouer is Unite)
                {
                    LstMainJ2.Remove(carteAJouer);
                    LstUniteJ2.Add((Unite)carteAJouer);
                }
                if (carteAJouer is Batiment)
                {
                    LstMainJ2.Remove(carteAJouer);
                    LstBatimentJ2.Add((Batiment)carteAJouer);
                }
                if (carteAJouer is Gadget)
                {
                    LstMainJ2.Remove(carteAJouer);
                    LstUsineRecyclageJ2.Add(carteAJouer);
                }
            }                                 
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
                //LstMainJ1.Add(leDeck.CartesDuDeck[0]);
                //leDeck.CartesDuDeck.RemoveAt(0);
            }
            else
            {
                //LstMainJ2.Add(leDeck.CartesDuDeck[0]);
                //leDeck.CartesDuDeck.RemoveAt(0);
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
        public void BrasserDeck(List<Carte> leDeck)
        {
            //var rnd = new Random();
            //leDeck.CartesDuDeck.OrderBy(item => rnd.Next());
            //return aBrasser;

            int n = leDeck.Count();
            var rnd = new Random();

            while( n > 1 )
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                Carte temp = leDeck[k];
                leDeck[k] = leDeck[n];
                leDeck[n] = temp;
            }


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
        /// <summary>
        /// Fonction pour défausser une carte de la main d'un joueur jusqu'à l'usine de recyclage
        /// </summary>
        /// <param name="carteADeffausser"></param>
        /// <param name="estJoueur1"></param>
        public void DeffausserCarte( Carte carteADeffausser, bool estJoueur1 )
        {
            if(estJoueur1)
            {
                LstMainJ1.Remove(carteADeffausser);
                LstUsineRecyclageJ1.Add(carteADeffausser);
            }
            else
            {
                LstMainJ2.Remove(carteADeffausser);
                LstUsineRecyclageJ2.Add(carteADeffausser);
            }
        }
        /// <summary>
        /// Deux unités font un combat et se font des dégats. Cette fonctionne assume qu'il n'y a aucun effet sur les cartes
        /// </summary>
        /// <param name="unite1"></param>
        /// <param name="unite2"></param>
        public void CombatUniteAucunEffet(Unite unite1, Unite unite2)
        {
            unite1.Defense -= unite2.Attaque;
            unite2.Defense -= unite1.Attaque;

            // Si la defense de l'unité est a zero ou moins elle est détruite.
            if( unite1.Defense < 1)
            {
                LstUniteJ1.Remove(unite1);
            }

            if (unite2.Defense < 1)
            {
                LstUniteJ2.Remove(unite2);
            }
        }


    }
}
