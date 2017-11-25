using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cosmos.metier.TrousseGlobale;

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
        #region Propriétés
        private int phase;
        private bool joueurActifEst1;
        private int nbTourComplet;

        public event PropertyChangedEventHandler PropertyChanged;
        public Effet AExecuter { get; set; }
        // Les deux joueurs
        private Joueur joueur1;
        private Joueur joueur2;
        // Mains des joueurs
        public List<Carte> LstMainJ1 { get; set; }

        public List<Carte> LstMainJ2 { get; set; }
        // Champ constructions
        public ChampConstructions ChampConstructionsJ1 { get; set; }
        public ChampConstructions ChampConstructionsJ2 { get; set; }

        // Champs de bataille
        public ChampBatailleUnites ChampBatailleUnitesJ1 { get; set; }
        public ChampBatailleUnites ChampBatailleUnitesJ2 { get; set; }


        // Usine de recyclage des joueurs / Défausse
        public List<Carte> LstUsineRecyclageJ1 { get; set; }
        public List<Carte> LstUsineRecyclageJ2 { get; set; }
        // Nombre de tour.
        public int NbTourComplet
        {
            get { return nbTourComplet; }
            set
            {
                nbTourComplet = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NbTourComplet"));
                }

            }
        }
        //Phase du jeu.
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
        // Permet de savoir si c'est le tour du joueur 1.
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
            LstUsineRecyclageJ2 = new List<Carte>();

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
                    return true;
            }
            else
            {
                aJouer = LstMainJ2[index];
                if (joueur2.RessourceActive - aJouer.Cout > temp)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Fonction qui permet de jouer une carte
        /// </summary>
        /// <param name="index">Position de la carte dans la main du joueur</param>
        /// <param name="position">Position dans le champs de bataille</param>
        public void JouerCarte(int index, int position)
        {
            // Le coup à pas été validé                 
            Carte aJouer;
            AExecuter = null;

            // Enlever la carte de la main du joueur et la mettre à l'endroit qu'elle va
            if (joueurActifEst1)
            {
                aJouer = LstMainJ1[index];

                Joueur1.RessourceActive -= aJouer.Cout;
                if (aJouer is Unite)
                {
                    ChampBatailleUnitesJ1.AjouterAuChamp(aJouer, position);
                    if (aJouer.EffetCarte != null)
                        AExecuter = aJouer.EffetCarte;
                }
                else if (aJouer is Batiment)
                {
                    ChampConstructionsJ1.AjouterAuChamp(aJouer);
                }
                else if (aJouer is Gadget)
                {
                    if (aJouer.EffetCarte != null)
                        AExecuter = aJouer.EffetCarte;
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
                    ChampBatailleUnitesJ2.AjouterAuChamp(aJouer, position);
                    if (aJouer.EffetCarte != null)
                        AExecuter = aJouer.EffetCarte;
                }
                if (aJouer is Batiment)
                {
                    ChampConstructionsJ2.AjouterAuChamp(aJouer);
                }
                if (aJouer is Gadget)
                {
                    if (aJouer.EffetCarte != null)
                        AExecuter = aJouer.EffetCarte;
                    LstUsineRecyclageJ2.Add(aJouer);
                }
                // On enleve la carte de la main
                LstMainJ2.Remove(aJouer);
                // Refresh all 
                RefreshAllEventArgs p = new RefreshAllEventArgs();
                TrousseGlobale TG = new TrousseGlobale();
                TG.OnRefreshAll(p);
            }
            if (AExecuter != null)
                ExecuterEffet();
        }

        private void ExecuterEffet()
        {
            if (AExecuter.Type == "impact")
            {
                // Si pas le nombre de cible n'est pas important, donc frappe tous ce qu'il y a dans cible.
                if (AExecuter.getCible() == 10 || AExecuter.getCible() == 11 || AExecuter.getNbCible() == 0)
                    ExecuterImpact();
                else if ( joueurActifEst1 )
                {
                    if (ChoixEffetPossible(AExecuter.getCible()) >= AExecuter.getNbCible())
                    {
                        // Lancer un evenement pour que parti le catch.
                        ChoisirCibleEventArgs p = new ChoisirCibleEventArgs(AExecuter.getCible(), AExecuter.getNbCible());
                        TrousseGlobale TG = new TrousseGlobale();
                        TG.OnChoisirCible(p);
                    }
                    else if (ChoixEffetPossible(AExecuter.getCible()) < AExecuter.getNbCible() && ChoixEffetPossible(AExecuter.getCible()) >0)
                    {
                        // Lancer un evenement pour que parti le catch.
                        ChoisirCibleEventArgs p = new ChoisirCibleEventArgs(AExecuter.getCible(), ChoixEffetPossible(AExecuter.getCible()));
                        TrousseGlobale TG = new TrousseGlobale();
                        TG.OnChoisirCible(p);
                    }
                    
                }
            }
            /*
                Cible
                0	Tout le monde
                1	Tout les enemies
                2	Tout les allié
                3	Tout les bâtiments
                4	Bâtiments Enemies
                5	Bâtiments Allié
                6	Tout les Unités
                7	Unités Enemies
                8	Unités Allié
                9	Tout les Hero
                10	Hero Enemies
                11	Hero Allié
                12	Tout les Unités/Hero
                13	Unités/Hero Enemies
                14	Unités/Hero Allié
                15	Tout les Unités/Bâtiments
                16	Unités/Bâtiments Enemies
                17	Unités/Bâtiments Allié
                18	Tout les bâtiments/hero
                19	Bâtiments/Hero Enemies
                20	Bâtiments/Hero Allié

                Valeur	
                0	Destroy
                1 à 60	Quantité de dégats

                NbCible	
                0	Tout le monde
                1-2-3-4-etc...	La quantité exacte.

                */
        }

        public void ExecuterEffetFinTour()
        {
            List<Effet> effetsAExecuter = new List<Effet>();
            ChampBatailleUnites unites = null;
            ChampConstructions batiments = null;
            Joueur receveur = null;
            if (!JoueurActifEst1)
            {
                unites = ChampBatailleUnitesJ1;
                batiments = ChampConstructionsJ1;
                receveur = Joueur2;
            }
            else
            {
                unites = ChampBatailleUnitesJ2;
                batiments = ChampConstructionsJ2;
                receveur = Joueur1;
            }
            if (unites.Champ1 != null && unites.Champ1.EffetCarte != null)
            {
                if (unites.Champ1.EffetCarte.Type == "radiation")
                {
                    effetsAExecuter.Add(unites.Champ1.EffetCarte);
                }
            }
            if (unites.Champ2 != null && unites.Champ2.EffetCarte != null)
            {
                if (unites.Champ2.EffetCarte.Type == "radiation")
                {
                    effetsAExecuter.Add(unites.Champ2.EffetCarte);
                }
            }
            if (unites.Champ3 != null && unites.Champ3.EffetCarte != null)
            {
                if (unites.Champ3.EffetCarte.Type == "radiation")
                {
                    effetsAExecuter.Add(unites.Champ3.EffetCarte);
                }
            }
            ExecuterEffets(effetsAExecuter,receveur);
        }

        private void ExecuterEffets(List<Effet> effetsAExecuter, Joueur receveur)
        {
            foreach (Effet unEffet in effetsAExecuter)
            {
                if (unEffet is Radiation)
                {
                    receveur.PointDeBlindage -= unEffet.getValeur();
                }
            }
        }

        public bool EffetPossible(int index)
        {
            Carte laCarte = LstMainJ1[index];
            if (laCarte.EffetCarte != null)
            {
                AExecuter = laCarte.EffetCarte;
                if ( AExecuter.Type == "impact" && ChoixEffetPossible(AExecuter.getNbCible()) > 0)
                    return true;
            }
            return false;
        }
        private int ChoixEffetPossible(int cible)
        {
            int resultat = 0;

            if (AExecuter.getCible() == 10 || AExecuter.getCible() == 9 || AExecuter.getCible() == 0 || AExecuter.getCible() == 1 || AExecuter.getCible() == 12 || AExecuter.getCible() == 13 || AExecuter.getCible() == 18 || AExecuter.getCible() == 19)
                resultat++;
            if (AExecuter.getCible() == 11 || AExecuter.getCible() == 9 || AExecuter.getCible() == 0 || AExecuter.getCible() == 2 || AExecuter.getCible() == 12 || AExecuter.getCible() == 14 || AExecuter.getCible() == 18 || AExecuter.getCible() == 20)
                resultat++;
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 1 || AExecuter.getCible() == 3 || AExecuter.getCible() == 4 || AExecuter.getCible() == 15 || AExecuter.getCible() == 16 || AExecuter.getCible() == 18 || AExecuter.getCible() == 19)
                resultat += ChampConstructionsJ2.EspaceOccupe();
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 2 || AExecuter.getCible() == 3 || AExecuter.getCible() == 5 || AExecuter.getCible() == 15 || AExecuter.getCible() == 17 || AExecuter.getCible() == 18 || AExecuter.getCible() == 20)
                resultat += ChampConstructionsJ1.EspaceOccupe();
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 1 || AExecuter.getCible() == 6 || AExecuter.getCible() == 7 || AExecuter.getCible() == 12 || AExecuter.getCible() == 13 || AExecuter.getCible() == 15 || AExecuter.getCible() == 16)
                resultat += ChampBatailleUnitesJ2.EspaceOccupe();
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 2 || AExecuter.getCible() == 6 || AExecuter.getCible() == 8 || AExecuter.getCible() == 12 || AExecuter.getCible() == 14 || AExecuter.getCible() == 15 || AExecuter.getCible() == 17)
                resultat += ChampBatailleUnitesJ1.EspaceOccupe();

            return resultat;
        }

        private void ExecuterImpact()
        {
            Joueur Attaquant, Defenseur;
            ChampConstructions ConstructionAttaquant, ConstructionDefenseur;
            ChampBatailleUnites BatailleAttaquant, BatailleDefenseur;
            if (JoueurActifEst1)
            {
                Attaquant = Joueur1;
                Defenseur = Joueur2;
                ConstructionAttaquant = ChampConstructionsJ1;
                ConstructionDefenseur = ChampConstructionsJ2;
                BatailleAttaquant = ChampBatailleUnitesJ1;
                BatailleDefenseur = ChampBatailleUnitesJ2;
            }
            else
            {
                Attaquant = Joueur2;
                Defenseur = Joueur1;
                ConstructionAttaquant = ChampConstructionsJ2;
                ConstructionDefenseur = ChampConstructionsJ1;
                BatailleAttaquant = ChampBatailleUnitesJ2;
                BatailleDefenseur = ChampBatailleUnitesJ1;
            }
            if (AExecuter.getCible() == 10 || AExecuter.getCible() == 9 || AExecuter.getCible() == 0 || AExecuter.getCible() == 1 || AExecuter.getCible() == 12 || AExecuter.getCible() == 13 || AExecuter.getCible() == 18 || AExecuter.getCible() == 19)
                Defenseur.PointDeBlindage -= AExecuter.getValeur();
            if (AExecuter.getCible() == 11 || AExecuter.getCible() == 9 || AExecuter.getCible() == 0 || AExecuter.getCible() == 2 || AExecuter.getCible() == 12 || AExecuter.getCible() == 14 || AExecuter.getCible() == 18 || AExecuter.getCible() == 20)
                Attaquant.PointDeBlindage -= AExecuter.getValeur();
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 1 || AExecuter.getCible() == 3 || AExecuter.getCible() == 4 || AExecuter.getCible() == 15 || AExecuter.getCible() == 16 || AExecuter.getCible() == 18 || AExecuter.getCible() == 19)
            {
                // TODO: Changer .Defense
                if (ConstructionDefenseur.Champ1 != null)
                    ConstructionDefenseur.Champ1.Defense -= AExecuter.getValeur();
                if (ConstructionDefenseur.Champ2 != null)
                    ConstructionDefenseur.Champ2.Defense -= AExecuter.getValeur();
                if (ConstructionDefenseur.Champ3 != null)
                    ConstructionDefenseur.Champ3.Defense -= AExecuter.getValeur();
                if (ConstructionDefenseur.Champ4 != null)
                    ConstructionDefenseur.Champ4.Defense -= AExecuter.getValeur();
            }
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 2 || AExecuter.getCible() == 3 || AExecuter.getCible() == 5 || AExecuter.getCible() == 15 || AExecuter.getCible() == 17 || AExecuter.getCible() == 18 || AExecuter.getCible() == 20)
            {
                // TODO: Changer Defense
                if (ConstructionAttaquant.Champ1 != null)
                    ConstructionAttaquant.Champ1.Defense -= AExecuter.getValeur();
                if (ConstructionAttaquant.Champ2 != null)
                    ConstructionAttaquant.Champ2.Defense -= AExecuter.getValeur();
                if (ConstructionAttaquant.Champ3 != null)
                    ConstructionAttaquant.Champ3.Defense -= AExecuter.getValeur();
                if (ConstructionAttaquant.Champ4 != null)
                    ConstructionAttaquant.Champ4.Defense -= AExecuter.getValeur();
            }
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 1 || AExecuter.getCible() == 6 || AExecuter.getCible() == 7 || AExecuter.getCible() == 12 || AExecuter.getCible() == 13 || AExecuter.getCible() == 15 || AExecuter.getCible() == 16)
            {
                if (BatailleDefenseur.Champ1 != null)
                    BatailleDefenseur.VieChamp1 -= AExecuter.getValeur();
                if (BatailleDefenseur.Champ2 != null)
                    BatailleDefenseur.VieChamp2 -= AExecuter.getValeur();
                if (BatailleDefenseur.Champ3 != null)
                    BatailleDefenseur.VieChamp3 -= AExecuter.getValeur();
            }
            if (AExecuter.getCible() == 0 || AExecuter.getCible() == 2 || AExecuter.getCible() == 6 || AExecuter.getCible() == 8 || AExecuter.getCible() == 12 || AExecuter.getCible() == 14 || AExecuter.getCible() == 15 || AExecuter.getCible() == 17)
            {
                if (BatailleAttaquant.Champ1 != null)
                    BatailleAttaquant.VieChamp1 -= AExecuter.getValeur();
                if (BatailleAttaquant.Champ2 != null)
                    BatailleAttaquant.VieChamp2 -= AExecuter.getValeur();
                if (BatailleAttaquant.Champ3 != null)
                    BatailleAttaquant.VieChamp3 -= AExecuter.getValeur();
            }
            DetruireBatiment();
            DetruireUnite();
        }

        /// <summary>
        /// Permet d'avancer les phases du jeu.
        /// </summary>
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
                }
                else
                {
                    joueurActifEst1 = true;
                }
            }
            PhaseChangeEventArgs p = new PhaseChangeEventArgs();
            TrousseGlobale TG = new TrousseGlobale();
            TG.OnPhaseChange(p);
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
        /// <param name="joueurUn">Affecte le joueur 1?</param>
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
        /// <summary>
        /// Pige une carte selon le joueur choisi.
        /// </summary>
        /// <param name="joueurUn">Affecte le joueur 1?</param>
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
        /// Piger une carte selon le tour.
        /// </summary>
        public void PigerCarte()
        {
            if (JoueurActifEst1)
            {
                if (Joueur1.DeckAJouer.NbCarteDeck == 0 )
                    Joueur1.UtiliserRecyclage(LstUsineRecyclageJ1);
                if (LstMainJ1.Count < 8)
                    LstMainJ1.Add(Joueur1.PigerCarte());
            }
            else
            {
                if (Joueur2.DeckAJouer.NbCarteDeck == 0)
                    Joueur2.UtiliserRecyclage(LstUsineRecyclageJ2);
                if (LstMainJ2.Count < 8)
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
        /// <summary>
        /// Genere une liste de coup valide pour l'AI.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Fonction de jouer une carte pour l'ai
        /// </summary>
        /// <param name="index"></param>
        public void JouerCarteAI(int index)
        {
            Carte aJouer;

            aJouer = LstMainJ2[index];

            Joueur2.RessourceActive -= aJouer.Cout;

            if (aJouer is Unite)
            {
                // TODO Décider ou jouer une carte via le flag du AI
                ChampBatailleUnitesJ2.AjouterAuChamp(aJouer, 1);
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
        /// <summary>
        /// Fonction qui retourne si la carte est un unité ou non.
        /// </summary>
        /// <param name="indexCarteZoomer"></param>
        /// <returns></returns>
        public bool CarteAJouerEstUnite(int indexCarteZoomer)
        {
            if (JoueurActifEst1)
            {
                if (LstMainJ1[indexCarteZoomer] is Unite)
                    return true;
                else
                    return false;
            }
            else
            {
                if (LstMainJ2[indexCarteZoomer] is Unite)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// Fonction qui dit au AI que c'est sont tour.
        /// </summary>
        public void NotifyAi()
        {
            if (!joueurActifEst1 && (Phase == 2 || Phase == 3))
            {
                Notify(); // Permet de dire au AI que c'est à son tour.
            }
        }
        /// <summary>
        /// Fonction qui dit au 2 joueurs de piger une carte.
        /// </summary>
        public void PigerCartes()
        {
            LstMainJ1.Add(Joueur1.PigerCarte());
            LstMainJ2.Add(Joueur2.PigerCarte());

        }
        /// <summary>
        /// Fonction pour executer une attaque.
        /// </summary>
        /// <param name="champ1">Est-ce que le champ 1 attaque?</param>
        /// <param name="champ2">Est-ce que le champ 2 attaque?</param>
        /// <param name="champ3">Est-ce que le champ 3 attaque?</param>
        public void ExecuterAttaque(bool champ1, bool champ2, bool champ3)
        {
            ChampBatailleUnites attaquant;
            ChampBatailleUnites defenseur;
            Joueur joueurDefense;
            if (JoueurActifEst1)
            {
                attaquant = ChampBatailleUnitesJ1;
                defenseur = ChampBatailleUnitesJ2;
                joueurDefense = Joueur2;
                if (ChampBatailleUnitesJ1.EstEnPreparationChamp1)
                    champ1 = false;
                if (ChampBatailleUnitesJ1.EstEnPreparationChamp2)
                    champ2 = false;
                if (ChampBatailleUnitesJ1.EstEnPreparationChamp3)
                    champ3 = false;
            }
            else
            {
                attaquant = ChampBatailleUnitesJ2;
                defenseur = ChampBatailleUnitesJ1;
                joueurDefense = Joueur1;
                if (ChampBatailleUnitesJ2.EstEnPreparationChamp1)
                    champ1 = false;
                if (ChampBatailleUnitesJ2.EstEnPreparationChamp2)
                    champ2 = false;
                if (ChampBatailleUnitesJ2.EstEnPreparationChamp3)
                    champ3 = false;
            }
            Tirer(attaquant, defenseur, joueurDefense, champ1, champ2, champ3);

        }
        /// <summary>
        /// Fonction qui detruit les batiments des joueurs selon leur points de vie
        /// </summary>
        public void DetruireBatiment()
        {
            List<Batiment> DetruitJoueur1;
            List<Batiment> DetruitJoueur2;
            DetruitJoueur1 = ChampConstructionsJ1.DetruireBatiments();
            DetruitJoueur2 = ChampConstructionsJ2.DetruireBatiments();
            foreach (Batiment unBatiment in DetruitJoueur1)
            {
                LstUsineRecyclageJ1.Add(unBatiment);
            }
            foreach (Batiment unBatiment in DetruitJoueur2)
            {
                LstUsineRecyclageJ2.Add(unBatiment);
            }
        }
        /// <summary>
        /// Fonction qui construit une liste des effets des batiments du joueur dont c'est le tour.
        /// </summary>
        public void EffetBatiments()
        {
            List<Effet> lstEffet = new List<Effet>();
            Joueur Actif;
            Joueur Passif;
            if (JoueurActifEst1)
            {
                Actif = Joueur1;
                Passif = Joueur2;
                lstEffet = ChampConstructionsJ1.RetournerEffets();
            }
            else
            {
                Actif = Joueur2;
                Passif = Joueur1;
                lstEffet = ChampConstructionsJ2.RetournerEffets();
            }
            ExecuterEffetsBatiments(Actif, Passif, lstEffet);
        }
        /// <summary>
        /// Fonction qui execute la liste des effets des batiments
        /// </summary>
        /// <param name="actif"></param>
        /// <param name="passif"></param>
        /// <param name="lstEffet"></param>
        private void ExecuterEffetsBatiments(Joueur actif, Joueur passif, List<Effet> lstEffet)
        {
            foreach (Effet unEffet in lstEffet)
            {
                if (unEffet.Type == "gainParTour")
                {
                    actif.RessourceActive += unEffet.GetRessourceJoueur();
                    passif.RessourceActive += unEffet.GetRessourceAdversaire();
                }
            }
        }

        /// <summary>
        /// Fonction qui détruit un unité
        /// </summary>
        public void DetruireUnite()
        {
            List<Unite> DetruiteJoueur1;
            List<Unite> DetruiteJoueur2;
            DetruiteJoueur1 = ChampBatailleUnitesJ1.DetruireUnite();
            DetruiteJoueur2 = ChampBatailleUnitesJ2.DetruireUnite();
            foreach (Unite unUnite in DetruiteJoueur1)
            {
                LstUsineRecyclageJ1.Add(unUnite);
            }
            foreach (Unite unUnite in DetruiteJoueur2)
            {
                LstUsineRecyclageJ2.Add(unUnite);
            }
        }
        /// <summary>
        /// Fonction qui rend les unités prêt à attaquer.
        /// </summary>
        public void PreparerTroupes()
        {
            if (!joueurActifEst1)
            {
                ChampBatailleUnitesJ1.Preparer();
            }
            else
            {
                ChampBatailleUnitesJ2.Preparer();
            }
        }
        /// <summary>
        /// Mets à jour les points de vie des unités sur le champs de bataille.
        /// </summary>
        /// <param name="attaquant">Le champs de bataille de l'attaquant</param>
        /// <param name="defenseur">Le champs de bataille du défenseur</param>
        /// <param name="joueurDefense">Le joueur qui reçoit l'attaque</param>
        /// <param name="champ1">Si on attaque ou pas sur le champs 1</param>
        /// <param name="champ2">Si on attaque ou pas sur le champs 2</param>
        /// <param name="champ3">Si on attaque ou pas sur le champs 3</param>
        private void Tirer(ChampBatailleUnites attaquant, ChampBatailleUnites defenseur, Joueur joueurDefense, bool champ1, bool champ2, bool champ3)
        {
            if (champ1 && attaquant.Champ1 != null)
            {
                if (defenseur.Champ1 != null)
                {
                    attaquant.VieChamp1 -= defenseur.AttChamp1;
                    defenseur.VieChamp1 -= attaquant.AttChamp1;
                }
                else
                    joueurDefense.PointDeBlindage -= attaquant.AttChamp1;
            }
            if (champ2 && attaquant.Champ2 != null)
            {
                if (defenseur.Champ2 != null)
                {
                    attaquant.VieChamp2 -= defenseur.AttChamp2;
                    defenseur.VieChamp2 -= attaquant.AttChamp2;
                }
                else
                    joueurDefense.PointDeBlindage -= attaquant.AttChamp2;
            }
            if (champ3 && attaquant.Champ3 != null)
            {
                if (defenseur.Champ3 != null)
                {
                    attaquant.VieChamp3 -= defenseur.AttChamp3;
                    defenseur.VieChamp3 -= attaquant.AttChamp3;
                }
                else
                    joueurDefense.PointDeBlindage -= attaquant.AttChamp3;
            }
        }

        public void ExecuterImpact(List<int> choix)
        {
            // Héros
            if (choix.Contains(100))
                Joueur1.PointDeBlindage -= AExecuter.getValeur();
            if (choix.Contains(200))
                Joueur2.PointDeBlindage -= AExecuter.getValeur();
            // Bâtiments
            if (choix.Contains(111))
                ChampConstructionsJ1.Champ1.Defense -= AExecuter.getValeur();
            if (choix.Contains(112))
                ChampConstructionsJ1.Champ2.Defense -= AExecuter.getValeur();
            if (choix.Contains(113))
                ChampConstructionsJ1.Champ3.Defense -= AExecuter.getValeur();
            if (choix.Contains(114))
                ChampConstructionsJ1.Champ4.Defense -= AExecuter.getValeur();
            if (choix.Contains(211))
                ChampConstructionsJ2.Champ1.Defense -= AExecuter.getValeur();
            if (choix.Contains(212))
                ChampConstructionsJ2.Champ2.Defense -= AExecuter.getValeur();
            if (choix.Contains(213))
                ChampConstructionsJ2.Champ3.Defense -= AExecuter.getValeur();
            if (choix.Contains(214))
                ChampConstructionsJ2.Champ4.Defense -= AExecuter.getValeur();
            // Unité
            if (choix.Contains(121))
                ChampBatailleUnitesJ1.VieChamp1 -= AExecuter.getValeur();
            if (choix.Contains(122))
                ChampBatailleUnitesJ1.VieChamp2 -= AExecuter.getValeur();
            if (choix.Contains(123))
                ChampBatailleUnitesJ1.VieChamp3 -= AExecuter.getValeur();
            if (choix.Contains(221))
                ChampBatailleUnitesJ2.VieChamp1 -= AExecuter.getValeur();
            if (choix.Contains(222))
                ChampBatailleUnitesJ2.VieChamp2 -= AExecuter.getValeur();
            if (choix.Contains(223))
                ChampBatailleUnitesJ2.VieChamp3 -= AExecuter.getValeur();
            DetruireUnite();
            DetruireBatiment();
        }

        public bool EspaceUniteEstDisponible()
        {
            if (JoueurActifEst1)
                return ChampBatailleUnitesJ1.EspaceDisponible();

            return ChampBatailleUnitesJ2.EspaceDisponible();
        }

        public bool EspaceBatimentEstDisponible()
        {
            if (JoueurActifEst1)
                return ChampConstructionsJ1.EspaceDisponible();

            return ChampConstructionsJ2.EspaceDisponible();
        }

        public bool CarteAJouerEstGadget(int indexCarteZoomer)
        {
            if (JoueurActifEst1)
            {
                if (LstMainJ1[indexCarteZoomer] is Gadget)
                    return true;
                else
                    return false;
            }
            else
            {
                if (LstMainJ2[indexCarteZoomer] is Gadget)
                    return true;
                else
                    return false;
            }
        }
    }
}
