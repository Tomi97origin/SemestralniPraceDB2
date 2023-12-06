using CommunityToolkit.Mvvm.ComponentModel;
using SemestralniPraceDB2.Models.Entities;
using SemestralniPraceDB2.Models;
using System.Collections.ObjectModel;
using System;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class InventarniPolozkaEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        private InventarniPolozka? inventarniPolozka;

        [ObservableProperty]
        private ObservableCollection<Supermarket> listOfSupermarkets = new(SupermarketService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Zbozi> listOfZbozi = new(ZboziService.GetAll());

        public InventarniPolozkaEditWindowViewModel()
        {
            InventarniPolozka = new()
            {
                Naskladneno = DateTime.Today
            };
        }

        public InventarniPolozkaEditWindowViewModel(InventarniPolozka inventarniPolozka)
        {
            InventarniPolozka = inventarniPolozka;
        }

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            InventarniPolozka = null;
            window.Close();
        }
    }
}