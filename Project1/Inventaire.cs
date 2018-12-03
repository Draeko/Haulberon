using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulberon
{
    public class Inventaire
    {
        public static List<PileObjets> ListeObjets { get; set; }

        public Inventaire()
        {
            ListeObjets = new List<PileObjets>();
        }

        public static void AjouteObjet(PileObjets ObjetPourAjout)
        {
            PileObjets objetExiste = null;
            foreach (PileObjets objet in ListeObjets)
            {
                if (objet.Id == ObjetPourAjout.Id) objetExiste = objet;
            }
            if (objetExiste != null) objetExiste.Quantite++;
            else ListeObjets.Add(ObjetPourAjout);
        }
    }
}
