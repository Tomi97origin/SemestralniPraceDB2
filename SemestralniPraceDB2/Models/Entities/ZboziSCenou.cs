using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    class ZboziSCenou
    {
        public int ID { get; set; }
        public string Nazev {  get; set; }
        public string Popis {  get; set; }
        public string KategorieZkratka {  get; set; }
        public string VyrobceZkratka {  get; set; }
        public string EAN {  get; set; }
        public double Cena {  get; set; }

        public ZboziSCenou()
        {
            ID = 0;
            Nazev = string.Empty;
            Popis = string.Empty;
            KategorieZkratka = string.Empty;
            VyrobceZkratka = string.Empty;
            EAN = string.Empty;
            Cena = 0.0;
        }

        public ZboziSCenou(int iD, string nazev, string popis, string kategorieZkratka, string vyrobceZkratka, string eAN, double cena)
        {
            ID = iD;
            Nazev = nazev;
            Popis = popis;
            KategorieZkratka = kategorieZkratka;
            VyrobceZkratka = vyrobceZkratka;
            EAN = eAN;
            Cena = cena;
        }
    }
}
