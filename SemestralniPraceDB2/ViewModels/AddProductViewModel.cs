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
            var vsechnyKategorie = KategorieService.GetAll();

            seznamKategorii = new ObservableCollection<Kategorie>(vsechnyKategorie);

            var vsichniVyrobci = VyrobceService.GetAll();

            seznamVyrobcu = new(vsichniVyrobci);
        }

        [RelayCommand]
        public void AddProduct()
        {
            MessageBox.Show($"Přidávám nové zboží: {NoveZbozi}.");
        }
    }
}
