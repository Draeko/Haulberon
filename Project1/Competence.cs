using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Haulberon
{
    public class Competence
    {
        public string Nom { get; private set; }
        public int DegatsMin { get; private set; }
        public int DegatsMax { get; private set; }
        public int Cooldown { get; private set; }
        public int CoutEnergie { get; private set; }
        public decimal ChancesCritique { get; private set; }
        public decimal ModifieurCritique { get; private set; }
        public bool EstMonocible { get; private set; }
        public string Type { get; private set; }
        public string Description { get; private set; }

        public int EnCooldown { get; set; }

        public Competence(int IdCompetence)
        {
            if (IdCompetence > 0)
            {
                XDocument doc = XDocument.Load("competences.xml");
                var competence = doc.Descendants("competence").Where(x => Convert.ToInt32(x.Attribute("id").Value) == IdCompetence).Single();

                this.Nom = competence.Attribute("nom").Value;
                this.DegatsMin = Convert.ToInt32(competence.Attribute("degatsMin").Value);
                this.DegatsMax = Convert.ToInt32(competence.Attribute("degatsMax").Value);
                this.Cooldown = Convert.ToInt32(competence.Attribute("cooldown").Value);
                this.CoutEnergie = Convert.ToInt32(competence.Attribute("coutEnergie").Value);
                this.ChancesCritique = Math.Round(decimal.Parse(competence.Attribute("chancesCritique").Value), 2);
                this.ModifieurCritique = Math.Round(decimal.Parse(competence.Attribute("modifieurCritique").Value), 2);
                this.EstMonocible = Convert.ToBoolean(competence.Attribute("monocible").Value);
                this.Type = competence.Attribute("type").Value.ToUpper();
                this.Description = competence.Attribute("description").Value;
            }
        }

        public void MiseEnCooldown()
        {
            this.EnCooldown = this.Cooldown;
        }
    }
}
