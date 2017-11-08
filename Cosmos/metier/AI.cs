using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cosmos.metier
{
    public class AI:Joueur , IObserver<TableDeJeu>
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
            Console.WriteLine("Le tour du AI est terminé");  // TEST 
        }

        public void OnError(Exception error)
        {
            // Implémentation obligatoire, might use, might not
            Console.WriteLine("Oops, une erreur est arrivée avec votre adversaire!");
        }

        public void OnNext(TableDeJeu jeu)
        {
            //Phase principale
            JouerCoup(jeu);
            // Phase de combat
            PhaseAttaqueAI(jeu);

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
        public AI( string nom, int diff, Ressource debutLevelRessource, Deck deckAI, TableDeJeu laTableDeJeu)
            :base()
        {
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


            // Permet de lier l'AI avec la table de jeu
            laTableDeJeu.Subscribe(this);
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
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    ChoixChampUnite = rnd.Next(1, 2);
                                   jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite =rnd.Next(2, 3);
                                   jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)]);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    ChoixChampUnite =((rnd.Next(1, 2) * 2) - 1);
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

                        ListeCoupsPermis = jeu.listeCoupValideAI();

                        // Rendu ici nous avons jouer une unité si possible
                        // On tente de jouer un bâtiment
                        if(ListeCoupsPermis.Count != 0)
                        {
                            foreach( int index in ListeCoupsPermis)
                            {
                                if ( jeu.LstMainJ2[index] is Batiment )
                                {
                                    if( jeu.ChampConstructionsJ2.Champ1 == null 
                                        || jeu.ChampConstructionsJ2.Champ2 == null
                                        || jeu.ChampConstructionsJ2.Champ3 == null
                                        || jeu.ChampConstructionsJ2.Champ4 == null)
                                    {
                                        jeu.JouerCarteAI(index);
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
                            if(jeu.NbTourComplet < 4)
                            {
                                foreach( int coup in ListeCoupsPermis)
                                {
                                    if(jeu.LstMainJ2[coup] is Batiment)
                                    {
                                        jeu.JouerCarte(coup);
                                        // Rafraichir la liste des coup permis suite à un coup
                                        ListeCoupsPermis = jeu.listeCoupValideAI();
                                    } 
                                }
                            }

                            if(ListeCoupsPermis.Count != 0)
                            {
                                // Maintenant, il est temps de vérifier si nous pouvons nous mettre dans le chemin de notre adversaire
                                // De plus, nous allons vérifier si nous avons moyen de vaincre dans un champ

                                foreach (int coup in ListeCoupsPermis)
                                {
                                    if (jeu.ChampBatailleUnitesJ1.Champ1 != null && ListeCoupsPermis.Count != 0 &&  jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {

                                        if (jeu.LstMainJ2[coup] is Unite)
                                        {
                                            if (jeu.LstMainJ2[coup].getAttaque() >= jeu.ChampBatailleUnitesJ1.Champ1.Defense)
                                            {
                                                ChoixChampUnite = 1;
                                                jeu.JouerCarteAI(coup);
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
                                                ChoixChampUnite = 2;
                                                jeu.JouerCarteAI(coup);
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
                                                ChoixChampUnite = 3;
                                                jeu.JouerCarteAI(coup);
                                                ListeCoupsPermis = jeu.listeCoupValideAI();
                                            }
                                        }
                                    }
                                }

                                // Si il reste encore un coup permis ici, on joue une seule carte
                                if ( ListeCoupsPermis.Count != 0)
                                {
                                    if (jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {
                                        ChoixChampUnite = 1;
                                        jeu.JouerCarteAI(ListeCoupsPermis.First());
                                    }
                                    else if(jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        ChoixChampUnite = 2;
                                        jeu.JouerCarteAI(ListeCoupsPermis.First());
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        ChoixChampUnite = 3;
                                        jeu.JouerCarteAI(ListeCoupsPermis.First());
                                    }
                                    ListeCoupsPermis = jeu.listeCoupValideAI();
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
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {                           
                            if ( !(AIPeutGagnerJouerCarte(ListeCoupsPermis, jeu)))
                            {
                                ListeCoupEvaluer = EvaluerListeCoup(ListeCoupsPermis, jeu);

                                int scoreMax = ListeCoupEvaluer.Max();
                                int c = 0;
                                int indexAJouer = 1;

                                foreach( int i in ListeCoupEvaluer )
                                {
                                    if ( ListeCoupEvaluer[c] == scoreMax  )
                                    {
                                        indexAJouer = i;
                                    }

                                    c++;
                                }
                                jeu.JouerCarteAI( indexAJouer);

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
                if ( listeCoupsPermis.Contains(compteur) && !(uneCarte is Batiment) )
                {
                    if(uneCarte is Unite)
                    {
                        // Une carte avec célérité peut attaquer immédiatement
                        if (uneCarte.EffetCarte.Type == "Célérité")
                        {
                            // Je vérifie si l'unité peut faire assez de dégat pour mettre l'adversaire a 0 ou moins. 
                            int totalPostAtt = jeu.Joueur1.PointDeBlindage - uneCarte.getAttaque();

                            if (totalPostAtt < 1)
                            {
                                if (jeu.ChampBatailleUnitesJ1.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                {
                                    jeu.JouerCarteAI( 1 );
                                    ChoixChampUnite = 1; // Flag pour décider où jouer la carte par la suite
                                    return true;
                                    
                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    jeu.JouerCarteAI(2);
                                    ChoixChampUnite = 2;
                                    return true;
                                    
                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ3 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    jeu.JouerCarteAI(3);
                                    ChoixChampUnite = 3;
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
                    if(uneCarte is Gadget)
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
                    AttaqueChamp1 = true;
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
                    if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable" 
                        || jeu.ChampBatailleUnitesJ1.Champ1 == null
                        ||  rnd.Next(1,4) != 1  )
                    {
                        AttaqueChamp1 = true;
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable" 
                        || jeu.ChampBatailleUnitesJ1.Champ2 == null
                        || rnd.Next(1, 4) != 2)
                    {
                        AttaqueChamp2 = true;
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable" 
                        || jeu.ChampBatailleUnitesJ1.Champ3 == null
                        || rnd.Next(1, 4) != 3)
                    {
                        AttaqueChamp3 = true;
                    }

                    break;
                #endregion
                /* ---------------------------------------------------------------------------------------------- 
                                                            NIVEAU 3
                    ---------------------------------------------------------------------------------------------- */
                case 3:
                    #region
                    // Pour le plaisir de la chose, un peu de RNG est encore présent, mais pas beaucoup    
                    // L'effet radiation avantage à ne pas attaquer. Les cartes avec cette effet n'attaque que si il n'y a pas de bloqueur   
                    // De plus, si l'unité adverse a plus de defense et d'attaque le AI n'attaque pas. 
                    if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type != "Radiation"
                        || ( jeu.ChampBatailleUnitesJ1.Champ1.Attaque > jeu.ChampBatailleUnitesJ2.Champ1.Attaque 
                        && jeu.ChampBatailleUnitesJ1.Champ1.Defense > jeu.ChampBatailleUnitesJ2.Champ1.Defense) )
                     {
                        if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                            || jeu.ChampBatailleUnitesJ1.Champ1 == null
                            || rnd.Next(1, 10) != 1)
                        {
                            AttaqueChamp1 = true;
                        }
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type != "Radiation"
                        || (jeu.ChampBatailleUnitesJ1.Champ2.Attaque > jeu.ChampBatailleUnitesJ2.Champ2.Attaque
                        && jeu.ChampBatailleUnitesJ1.Champ2.Defense > jeu.ChampBatailleUnitesJ2.Champ2.Defense))
                    {
                        if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                           || jeu.ChampBatailleUnitesJ1.Champ2 == null
                           || rnd.Next(1, 10) != 2)
                        {
                            AttaqueChamp2 = true;
                        }
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type != "Radiation"
                        || (jeu.ChampBatailleUnitesJ1.Champ3.Attaque > jeu.ChampBatailleUnitesJ2.Champ3.Attaque
                        && jeu.ChampBatailleUnitesJ1.Champ3.Defense > jeu.ChampBatailleUnitesJ2.Champ3.Defense))
                    {
                        if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                           || jeu.ChampBatailleUnitesJ1.Champ3 == null
                           || rnd.Next(1, 10) != 3)
                        {
                            AttaqueChamp3 = true;
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
                    switch(Strategie){
                        case 1:
                        #region
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ1.Champ1 == null
                                || rnd.Next(1, 4) != 1)
                            {
                                AttaqueChamp1 = true;
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ1.Champ2 == null
                                || rnd.Next(1, 4) != 2)
                            {
                                AttaqueChamp2 = true;
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                                || jeu.ChampBatailleUnitesJ1.Champ3 == null
                                || rnd.Next(1, 4) != 3)
                            {
                                AttaqueChamp3 = true;
                            }
                            break;
                        #endregion
                        case 2:
                            #region
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type != "Radiation"
                              || (jeu.ChampBatailleUnitesJ1.Champ1.Attaque > jeu.ChampBatailleUnitesJ2.Champ1.Attaque
                              && jeu.ChampBatailleUnitesJ1.Champ1.Defense > jeu.ChampBatailleUnitesJ2.Champ1.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                                    || jeu.ChampBatailleUnitesJ1.Champ1 == null)
                                {
                                    AttaqueChamp1 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ2.Attaque > jeu.ChampBatailleUnitesJ2.Champ2.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ2.Defense > jeu.ChampBatailleUnitesJ2.Champ2.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                                   || jeu.ChampBatailleUnitesJ1.Champ2 == null)
                                {
                                    AttaqueChamp2 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ3.Attaque > jeu.ChampBatailleUnitesJ2.Champ3.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ3.Defense > jeu.ChampBatailleUnitesJ2.Champ3.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                                   || jeu.ChampBatailleUnitesJ1.Champ3 == null)
                                {
                                    AttaqueChamp3 = true;
                                }
                            }
                            #endregion
                            break;
                        case 3:
                            #region
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ1.Attaque > jeu.ChampBatailleUnitesJ2.Champ1.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ1.Defense > jeu.ChampBatailleUnitesJ2.Champ1.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                                    || jeu.ChampBatailleUnitesJ1.Champ1 == null)
                                {
                                    AttaqueChamp1 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ2.Attaque > jeu.ChampBatailleUnitesJ2.Champ2.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ2.Defense > jeu.ChampBatailleUnitesJ2.Champ2.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                                   || jeu.ChampBatailleUnitesJ1.Champ2 == null)
                                {
                                    AttaqueChamp2 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ3.Attaque > jeu.ChampBatailleUnitesJ2.Champ3.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ3.Defense > jeu.ChampBatailleUnitesJ2.Champ3.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                                   || jeu.ChampBatailleUnitesJ1.Champ3 == null)
                                {
                                    AttaqueChamp3 = true;
                                }
                            }
                            break;
                            #endregion
                        case 4:
                            #region
                            if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type != "Radiation"
                              || (jeu.ChampBatailleUnitesJ1.Champ1.Attaque > jeu.ChampBatailleUnitesJ2.Champ1.Attaque
                              && jeu.ChampBatailleUnitesJ1.Champ1.Defense > jeu.ChampBatailleUnitesJ2.Champ1.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable"
                                    || jeu.ChampBatailleUnitesJ1.Champ1 == null)
                                {
                                    AttaqueChamp1 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ2.Attaque > jeu.ChampBatailleUnitesJ2.Champ2.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ2.Defense > jeu.ChampBatailleUnitesJ2.Champ2.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable"
                                   || jeu.ChampBatailleUnitesJ1.Champ2 == null)
                                {
                                    AttaqueChamp2 = true;
                                }
                            }
                            if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type != "Radiation"
                                || (jeu.ChampBatailleUnitesJ1.Champ3.Attaque > jeu.ChampBatailleUnitesJ2.Champ3.Attaque
                                && jeu.ChampBatailleUnitesJ1.Champ3.Defense > jeu.ChampBatailleUnitesJ2.Champ3.Defense))
                            {
                                if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "AttaqueFurtive"
                                || jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable"
                                   || jeu.ChampBatailleUnitesJ1.Champ3 == null)
                                {
                                    AttaqueChamp3 = true;
                                }
                            }
                            #endregion
                            break;
                        case 5: // PRESS THE ATTACK AT ALL COST
                            AttaqueChamp1 = true;
                            AttaqueChamp2 = true;
                            AttaqueChamp3 = true;
                            break;
                        default: // Si jamais un bug survient, on attaque avec tout.
                            AttaqueChamp1  = true;
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
        /// De base, le AI n'attaque pas et l'algo decide de changer pour l'offense si nécessaire
        /// </summary>
        public void ReinitialiserChampAttaque()
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
            var rnd = new Random(DateTime.Now.Millisecond);

            // Je vérifie si l'adversaire peut gagner par le combat. Si oui, je vais aller dans une différente branche de réponse
            // De plus, pour sauver un peu de temps on regarde pas cette possibilité en début de partie
            if (jeu.NbTourComplet > 3 && PossibiliteDefaiteUnite(jeu) )
            {
                
                //TODO
            }
            else
            {
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
                    if (jeu.LstMainJ2[index] is Batiment)
                    {

                        // Revenir ici lorsque le choix de deck serait fait TODO

                        if (jeu.NbTourComplet > 4)
                        {
                            score = int.MinValue;
                        }
                        else if (jeu.NbTourComplet == 0 && jeu.ChampConstructionsJ1.Champ1 == null)
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
                    if (jeu.LstMainJ2[index] is Unite)
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
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité")
                                {
                                    score += 16;
                                }
                                break;
                            #endregion
                            case 1:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité")
                                {
                                    score += 16;
                                }

                                break;
                            #endregion
                            case 2:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité" && ExisteChampUniteVideEnemi(jeu))
                                {
                                    score += 16;
                                }
                                break;
                            #endregion
                            case 3:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité" && ExisteChampUniteVideEnemi(jeu))
                                {
                                    score += 16;
                                }
                                break;
                            #endregion
                            case 4:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité" && ExisteChampUniteVideEnemi(jeu))
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
                    if (jeu.LstMainJ2[index] is Gadget)
                    {
                        // TODO 
                        // Pour l'instant, un gadget a un score assigné au hasard                    
                        score = rnd.Next(-100, 550);
                    }
                    #endregion

                    // On rajoute le score dans la nouvelle liste
                    listeCoupsPermisEvaluer.Add(score);
                }
            }

            return listeCoupsPermisEvaluer;
        }
        /// <summary>
        /// Vérifie si il existe un champ vide du coté du joueur
        /// </summary>
        /// <param name="jeu"></param>
        /// <returns></returns>
        public bool ExisteChampUniteVideEnemi(TableDeJeu jeu)
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
        public int PeutOblitererCombat(TableDeJeu jeu, int index)
        {
            // Je vérifie ici si lors d'un combat mon unité va non seulement détruire l'enemi, mais en plus survivre
            if ( jeu.ChampBatailleUnitesJ1.Champ1.Defense <= jeu.LstMainJ2[index].getAttaque() 
                && jeu.ChampBatailleUnitesJ1.Champ1.Attaque < jeu.LstMainJ2[index].getDefense() )
            {
                return 1;
            }
            if (jeu.ChampBatailleUnitesJ1.Champ2.Defense <= jeu.LstMainJ2[index].getAttaque()
                && jeu.ChampBatailleUnitesJ1.Champ2.Attaque < jeu.LstMainJ2[index].getDefense())
            {
                return 2;
            }
            if (jeu.ChampBatailleUnitesJ1.Champ3.Defense <= jeu.LstMainJ2[index].getAttaque()
                 && jeu.ChampBatailleUnitesJ1.Champ3.Attaque < jeu.LstMainJ2[index].getDefense())
            {
                return 3;
            }

            return -1;
        }
    
        /// <summary>
        /// Je vérifie si une unité peut amener le AI a la défaite
        /// </summary>
        /// <returns></returns>
        public bool PossibiliteDefaiteUnite( TableDeJeu jeu )
        {
            int totalDMG = 0;

            if ( jeu.ChampBatailleUnitesJ1.Champ1 == null )
            {
                totalDMG += jeu.ChampBatailleUnitesJ1.Champ1.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ1.Champ2 == null)
            {
                totalDMG += jeu.ChampBatailleUnitesJ1.Champ2.getAttaque();
            }
            if (jeu.ChampBatailleUnitesJ1.Champ3 == null)
            {
                totalDMG += jeu.ChampBatailleUnitesJ1.Champ3.getAttaque();
            }

            if( totalDMG <= jeu.Joueur2.PointDeBlindage)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Modifier le int "Strategie" pour définir comment le AI veut jouer. 
        /// </summary>
        public void EvaluerConditionVictoire(TableDeJeu jeu)
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

            totalAttJ += jeu.ChampBatailleUnitesJ1.Champ1.getAttaque();
            totalAttJ += jeu.ChampBatailleUnitesJ1.Champ2.getAttaque();
            totalAttJ += jeu.ChampBatailleUnitesJ1.Champ2.getAttaque();

            totalAttAI += jeu.ChampBatailleUnitesJ2.Champ1.getAttaque();
            totalAttAI += jeu.ChampBatailleUnitesJ2.Champ2.getAttaque();
            totalAttAI += jeu.ChampBatailleUnitesJ2.Champ3.getAttaque();

            if ( totalAttAI < totalAttJ )
            {
                score -= 1;
            }
            else
            {
                score += 1;
            }

            // On vérifie si un joueur a beaucoup plus de cartes 
            if ( jeu.LstMainJ1.Count() - jeu.LstMainJ2.Count() > 4 )
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
            if (  (jeu.Joueur1.RessourceActive.AlainDollars + jeu.Joueur1.RessourceActive.BarilNucleaire + jeu.Joueur1.RessourceActive.Charronite)
                - (jeu.Joueur2.RessourceActive.AlainDollars + jeu.Joueur2.RessourceActive.BarilNucleaire + jeu.Joueur2.RessourceActive.Charronite)
                > 12 )

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
            if ( score > 2 )
            {
                Strategie = 5;
            }
            if ( score == 1 )
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

