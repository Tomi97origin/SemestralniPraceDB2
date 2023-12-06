using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class ObjednavkaEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Objednavka? objednavka;

        [ObservableProperty]
        private ObservableCollection<Supermarket> listOfSupermarkets = new(SupermarketService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Dodavatel> listOfDodavatele = new(DodavatelService.GetAll());


        public ObjednavkaEditWindowViewModel()
        {
            Objednavka = new()
            {
                Vytvoreno = System.DateTime.Today,
                Splatnost = System.DateTime.Today.AddDays(7)
            };
        }

        public ObjednavkaEditWindowViewModel(Objednavka? objednavka)
        {
            Objednavka = objednavka;
        }



        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Objednavka = null;
            window.Close();
        }
    }
}