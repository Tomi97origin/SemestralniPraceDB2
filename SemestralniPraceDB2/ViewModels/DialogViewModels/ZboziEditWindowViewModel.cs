using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class ZboziEditWindowViewModel : BaseViewModel
    {

        [ObservableProperty]
        public Zbozi? zbozi = new();

        [ObservableProperty]
        private ObservableCollection<Kategorie> listOfKategorie = new(KategorieService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Vyrobce> listOfVyrobci = new(VyrobceService.GetAll());


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Zbozi = null;
            window.Close();
        }

    }
}