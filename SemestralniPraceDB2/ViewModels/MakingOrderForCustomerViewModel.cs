using CommunityToolkit.Mvvm.ComponentModel;
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

            public VybraneZbozi(int iD,int idInvent, string nazev, string carovyKod, double cenaKs, int mnozstvi)
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
        public Pokladna vybranaPokladna;

        [ObservableProperty]
        public ObservableCollection<InventarniPolozkaSCenou> seznamZboziSCenou;

        [ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(SeznamZboziSCenou))]
        public InventarniPolozkaSCenou? vybraneZboziSCenou;

        [ObservableProperty]
        public ObservableCollection<VybraneZbozi> seznamVybranehoZbozi = new();

        [ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(SeznamVybranehoZbozi))]
        public VybraneZbozi? vybraneVybraneZbozi;

        [ObservableProperty]
        public ObservableCollection<Supermarket> seznamSupermarketu;

        [ObservableProperty]
        public Supermarket? vybranySupermarket;

        [ObservableProperty]
        public ObservableCollection<Vernostni_karta> seznamVerKaret;

        [ObservableProperty]
        public Vernostni_karta? vybranaVerKarta;

        [ObservableProperty]
        private int someOtheMember;



        public MakingOrderForCustomerViewModel()
        {
            SeznamSupermarketu = new(SupermarketService.GetAll());
            SeznamPokladen = new();
            SeznamZboziSCenou = new();
            SeznamVerKaret = new(VernostniKartaService.GetAll());
            SeznamVerKaret.Insert(0, new Vernostni_karta() { Jmeno = string.Empty, Cislo_Karty = string.Empty});
            VybranySupermarket = SeznamSupermarketu.First();
        }

        partial void OnVybranySupermarketChanged(Supermarket? value)
        {
            RefreshSupermarketChange();
        }

        private void RefreshSupermarketChange()
        {
            SeznamPokladen.Clear();
            var pokladny = (PokladnaService.GetFromSupermarket(vybranySupermarket));
            foreach (var i in pokladny) SeznamPokladen.Add(i);
            if (SeznamPokladen.Count > 0)
            {
                VybranaPokladna = SeznamPokladen.First();
            }
            SeznamZboziSCenou.Clear();
            var seznam = InventarniPolozkaService.GetAllZboziWithCurentPriceFromInventory(vybranySupermarket);
            foreach (var v in seznam)
            {
                SeznamZboziSCenou.Add(v);
            }
            SeznamVybranehoZbozi.Clear();
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

                if (SeznamVybranehoZbozi.Any(vybrane => vybrane.ID == polozkaKPridani.ID) )
                {
                    
                        var i = SeznamVybranehoZbozi.ToList().FindIndex(vybrane => vybrane.ID == polozkaKPridani.ID);
                        SeznamVybranehoZbozi[i].Mnozstvi++;
                        
                }
                else
                {
                    SeznamVybranehoZbozi.Add(polozkaKPridani);
                }
                VybraneZboziSCenou.Mnozstvi--;
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
            }
            else
            {
                MessageBox.Show("Vyberte položku k odebrání.");
            }
        }


       [RelayCommand]
        public void PotvrditNakup()
        {
            if (SeznamVybranehoZbozi.Count < 1)
            {
                MessageBox.Show("Nelze vytvořit prázdnou objednávku.");
            }
            /*else if (VybranyDodavatel is null)
            {
                MessageBox.Show("Vyberte dodavatele zboží.");
            }*/
            else if (VybranySupermarket is null)
            {
                MessageBox.Show("Vyberte cílový supermarket.");
            }
            else
            {
                List<ProdaneZbozi> polozky = new();
                /*List<ObjednaneZbozi> seznamIdObjednanehoZbozi = new();
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
                .Create(novaObjednavka, seznamIdObjednanehoZbozi);*/
                //UctenkaService.Create()
            }
        }

    }
}