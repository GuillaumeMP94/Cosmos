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

        public void OnNext(TableDeJeu table)
        {
            // Phase de ressource
            //TODO
            table.AvancerPhase();
            System.Threading.Thread.Sleep(1000); // Ça marche, je sais pas a quel point
            // Phase principale
            JouerCoup(table);
            table.AvancerPhase();
            System.Threading.Thread.Sleep(1000);
            // Phase de combat
            table.AvancerPhase();
            System.Threading.Thread.Sleep(1000);
            // Phase de fin
            table.AvancerPhase();
            System.Threading.Thread.Sleep(1000);
            // Fin du tour

        }
        #endregion

        private int difficulte;

        public int ChoixChampUnite { get; set; }
        //public bool JoueChamp2 { get; set; }
        //public bool JoueChamp3 { get; set; }
        public bool AttaqueChamp1 { get; set; }
        public bool AttaqueChamp2 { get; set; }
        public bool AttaqueChamp3 { get; set; }


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
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], rnd.Next(1, 3));
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], rnd.Next(1, 2));
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], rnd.Next(2, 3));
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], ((rnd.Next(1, 2) * 2  ) -1) );
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                {
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], 1);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], 2);
                                }
                                else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    jeu.JouerCarteAI(ListeCoupsPermisUnite[rnd.Next(0, ListeCoupsPermisUnite.Count)], 3);
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
                                        jeu.JouerCarteAI(index, 0); // Je passe 0 ici, mais ça sert à rien dans le cas d'un bâtiment
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
                                            jeu.JouerCarteAI(coup, 0);
                                            // Rafraichir la liste des coup permis suite à un coup
                                            ListeCoupsPermis = jeu.listeCoupValideAI();
                                        }
                                    }
                                }
                                // Notre AI est toujours pas vraiment bon. Il joue encore n'importe quoi rendu ici     
                                // TODO répétition de code innutile                          
                                if(ListeCoupsPermis.Count != 0)
                                {
                                    if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null
                                        && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], rnd.Next(1, 3));
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], rnd.Next(1, 2));
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], rnd.Next(2, 3));
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ1 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], ((rnd.Next(1, 2) * 2) - 1));
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ1 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], 1);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], 2);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)], 3);
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
                                                jeu.JouerCarteAI(coup,1);
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
                                                jeu.JouerCarteAI(coup,2);
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
                                                jeu.JouerCarteAI(coup,3);
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
                                        jeu.JouerCarteAI(ListeCoupsPermis.First(), 1 );
                                    }
                                    else if(jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis.First(), 2);
                                    }
                                    else if (jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                    {
                                        jeu.JouerCarteAI(ListeCoupsPermis.First(), 3);
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
                            // Le AI va maintenant implémenter une stratégie tour par tour
                            switch( jeu.NbTourComplet)
                            {
                                case 1:
                                    // Au premier tour, les bâtiments ou les unité à faible coût sont priorisé
                                    ListeCoupEvaluer = evaluerListeCoup(ListeCoupsPermis, jeu);


                                    break;
                                case 2:
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    break;
                                default:
                                    break;


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
                                    jeu.JouerCarteAI( 1,1 );                              

                                    return true;
                                    
                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    jeu.JouerCarteAI(2,2);

                                    return true;
                                    
                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ3 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    jeu.JouerCarteAI(3,3);

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
        private List<int> evaluerListeCoup(List<int> listeCoupsPermis, TableDeJeu jeu)
        {
            // Si le score a maxvalue, ça veut dire que le coup est le meilleur coup possible en ce moment
            // Si le score a minvalue, ça veut dire que le coup est le pire coup possible
            // Le AI sait rendu ici qu'il ne peut pas gagner puisque la première chose qu'il a fait était de déterminer si la victoire était possible.

            var listeCoupsPermisEvaluer = new List<int>();
            int score;
            var rnd = new Random(DateTime.Now.Millisecond);

            // Je vérifie si l'adversaire peut gagner par le combat. Si oui, je vais aller dans une différente branche de réponse
            if (PossibiliteDefaiteUnite(jeu) && jeu.NbTourComplet > 3)
            {

            }
            else
            {
                foreach (int index in listeCoupsPermis)
                {
                    score = 0;

                    // Si le AI peut optimiser l'utilisation de ses ressources, le score augmente dramatiquement
                    if (jeu.Joueur2.RessourceActive - jeu.LstMainJ2[index].Cout == new Ressource(0, 0, 0))
                    {
                        score += 299;
                    }
                    // ------------------  Bâtiment -----------------------
                    #region
                    if (jeu.LstMainJ2[index] is Batiment)
                    {
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
                            score = 500 - (jeu.NbTourComplet * 100);
                        }
                        else
                        {
                            score = 450 - (jeu.NbTourComplet * 100);
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
                            score += 301;
                        }
                        // Je vérifie si mon unité peut dramatiquement gagner une "lane"
                        if (PeutOblitererCombat(jeu, index) != -1)
                        {
                            score += 275;
                        }
                        switch (jeu.NbTourComplet)
                        {
                            // Chaque tour implique une approche différente pour le score des unités
                            case 0:
                                #region
                                if (jeu.ChampBatailleUnitesJ1.Champ1 != null || jeu.ChampBatailleUnitesJ1.Champ2 != null || jeu.ChampBatailleUnitesJ1.Champ3 != null)
                                {
                                    score += 551; // Le score doit allé plus haut qu'un gadget
                                }
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité")
                                {
                                    score += 160;
                                }
                                break;
                            #endregion
                            case 1:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité")
                                {
                                    score += 160;
                                }

                                break;
                            #endregion
                            case 2:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité" && ExisteChampUniteVideEnemi(jeu))
                                {
                                    score += 160;
                                }
                                break;
                            #endregion
                            case 3:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité" && ExisteChampUniteVideEnemi(jeu))
                                {
                                    score += 160;
                                }
                                break;
                            #endregion
                            case 4:
                                #region
                                if (jeu.LstMainJ2[index].EffetCarte.Type == "Célérité" && ExisteChampUniteVideEnemi(jeu))
                                {
                                    score += 160;
                                }

                                break;
                            #endregion
                            default:
                                #region
                                // Le mode par défaut représente le "late game"
                                score += jeu.LstMainJ2[index].Cout.Charronite * 25;
                                score += jeu.LstMainJ2[index].Cout.BarilNucleaire * 25;
                                score += jeu.LstMainJ2[index].Cout.AlainDollars * 25;
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

    }
}

