using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class CenaEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Cena? cena;

        [ObservableProperty]
        private ObservableCollection<Zbozi> listOfZbozi = new(ZboziService.GetAll());

        public CenaEditWindowViewModel()
        {
            Cena = new()
            {
                PlatnostOd = DateTime.Today.AddDays(1)
            };
        }

        public CenaEditWindowViewModel(Cena? cena)
        {
            Cena = cena;
        }


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Cena = null;
            window.Close();
        }
    }
}