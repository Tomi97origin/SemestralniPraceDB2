using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class VydavatelEditWindowViewModel :BaseViewModel
    {

        [ObservableProperty]
        public Vydavatel? vydavatel = new();


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Vydavatel = null;
            window.Close();
        }
    }
}