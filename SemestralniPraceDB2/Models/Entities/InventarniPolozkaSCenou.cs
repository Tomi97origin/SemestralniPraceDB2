using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.Models.Entities
{
    public partial class InventarniPolozkaSCenou : ObservableObject
    {
        public int IdZbozi { get; set; }
        public int IdInventPolozky { get; set; }
        public string Nazev { get; set; }
        public string Popis { get; set; }
        public string Kategorie { get; set; }
        public string Vyrobce { get; set; }
        public string EAN { get; set; }
        public double Cena { get; set; }

        [ObservableProperty]
        public int mnozstvi;

        public InventarniPolozkaSCenou()
        {
            IdZbozi = 0;
            Nazev = string.Empty;
            Popis = string.Empty;
            Kategorie = string.Empty;
            Vyrobce = string.Empty;
            EAN = string.Empty;
            Cena = 0.0;
            Mnozstvi = 0;
        }

        public InventarniPolozkaSCenou(int iD, string nazev, string popis, string kategorieZkratka, string vyrobceZkratka, string eAN, double cena, int pocet)
        {
            IdZbozi = iD;
            Nazev = nazev;
            Popis = popis;
            Kategorie = kategorieZkratka;
            Vyrobce = vyrobceZkratka;
            EAN = eAN;
            Cena = cena;
            mnozstvi = pocet;
        }
    }
}
