using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class VernostniKartaEditWindowViewModel : BaseViewModel
    {

        [ObservableProperty]
        public Vernostni_karta? vernostniKarta;

        public VernostniKartaEditWindowViewModel()
        {
            VernostniKarta = new() {Zalozeni = DateTime.Today };
        }

        public VernostniKartaEditWindowViewModel(Vernostni_karta? vernostniKarta)
        {
            VernostniKarta = vernostniKarta;
        }

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            VernostniKarta = null;
            window.Close();
        }
    }
}