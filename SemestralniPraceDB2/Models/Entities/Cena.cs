using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Cena
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Platnost od")]
        public DateTime? PlatnostOd { get; set; }

        [DisplayName("Platnost do")]
        public DateTime? PlatnostDo { get; set; }

        [DisplayName("Částka")]
        public double Castka { get; set; }

        [DisplayName("Zboží")]
        public Zbozi? Zbozi { get; set; }

        public override string ToString()
        {
            string zboziInfo = String.Empty;
            if (Zbozi is not null)
            {
                zboziInfo = $", zboží: {Zbozi.Nazev} ({Zbozi.EAN}))";
            }
            return $"Od {PlatnostOd:dd.mm.yyyy}, Do {PlatnostDo:dd.mm.yyyy}, {Castka} Kč" + zboziInfo;
        }


    }
}
