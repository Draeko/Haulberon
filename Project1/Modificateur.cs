using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulberon
{
    public class Modificateur
    {
        public decimal Force { get; set; }
        public decimal Defense { get; set; }
        public decimal RegenPV { get; set; }
        public int PerteEnergie { get; set; }
        public int Duree { get; set; }
        public bool VolDeVie { get; set; }

        public Modificateur(string Nom)
        {
            switch(Nom)
            {
                case "Berserker":
                    this.Force = 2;
                    this.Defense = -1;
                    this.Duree = 3;
                    VolDeVie = true;
                    break;
                
            }
        }

        public Modificateur() { }
    }
}
