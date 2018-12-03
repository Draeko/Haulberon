using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Haulberon
{
    public class PileObjets
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Quantite { get; set; }
        public List<Effet> ListeEffets { get; set; }
        public string Description { get; set; }

        public PileObjets(int Id)
        {
            this.Id = Id;

            XDocument doc = XDocument.Load("objets.xml");
            var objetChoisi = doc.Descendants("objet").Where(x => x.Attribute("id").Value == Id.ToString()).Single();

            this.Nom = objetChoisi.Attribute("nom").Value;
            this.Quantite = 1;
            this.Description = objetChoisi.Attribute("description").Value;

            this.ListeEffets = new List<Effet>();
            foreach (XElement effet in objetChoisi.Descendants("effet"))
            {
                ListeEffets.Add(new Effet(Convert.ToInt32(effet.Attribute("id").Value)));
            }
        }
    }
}
