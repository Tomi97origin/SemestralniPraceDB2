﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualBasic;
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

    partial class MakingOrderForCustomerViewModel : BaseViewModel
    {

        public partial class VybraneZbozi : ObservableObject
        {
            public int ID { get; set; }
            public int IdInvent { get; set; }
            public string Nazev { get; set; }
            public string CarovyKod { get; set; }
            public double CenaKs { get; set; }

            [ObservableProperty]
            [NotifyPropertyChangedFor(nameof(Cena))]
            public int mnozstvi;
            public double Cena => CenaKs * Mnozstvi;

            public VybraneZbozi(int iD, int idInvent, string nazev, string carovyKod, double cenaKs, int mnozstvi)
            {
                ID = iD;
                IdInvent = idInvent;
                Nazev = nazev;
                CarovyKod = carovyKod;
                CenaKs = cenaKs;
                Mnozstvi = mnozstvi;
            }
        }

        [ObservableProperty]
        public double cenaCelkem;

        [ObservableProperty]
        public ObservableCollection<Pokladna> seznamPokladen;

        [ObservableProperty]
        public Pokladna? vybranaPokladna;

        [ObservableProperty]
        public ObservableCollection<InventarniPolozkaSCenou> seznamZboziSCenou;

        [ObservableProperty]
        public InventarniPolozkaSCenou? vybraneZboziSCenou;

        [ObservableProperty]
        public ObservableCollection<VybraneZbozi> seznamVybranehoZbozi = new();

        [ObservableProperty]
        public VybraneZbozi? vybraneVybraneZbozi;

        [ObservableProperty]
        public ObservableCollection<Supermarket> seznamSupermarketu;

        [ObservableProperty]
        public Supermarket? vybranySupermarket;

        [ObservableProperty]
        public ObservableCollection<Vernostni_karta> seznamVerKaret;

        [ObservableProperty]
        public Vernostni_karta vybranaVerKarta;

        [ObservableProperty]
        public ObservableCollection<string> typPlatby = new() { "Hotovost", "Karta" };

        [ObservableProperty]
        public string vybranyTypPlatby;

        [ObservableProperty]
        public double? vraceno;

        [ObservableProperty]
        public string? cisloKarty;

        [ObservableProperty]
        public bool debit;

        [ObservableProperty]
        public ObservableCollection<Vydavatel> seznamVydavatelu;

        [ObservableProperty]
        public Vydavatel? vybranyVydavatel;

        public MakingOrderForCustomerViewModel()
        {
            SetUp();
        }

        public void SetUp()
        {
            SeznamSupermarketu = new(SupermarketService.GetAll());
            SeznamPokladen = new();
            SeznamZboziSCenou = new();
            SeznamVerKaret = new(VernostniKartaService.GetAll());
            SeznamVerKaret.Insert(0, new Vernostni_karta() { Jmeno = string.Empty, Cislo_Karty = string.Empty });
            SeznamVydavatelu = new(VydavatelService.GetAll());
            VybranaVerKarta = SeznamVerKaret.First();
            if (SeznamVydavatelu.Count > 0)
            {
                VybranyVydavatel = SeznamVydavatelu.First();
            }
            if (SeznamSupermarketu.Count > 0)
            {
                VybranySupermarket = SeznamSupermarketu.First();
            }
            if (TypPlatby.Count > 0)
            {
                VybranyTypPlatby = TypPlatby.First();
            }
            Refresh();
        }

        partial void OnVybranySupermarketChanged(Supermarket? value)
        {
            Refresh();
        }

        void RefreshTotalPrice()
        {
            CenaCelkem = SeznamVybranehoZbozi.Select(x => x.Cena).Sum();
        }

        private void RefreshSupermarketChange()
        {
            if (VybranySupermarket is null)
            {
                return;
            }
            var pokladny = (PokladnaService.GetFromSupermarket(VybranySupermarket));
            foreach (var i in pokladny) SeznamPokladen.Add(i);
            if (SeznamPokladen.Count > 0)
            {
                VybranaPokladna = SeznamPokladen.First();
            }
            var seznam = InventarniPolozkaService.GetAllZboziWithCurentPriceFromInventory(VybranySupermarket);
            foreach (var v in seznam)
            {
                SeznamZboziSCenou.Add(v);
            }
            RefreshTotalPrice();
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
                if (VybraneZboziSCenou.Mnozstvi <= 0)
                {
                    return;
                }
                var polozkaKPridani = new VybraneZbozi(
                    VybraneZboziSCenou.IdZbozi,
                    VybraneZboziSCenou.IdInventPolozky,
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
                VybraneZboziSCenou.Mnozstvi--;
                RefreshTotalPrice();
            }
        }


        [RelayCommand]
        public void OdebratPolozku()
        {
            if (VybraneVybraneZbozi is not null)
            {
                var vybrane = SeznamZboziSCenou.ToList().FindIndex(vybrane => vybrane.IdInventPolozky == VybraneVybraneZbozi.IdInvent);
                SeznamZboziSCenou[vybrane].Mnozstvi++;
                if (SeznamVybranehoZbozi.Where(p => p.Mnozstvi > 1).Any(vybrane => vybrane.ID == VybraneVybraneZbozi.ID))
                {

                    VybraneVybraneZbozi.Mnozstvi--;
                }
                else
                {
                    SeznamVybranehoZbozi.Remove(VybraneVybraneZbozi);
                }
                RefreshTotalPrice();
            }
            else
            {
                MessageBox.Show("Vyberte položku k odebrání.");
            }
        }


        [RelayCommand]
        public void PotvrditNakup()
        {
            Platba platba = new();
            platba.Hotovost = VybranyTypPlatby.Equals(TypPlatby[0]);
            if (!string.IsNullOrEmpty(VybranaVerKarta.Cislo_Karty))
            {
                platba.Vernostni_Karta = VybranaVerKarta;
            }
            if(VybranaPokladna is null)
            {
                MessageBox.Show("Vyberte pokladnu.");
                return;
            }
            if (SeznamVybranehoZbozi.Count < 1)
            {
                MessageBox.Show("Nelze vytvořit prázdný nákup.");
                return;
            }
            else if (platba.Hotovost)
            {
                if (Vraceno is null)
                {
                    MessageBox.Show("Neplatná hodnota Vráceno.");
                    return;
                }
                platba.Vraceno = (double)Vraceno;
            }
            else
            {
                if (CisloKarty is null || CisloKarty.Length < 16)
                {
                    MessageBox.Show("Neplatné Číslo karty.");
                    return;
                }
                platba.Debit = (short?)(Debit ? 1 : 0);
                platba.Vydavatel = VybranyVydavatel;
                platba.CisloKarty = CisloKarty;
            }

            Uctenka uctenka = new Uctenka()
            {
                Id = 0,
                CelkovaCena = CenaCelkem,
                Pokladna = VybranaPokladna,
                Vytvoreno = DateTime.Now,
                Platba = platba
            };

            List<ProdaneZbozi> polozky = (from zbozi in SeznamVybranehoZbozi
                                          select new ProdaneZbozi
                                          {
                                              Cena = zbozi.Cena,
                                              Mnozstvi = zbozi.Mnozstvi,
                                              Zbozi = new() { Id = zbozi.ID },
                                              Uctenka = uctenka
                                          }).ToList();

            List<InventarniPolozkaSCenou> inventarZmeny = (from zbozi in SeznamZboziSCenou
                                                           from vybraneZbozi in SeznamVybranehoZbozi
                                                           where zbozi.IdInventPolozky == vybraneZbozi.IdInvent
                                                           select new InventarniPolozkaSCenou
                                                           {
                                                               IdInventPolozky = zbozi.IdInventPolozky,
                                                               Mnozstvi = vybraneZbozi.Mnozstvi
                                                           }).ToList();

            var result = UctenkaService.Create(uctenka, polozky, inventarZmeny);
            if (!result)
            {
                MessageBox.Show("Zboží vyprodáno.");
                return;
            }
            SetUp();
            
        }

        private void Refresh()
        {
            Vraceno = 0;
            VybranyTypPlatby = TypPlatby.First();
            CisloKarty = string.Empty;
            Debit = false;
            VybranyVydavatel = SeznamVydavatelu.First();
            VybranaVerKarta = SeznamVerKaret.First();
            SeznamPokladen.Clear();
            SeznamZboziSCenou.Clear();
            SeznamVybranehoZbozi.Clear();
            CenaCelkem = 0;
            RefreshSupermarketChange();
        }
    }
}