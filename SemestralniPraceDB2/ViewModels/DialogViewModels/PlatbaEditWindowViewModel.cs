using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class PlatbaEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Platba? platba = new();

        [ObservableProperty]
        private ObservableCollection<Vydavatel> listOfVydavatele = new(VydavatelService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Vernostni_karta> listOfVernostniKarty = new(VernostniKartaService.GetAll());

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Platba = null;
            window.Close();
        }

    }
}