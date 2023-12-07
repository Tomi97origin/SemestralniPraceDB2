using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using SemestralniPraceDB2.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class PlnyUvazekEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public PlnyUvazek? plnyUvazek;

        [ObservableProperty]
        private ObservableCollection<PlnyUvazek> listOfVedouci = new(PlnyUvazekService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Supermarket> listOfSupermarkets = new(SupermarketService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Role> listOfRoles = new(RoleService.GetAll());

        [ObservableProperty]
        private ObservableCollection<ObrazekZamestnance> listOfObrazky = new(ObrazekZamestnanceService.GetAll());

        public PlnyUvazekEditWindowViewModel()
        {
            PlnyUvazek = new PlnyUvazek
            {
                Adresa = new(),
                Nastup = DateTime.Today,
                TypUvazku = 1,
                PlatnostDo = DateTime.Today.AddYears(2)
            };
        }


        public PlnyUvazekEditWindowViewModel(PlnyUvazek? plnyUvazek)
        {
            PlnyUvazek = plnyUvazek;
        }

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            PlnyUvazek = null;
            window.Close();
        }
    }
}