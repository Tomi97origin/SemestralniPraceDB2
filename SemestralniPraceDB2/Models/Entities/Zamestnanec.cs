using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public class Zamestnanec : IDBEntity
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

        [Browsable(false)]
        public short TypUvazku { get; set; }

        [DisplayName("Typ úvazku")]
        public string TypUvazkuText => TypUvazku == 0 ? "Brigádník" : "Plný úvazek";

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
            if (Jmeno is null)
            {
                return $"Zaměstnanec {Id}";
            }
            else
            {
                StringBuilder str = new();

                str.Append($"{Jmeno}");

                if (Prijmeni is not null) { str.Append($" {Prijmeni}"); }
                //if (OsobniCislo is not null) { str.Append($", pid: {OsobniCislo}"); }
                //if (TelCislo is not null) { str.Append($", tel: {TelCislo}"); }
                //str.Append($", nástup: {Nastup.ToString("dd. MM. yyyy")}");
                //if (Vedouci is not null) { str.Append($", vedoucí pracovník: {Vedouci.CeleJmeno()}"); }
                //if (Supermarket is not null) { str.Append($", prodejna: {Supermarket}"); }
                //if (Adresa is not null) { str.Append($", bytem: {Adresa}"); }
                //if (Role is not null) { str.Append($", {Role}"); }
                //if (ObrazekZamestnance is not null) { str.Append($", {ObrazekZamestnance}"); }

                return str.ToString();
            }
        }

        public string CeleJmeno()
        {
            StringBuilder str = new();

            if (Jmeno is not null) { str.Append($"{Jmeno}"); }
            if (Prijmeni is not null) { str.Append($" {Prijmeni}"); }

            return str.ToString();
        }

        public string DataToText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Jmeno} ");
            sb.Append($"{Prijmeni} ");
            sb.Append($"{OsobniCislo} ");
            sb.Append($"{TelCislo} ");
            sb.Append($"{Nastup} ");
            sb.Append($"{TypUvazkuText} ");
            sb.Append($"{Vedouci?.DataToText()} ");
            sb.Append($"{Supermarket?.DataToText()} ");
            sb.Append($"{Adresa?.DataToText()} ");
            sb.Append($"{Role?.DataToText()} ");

            return sb.ToString().Trim();
        }

    }
}
