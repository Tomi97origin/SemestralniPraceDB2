using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using SemestralniPraceDB2.Models;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;

namespace SemestralniPraceDB2.ViewModels
{
    partial class UserEmulationViewModel:BaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<Uzivatel> seznamUzivatelu;

        [ObservableProperty]
        public Uzivatel? selectedUzivatel;


        private readonly IMessenger messenger = WeakReferenceMessenger.Default;


        [RelayCommand]
        private void EmulovatUzivatele()
        {
            if(SelectedUzivatel is null)
            {
                MessageBox.Show("Nejdříve vyberte uživatele");
            }
            else
            {
                UzivateleService.Emulate(SelectedUzivatel);

                MainWindowViewModel.topMenuVM.UpdateView("default");
                messenger.Send<UserEmulation>(new(SelectedUzivatel));
                MessageBox.Show($"Nyní se emuluje uživatel {SelectedUzivatel.Username}");
            }

        }

        public UserEmulationViewModel()
        {
            SeznamUzivatelu = new();
            Refresh();
        }

        public void Refresh()
        {
            SeznamUzivatelu = new(UzivateleService.GetAll());
        }
    }
}