using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels
{
    partial class UserActivationViewModel : BaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<Uzivatel> seznamUzivatelu;

        [RelayCommand]
        private void ZapsatZmeny()
        {
            var dbList = UzivateleService.GetAll();

            for (int i = 0; i < dbList.Count; i++)
            {
                if (dbList[i].Active != SeznamUzivatelu[i].Active)
                {
                    UzivateleService.Update(SeznamUzivatelu[i]);
                }
            }
            MessageBox.Show("Změny zapsány", "Operace dokončena", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public UserActivationViewModel()
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
