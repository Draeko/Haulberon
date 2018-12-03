using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Haulberon
{
    public class Combat
    {
        private Personnage PersonnageCapture;

        //private Competence ChoixAttaque(Personnage Perso, IList<Personnage> GroupeEnnemis)
        //{
        //    int indiceCompetence = Program.UI.EcranChoixAttaque(Perso, GroupeEnnemis);
        //    return Perso.ListeCompetences[indiceCompetence];
        //}

        //private void TraitementInventaire(IList<Personnage> ListeCibles)
        //{
        //    int indiceObjet = Program.UI.EcranInventaire();

        //    if (indiceObjet >= 0)
        //    {
        //        PileObjets objetChoisi = Program.Inventaire[indiceObjet];
        //        List<Personnage> listeCiblesObjet = new List<Personnage>();
        //        if (objetChoisi.Id != 4)
        //        {
        //            foreach (Personnage perso in Program.ListePersonnages)
        //            {
        //                listeCiblesObjet.Add(perso);
        //            }
        //        }
                
        //        foreach (Personnage cible in ListeCibles)
        //        {
        //            listeCiblesObjet.Add(cible);
        //        }

        //        int indiceCible = Program.UI.EcranChoixCible(listeCiblesObjet);
        //        Personnage cibleObjet = listeCiblesObjet[indiceCible];

        //        foreach(Effet effet in objetChoisi.ListeEffets)
        //        {
        //            if (effet.Duree > 0)
        //            {
        //                Modificateur modificateur = new Modificateur
        //                {
        //                    Duree = effet.Duree
        //                };
        //                PropertyInfo propriete = modificateur.GetType().GetProperty(effet.Statistique);
        //                propriete.SetValue(modificateur, effet.Valeur);
        //                cibleObjet.ListeModificateurs.Add(modificateur);
        //            }
        //            else
        //            {
        //                if (effet.Statistique == "Id")
        //                {
        //                    Console.WriteLine("  " + cibleObjet.Nom + " rejoint l'équipe.");
        //                    PersonnageCapture = cibleObjet;
        //                    ListeCibles.Remove(cibleObjet);
        //                }
        //                else
        //                {
        //                    PropertyInfo propriete = cibleObjet.GetType().GetProperty(effet.Statistique);
        //                    if (propriete.PropertyType == typeof(decimal)) propriete.SetValue(cibleObjet, (decimal)propriete.GetValue(cibleObjet) + effet.Valeur);
        //                    if (propriete.PropertyType == typeof(int)) propriete.SetValue(cibleObjet, (int)propriete.GetValue(cibleObjet) + effet.Valeur);
        //                }
        //            }
        //        }

        //        objetChoisi.Quantite--;
        //        if (objetChoisi.Quantite == 0)
        //        {
        //            Program.Inventaire.Remove(objetChoisi);
        //        }
        //    }
        //}

        public bool PersonnagesVivants(IList<Personnage> ListePersonnages)
        {
            return ListePersonnages.Count(p => p.PvRestant > 0) > 0;
        }

    //    public Combat(IList<Personnage> GroupeJoueur, IList<Personnage> GroupeEnnemis)
    //    {
    //        EntierAleatoire = new Random();
    //        while (GroupeEnnemis.Count > 0 && PersonnagesVivants(GroupeJoueur))
    //        {
    //            //Retrait des modificateurs si la durée a expirée
    //            foreach (Personnage perso in GroupeJoueur)
    //            {
    //                List<Modificateur> modificateurs = perso.ListeModificateurs.ToList();
    //                foreach (Modificateur modificateur in modificateurs)
    //                {
    //                    modificateur.Duree--;
    //                    if (modificateur.Duree < 0) perso.ListeModificateurs.Remove(modificateur);
    //                }
    //            }

    //            /********************************************************************/
    //            foreach (Personnage perso in GroupeJoueur)
    //            {
    //                if (GroupeEnnemis.Count == 0) break;
    //                Competence competenceChoisie = null;
    //                int choixAction = Program.UI.EcranChoixAction(GroupeEnnemis, perso.Nom);
    //                switch (choixAction)
    //                {
    //                    case 0:
    //                        if (perso.EstEtourdie)
    //                        {
    //                            Console.WriteLine("  " + perso.Nom + " est étourdie. Je reste frais ..");
    //                        }
    //                        else
    //                            competenceChoisie = ChoixAttaque(perso, GroupeEnnemis);
    //                        break;
    //                    case 1: TraitementInventaire(GroupeEnnemis); break;
    //                }

    //                if (competenceChoisie != null)
    //                {
    //                    if (perso.EstEtourdie)
    //                    {
    //                        perso.EstEtourdie = false;
    //                    }
    //                    else
    //                    {
    //                        if (competenceChoisie.EnCooldown == 0)
    //                        {
    //                            if (competenceChoisie.CoutEnergie <= perso.Energie)
    //                            {
    //                                perso.Energie -= competenceChoisie.CoutEnergie;
    //                                perso.Action(GroupeEnnemis, competenceChoisie);
    //                            }
    //                            else
    //                            {
    //                                Console.Clear();
    //                                Console.WriteLine("  Pas assez d'énergie ! Pourquoi quand je bourre ça bourre pas ??!?");
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            if (PersonnageCapture != null)
    //            {
    //                GroupeJoueur.Add(PersonnageCapture);
    //                PersonnageCapture = null;
    //            }

    //            //Attaques des ennemis
    //            foreach (Personnage ennemi in GroupeEnnemis)
    //            {
    //                if (ennemi.EstEtourdie)
    //                {
    //                    Console.WriteLine("  " + ennemi.Nom + " est étourdie ! Ne peut pas attaquer !");
    //                    ennemi.EstEtourdie = false;
    //                }
    //                else if (ennemi.PvRestant > 0)
    //                {
    //                    int cibleAleatoire = Program.EntierAleatoire.Next(0, Program.ListePersonnages.Count);
    //                    ennemi.Attaque(Program.ListePersonnages[cibleAleatoire], ennemi.ListeCompetences[0]);
    //                }
    //            }
    //            Console.WriteLine();

    //            //Traitement des Cooldowns
    //            foreach (Personnage perso in GroupeJoueur)
    //            {
    //                foreach (Competence competence in perso.ListeCompetences)
    //                {
    //                    if (competence.EnCooldown > 0)
    //                        competence.EnCooldown--;
    //                }
    //            }
    //        }
    //    }
    }
}
