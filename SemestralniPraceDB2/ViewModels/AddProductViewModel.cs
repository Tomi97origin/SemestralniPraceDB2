using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels
{
    partial class AddProductViewModel : BaseViewModel
    {

        [ObservableProperty]
        public Zbozi noveZbozi = new();

        [ObservableProperty]
        public ObservableCollection<Kategorie> seznamKategorii;

        [ObservableProperty]
        public ObservableCollection<Vyrobce> seznamVyrobcu;

        public AddProductViewModel()
        {
            SeznamKategorii = new();
            SeznamVyrobcu = new();

            Refresh();
        }

        public void Refresh()
        {
            SeznamKategorii = new ObservableCollection<Kategorie>(KategorieService.GetAll());
            SeznamVyrobcu = new(VyrobceService.GetAll());
        }

        [RelayCommand]
        public void AddProduct()
        {
            if (NewProductIsValid())
            {
                ZboziService.Create(NoveZbozi);
                MessageBox.Show($"Přidána nové zboží:\n{NoveZbozi}.");
            }
            else
            {
                MessageBox.Show("Zboží není vyplněno validně");
            }
        }

        private bool NewProductIsValid()
        {
            if (NoveZbozi.Nazev == string.Empty) return false;
            if (NoveZbozi.Popis == string.Empty) return false;
            if (NoveZbozi.EAN == string.Empty) return false;
            if (NoveZbozi.Kategorie is null) return false;
            if (NoveZbozi.Vyrobce is null) return false;

            return true;
        }
    }
}
