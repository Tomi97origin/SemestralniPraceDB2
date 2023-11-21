using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Zamestnanec
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName("Jméno")]
        public string? Jmeno { get; set; }

        [DisplayName("Příjmení")]
        public string? Prijmeni { get; set; }

        [DisplayName("Osobní číslo")]
        public string? OsobniCislo { get; set; }

        [DisplayName("Telefonní číslo")]
        public string? TelCislo { get; set; }

        [DisplayName("Nástup")]
        public DateTime Nastup { get; set; }

        [DisplayName("Typ úvazku")]
        public short TypUvazku { get; set; }

        [DisplayName("Vedoucí")]
        public PlnyUvazek? Vedouci { get; set; }

        [DisplayName("Supermarket")]
        public Supermarket? Supermarket { get; set; }

        [DisplayName("Adresa")]
        public Adresa? Adresa { get; set; }

        [DisplayName("Role")]
        public Role? Role { get; set; }

        [DisplayName("Obrázek zaměstnance")]
        public ObrazekZamestnance? ObrazekZamestnance { get; set; }


        override public string ToString()
        {
            return $"{Jmeno} {Prijmeni}, {OsobniCislo}, {TelCislo}, " +
                $"{Nastup}, {TypUvazku}, {Vedouci}, {Supermarket}, {Adresa}, {Role}";
        }
    }
}
