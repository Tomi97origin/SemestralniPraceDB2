using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class ProdaneZboziEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public ProdaneZbozi? prodaneZbozi = new();


        [ObservableProperty]
        private ObservableCollection<Zbozi> listOfZbozi = new(ZboziService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Uctenka> listOfUctenky = new(UctenkaService.GetAll());


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            ProdaneZbozi = null;
            window.Close();
        }
    }
}