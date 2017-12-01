using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Cosmos.metier
{
    public class AI : Joueur, IObserver<TableDeJeu>
    {
        #region Code relié au patron observateur

        private IDisposable unsubscriber;

        public void Subscribe(IObservable<TableDeJeu> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            // Implémentation obligatoire, might use, might not      
            MessageBox.Show("Le tour du AI est terminé");
        }

        public void OnError(Exception error)
        {
            // Implémentation obligatoire, might use, might not
            MessageBox.Show("Oops, une erreur est arrivée avec votre adversaire!");
        }

        public void OnNext(TableDeJeu jeu)
        {
            if (jeu.Phase == 2)
            {
                Task.Delay(1000).ContinueWith(_ =>
                {
                    //Phase principale
                    JouerCoup(jeu);

                });

            }
            if (jeu.Phase == 3)
            {
                Task.Delay(1000).ContinueWith(_ =>
                {

                    // Phase de combat
                    PhaseAttaqueAI(jeu);
                });
            }

        }
        #endregion

        private int difficulte;

        public int ChoixChampUnite { get; set; }
        public bool AttaqueChamp1 { get; set; }
        public bool AttaqueChamp2 { get; set; }
        public bool AttaqueChamp3 { get; set; }
        // Plus le strategie est haute, plus le AI est aggressif  ( Score de 1 à 5 )
        public int Strategie { get; set; }


        #region Propriétés
        public string Nom { get; set; }
        #endregion
        #region Constructeur
        public AI(int diff, Ressource debutLevelRessource, Deck deckAI)
            : base()
        {
            string nom = "";
            switch (diff)
            {
                case 1:
                    nom = "Robot Turenne";
                    break;
                case 2:
                    nom = "Hesells Thegardens";
                    break;
                case 3:
                    nom = "Charronitoman";
                    break;
                case 4:
                    nom = "Kesh Sleshall";
                    break;
                case 5:
                    nom = "Docteur Brown";
                    break;
            }
            Nom = nom;
            difficulte = diff;
            PointDeBlindage = 25;
            RessourceActive = new Ressource(0, 0, 0);
            LevelRessource = debutLevelRessource;
            DeckAJouer = deckAI;

            // Propriété pour que le AI keep track de quand il doit attaquer ou pas
            // De base il attaque toujours
            AttaqueChamp1 = true;
            AttaqueChamp2 = true;
            AttaqueChamp3 = true;

        }
        #endregion

        /// <summary>
        /// Le AI joue sont tour selon le niveau de difficulté auquel il a été innitialisé
        /// </summary>
        /// <param name="jeu">La table de jeu</param>
        private void JouerCoup(TableDeJeu jeu)
        {

            //ReinitialiserChampJouer();
            List<int> ListeCoupsPermis;
            List<int> ListeCoupsPermisUnite;
            List<int> ListeCoupEvaluer;
            Random rnd = new Random(DateTime.Now.Millisecond);

            ListeCoupsPermis = jeu.listeCoupValideAI();
            ListeCoupsPermisUnite = jeu.listeCoupValideUniteAI();

            switch (difficulte)
            {
                case 1:
                    /*-------------------------------------------------------------------------------------------------------------------------------------------------------
                                                                                                    NIVEAU 1
                    ------------------------------------------------------------------------------------------------------------------------------------------------------ */
                    #region
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            // Le AI joue n'importe quoi de valide n'importe où si c'est vide
                            // Qui sait, peut-être qu'il va vraiment bien jouer! 
                            // Par contre, le AI priorise les unités, puis batiment et/ou gadget
                            if (ListeCoupsPermisUnite.Count != 0)
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null
                                    && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite = rnd.Next(1, 3);
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    ChoixChampUnite = rnd.Next(1, 2);
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite = rnd.Next(2, 3);
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite = ((rnd.Next(1, 2) * 2) - 1);
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                {
                                    ChoixChampUnite = 1;
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    ChoixChampUnite = 2;
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite = 3;
                                    jeu.JouerCarte(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ChoixChampUnite);
                                }
                            }
                        }

                        ListeCoupsPermis = jeu.listeCoupValideAI();

                        // Rendu ici nous avons jouer une unité si possible
                        // On tente de jouer un bâtiment
                        if (ListeCoupsPermis.Count != 0)
                        {
                            foreach (int index in ListeCoupsPermis)
                            {
                                if (jeu.LstMainJ2[index] is Batiment)
                                {
                                    if (jeu.ChampConstructionsJ2.Champ1 == null
                                        || jeu.ChampConstructionsJ2.Champ2 == null
                                        || jeu.ChampConstructionsJ2.Champ3 == null
                                        || jeu.ChampConstructionsJ2.Champ4 == null)
                                    {
                                        jeu.JouerCarte(index, ChoixChampUnite);
                                        break;
                                    }
                                }
                                else if (jeu.LstMainJ2[index] is Gadget)
                                {
                                    //TODO implémentation des gadgets
                                    //Pour l'instant il se passe rien si jamais on tombe sur un gadget
                                }

                            }
                        }

                    }
                    break;
                #endregion
                case 2:
                    /*-------------------------------------------------------------------------------------------------------------------------------------------------------
                                                                                                    NIVEAU 2
                    ------------------------------------------------------------------------------------------------------------------------------------------------------ */
                    #region
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            if (AIPeutGagnerJouerCarte(ListeCoupsPermis, jeu))
                            {
                                // Rien à faire ici, la fonction à fait le nécessaire
                                // Nous allons sauté par dessus le code qui suit puisque le AI va gagner                             
                            }
                            else
                            {
                                // Premièrement, le AI essaie de jouer un bâtiment si nous sommes en début de parti. 
                                // Ceux-ci génère un avantage à long terme, mais deviennent éventuellement innutile
                                if (jeu.NbTourComplet < 4)
                                {
                                    foreach (int coup in ListeCoupsPermis)
                                    {
                                        if (jeu.LstMainJ2[coup] is Batiment)
                                        {
                                            jeu.JouerCarteAI(coup);
                                            // Rafraichir la liste des coup permis suite à un coup
                                            ListeCoupsPermis = jeu.listeCoupValideAI();
                                        }
                                    }
                                }
                                // Notre AI est toujours pas vraiment bon. Il joue encore n'importe quoi rendu ici     
                                // TODO répétition de code innutile                          
                                if (ListeCoupsPermisUnite.Count != 0)
                                {
                                    if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null
                                        && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        ChoixChampUnite = rnd.Next(1, 3);
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        ChoixChampUnite = rnd.Next(1, 2);
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        ChoixChampUnite = rnd.Next(2, 3);
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        ChoixChampUnite = ((rnd.Next(1, 2) * 2) - 1);
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {
                                        ChoixChampUnite = 1;
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        ChoixChampUnite = 2;
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        ChoixChampUnite = 3;
                                        jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                case 3:
                    /*-------------------------------------------------------------------------------------------------------------------------------------------------------
                                                                                                    NIVEAU 3
                    ------------------------------------------------------------------------------------------------------------------------------------------------------ */
                    #region
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            // Premièrement, le AI essaie de jouer un bâtiment si nous sommes en début de parti. 
                            // Ceux-ci génère un avantage à long terme, mais deviennent éventuellement innutile
                            if (jeu.NbTourComplet < 4)
                            {
                                foreach (int coup in ListeCoupsPermis)
                                {
                                    if (jeu.LstMainJ2[coup] is Batiment)
                                    {
                                        jeu.JouerCarte(coup, 0);
                                        // Rafraichir la liste des coup permis suite à un coup
                                        ListeCoupsPermis = jeu.listeCoupValideAI();
                                    }
                                }
                            }

                            if (ListeCoupsPermis.Count != 0)
                            {
                                // Maintenant, il est temps de vérifier si nous pouvons nous mettre dans le chemin de notre adversaire
                                // De plus, nous allons vérifier si nous avons moyen de vaincre dans un champ

                                foreach (int coup in ListeCoupsPermis)
                                {
                                    if (jeu.ChampBatailleUnitesJ1.Champ1 != null && ListeCoupsPermis.Count != 0 && jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {

                                        if (jeu.LstMainJ2[coup] is Unite)
                                        {
                                            if (jeu.LstMainJ2[coup].getAttaque() >= jeu.ChampBatailleUnitesJ1.Champ1.Defense)
                                            {
                                                //ChoixChampUnite = 1;
                                                jeu.JouerCarte(coup, 1);
                                                ListeCoupsPermis = jeu.listeCoupValideAI();
                                            }
                                        }
                                    }
                                    if (jeu.ChampBatailleUnitesJ1.Champ2 != null && ListeCoupsPermis.Count != 0 && jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {

                                        if (jeu.LstMainJ2[coup] is Unite)
                                        {
                                            if (jeu.LstMainJ2[coup].getAttaque() >= jeu.ChampBatailleUnitesJ1.Champ2.Defense)
                                            {
                                                //ChoixChampUnite = 2;
                                                jeu.JouerCarte(coup, 2);
                                                ListeCoupsPermis = jeu.listeCoupValideAI();
                                            }
                                        }
                                    }
                                    if (jeu.ChampBatailleUnitesJ1.Champ3 != null && ListeCoupsPermis.Count != 0 && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        if (jeu.LstMainJ2[coup] is Unite)
                                        {
                                            if (jeu.LstMainJ2[coup].getAttaque() >= jeu.ChampBatailleUnitesJ1.Champ3.Defense)
                                            {
                                                //ChoixChampUnite = 3;
                                                jeu.JouerCarte(coup, 3);
                                                ListeCoupsPermis = jeu.listeCoupValideAI();
                                            }
                                        }
                                    }
                                }

                                // Si il reste encore un coup permis ici, on joue une seule carte
                                if (ListeCoupsPermis.Count != 0)
                                {
                                    if (jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {
                                        //ChoixChampUnite = 1;
                                        jeu.JouerCarte(ListeCoupsPermis.First(), 1);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        //ChoixChampUnite = 2;
                                        jeu.JouerCarte(ListeCoupsPermis.First(), 2);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        //ChoixChampUnite = 3;
                                        jeu.JouerCarte(ListeCoupsPermis.First(), 3);
                                    }
                                    ListeCoupsPermis = jeu.listeCoupValideAI(); // why tho
                                }
                            }
                        }

                        jeu.AvancerPhase();

                    }
                    break;
                #endregion
                case 4:
                    /*-------------------------------------------------------------------------------------------------------------------------------------------------------
                                                                                                    NIVEAU 4
                    ------------------------------------------------------------------------------------------------------------------------------------------------------ */
                    #region
                    // TODO
                    // En ce moment, le AI peut seulement jouer une carte par tour
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            if (!(AIPeutGagnerJouerCarte(ListeCoupsPermis, jeu))) // AiPeutGagnerJouerCarte va jouer la carte si vrai
                            {
                                if (jeu.NbTourComplet > 3 && PossibiliteDefaiteUnite(jeu)) // si le AI peut perdre on joue en conséquence
                                {
                                    JouerEmpecherDefaite2(jeu, ListeCoupsPermis);
                                }
                                else
                                {
                                    ListeCoupEvaluer = EvaluerListeCoup(ListeCoupsPermis, jeu);

                                    int scoreMax = ListeCoupEvaluer.Max();
                                    int c = 0;
                                    int indexAJouer = 1;

                                    foreach (int i in ListeCoupEvaluer)
                                    {
                                        if (ListeCoupEvaluer[c] == scoreMax)
                                        {
                                            indexAJouer = ListeCoupsPermis[c];

                                            if (ListeCoupEvaluer[c] > 0)
                                            {
                                                ChoixChampUnite = ChoisirOuJouerCarte(jeu, indexAJouer);
                                                jeu.JouerCarte(indexAJouer, ChoixChampUnite);

                                                // Si je veux du code pour jouer une deuxième carte c'est ici.

                                                jeu.AvancerPhase();
                                                return;
                                            }
                                        }
                                        c++;
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                case 5:
                    /*-------------------------------------------------------------------------------------------------------------------------------------------------------
                                                                                                   NIVEAU 5
                   ------------------------------------------------------------------------------------------------------------------------------------------------------ */
                    #region
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                        }
                    }
                    break;
                    #endregion
            }

            // Rendu ici la phase principale est terminée
            jeu.AvancerPhase();

        }

        /// <summary>
        /// Fonction pour déterminer ou le AI veut jouer sa carte
        /// </summary>
        /// <param name="jeu"></param>
        /// <param name="indexAJouer"></param>
        /// <returns></returns>
        private int ChoisirOuJouerCarte(TableDeJeu jeu, int indexAJouer)
        {
            int champvide = DeterminerChampVide(jeu);

            // Il n'y a null part à jouer
            if (champvide >= 111000)
            {
                return -1;
            }

            // Si il n'y a qu'un seul endroit à jouer, on joue la
            // TODO : Évaluer si ça vaut la peine de jouer la pour potentiellement attendre un tour pour jouer.
            if (jeu.ChampBatailleUnitesJ2.Champ1 == null
                && jeu.ChampBatailleUnitesJ2.Champ2 != null
                && jeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                return 1;
            }
            else if (jeu.ChampBatailleUnitesJ2.Champ1 != null
                && jeu.ChampBatailleUnitesJ2.Champ2 == null
                && jeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                return 2;
            }
            else if (jeu.ChampBatailleUnitesJ2.Champ1 != null
                && jeu.ChampBatailleUnitesJ2.Champ2 != null
                && jeu.ChampBatailleUnitesJ2.Champ3 == null)
            {
                return 3;
            }

            // On vérifie si nous pouvons obliter un champ
            int choixOuJouer = PeutOblitererCombat(jeu, indexAJouer);

            if (choixOuJouer != -1)
            {
                return choixOuJouer;
            }

            Random rnd = new Random(DateTime.Now.Millisecond);

            // Gestion des effets
            if (jeu.LstMainJ2[indexAJouer].EffetCarte != null)
            {
                if (jeu.LstMainJ2[indexAJouer].EffetCarte.Type == "Radiation")
                {
                    #region              
                    switch (champvide)
                    {
                        // Si quelqu'un autre que Franck essaie de comprendre ce qui se passe ici, oubliez ça.
                        case 1:
                            return rnd.Next(2, 3);
                        case 10:
                            return ((rnd.Next(1, 2) * 2) - 1);
                        case 100:
                            return rnd.Next(1, 2);
                        case 11:
                            return 3;
                        case 101:
                            return 2;
                        case 110:
                            return 1;

                        case 1011:
                            return 3;
                        case 1010:
                            return 3;
                        case 1101:
                            return 2;
                        case 1001:
                            return 2;
                        case 1110:
                            return rnd.Next(2, 3);
                        case 1000:
                            return rnd.Next(2, 3);

                        case 10001:
                            return 3;
                        case 10011:
                            return 3;
                        case 11001:
                            return 3;
                        case 10100:
                            return 1;
                        case 10110:
                            return 1;
                        case 10010:
                            return ((rnd.Next(1, 2) * 2) - 1);

                        case 100000:
                            return rnd.Next(1, 2);
                        case 100100:
                            return rnd.Next(1, 2);
                        case 100001:
                            return 2;
                        case 100101:
                            return 2;
                        case 101101:
                            return 2;
                        case 101001:
                            return 2;
                        case 101000:
                            return 2;
                        case 110000:
                            return 1;
                        case 100010:
                            return 1;
                        case 110010:
                            return 1;
                        case 100110:
                            return 1;
                        case 110110:
                            return 1;


                        default:
                            return rnd.Next(1, 3);
                    }
                    #endregion
                }
                else if (jeu.LstMainJ2[indexAJouer].EffetCarte.Type == "AttaqueFurtive")
                {
                    //TODO
                }
                else if (jeu.LstMainJ2[indexAJouer].EffetCarte.Type == "TODO")
                {

                }
            }

            bool tradeFav1 = false;
            bool tradeFav2 = false;
            bool tradeFav3 = false;

            // Gerer les trade. Est-ce que je veux mettre ma carte dans un endroit ou je vais
            // amener à la destruction des deux cartes, ou est ce que je veux passer des dmg.
            if (jeu.ChampBatailleUnitesJ2.Champ1 == null
                && jeu.ChampBatailleUnitesJ1.Champ1 != null
                && jeu.ChampBatailleUnitesJ1.Champ1.getDefense() - jeu.LstMainJ2[indexAJouer].getAttaque() <= 0
                && jeu.LstMainJ2[indexAJouer].getDefense() - jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() <= 0
                && jeu.LstMainJ2[indexAJouer].getAttaque() < jeu.ChampBatailleUnitesJ1.Champ1.getAttaque())
            {
                tradeFav1 = true;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ2 == null
                && jeu.ChampBatailleUnitesJ1.Champ2 != null
                && jeu.ChampBatailleUnitesJ1.Champ2.getDefense() - jeu.LstMainJ2[indexAJouer].getAttaque() <= 0
                && jeu.LstMainJ2[indexAJouer].getDefense() - jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() <= 0
                && jeu.LstMainJ2[indexAJouer].getAttaque() < jeu.ChampBatailleUnitesJ1.Champ2.getAttaque())
            {
                tradeFav2 = true;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ3 == null
                && jeu.ChampBatailleUnitesJ1.Champ3 != null
                && jeu.ChampBatailleUnitesJ1.Champ3.getDefense() - jeu.LstMainJ2[indexAJouer].getAttaque() <= 0
                && jeu.LstMainJ2[indexAJouer].getDefense() - jeu.ChampBatailleUnitesJ1.Champ3.getAttaque() <= 0
                && jeu.LstMainJ2[indexAJouer].getAttaque() < jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
            {
                tradeFav3 = true;
            }

            // On commence par vérifier si il y a seulement un endroit ou un situation de trade favorable existe. 
            // Si oui on joue simplement la
            if (tradeFav1 == true
                && tradeFav2 == false
                && tradeFav3 == false)
            {
                return 1;
            }
            else if (tradeFav1 == false
                && tradeFav2 == true
                && tradeFav3 == false)
            {
                return 2;
            }
            else if (tradeFav1 == false
                && tradeFav2 == false
                && tradeFav3 == true)
            {
                return 3;
            }

            // La on a deux endroit ou trois, il faut en choisir une
            if (tradeFav1 == true      
                && tradeFav2 == true
                && tradeFav3 == false)
            {
                if (jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() == jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() )
                {
                    return ( rnd.Next(1,2) );
                }
                else if (jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ2.getAttaque())
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else if (tradeFav1 == false
                    && tradeFav2 == true
                    && tradeFav3 == true)
            {
                if (jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() == jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return (rnd.Next(2, 3));
                }
                else if (jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else if (tradeFav1 == true
                    && tradeFav2 == false
                    && tradeFav3 == true)
            {
                if (jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() == jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return (((rnd.Next(1, 2))*2)-1);
                }
                else if (jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            else if (tradeFav1 == true
                    && tradeFav2 == true
                    && tradeFav3 == true)
            {
                if (jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() == jeu.ChampBatailleUnitesJ1.Champ2.getAttaque()
                    && jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() == jeu.ChampBatailleUnitesJ1.Champ3.getAttaque()
                    && jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() == jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return (rnd.Next(1, 3));
                }
                else if (jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ2.getAttaque()
                    && jeu.ChampBatailleUnitesJ1.Champ1.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return 1;
                }
                else if (jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ1.getAttaque()
                    && jeu.ChampBatailleUnitesJ1.Champ2.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ3.getAttaque())
                {
                    return 2;
                }
                else if (jeu.ChampBatailleUnitesJ1.Champ3.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ1.getAttaque()
                    && jeu.ChampBatailleUnitesJ1.Champ3.getAttaque() > jeu.ChampBatailleUnitesJ1.Champ2.getAttaque())
                {
                    return 3;
                }
                else
                {
                    // TODO 
                    // Cette situation est tellement rare que je vais pas la faire. Si jamais j'ai du temps libre ça serait bien de
                    // la gerer correctement.
                    return (rnd.Next(1, 3));
                }
            }

            //Rendu ici, c'est basicly des situations que j'ai pas pensé à alors on va faire n'importe quoi.
            if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null
                && jeu.ChampBatailleUnitesJ2.Champ3 == null)
            {
                choixOuJouer = rnd.Next(1, 3);
            }
            else if (jeu.ChampBatailleUnitesJ2.Champ1 == null
                && jeu.ChampBatailleUnitesJ2.Champ2 == null)
            {
                choixOuJouer = rnd.Next(1, 2);
            }
            else if (jeu.ChampBatailleUnitesJ2.Champ2 == null
                && jeu.ChampBatailleUnitesJ2.Champ3 == null)
            {
                choixOuJouer = rnd.Next(2, 3);
            }
            else if (jeu.ChampBatailleUnitesJ2.Champ1 == null
                && jeu.ChampBatailleUnitesJ2.Champ3 == null)
            {
                choixOuJouer = ((rnd.Next(1, 2) * 2) - 1);
            }

            return choixOuJouer;
        }

        /// <summary>
        /// 0 = Tout est vide
        /// 1 =     1  0  0         1000   =  0  0  0
        ///         0  0  0                   1  0  0
        ///         
        /// 10 =    0  1  0         10000  =  0  0  0
        ///         0  0  0                   0  1  0
        ///         
        /// 100 =   0  0  1         100000 =  0  0  0
        ///         0  0  0                   0  0  1
        ///         
        /// 
        /// </summary>
        /// <param name="jeu"></param>
        /// <returns></returns>
        private int DeterminerChampVide(TableDeJeu jeu)
        {
            int DeterminerChampVide = 0;

            if (jeu.ChampBatailleUnitesJ1.Champ1 != null)
            {
                DeterminerChampVide += 1;
            }
            if (jeu.ChampBatailleUnitesJ1.Champ2 != null)
            {
                DeterminerChampVide += 10;
            }
            if (jeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                DeterminerChampVide += 100;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                DeterminerChampVide += 1000;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ2 != null)
            {
                DeterminerChampVide += 10000;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                DeterminerChampVide += 100000;
            }

            return DeterminerChampVide;
        }




        /// <summary>
        /// Si une carte permet de gagner, son index est retourner. De plus, certain "flag" sont trigger pour déterminer
        /// comment elle doit être joué
        /// </summary>
        /// <param name="listeCoupsPermis"></param>
        /// <param name="jeu"></param>
        /// <returns></returns>
        private bool AIPeutGagnerJouerCarte(List<int> listeCoupsPermis, TableDeJeu jeu)
        {
            int compteur = 0;
            // Si je trouve une carte qui gagne, la fonctionne se termine. Pas besoin de savoir si
            // plusieurs cartes peuvent gagner.
            foreach (Carte uneCarte in jeu.LstMainJ2)
            {
                // Si le coup est permit, nous allons valider si celui-ci est gagnant
                // Si la carte est un batiment, rien ne sert de la vérifier
                // TODO, si jamais la nature des batiment change, le code ici devrait être modifié
                if (listeCoupsPermis.Contains(compteur) && !(uneCarte is Batiment))
                {
                    if (uneCarte is Unite)
                    {
                        // Une carte avec célérité peut attaquer immédiatement
                        if (uneCarte.EffetCarte != null && uneCarte.EffetCarte.Type == "Célérité")
                        {
                            // Je vérifie si l'unité peut faire assez de dégat pour mettre l'adversaire a 0 ou moins. 
                            int totalPostAtt = jeu.Joueur1.PointDeBlindage - uneCarte.getAttaque();

                            if (totalPostAtt < 1)
                            {
                                if (jeu.ChampBatailleUnitesJ1.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                {
                                    ChoixChampUnite = 1; // Flag pour décider où jouer la carte par la suite
                                    jeu.JouerCarteAI(1);
                                    return true;

                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    ChoixChampUnite = 2;
                                    jeu.JouerCarteAI(2);
                                    return true;

                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ3 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite = 3;
                                    jeu.JouerCarteAI(3);
                                    return true;

                                }
                            }
                        }
                        // Si d'autre unité peuvent avoir un effet qui amene à la victoire
                        //
                        //
                        //
                        // Les inserer ici
                    }
                    if (uneCarte is Gadget)
                    {
                        // Si le joueur AI peut faire des dégât direct qui serait létal, il le fait

                        //TODO implémentation Gadget
                    }
                }
                compteur++;
            }
            return false;
        }

        /// <summary>
        /// Selon la difficulté, le AI va décider comment faire sa phase d'attaque
        /// </summary>
        /// <param name="jeu">La table de jeu</param>
        private void PhaseAttaqueAI(TableDeJeu jeu)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            ReinitialiserChampAttaque();
            switch (difficulte)
            {
                /* ---------------------------------------------------------------------------------------------- 
                                                            NIVEAU 1
                ---------------------------------------------------------------------------------------------- */
                case 1:
                    #region
                    // YOLO. L'AI attaque toujours avec toutes ses unités
                    AttaqueChamp1 = true;
                    AttaqueChamp2 = true;
                    AttaqueChamp3 = true;
                    break;

                // jeu.attaqueChamp1 = true
                // etc
                // TODO

                #endregion
                /* ---------------------------------------------------------------------------------------------- 
                                                            NIVEAU 2
                   ---------------------------------------------------------------------------------------------- */
                case 2:
                    #region
                    // Si l'unité est imblocable, elle attaque toujours
                    // SI il n'y a pas d'enemi devant, le AI attaque toujours
                    //
                    // De plus, le AI à une chance sur quatre de ne pas attaquer devant l'inconnu
                    // Chaque case n'ont pas le même "flag" de random par contre, il est impossible qu'il n'attaque pas avec plus qu'une unité                     
                    if (jeu.ChampBatailleUnitesJ2.Champ1 != null)
                    {
                        if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte != null)
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive")
                            {
                                AttaqueChamp1 = true;
                            }
                        }
                        if (jeu.ChampBatailleUnitesJ1.Champ1 == null
                            || rnd.Next(1, 4) != 1)
                        {
                            AttaqueChamp1 = true;
                        }
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ2 != null)
                    {
                        if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte != null)
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive")
                            {
                                AttaqueChamp2 = true;
                            }
                        }
                        if (jeu.ChampBatailleUnitesJ1.Champ2 == null
                            || rnd.Next(1, 4) != 2)
                        {
                            AttaqueChamp2 = true;
                        }
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ3 != null)
                    {
                        if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte != null)
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive")
                            {
                                AttaqueChamp3 = true;
                            }
                        }
                        if (jeu.ChampBatailleUnitesJ1.Champ3 == null
                            || rnd.Next(1, 4) != 3)
                        {
                            AttaqueChamp3 = true;
                        }
                    }

                    break;
                #endregion
                /* ---------------------------------------------------------------------------------------------- 
                                                            NIVEAU 3
                    ---------------------------------------------------------------------------------------------- */
                case 3:
                    #region
                    // Pour le plaisir de la chose, un peu de RNG est encore présent, mais pas beaucoup      
                    // De plus, si l'unité adverse a plus de defense et d'attaque le AI n'attaque pas. 
                    // Avant toute chose je vérifie
                    if (jeu.ChampBatailleUnitesJ2.Champ1 != null)
                    {
                        if (PeutOblitererCombatTableJeu(jeu, 1))
                        {
                            AttaqueChamp1 = true;
                        }
                        else if ((jeu.ChampBatailleUnitesJ1.Champ1.Attaque > jeu.ChampBatailleUnitesJ2.Champ1.Attaque
                            && jeu.ChampBatailleUnitesJ1.Champ1.Defense > jeu.ChampBatailleUnitesJ2.Champ1.Defense))
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte != null)
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive")
                                {
                                    AttaqueChamp1 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ1.Champ1 == null
                            || rnd.Next(1, 10) != 1)
                            {
                                AttaqueChamp1 = true;
                            }
                        }
                    }
                    // --------------------------- CHAMP 2 ----------------------------------------------------------
                    if (jeu.ChampBatailleUnitesJ2.Champ2 != null)
                    {
                        if (PeutOblitererCombatTableJeu(jeu, 2))
                        {
                            AttaqueChamp2 = true;
                        }
                        else if ((jeu.ChampBatailleUnitesJ1.Champ2.Attaque > jeu.ChampBatailleUnitesJ2.Champ2.Attaque
                            && jeu.ChampBatailleUnitesJ1.Champ2.Defense > jeu.ChampBatailleUnitesJ2.Champ2.Defense))
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte != null)
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive")
                                {
                                    AttaqueChamp2 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ1.Champ2 == null
                            || rnd.Next(1, 10) != 1)
                            {
                                AttaqueChamp2 = true;
                            }
                        }
                    }
                    // --------------------------- CHAMP 3 ----------------------------------------------------------
                    if (jeu.ChampBatailleUnitesJ2.Champ3 != null)
                    {
                        if (PeutOblitererCombatTableJeu(jeu, 3))
                        {
                            AttaqueChamp3 = true;
                        }
                        else if ((jeu.ChampBatailleUnitesJ1.Champ3.Attaque > jeu.ChampBatailleUnitesJ2.Champ3.Attaque
                            && jeu.ChampBatailleUnitesJ1.Champ3.Defense > jeu.ChampBatailleUnitesJ2.Champ3.Defense))
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte != null)
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive")
                                {
                                    AttaqueChamp3 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ1.Champ3 == null
                            || rnd.Next(1, 10) != 1)
                            {
                                AttaqueChamp3 = true;
                            }
                        }
                    }

                    break;
                #endregion
                /* ---------------------------------------------------------------------------------------------- 
                                                            NIVEAU 4
                    ---------------------------------------------------------------------------------------------- */
                case 4:
                    #region
                    // Plus la strategie a une valeur haute, plus le AI est aggressif
                    EvaluerConditionVictoire(jeu);

                    // Section de code applicable peu importe la stratégie
                    // Si un flag d'attaque est mit a true, plus tard nous allons sauté par dessus de grande section de code
                    if (jeu.ChampBatailleUnitesJ2.Champ1 != null)
                    {
                        if (jeu.ChampBatailleUnitesJ1.Champ1 == null
                            || PeutOblitererCombatTableJeu(jeu, 1))
                        {
                            AttaqueChamp1 = true;
                        }
                        else if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte != null)
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable")
                            {
                                AttaqueChamp1 = true;
                            }
                        }
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ2 != null)
                    {
                        if (jeu.ChampBatailleUnitesJ1.Champ2 == null
                            || PeutOblitererCombatTableJeu(jeu, 2))
                        {
                            AttaqueChamp2 = true;
                        }
                        else if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte != null)
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable")
                            {
                                AttaqueChamp2 = true;
                            }
                        }
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ3 != null)
                    {
                        if (jeu.ChampBatailleUnitesJ1.Champ3 == null
                            || PeutOblitererCombatTableJeu(jeu, 3))
                        {
                            AttaqueChamp3 = true;
                        }
                        else if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte != null)
                        {
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable")
                            {
                                AttaqueChamp3 = true;
                            }
                        }
                    }
                    // Section de choix d'attaque adaptatif à la stratégie
                    switch (Strategie)
                    {
                        case 1:
                            #region
                            // Pour l'instant rien
                            break;
                        #endregion
                        case 2:
                            #region
                            // Pour l'instant rien
                            break;
                        #endregion
                        case 3:
                            #region
                            if (!AttaqueChamp1
                                && jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type != "Radiation" &&
                                CombatEstProfitable(jeu, 1))
                            {
                                AttaqueChamp1 = true;
                            }
                            // ---------------------------- Champ 2 --------------------------------------
                            if (!AttaqueChamp2
                                && jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type != "Radiation" &&
                                CombatEstProfitable(jeu, 2))
                            {
                                AttaqueChamp2 = true;
                            }
                            // ---------------------------- Champ 3 --------------------------------------
                            if (!AttaqueChamp3
                                && jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type != "Radiation" &&
                                CombatEstProfitable(jeu, 3))
                            {
                                AttaqueChamp3 = true;
                            }
                            break;
                        #endregion
                        case 4:
                            #region
                            if (!AttaqueChamp1
                                && jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type != "Radiation" &&
                                CombatEstProfitable(jeu, 1))
                            {
                                AttaqueChamp1 = true;
                            }
                            // ---------------------------- Champ 2 --------------------------------------
                            if (!AttaqueChamp2
                                && jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type != "Radiation" &&
                                CombatEstProfitable(jeu, 2))
                            {
                                AttaqueChamp2 = true;
                            }
                            // ---------------------------- Champ 3 --------------------------------------
                            if (!AttaqueChamp3
                                && jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type != "Radiation" &&
                                CombatEstProfitable(jeu, 3))
                            {
                                AttaqueChamp3 = true;
                            }
                            #endregion
                            break;
                        case 5: // PRESS THE ATTACK AT ALL COST
                            #region
                            AttaqueChamp1 = true;
                            AttaqueChamp2 = true;
                            AttaqueChamp3 = true;
                            break;
                        #endregion
                        default: // Si jamais un bug survient, on attaque avec tout.
                            AttaqueChamp1 = true;
                            AttaqueChamp2 = true;
                            AttaqueChamp3 = true;
                            break;
                    }
                    break;
                #endregion
                /* ---------------------------------------------------------------------------------------------- 
                                                            NIVEAU 5
                    ---------------------------------------------------------------------------------------------- */
                case 5:
                    #region
                    break;
                    #endregion
            }

            // La phase d'attaque est terminé
            jeu.AvancerPhase();

        }
        /// <summary>
        /// Permet de déterminer si un combat est profitable à long terme dans des conditions non dramatique
        /// </summary>
        /// <param name="jeu"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool CombatEstProfitable(TableDeJeu jeu, int position)
        {
            // Je vérifie si je peut soit faire un échange ou faire un échange/oblitération sur deux tour
            if (position == 1)
            {
                if (jeu.ChampBatailleUnitesJ1.Champ1.Defense <= jeu.ChampBatailleUnitesJ2.Champ1.Attaque)
                {
                    return true;
                }
                else if ((jeu.ChampBatailleUnitesJ2.Champ1.Attaque * 2) > jeu.ChampBatailleUnitesJ1.Champ1.Defense
                    && jeu.ChampBatailleUnitesJ1.Champ1.Attaque <= jeu.ChampBatailleUnitesJ2.Champ1.Defense)
                {
                    return true;
                }
            }
            else if (position == 2)
            {
                if (jeu.ChampBatailleUnitesJ1.Champ2.Defense <= jeu.ChampBatailleUnitesJ2.Champ2.Attaque)
                {
                    return true;
                }
                else if ((jeu.ChampBatailleUnitesJ2.Champ2.Attaque * 2) > jeu.ChampBatailleUnitesJ1.Champ2.Defense
                    && jeu.ChampBatailleUnitesJ1.Champ2.Attaque <= jeu.ChampBatailleUnitesJ2.Champ2.Defense)
                {
                    return true;
                }
            }
            else
            {
                if (jeu.ChampBatailleUnitesJ1.Champ3.Defense <= jeu.ChampBatailleUnitesJ2.Champ3.Attaque)
                {
                    return true;
                }
                else if ((jeu.ChampBatailleUnitesJ2.Champ3.Attaque * 2) > jeu.ChampBatailleUnitesJ1.Champ3.Defense
                    && jeu.ChampBatailleUnitesJ1.Champ3.Attaque <= jeu.ChampBatailleUnitesJ2.Champ3.Defense)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// De base, le AI n'attaque pas et l'algo decide de changer pour l'offense si nécessaire
        /// </summary>
        private void ReinitialiserChampAttaque()
        {
            AttaqueChamp1 = false;
            AttaqueChamp2 = false;
            AttaqueChamp3 = false;
        }

        /// <summary>
        /// Fonction qui permet d'attribuer un score à chaque coup d'une main pour le AI niveau quatre
        /// </summary>
        /// <param name="listeCoupsPermis"></param>
        /// <returns>Liste de coup qui ont été évalué</returns>
        private List<int> EvaluerListeCoup(List<int> listeCoupsPermis, TableDeJeu jeu)
        {
            // Si le score a maxvalue, ça veut dire que le coup est le meilleur coup possible en ce moment
            // Si le score a minvalue, ça veut dire que le coup est le pire coup possible
            // Le AI sait rendu ici qu'il ne peut pas gagner puisque la première chose qu'il a fait était de déterminer si la victoire était possible.

            var listeCoupsPermisEvaluer = new List<int>();
            int score;
            Random rnd = new Random(DateTime.Now.Millisecond);

            // Je vérifie si l'adversaire peut gagner par le combat. Si oui, je vais aller dans une différente branche de réponse
            // De plus, pour sauver un peu de temps on regarde pas cette possibilité en début de partie
            //if (jeu.NbTourComplet > 3 && PossibiliteDefaiteUnite(jeu) )
            //{
            //    JouerEmpecherDefaite2(jeu, listeCoupsPermis);                
            //}
            //else
            //{
            foreach (int index in listeCoupsPermis)
            {
                score = 0;

                // Si le AI peut optimiser l'utilisation de ses ressources, le score augmente dramatiquement
                if (jeu.Joueur2.RessourceActive - jeu.LstMainJ2[index].Cout == new Ressource(0, 0, 0))
                {
                    score += 29;
                }
                // ------------------  Bâtiment -----------------------
                #region
                if (jeu.LstMainJ2[index] is Batiment && score != int.MaxValue)
                {

                    // Revenir ici lorsque le choix de deck serait fait TODO

                    if (jeu.NbTourComplet > 4)
                    {
                        score = int.MinValue;
                    }
                    else if (jeu.NbTourComplet == 0)
                    {
                        score = int.MaxValue;
                    }
                    else if (jeu.ChampConstructionsJ1.Champ1 == null && jeu.ChampConstructionsJ1.Champ2 == null)
                    {
                        score += 50 - (jeu.NbTourComplet * 10);
                    }
                    else
                    {
                        score += 45 - (jeu.NbTourComplet * 10);
                    }
                }
                #endregion
                // ------------------  Unité -----------------------
                #region
                if (jeu.LstMainJ2[index] is Unite && score != int.MaxValue)
                {

                    // Je vérifie si mon champ de bataille est completement vide
                    if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                    {
                        score += 31;
                    }
                    // Je vérifie si mon unité peut dramatiquement gagner une "lane"
                    if (PeutOblitererCombat(jeu, index) != -1)
                    {
                        score += 27;
                    }
                    switch (jeu.NbTourComplet)
                    {
                        // Chaque tour implique une approche différente pour le score des unités
                        case 0:
                            #region
                            if (jeu.ChampBatailleUnitesJ1.Champ1 == null && jeu.ChampBatailleUnitesJ1.Champ2 == null && jeu.ChampBatailleUnitesJ1.Champ3 == null)
                            {
                                score += 56; // Le score doit allé plus haut qu'un gadget
                            }
                            if (jeu.LstMainJ2[index].EffetCarte != null
                                && jeu.LstMainJ2[index].EffetCarte.Type == "TODO")
                            {
                                score += 16;
                            }
                            break;
                            #endregion
                        default:
                            #region
                            // Le mode par défaut représente le "late game"
                            score += jeu.LstMainJ2[index].Cout.Charronite * 3;
                            score += jeu.LstMainJ2[index].Cout.BarilNucleaire * 3;
                            score += jeu.LstMainJ2[index].Cout.AlainDollars * 3;
                            break;
                            #endregion
                    }


                }
                #endregion
                // ------------------  Gadget -----------------------
                #region
                if (jeu.LstMainJ2[index] is Gadget && score != int.MaxValue)
                {
                    // TODO 
                    // Pour l'instant, un gadget a un score assigné au hasard                    
                    score = rnd.Next(-100, 14);
                }
                #endregion

                // On rajoute le score dans la nouvelle liste
                listeCoupsPermisEvaluer.Add(score);
            }
            //}

            return listeCoupsPermisEvaluer;
        }

        private void JouerEmpecherDefaite(TableDeJeu jeu, List<int> listeCoupsPermis)
        {
            //bool peutPerdre = true;
            int nbBloque = 0;
            List<int> lstCoupEvaluer = new List<int>();
            List<int> lstCoupAJouer = new List<int>();
            List<int> lstDMG = new List<int>();

            if (jeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                lstDMG.Add(jeu.ChampBatailleUnitesJ1.Champ1.Attaque);
                nbBloque++;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ2 != null)
            {
                lstDMG.Add(jeu.ChampBatailleUnitesJ1.Champ2.Attaque);
                nbBloque++;
            }
            if (jeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                lstDMG.Add(jeu.ChampBatailleUnitesJ1.Champ3.Attaque);
                nbBloque++;
            }

            // TODO
            // Pour l'instant j'évalue les carte de la même façon même si la défaite approche
            lstCoupEvaluer = EvaluerListeCoup(listeCoupsPermis, jeu);

            // Il faut enlever les cartes qui ne sont pas des unité
            foreach (int index in lstCoupEvaluer)
            {
                if (!(jeu.LstMainJ2[index] is Unite))
                {
                    lstCoupEvaluer.Remove(index);
                }
            }

            // On vérifie si un nombre de coups nécessaires sont possible. Si non : La défaite est innévitable on sort d'ici.
            if (lstCoupEvaluer.Count() - nbBloque < 0)
            {
                return;
            }

            int scoreMax = lstCoupEvaluer.Max();
            int c = 0;
            int indexAJouer = 1;

            foreach (int i in lstCoupEvaluer)
            {
                if (lstCoupEvaluer[c] == scoreMax)
                {
                    indexAJouer = i;
                }
                c++;
            }
            // Vérifier si je dois jouer 1, 2 ou 3 unité
            switch (nbBloque)
            {
                case 1:
                    if (jeu.ChampBatailleUnitesJ2.Champ1 != null && jeu.ChampBatailleUnitesJ1.Champ1.Attaque == lstDMG.Max())
                    {
                        ChoixChampUnite = 1;
                        jeu.JouerCarteAI(indexAJouer);

                    }
                    else if (jeu.ChampBatailleUnitesJ2.Champ1 != null && jeu.ChampBatailleUnitesJ1.Champ2.Attaque == lstDMG.Max())
                    {
                        ChoixChampUnite = 2;
                        jeu.JouerCarteAI(indexAJouer);
                    }
                    else if (jeu.ChampBatailleUnitesJ2.Champ1 != null && jeu.ChampBatailleUnitesJ1.Champ3.Attaque == lstDMG.Max())
                    {
                        ChoixChampUnite = 3;
                        jeu.JouerCarteAI(indexAJouer);
                    }
                    break;
                case 2:
                    // Il faut que je trouve une combinaison de deux cartes jouable

                    // Fonctionnera pas, me faut les index
                    lstCoupAJouer = trouverLstUniteJouable(jeu, lstCoupEvaluer, 2);


                    // Nous avons deux cartes, on va essayer de pas faire n'importe quoi avec
                    //if (PeutOblitererCombat(jeu, )  )
                    //{

                    //}

                    break;
                case 3:

                    break;
                default:
                    return;
                    // Pas supposé venir ici, si jamais on arrive ici je vais simplement terminé la fonction             
            }
        }

        private void JouerEmpecherDefaite2(TableDeJeu jeu, List<int> listeCoupsPermis)
        {

            List<int> lstCoupEvaluer = new List<int>();
            List<int> lstCoupAJouer = new List<int>();
            List<int> lstDMG = new List<int>();

            lstCoupEvaluer = EvaluerListeCoup(listeCoupsPermis, jeu);

            int scoreMax = lstCoupEvaluer.Max();
            int c = 0;
            int indexAJouer = 1;

            foreach (int i in lstCoupEvaluer)
            {
                if (lstCoupEvaluer[c] == scoreMax)
                {
                    indexAJouer = i;
                }
                c++;
            }

            // Block pour si il y a seulement une carte qui threaten une défaite
            if (jeu.ChampBatailleUnitesJ1.Champ1 != null
                && jeu.ChampBatailleUnitesJ1.Champ2 == null
                && jeu.ChampBatailleUnitesJ1.Champ3 == null)
            {
                jeu.JouerCarte(indexAJouer, 1);
            }
            else if (jeu.ChampBatailleUnitesJ1.Champ1 == null
                && jeu.ChampBatailleUnitesJ1.Champ2 != null
                && jeu.ChampBatailleUnitesJ1.Champ3 == null)
            {
                jeu.JouerCarte(indexAJouer, 2);
            }
            else if (jeu.ChampBatailleUnitesJ1.Champ1 == null
                && jeu.ChampBatailleUnitesJ1.Champ2 == null
                && jeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                jeu.JouerCarte(indexAJouer, 3);
            }


        }

        /// <summary>
        /// Retourne une liste de la première combinaison de carte jouable trouvé
        /// </summary>
        /// <param name="jeu"></param>
        /// <param name="lstCoupEvaluer"></param>
        /// <param name="nbCarteAJouer"></param>
        /// <returns></returns>
        private List<int> trouverLstUniteJouable(TableDeJeu jeu, List<int> lstCoupEvaluer, int nbCarteAJouer)
        {
            List<int> lstAJouer = new List<int>();

            if (nbCarteAJouer == 2)
            {
                foreach (int indexX in lstCoupEvaluer)
                {
                    foreach (int indexY in lstCoupEvaluer)
                    {
                        if (indexX != indexY
                            && (jeu.LstMainJ2[indexX].Cout + jeu.LstMainJ2[indexY].Cout) > new Ressource(-1, -1, -1))
                        {
                            lstAJouer.Add(indexX);
                            lstAJouer.Add(indexY);

                            return lstAJouer;
                        }
                    }
                }
            }
            else
            {
                foreach (int indexX in lstCoupEvaluer)
                {
                    foreach (int indexY in lstCoupEvaluer)
                    {
                        foreach (int indexZ in lstCoupEvaluer)
                        {
                            if (indexX != indexY && indexX != indexZ && indexY != indexZ
                                && (jeu.LstMainJ2[indexX].Cout + jeu.LstMainJ2[indexY].Cout + jeu.LstMainJ2[indexZ].Cout) > new Ressource(-1, -1, -1))
                            {
                                lstAJouer.Add(indexX);
                                lstAJouer.Add(indexY);
                                lstAJouer.Add(indexZ);

                                return lstAJouer;
                            }
                        }

                    }
                }
            }
            return lstAJouer;
        }

        /// <summary>
        /// Vérifie si il existe un champ vide du coté du joueur
        /// </summary>
        /// <param name="jeu"></param>
        /// <returns></returns>
        private bool ExisteChampUniteVideEnemi(TableDeJeu jeu)
        {
            if (jeu.ChampBatailleUnitesJ1.Champ1 == null)
            {
                return true;
            }
            else if (jeu.ChampBatailleUnitesJ1.Champ2 == null)
            {
                return true;
            }
            else if (jeu.ChampBatailleUnitesJ1.Champ3 == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de déterminer si une "lane" peut être dramatiquement gagner en jouant une carte dans celle-ci
        /// </summary>
        /// <param name="jeu"></param>
        /// <returns>Retourne -1 si aucun champ permet de completement gagner une "lane"
        /// Sinon retourne la "lane" ou un tel combat peut-être gagné </returns>
        private int PeutOblitererCombat(TableDeJeu jeu, int index)
        {
            int score1 = 0;
            int score2 = 0;
            int score3 = 0;

            // Je vérifie ici si lors d'un combat mon unité va non seulement détruire l'enemi, mais en plus survivre
            if (jeu.ChampBatailleUnitesJ1.Champ1 != null
                && jeu.ChampBatailleUnitesJ2.Champ1 == null
                && jeu.ChampBatailleUnitesJ1.Champ1.Defense <= jeu.LstMainJ2[index].getAttaque()
                && jeu.ChampBatailleUnitesJ1.Champ1.Attaque < jeu.LstMainJ2[index].getDefense())
            {
                score1 = jeu.ChampBatailleUnitesJ1.Champ1.Defense
                       + jeu.ChampBatailleUnitesJ1.Champ1.Attaque;
            }
            if (jeu.ChampBatailleUnitesJ1.Champ2 != null
                && jeu.ChampBatailleUnitesJ2.Champ2 == null
                && jeu.ChampBatailleUnitesJ1.Champ2.Defense <= jeu.LstMainJ2[index].getAttaque()
                && jeu.ChampBatailleUnitesJ1.Champ2.Attaque < jeu.LstMainJ2[index].getDefense())
            {
                score2 = jeu.ChampBatailleUnitesJ1.Champ2.Defense
                       + jeu.ChampBatailleUnitesJ1.Champ2.Attaque;
            }
            if (jeu.ChampBatailleUnitesJ1.Champ3 != null
                && jeu.ChampBatailleUnitesJ2.Champ3 == null
                && jeu.ChampBatailleUnitesJ1.Champ3.Defense <= jeu.LstMainJ2[index].getAttaque()
                 && jeu.ChampBatailleUnitesJ1.Champ3.Attaque < jeu.LstMainJ2[index].getDefense())
            {
                score3 = jeu.ChampBatailleUnitesJ1.Champ3.Defense
                       + jeu.ChampBatailleUnitesJ1.Champ3.Attaque;
            }

            // Si on entre ici, une oblitération est possible à quelque part.
            if (score1 != 0 || score2 != 0 || score3 != 0)
            {
                Random rnd = new Random(DateTime.Now.Millisecond);

                // Lane 1 est la meilleur oblitération
                if (score1 > score2 && score1 > score3)
                {
                    return 1;
                }
                else if (score2 > score3 && score2 > score1)
                {
                    return 2;
                }
                else if (score3 > score2 && score3 > score1 )
                {
                    return 3;
                }
                else
                {
                    if (score1 == score3 && score2 == score3)
                    {
                        return rnd.Next(1, 3);
                    }
                    else if (score1 == score2
                        && score1 > score3)
                    {
                        return rnd.Next(1, 2);
                    }
                    else if (score2 == score3
                        && score2 > score1)
                    {
                        return rnd.Next(2, 3);
                    }
                    else if(score1 == score3
                        && score1 > score2)
                    {
                        return ((rnd.Next(1, 2) * 2) - 1);
                    }                
                }
            }

            return -1;
        }
        /// <summary>
        /// Permet de déterminer si un combat peut être gagner sans perte
        /// </summary>
        /// <param name="jeu"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool PeutOblitererCombatTableJeu(TableDeJeu jeu, int position)
        {
            // Je vérifie ici si lors d'un combat mon unité va non seulement détruire l'enemi, mais en plus survivre
            if (jeu.ChampBatailleUnitesJ1.Champ1 != null
                && jeu.ChampBatailleUnitesJ2.Champ1 != null
                && position == 1
                && jeu.ChampBatailleUnitesJ1.Champ1.Defense <= jeu.ChampBatailleUnitesJ2.Champ1.Attaque
                && jeu.ChampBatailleUnitesJ2.Champ1.Defense > jeu.ChampBatailleUnitesJ1.Champ1.Attaque)
            {
                return true;
            }
            else if (jeu.ChampBatailleUnitesJ1.Champ2 != null
                && jeu.ChampBatailleUnitesJ2.Champ2 != null
                && position == 2
                && jeu.ChampBatailleUnitesJ1.Champ2 != null
                && jeu.ChampBatailleUnitesJ1.Champ2.Defense <= jeu.ChampBatailleUnitesJ2.Champ2.Attaque
                && jeu.ChampBatailleUnitesJ2.Champ2.Defense > jeu.ChampBatailleUnitesJ1.Champ2.Attaque)
            {
                return true;
            }
            else if (jeu.ChampBatailleUnitesJ1.Champ3 != null
                && jeu.ChampBatailleUnitesJ2.Champ3 != null
                && jeu.ChampBatailleUnitesJ1.Champ3.Defense <= jeu.ChampBatailleUnitesJ2.Champ3.Attaque
                && jeu.ChampBatailleUnitesJ2.Champ3.Defense > jeu.ChampBatailleUnitesJ1.Champ3.Attaque)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Je vérifie si une unité peut amener le AI a la défaite
        /// </summary>
        /// <returns></returns>
        private bool PossibiliteDefaiteUnite(TableDeJeu jeu)
        {
            int totalDMG = 0;

            if (jeu.ChampBatailleUnitesJ1.Champ1 != null)
            {
                totalDMG += jeu.ChampBatailleUnitesJ1.Champ1.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ1.Champ2 != null)
            {
                totalDMG += jeu.ChampBatailleUnitesJ1.Champ2.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                totalDMG += jeu.ChampBatailleUnitesJ1.Champ3.getAttaque();
            }

            if (totalDMG >= jeu.Joueur2.PointDeBlindage)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Modifier le int "Strategie" pour définir comment le AI veut jouer. 
        /// </summary>
        private void EvaluerConditionVictoire(TableDeJeu jeu)
        {
            // On augmente le score pour des conditions propice à l'offense
            // On descent le score pour des conditions propice à la défense
            int score = 0;
            int pointHPavance = 0;

            // Si nous sommes menacé de défaite, nous allons arrêter ici et se mettre en full defense
            // TODO : Peut-être pas nécessaire
            if (PossibiliteDefaiteUnite(jeu))
            {
                Strategie = 1;
                return;
            }

            // Comparaison des points de blindages

            pointHPavance = jeu.Joueur2.PointDeBlindage - jeu.Joueur1.PointDeBlindage;

            if (pointHPavance > 20) // 21+
            {
                score += 5;
            }
            else if (pointHPavance > 15) // 16-20
            {
                score += 4;
            }
            else if (pointHPavance > 10) // 11-15
            {
                score += 2;
            }
            else if (pointHPavance > 5) // 6-10
            {
                score += 1;
            }
            else if (pointHPavance > -5) // -5 a 5
            {
            }
            else if (pointHPavance > -10) // -10 a -6
            {
                score -= 1;
            }
            else if (pointHPavance > -15) // -15 a -11
            {
                score -= 2;
            }
            else if (pointHPavance > -20) // -20 a -16
            {
                score -= 4;
            }
            else // -21 et moins
            {
                score -= 5;
            }

            // On vérifie quel joueur à le plus de puissante de faire des dégâts
            int totalAttAI = 0;
            int totalAttJ = 0;
            if (jeu.ChampBatailleUnitesJ1.Champ1 != null)
            {
                totalAttJ += jeu.ChampBatailleUnitesJ1.Champ1.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ1.Champ2 != null)
            {
                totalAttJ += jeu.ChampBatailleUnitesJ1.Champ2.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ1.Champ3 != null)
            {
                totalAttJ += jeu.ChampBatailleUnitesJ1.Champ3.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ2.Champ1 != null)
            {
                totalAttAI += jeu.ChampBatailleUnitesJ2.Champ1.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ2.Champ2 != null)
            {
                totalAttAI += jeu.ChampBatailleUnitesJ2.Champ2.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ2.Champ3 != null)
            {
                totalAttAI += jeu.ChampBatailleUnitesJ2.Champ3.getAttaque();
            }

            if (totalAttAI < totalAttJ)
            {
                score -= 1;
            }
            else
            {
                score += 1;
            }

            // On vérifie si un joueur a beaucoup plus de cartes 
            if (jeu.LstMainJ1.Count() - jeu.LstMainJ2.Count() > 4)
            {
                score -= 1;
            }
            if (jeu.LstMainJ2.Count() - jeu.LstMainJ1.Count() > 4)
            {
                score += 1;
            }

            // On vérifie si il y a une bonne ou vraiment large différence de ressource entre les deux joueurs
            // Si la différence est dans une marge de 5, il n'y a pas de problème
            // Il ne faut pas oublier ici que le AI vient de recevoir 6 ou + ressources, mais aussi que le AI
            // n'a pas joué encore d'unité. Ces deux faits devrait s'entrebalancer entre le calcul du 
            // board state et le calcul des ressources.
            if ((jeu.Joueur1.RessourceActive.AlainDollars + jeu.Joueur1.RessourceActive.BarilNucleaire + jeu.Joueur1.RessourceActive.Charronite)
                - (jeu.Joueur2.RessourceActive.AlainDollars + jeu.Joueur2.RessourceActive.BarilNucleaire + jeu.Joueur2.RessourceActive.Charronite)
                > 12)

            {
                score -= 2; // Joueur a au moins 13 ressources d'avance
            }
            else if ((jeu.Joueur1.RessourceActive.AlainDollars + jeu.Joueur1.RessourceActive.BarilNucleaire + jeu.Joueur1.RessourceActive.Charronite)
                - (jeu.Joueur2.RessourceActive.AlainDollars + jeu.Joueur2.RessourceActive.BarilNucleaire + jeu.Joueur2.RessourceActive.Charronite)
                > 5)
            {
                score -= 1; // Joueur a entre 5 et 12 ressources d'avance
            }
            else if ((jeu.Joueur2.RessourceActive.AlainDollars + jeu.Joueur2.RessourceActive.BarilNucleaire + jeu.Joueur2.RessourceActive.Charronite)
                - (jeu.Joueur1.RessourceActive.AlainDollars + jeu.Joueur1.RessourceActive.BarilNucleaire + jeu.Joueur1.RessourceActive.Charronite)
                > 5)
            {
                score += 1; // AI a entre 5 et 12 ressources d'avance
            }
            else if ((jeu.Joueur2.RessourceActive.AlainDollars + jeu.Joueur2.RessourceActive.BarilNucleaire + jeu.Joueur2.RessourceActive.Charronite)
                - (jeu.Joueur1.RessourceActive.AlainDollars + jeu.Joueur1.RessourceActive.BarilNucleaire + jeu.Joueur1.RessourceActive.Charronite)
                > 12)
            {
                score += 2; // AI a au moins 13 ressources d'avance
            }


            // Selon le score on attribue une strategie
            if (score > 2)
            {
                Strategie = 5;
            }
            if (score == 1)
            {
                Strategie = 4;
            }
            if (score == 0)
            {
                Strategie = 3;
            }
            if (score == -1)
            {
                Strategie = 2;
            }
            if (score < 2)
            {
                Strategie = 1;
            }
        }


    }
}

