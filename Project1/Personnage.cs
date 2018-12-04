using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Haulberon
{
    public class Personnage
    {
        public int Id { get; private set; }
        public string Nom { get; private set; }
        public int Pv_max { get; private set; }
        public int Defense { get; private set; }
        private int energie;
        public int Energie
        {
            get { return energie; }
            set
            {
                if (value > 100) energie = 100;
                else energie = value;
            }
        }
        private decimal pv_restant;
        public decimal PvRestant
        {
            get { return pv_restant; }
            set
            {
                if (value < 0) pv_restant = 0;
                if (value >= Pv_max) pv_restant = Pv_max;
                else pv_restant = value;
            }
        }

        public bool EstAdversaire;
        public bool EstEtourdie { get; set; }
        public decimal Degats_infliges { get; private set; }
        public decimal Degats_infliges_reel { get; private set; }

        Membre BrasGauche;
        Membre BrasDroit;
        Membre Jambes;

        public Animation Animation;
        public float Vitesse { get; set; }
       
        public List<Competence> ListeCompetences { get; private set; }
        public List<Modificateur> ListeModificateurs { get; set; }

        public Personnage(int Id, string Categorie)
        {
            XDocument doc = XDocument.Load("personnages.xml");
            var personnage = doc.Descendants(Categorie).Elements("personnage").Where(x => Convert.ToInt32(x.Attribute("id").Value) == Id).Single();

            this.Id = Id;
            this.Nom = personnage.Attribute("nom").Value;
            this.Pv_max = Convert.ToInt32(personnage.Attribute("pv").Value);
            this.Defense = Convert.ToInt32(personnage.Attribute("defense").Value);
            this.Energie = Convert.ToInt32(personnage.Attribute("energie").Value);

            this.PvRestant = this.Pv_max;

            ListeCompetences = new List<Competence>()
                        {
                            new Competence(Convert.ToInt32(personnage.Attribute("competence1").Value)),
                            new Competence(Convert.ToInt32(personnage.Attribute("competence2").Value)),
                            new Competence(Convert.ToInt32(personnage.Attribute("competence3").Value))
                        };
            ListeModificateurs = new List<Modificateur>();

            this.EstAdversaire = (Categorie == "jouable") ? false : true;
            
            /**************************************/
            Animation = new Animation(this.Nom);
        }

        private void EffetDeLaCompetence(string NomCompetence, Personnage Cible, Random EntierAleatoire)
        {
            switch(NomCompetence)
            {
                case "Attaque Normal":
                    this.Energie += 10;
                    break;
                case "Coup de bouclier":
                    int ChanceEtourdie = EntierAleatoire.Next(0, 100);
                    if (ChanceEtourdie <= 75) Cible.EstEtourdie = true;
                    break;
            }
        }


        public void AppliqueModificateur(Competence CompetenceChoisie)
        {
            this.ListeModificateurs.Add(new Modificateur(CompetenceChoisie.Nom));
            Console.WriteLine(this.Nom + " gagne le bonus " + CompetenceChoisie.Nom);
        }

        public void Soin(Personnage Cible, Competence CompetenceChoisie, Random EntierAleatoire)
        {
            //Application du soin
            int Variation = EntierAleatoire.Next(CompetenceChoisie.DegatsMin, CompetenceChoisie.DegatsMax);
            decimal montantSoin = ((Cible.Pv_max - Cible.PvRestant) - Variation) >= 0 
                ? Variation
                : Variation + ((Cible.Pv_max - Cible.PvRestant) - Variation);

            Cible.PvRestant += montantSoin;
            Console.WriteLine("  " + this.Nom + " soigne " + Cible.Nom + " de " + montantSoin + " PVs.");
        }

        public void Attaque(Personnage Cible, Competence CompetenceChoisie, Random EntierAleatoire)
        {
            Degats degatsInfliges = new Degats
            {
                Defense = Cible.Defense,
                ModifieurCritique = CompetenceChoisie.ModifieurCritique,
                Variation = EntierAleatoire.Next(CompetenceChoisie.DegatsMin, CompetenceChoisie.DegatsMax),
                EstCritique = EntierAleatoire.Next(0, 100) < CompetenceChoisie.ChancesCritique
            };

            //Verifier Bonus/Malus
            foreach (Modificateur modificateur in this.ListeModificateurs)
            {
                degatsInfliges.Variation += modificateur.Force;
                if(!degatsInfliges.VolDeVie) degatsInfliges.VolDeVie = modificateur.VolDeVie;
            }
            foreach(Modificateur modificateur in Cible.ListeModificateurs)
            {
                degatsInfliges.Defense += modificateur.Defense;
            }

            EffetDeLaCompetence(CompetenceChoisie.Nom, Cible, EntierAleatoire);

            this.Degats_infliges = degatsInfliges.Calcul();
            this.Degats_infliges_reel = this.Degats_infliges;
            if (this.Degats_infliges > Cible.pv_restant) this.Degats_infliges_reel = Cible.PvRestant;
            Cible.PvRestant -= this.Degats_infliges_reel;
            if (Cible.PvRestant <= 0)
            {
                ObtenirLoot(Cible.Id, EntierAleatoire);
            }
            if (degatsInfliges.VolDeVie)
            {
                this.PvRestant = PvRestant + (this.Degats_infliges_reel * 3 / 4);
            }
        }

        private void ObtenirLoot(int IdEnnemi, Random EntierAleatoire)
        {
            XDocument doc = XDocument.Load("loot.xml");
            
            try
            {
                var loot = doc.Descendants("loot").Where(x => Convert.ToInt32(x.Attribute("bestiaireId").Value) == IdEnnemi);
                foreach(XElement element in loot)
                {
                    int chanceLoot = Convert.ToInt32(element.Attribute("chance").Value);
                    if (chanceLoot > EntierAleatoire.Next(0, 100))
                    {
                        PileObjets objetLoot = new PileObjets(Convert.ToInt32(element.Attribute("objetId").Value));
                        Inventaire.AjouteObjet(objetLoot);
                        Console.WriteLine("  Objet trouvé : " + objetLoot.Nom);
                    }
                }
                
            }
            catch(Exception e)
            {

            }
            
        }
    }
}
