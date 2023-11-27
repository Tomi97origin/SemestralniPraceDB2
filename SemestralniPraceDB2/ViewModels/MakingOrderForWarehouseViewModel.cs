using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;


namespace SemestralniPraceDB2.ViewModels
{

    partial class MakingOrderForWarehouseViewModel : BaseViewModel
    {

        public partial class VybraneZbozi : ObservableObject
        {
            public int ID { get; set; }
            public string Nazev { get; set; }
            public string CarovyKod { get; set; }
            public double CenaKs { get; set; }

            [ObservableProperty]
            [NotifyPropertyChangedFor(nameof(Cena))]
            public int mnozstvi;
            public double Cena => CenaKs * Mnozstvi;

            public VybraneZbozi(int iD, string nazev, string carovyKod, double cenaKs, int mnozstvi)
            {
                ID = iD;
                Nazev = nazev;
                CarovyKod = carovyKod;
                CenaKs = cenaKs;
                Mnozstvi = mnozstvi;
            }
        }

        [ObservableProperty]
        public double cenaCelkem;

        [ObservableProperty]
        public ObservableCollection<Dodavatel> seznamDodavatelu;

        [ObservableProperty]
        public Dodavatel vybranyDodavatel;

        [ObservableProperty]
        public ObservableCollection<ZboziSCenou> seznamZboziSCenou;

        [ObservableProperty]
        public ZboziSCenou? vybraneZboziSCenou;

        [ObservableProperty]
        public ObservableCollection<VybraneZbozi> seznamVybranehoZbozi = new();

        [ObservableProperty]
        public VybraneZbozi? vybraneVybraneZbozi;

        [ObservableProperty]
        public ObservableCollection<Supermarket> seznamSupermarketu;

        [ObservableProperty]
        public Supermarket? vybranySupermarket;

        [ObservableProperty]
        private int someOtheMember;



        public MakingOrderForWarehouseViewModel()
        {
            SeznamDodavatelu = new(DodavatelService.GetAll());
            if (SeznamDodavatelu.Count == 0)
            {
                MessageBox.Show("Nebyli nalezeni žádní dodavatelé, zkontrolujte připojení k databázi.");
                VybranyDodavatel = new();
                SeznamZboziSCenou = new();
            }
            else
            {
                VybranyDodavatel = SeznamDodavatelu.First();

                //naplnění seznamu zboží s cenou
                SeznamZboziSCenou = new();
                var seznam = CenaService.GetAllZboziWithCurentPrice(); //vrací seznam cen s vyplněným zbožím
                foreach (var v in seznam)
                {
                    if (v.Zbozi is not null)
                    {
                        var z = new ZboziSCenou(v.Zbozi.Id, v.Zbozi.Nazev, v.Zbozi.Popis, v.Zbozi.Kategorie.Zkratka ?? v.Zbozi.Kategorie.Nazev, v.Zbozi.Vyrobce.Zkratka, v.Zbozi.EAN, v.Castka);

                        SeznamZboziSCenou.Add(z);
                    }
                }
            }

            //načtení seznamu supermarketů a jejich adres
            SeznamSupermarketu = new(SupermarketService.GetAll());
            foreach (var s in SeznamSupermarketu)
            {
                if (s.Adresa is not null)
                {
                    s.Adresa = AdresaService.Get(s.Adresa) ?? new();
                }
            }
        }


        [RelayCommand]
        public void PridatPolozku()
        {
            if (VybraneZboziSCenou is null)
            {
                MessageBox.Show("Vyberte položku k přidání.");
            }
            else
            {
                var polozkaKPridani = new VybraneZbozi(
                    VybraneZboziSCenou.ID,
                    VybraneZboziSCenou.Nazev,
                    VybraneZboziSCenou.EAN,
                    VybraneZboziSCenou.Cena,
                    1);

                if (SeznamVybranehoZbozi.Any(vybrane => vybrane.ID == polozkaKPridani.ID))
                {
                    var i = SeznamVybranehoZbozi.ToList().FindIndex(vybrane => vybrane.ID == polozkaKPridani.ID);
                    SeznamVybranehoZbozi[i].Mnozstvi++;
                }
                else
                {
                    SeznamVybranehoZbozi.Add(polozkaKPridani);
                }
            }
        }


        [RelayCommand]
        public void OdebratPolozku()
        {
            if (VybraneVybraneZbozi is not null)
            {
                SeznamVybranehoZbozi.Remove(VybraneVybraneZbozi);
            }
            else
            {
                MessageBox.Show("Vyberte položku k odebrání.");
            }
        }


        [RelayCommand]
        public void PotvrditObjednavku()
        {
            if (SeznamVybranehoZbozi.Count < 1)
            {
                MessageBox.Show("Nelze vytvořit prázdnou objednávku.");
            }
            else if (VybranyDodavatel is null)
            {
                MessageBox.Show("Vyberte dodavatele zboží.");
            }
            else if (VybranySupermarket is null)
            {
                MessageBox.Show("Vyberte cílový supermarket.");
            }
            else
            {
                List<ObjednaneZbozi> seznamIdObjednanehoZbozi = new();
                CenaCelkem = 0;

                foreach (var item in SeznamVybranehoZbozi)
                {
                    CenaCelkem += item.Cena;
                    ObjednaneZbozi objednaneZbozi = new(item.Mnozstvi, item.Cena, new Zbozi() { Id = item.ID });
                    seznamIdObjednanehoZbozi.Add(objednaneZbozi);
                }



                Objednavka novaObjednavka = new(
                    0,
                    DateTime.Now,
                    DateTime.Now.AddDays(30),
                    CenaCelkem,
                    VybranySupermarket,
                    VybranyDodavatel);



                ObjednavkaService.Create(novaObjednavka, seznamIdObjednanehoZbozi);

                StringBuilder str = new();
                str.AppendLine("Vytvářím novou objednávku.");
                str.AppendLine($"Od dodavatele {VybranyDodavatel.Nazev}");
                str.AppendLine($"Pro supermarket {VybranySupermarket.Adresa}");
                str.AppendLine("Seznam zboží:");

                foreach (var item in SeznamVybranehoZbozi)
                {
                    str.AppendLine($"{item.Nazev}, {item.Mnozstvi} ks.");
                }

                MessageBox.Show(str.ToString());
            }
        }


    }
}