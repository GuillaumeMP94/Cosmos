using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    public class TableDeJeu : INotifyPropertyChanged
    {
        #region Code relié au IObserver

        List<IObserver<TableDeJeu>> observers;

        // TODO commentaire
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<TableDeJeu>> _observers;
            private IObserver<TableDeJeu> _observer;

            public Unsubscriber(List<IObserver<TableDeJeu>> observers, IObserver<TableDeJeu> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<TableDeJeu> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        /// <summary>
        /// Permet de détecter les évènements et permettre au programme de continuer
        /// </summary>
        private void Notify()
        {
            foreach (IObserver<TableDeJeu> ob in observers)
            {
                ob.OnNext(this);
            }
        }
        #endregion

        private int phase;
        private bool joueurActifEst1;
        private int nbTourComplet;

        public event PropertyChangedEventHandler PropertyChanged;

        // Les deux joueurs
        private Joueur joueur1;
        private Joueur joueur2;
        #region Propriétés
        // Mains des joueurs
        public List<Carte> LstMainJ1 { get; set; }
        public List<Carte> LstMainJ2 { get; set; }


        //public List<Batiment> LstBatimentJ1 { get; set; } // Bâtiment du joueur 1, celui qui commence la parti
        //public List<Batiment> LstBatimentJ2 { get; set; }
        public ChampConstructions ChampConstructionsJ1 { get; set; }
        public ChampConstructions ChampConstructionsJ2 { get; set; }

        //public List<Unite> LstUniteJ1 { get; set; } // Liste des unités du joueurs 1, maximum de 3.
        //public List<Unite> LstUniteJ2 { get; set; }
        public ChampBatailleUnites ChampBatailleUnitesJ1 { get; set; }
        public ChampBatailleUnites ChampBatailleUnitesJ2 { get; set; }


        // Usine de recyclage des joueurs / Défausse
        public List<Carte> LstUsineRecyclageJ1 { get; set; }
        public List<Carte> LstUsineRecyclageJ2 { get; set; }

        public int NbTourComplet
        {
            get { return phase; }
            set
            {
                nbTourComplet = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NbTourComplet"));
                }

            }
        }

        public int Phase
        {
            get { return phase; }
            set
            {
                phase = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Phase"));
                }

            }
        }
        public bool JoueurActifEst1
        {
            get { return joueurActifEst1; }
            set
            {
                joueurActifEst1 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("JoueurActifEst1"));
                }

            }
        }

        public Joueur Joueur1
        {
            get { return joueur1; }
            set
            {
                joueur1 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Joueur1"));
                }

            }
        }
        public void PigerCartes()
        {
            LstMainJ1.Add(Joueur1.PigerCarte());
            LstMainJ2.Add(Joueur2.PigerCarte());

        }

        public Joueur Joueur2
        {
            get { return joueur2; }
            set
            {
                joueur2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Joueur2"));
                }

            }
        }

        public void ExecuterAttaque(bool champ1, bool champ2, bool champ3)
        {
            if (champ1 && ChampBatailleUnitesJ1.Champ1 != null)
            {
                if (ChampBatailleUnitesJ2.Champ1 != null)
                {
                    ChampBatailleUnitesJ1.Champ1 = ChampBatailleUnitesJ1.Champ1 - ChampBatailleUnitesJ2.Champ1;
                    ChampBatailleUnitesJ2.Champ2 = ChampBatailleUnitesJ2.Champ1 - ChampBatailleUnitesJ1.Champ1;
                }
                else
                {
                    Joueur2.PointDeBlindage = Joueur2.PointDeBlindage - ChampBatailleUnitesJ1.Champ1.Attaque;
                }
            }
        }
        #endregion
        #region Constructeurs
        // Constructeur de la table de jeu.
        // Celle-ci à besoin d'avoir les decks des deux joueurs pour mélanger ceux-ci et distribuer les mains de départs.
        public TableDeJeu(Joueur joueurUn, Joueur joueurDeux)
        {
            // Initialise la liste d'observateurs.
            observers = new List<IObserver<TableDeJeu>>();

            Joueur1 = joueurUn;
            Joueur2 = joueurDeux;

            LstMainJ1 = new List<Carte>();
            LstMainJ2 = new List<Carte>();


            ChampConstructionsJ1 = new ChampConstructions();
            ChampConstructionsJ2 = new ChampConstructions();

            ChampBatailleUnitesJ1 = new ChampBatailleUnites();
            ChampBatailleUnitesJ2 = new ChampBatailleUnites();


            LstUsineRecyclageJ1 = new List<Carte>();
            LstUsineRecyclageJ1 = new List<Carte>();

            Phase = 1; // La partie commence en phase "1", c'est à dire la phase de ressource. Il n'y a pas de phase 0.

            // Aucun tour ont été completé au début de la partie
            NbTourComplet = 0;

            JoueurActifEst1 = true; // Il y a deux joueur : Joueur 1 et joueur 2. Lorsque ce bool est vrai, c'est au joueur 1 et vice-versa.

            // Brasser les deck
            BrasserDecks();

            // Donner une main à chaque joueurs 
            int compteurNbCarte = 0;
            while (compteurNbCarte != 6)
            {
                PigerCartes();

                compteurNbCarte++;
            }

        }
        #endregion
        /// <summary>
        /// Fonction qui attribu les ressources par rapport au niveau des ressources du joueur
        /// </summary>
        /// <param name="joueurUn"></param>
        public void AttribuerRessourceLevel()
        {
            if (JoueurActifEst1)
            {
                Joueur1.ModifierRessource(true, Joueur1.LevelRessource);
            }
            else
            {
                Joueur2.ModifierRessource(true, Joueur2.LevelRessource);
            }
        }
        /// <summary>
        /// Fonction qui modifie les ressources
        /// </summary>
        /// <param name="joueurUn">Est-ce le joueur1?</param>
        /// <param name="addition">Est-ce une addition?</param>
        /// <param name="valeurs">Valeurs qui modifie les ressources</param>
        public void ModifierRessource(bool joueurUn, bool addition, Ressource valeurs)
        {
            if (joueurUn)
            {
                Joueur1.ModifierRessource(addition, valeurs);
            }
            else
            {
                Joueur2.ModifierRessource(addition, valeurs);
            }
        }

        /// <summary>
        /// Retourne vrai si le coup est valide
        /// </summary>
        /// <param name="carteAJouer"></param>
        /// <param name="leJoueur"></param>
        /// <returns></returns>
        public bool validerCoup(int index)
        {
            Ressource temp = new Ressource(-1, -1, -1);
            Carte aJouer;

            // Si suite à la soustraction les ressources du joueurs sont à zéro ou plus, le coup est valide.
            if (JoueurActifEst1)
            {
                aJouer = LstMainJ1[index];
                if (joueur1.RessourceActive - aJouer.Cout > temp)
                {
                    return true;
                }
            }
            else
            {
                aJouer = LstMainJ2[index];
                if (joueur2.RessourceActive - aJouer.Cout > temp)
                {
                    return true;
                }
            }
            return false;
        }

        public void JouerCarte(int index)
        {
            // Le coup à pas été validé                 
            Carte aJouer;

            // Enlever la carte de la main du joueur et la mettre à l'endroit qu'elle va
            if (joueurActifEst1)
            {
                aJouer = LstMainJ1[index];

                Joueur1.RessourceActive -= aJouer.Cout;
                if (aJouer is Unite)
                {
                    ChampBatailleUnitesJ1.AjouterAuChamp(aJouer, 1);
                }
                else if (aJouer is Batiment)
                {
                    ChampConstructionsJ1.AjouterAuChamp(aJouer);
                }
                else if (aJouer is Gadget)
                {
                    LstUsineRecyclageJ1.Add(aJouer);
                }
                // On enleve la carte de la main
                LstMainJ1.Remove(aJouer);

            }
            else
            {
                aJouer = LstMainJ2[index];

                // On enleve les ressources au joueurs
                Joueur2.RessourceActive -= aJouer.Cout;

                if (aJouer is Unite)
                {
                    ChampBatailleUnitesJ1.AjouterAuChamp(aJouer, 1);
                }
                if (aJouer is Batiment)
                {
                    ChampConstructionsJ1.AjouterAuChamp(aJouer);
                }
                if (aJouer is Gadget)
                {
                    LstUsineRecyclageJ2.Add(aJouer);
                }
                // On enleve la carte de la main
                LstMainJ2.Remove(aJouer);

            }


        }

        public void AvancerPhase()
        {
            if (Phase != 4)
            {
                Phase++;
            }
            else
            {
                Phase = 1;

                if (JoueurActifEst1)
                {
                    joueurActifEst1 = false;
                    Notify(); // Permet de dire au AI que c'est à son tour.
                }
                else
                {
                    joueurActifEst1 = true;
                }
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

            while (n > 1)
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
        public void DeffausserCarte(Carte carteADeffausser, bool estJoueur1)
        {
            if (estJoueur1)
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
            if (unite1.Defense < 1)
            {
                //LstUniteJ1.Remove(unite1);
            }

            if (unite2.Defense < 1)
            {
                //LstUniteJ2.Remove(unite2);
            }
        }
        /// <summary>
        /// Brasse LES decks des joueurs
        /// </summary>
        public void BrasserDecks()
        {
            Joueur1.BrasserDeck();
            Joueur2.BrasserDeck();
        }
        /// <summary>
        /// Brasse le deck du joueur selon le tour
        /// </summary>
        public void BrasserDeck()
        {
            if (JoueurActifEst1)
            {
                Joueur1.BrasserDeck();
            }
            else
            {
                Joueur2.BrasserDeck();
            }
        }
        /// <summary>
        /// Brasse le deck du joueur choisi
        /// </summary>
        /// <param name="joueurUn"></param>
        public void BrasserDeck(bool joueurUn)
        {
            if (joueurUn)
            {
                Joueur1.BrasserDeck();
            }
            else
            {
                Joueur2.BrasserDeck();
            }
        }
        public void PigerCarte(bool joueurUn)
        {
            if (joueurUn)
            {
                LstMainJ1.Add(Joueur1.PigerCarte());
            }
            else
            {
                LstMainJ2.Add(Joueur2.PigerCarte());
            }
        }
        /// <summary>
        /// Permet de générer une liste de coup valide
        /// </summary>
        /// <returns>Liste des index des coup valide</returns>
        public List<int> listeCoupValideAI()
        {
            List<int> listeCoupPermis = new List<int>();

            int compteur = 0;

            foreach (Carte uneCarte in LstMainJ2)
            {
                if (validerCoup(compteur))
                {
                    listeCoupPermis.Add(compteur);
                }
                compteur++;
            }
            return listeCoupPermis;
        }

        public List<int> listeCoupValideUniteAI()
        {
            List<int> listeCoupPermis = new List<int>();

            int compteur = 0;

            foreach (Carte uneCarte in LstMainJ2)
            {
                if (validerCoup(compteur) && uneCarte is Unite)
                {
                    listeCoupPermis.Add(compteur);
                }
                compteur++;
            }
            return listeCoupPermis;
        }


        public void JouerCarteAI(int index)
        {
            Carte aJouer;

            aJouer = LstMainJ2[index];

            Joueur2.RessourceActive -= aJouer.Cout;

            if (aJouer is Unite)
            {
                // TODO Décider ou jouer une carte via le flag du AI
                ChampBatailleUnitesJ2.AjouterAuChamp(aJouer, -15 /*TODO ai.ChoixChampUnite */);
            }
            else if (aJouer is Batiment)
            {
                ChampConstructionsJ2.AjouterAuChamp(aJouer);
            }
            else if (aJouer is Gadget)
            {
                LstUsineRecyclageJ2.Add(aJouer);
            }
            // On enleve la carte de la main
            LstMainJ2.Remove(aJouer);


        }
    }
}
