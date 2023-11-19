using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Zamestnanec
    {
        public int Id { get; set; }
        public string? Jmeno { get; set; }
        public string? Prijmeni { get; set; }
        public string? OsobniCislo { get; set; }
        public string? TelCislo { get; set; }
        public DateTime Nastup { get; set; }
        public short TypUvazku { get; set; }
        public PlnyUvazek? Vedouci { get; set; }
        public Supermarket? Supermarket { get; set; }
        public Adresa? Adresa { get; set; }
        public Role? Role { get; set; }
        public ObrazekZamestnance? ObrazekZamestnance { get; set; }


        override public string ToString()
        {
            return $"{Jmeno} {Prijmeni}, {OsobniCislo}, {TelCislo}, " +
                $"{Nastup}, {TypUvazku}, {Vedouci}, {Supermarket}, {Adresa}, {Role}";
        }
    }
}
