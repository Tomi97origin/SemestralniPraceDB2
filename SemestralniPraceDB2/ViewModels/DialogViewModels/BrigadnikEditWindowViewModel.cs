using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class BrigadnikEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Brigadnik? brigadnik;

        [ObservableProperty]
        private ObservableCollection<short> listOfUvazky = new() { 0, 1 };

        [ObservableProperty]
        private ObservableCollection<PlnyUvazek> listOfVedouci = new(PlnyUvazekService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Supermarket> listOfSupermarkets = new(SupermarketService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Role> listOfRoles = new(RoleService.GetAll());

        public BrigadnikEditWindowViewModel()
        {
            Brigadnik = new Brigadnik();
            Brigadnik.Adresa = new();
            Brigadnik.Nastup = DateTime.Today;
        }

        public BrigadnikEditWindowViewModel(Brigadnik? brigadnik)
        {
            Brigadnik = brigadnik;
        }


        //pozn.: tento zakomentovaný kód je základem pro načítání uložených dat do komboboxů při úpravě.
        //[ObservableProperty]
        //private PlnyUvazek? selectedVedouci;

        //[ObservableProperty]
        //private Supermarket? selectedSupermarket;

        //[ObservableProperty]
        //private Role? selectedRole;


        //partial void OnSelectedVedouciChanged(PlnyUvazek? value)
        //{
        //    if (Brigadnik != null)
        //    {
        //        if (SelectedVedouci is not null) Brigadnik.Vedouci = SelectedVedouci;
        //    }
        //}

        //partial void OnSelectedRoleChanged(Role? value)
        //{
        //    if (Brigadnik != null)
        //    {
        //        if (SelectedRole is not null) Brigadnik.Role = SelectedRole;
        //    }
        //}

        //partial void OnSelectedSupermarketChanged(Supermarket? value)
        //{
        //    if (Brigadnik != null)
        //    {
        //        if (SelectedSupermarket is not null) Brigadnik.Supermarket = SelectedSupermarket;
        //    }
        //}

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Brigadnik = null;
            window.Close();
        }
    }
}