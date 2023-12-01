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
            //todo
                //SelectedPurchaseDetails = new(ProdaneZboziService.GetFromPlatba(SelectedPurchase);
            }
        }

        [RelayCommand]
        public void LoadPurchases()
        {
            if (CustomerCardNumber is not null)
            {
                //var karta = VernostniKartaService.GetFromCardNumber(CustomerCardNumber);
                //PurchaseList = new(PlatbaService.GetPlatbyZakaznika(karta));
            }
            else
            {
                MessageBox.Show("Zadejte číslo karty");
            }


        }

    }
}
