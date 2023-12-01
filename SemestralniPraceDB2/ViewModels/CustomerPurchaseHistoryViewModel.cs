using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels
{
    partial class CustomerPurchaseHistoryViewModel : BaseViewModel
    {

        [ObservableProperty]
        public string? customerCardNumber;

        [ObservableProperty]
        public ObservableCollection<Platba>? purchaseList;

        [ObservableProperty]
        public Platba? selectedPurchase;

        [ObservableProperty]
        public ObservableCollection<ProdaneZbozi>? selectedPurchaseDetails;


        partial void OnSelectedPurchaseChanged(Platba? value)
        {
            if(SelectedPurchase is not null)
            {
                SelectedPurchaseDetails = new(ProdaneZboziService.GetFromPlatba(SelectedPurchase));
                foreach (var i in SelectedPurchaseDetails)
                {
                    if (i.Uctenka is not null) i.Uctenka = UctenkaService.Get(i.Uctenka)??new();
                }
            }
        }

        [RelayCommand]
        public void LoadPurchases()
        {
            if (CustomerCardNumber is not null)
            {
                PurchaseList = new(PlatbaService.GetPlatbyZakaznika(CustomerCardNumber));
                foreach(var i in PurchaseList)
                {
                    if (i.Vernostni_Karta is not null) i.Vernostni_Karta = VernostniKartaService.Get(i.Vernostni_Karta);
                    if (i.Vydavatel is not null) i.Vydavatel = VydavatelService.Get(i.Vydavatel);
                }
            }
            else
            {
                MessageBox.Show("Zadejte číslo karty");
            }


        }

    }
}
