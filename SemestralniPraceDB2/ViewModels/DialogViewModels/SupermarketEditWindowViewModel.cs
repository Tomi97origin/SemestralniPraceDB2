using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using System.Reflection;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class SupermarketEditWindowViewModel:BaseViewModel
    {
        [ObservableProperty]
        public Supermarket? supermarket;

        public SupermarketEditWindowViewModel()
        {
            Supermarket = new() { Adresa = new() };
        }

        public SupermarketEditWindowViewModel(Supermarket? supermarket)
        {
            Supermarket = supermarket;
        }


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Supermarket = null;
            window.Close();
        }

    }
}