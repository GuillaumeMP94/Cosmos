﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.metier
{
    public class Deck
    {
        #region Propriétés
        public int IdDeck { get; set; }
        public string Nom { get; set; }
        public List<Carte> CartesDuDeck { get; set; }
        public bool EstChoisi { get; set; }
        #endregion
        #region Constructeurs

        public Deck(int idDeck, string nom, bool estChoisi)
        {
            IdDeck = idDeck;
            Nom = nom;

            EstChoisi = estChoisi;
        }

        /// <summary>
        /// Constructeur qui permet de faire une Deep copy du deck passé en paramêtre.
        /// </summary>
        /// <param name="aCopier"></param>
        public Deck(Deck aCopier)
        {

            IdDeck = aCopier.IdDeck;
            Nom = aCopier.Nom;
            CartesDuDeck = new List<Carte>();
            foreach (Carte uneCarte in aCopier.CartesDuDeck)
            {
               this.CartesDuDeck.Add(uneCarte.Clone());
            }
            EstChoisi = aCopier.EstChoisi;

        }
        #endregion

        public Carte PigerCarte()
        {

            var temp = CartesDuDeck[0].Clone();
            CartesDuDeck.RemoveAt(0);

            return temp;

        }

        public void BrasserDeck()
        {
            int n = CartesDuDeck.Count();
            var rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                Carte temp = CartesDuDeck[k];
                CartesDuDeck[k] = CartesDuDeck[n];
                CartesDuDeck[n] = temp;
            }
        }

    }
}
