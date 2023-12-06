using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class DodavatelEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Dodavatel? dodavatel;

        public DodavatelEditWindowViewModel()
        {
            Dodavatel = new();
            Dodavatel.Adresa = new();
        }

        public DodavatelEditWindowViewModel(Dodavatel? dodavatel)
        {
            Dodavatel = dodavatel;
        }

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Dodavatel = null;
            window.Close();
        }
    }
}