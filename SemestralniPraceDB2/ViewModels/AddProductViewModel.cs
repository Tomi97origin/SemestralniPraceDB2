using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public ObservableCollection<Kategorie> seznamKategorii = new()
        {
            new Kategorie(1,"Drogerie","dro"),
            new Kategorie(2,"Čerstvé zboží", "fresh")
        };

        [ObservableProperty]
        public ObservableCollection<Vyrobce> seznamVyrobcu = new()
        {
            new Vyrobce(1,"OPOCHEM","Opch"),
            new Vyrobce(2,"Řeznictví Mikulov", "řez. mik.")
        };


        [RelayCommand]
        public void AddProduct()
        {
            MessageBox.Show($"Přidávám nové zboží: {NoveZbozi}.");
        }
    }
}
