using System;
using System.Xml.Linq;
using System.Linq;

namespace Haulberon
{
    public class Effet
    {
        public int Id { get; set; }
        public string Statistique { get; set; }
        public decimal Valeur { get; set; }
        public int Duree { get; set; }

        public Effet(int Id)
        {
            XDocument doc = XDocument.Load("effets.xml");
            var effet = doc.Descendants("effet").Where(x => x.Attribute("id").Value == Id.ToString()).Single();

            this.Id = Id;
            this.Statistique = effet.Attribute("statistique").Value;
            this.Valeur = Convert.ToDecimal(effet.Attribute("valeur").Value);
            this.Duree = Convert.ToInt32(effet.Attribute("duree").Value);
        }
    }
}
