using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class ObjednaneZboziEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public ObjednaneZbozi? objednaneZbozi = new();

        [ObservableProperty]
        private ObservableCollection<Objednavka> listOfObjednavky = new(ObjednavkaService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Zbozi> listOfZbozi = new(ZboziService.GetAll());



        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            ObjednaneZbozi = null;
            window.Close();
        }
    }
}