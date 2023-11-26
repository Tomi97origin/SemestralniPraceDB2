using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            public double Cena { get; set; }

            [ObservableProperty]
            public int mnozstvi;

            public VybraneZbozi(int iD, string nazev, string carovyKod, double cena, int mnozstvi)
            {
                ID = iD;
                Nazev = nazev;
                CarovyKod = carovyKod;
                Cena = cena;
                Mnozstvi = mnozstvi;
            }
        }

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

        public MakingOrderForWarehouseViewModel()
        {
            MessageBox.Show("Hellou");

            SeznamDodavatelu = new(DodavatelService.GetAll());
            if (SeznamDodavatelu.Count == 0)
            {
                MessageBox.Show("Nebyli nalezeni žádní dodavatelé");
                VybranyDodavatel = new();
                SeznamZboziSCenou = new();
            }
            else
            {
                VybranyDodavatel = SeznamDodavatelu.First();
                //ZboziSCenouService.GetPodleDodavatele(VybranyDodavatel);
            }

            //CenaService.GetCurrent(new());

            SeznamZboziSCenou = new() //todo: toto odstranit a zavolat výše viz předpřipravený kód
                    {
                       new ZboziSCenou(10,$"nejake zbozi od dodavatele {vybranyDodavatel}",
                       "popis tohoto", "nejZboz", "Zkratka Dod", "123123", 59.60)
                    };

            SeznamSupermarketu = new(SupermarketService.GetAll());
        }

        partial void OnVybranyDodavatelChanged(Dodavatel? value)
        {
            SeznamZboziSCenou = new() //todo: toto odstranit a zavolat výše viz předpřipravený kód
                    {
                       new ZboziSCenou(10,$"nejake zbozi od dodavatele {value}",
                       "popis tohoto", "nejZboz", "Zkratka Dod", "123123", 59.60)
                    };
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
            if (VybranySupermarket is null)
            {
                MessageBox.Show("Vyberte cílový supermarket.");
            }
            else if (SeznamVybranehoZbozi.Count < 1)
            {
                MessageBox.Show("Nelze vytvořit prázdnou objednávku.");
            }
            else
            {

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