using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class UctenkaEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Uctenka? uctenka;

        [ObservableProperty]
        private ObservableCollection<Pokladna> listOfPokladny = new(PokladnaService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Platba> listOfPlatby = new(PlatbaService.GetAll());


        public UctenkaEditWindowViewModel()
        {
            Uctenka = new() { Vytvoreno = System.DateTime.Today};
        }

        public UctenkaEditWindowViewModel(Uctenka? uctenka)
        {
            Uctenka = uctenka;
        }


        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Uctenka = null;
            window.Close();
        }

    }
}