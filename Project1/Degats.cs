using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulberon
{
    public class Degats
    {
        public decimal Defense;
        public decimal Variation;
        public bool EstCritique;
        public decimal ModifieurCritique;
        public bool VolDeVie;

        public decimal Calcul()
        {
            decimal degats = Math.Round(Variation - (Variation * Defense / 100), 2);
            if (degats < 0) degats = 0;
            if (EstCritique && degats > 0)
            {
                return degats * ModifieurCritique;
            }
            return degats;
        }
    }
}
