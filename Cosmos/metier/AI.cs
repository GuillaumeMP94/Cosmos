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
                case 3:
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




    }

}

