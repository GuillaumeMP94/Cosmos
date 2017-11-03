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

        public bool JoueChamp1 { get; set; }
        public bool JoueChamp2 { get; set; }
        public bool JoueChamp3 { get; set; }
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
            ReinitialiserChampJouer();
            List<int> ListeCoupsPermis;
            Random rnd = new Random(DateTime.Now.Millisecond);

            ListeCoupsPermis = jeu.listeCoupValideAI();

            switch (difficulte)
            {
                case 1:              
                if (jeu.JoueurActifEst1 == false)
                {
                    if (ListeCoupsPermis.Count != 0)
                    {
                        //jeu.AIPassePhase = false;
                        jeu.JouerCarte(ListeCoupsPermis[rnd.Next(0, ListeCoupsPermis.Count)]);
                        jeu.AvancerPhase();
                    }
                    else
                    {
                        // Si le AI n'a aucun coup valide, il passe en phase d'attaque.

                        //jeu.AIPassePhase = true; 
                        jeu.AvancerPhase();
                    }

                }
                    break;
                case 2:
                    // TODO   EN COURS
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            int indexGagnant = AIPeutGagnerJouerCarte(ListeCoupsPermis, jeu);
                            if ( indexGagnant != -1)
                            {
                                jeu.JouerCarte(indexGagnant);
                            }
                            else
                            {

                            }
                            
                            jeu.AvancerPhase();
                        }
                        else
                        {
                            // Si le AI n'a aucun coup valide, il passe en phase d'attaque.

                            jeu.AvancerPhase();
                        }

                    }
                    break;
                case 3:
                    // TODO
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                        
                            jeu.AvancerPhase();
                        }
                        else
                        {
                            // Si le AI n'a aucun coup valide, il passe en phase d'attaque.

                            jeu.AvancerPhase();
                        }
                    }
                    break;
                case 4:
                    // TODO
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            //jeu.AIPassePhase = false;
                            
                            jeu.AvancerPhase();
                        }
                        else
                        {
                            // Si le AI n'a aucun coup valide, il passe en phase d'attaque.

                            //jeu.AIPassePhase = true; 
                            jeu.AvancerPhase();
                        }
                    }
                    break;
                case 5:
                    //TODO
                    if (jeu.JoueurActifEst1 == false)
                    {
                        if (ListeCoupsPermis.Count != 0)
                        {
                            //jeu.AIPassePhase = false;
                            
                            jeu.AvancerPhase();
                        }
                        else
                        {
                            // Si le AI n'a aucun coup valide, il passe en phase d'attaque.

                            //jeu.AIPassePhase = true; 
                            jeu.AvancerPhase();
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Si une carte permet de gagner, son index est retourner. De plus, certain "flag" sont trigger pour déterminer
        /// comment elle doit être joué
        /// </summary>
        /// <param name="listeCoupsPermis"></param>
        /// <param name="jeu"></param>
        /// <returns></returns>
        private int AIPeutGagnerJouerCarte(List<int> listeCoupsPermis, TableDeJeu jeu)
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
                                    JoueChamp1 = true;
                                    AttaqueChamp1 = true;                                   

                                    return compteur;
                                    
                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ2 == null && jeu.ChampBatailleUnitesJ2.Champ2 == null)
                                {
                                    JoueChamp2 = true;
                                    AttaqueChamp2 = true;

                                    return compteur;
                                    
                                }
                                if (jeu.ChampBatailleUnitesJ1.Champ3 == null && jeu.ChampBatailleUnitesJ2.Champ3 == null)
                                {
                                    JoueChamp3 = true;
                                    AttaqueChamp3 = true;

                                    return compteur;
                                    
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

                    }



                }
                compteur++;
            }

            // Si aucune carte dans l'index permet la victoire, la fonction retourne un index négatif qui ne peux pas exister, donc "false"
            return -1;
        }

        /// <summary>
        /// Selon la difficulté, le AI va décider comment faire sa phase d'attaque
        /// </summary>
        /// <param name="jeu">La table de jeu</param>
        private void PhaseAttaqueAI(TableDeJeu jeu)
        {
            switch (difficulte)
            {
                case 1:
                    // YOLO. L'AI attaque toujours
                    AttaqueChamp1 = true;
                    AttaqueChamp1 = true;
                    AttaqueChamp3 = true;


                    break;
                case 2:
                    // Idée : Mettre un peu de RNG. 

                    // Si l'unité est imblocable, elle attaque toujours
                    if (jeu.ChampBatailleUnitesJ2.Champ1.EffetCarte.Type == "Imblocable")
                    {
                        AttaqueChamp1 = true;
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ2.EffetCarte.Type == "Imblocable")
                    {
                        AttaqueChamp2 = true;
                    }
                    if (jeu.ChampBatailleUnitesJ2.Champ3.EffetCarte.Type == "Imblocable")
                    {
                        AttaqueChamp3 = true;
                    }

                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }

        public void ReinitialiserChampJouer()
        {
            JoueChamp1 = false;
            JoueChamp2 = false;
            JoueChamp3 = false;
        }

    }

}

