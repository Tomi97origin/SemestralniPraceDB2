using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class PokladnaEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Pokladna? pokladna = new();

        [ObservableProperty]
        private ObservableCollection<Supermarket> listOfSupermarkety = new(SupermarketService.GetAll());


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Pokladna = null;
            window.Close();
        }
    }
}