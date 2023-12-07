using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models.Entities;
using SemestralniPraceDB2.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class ZamestnanecEditWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Zamestnanec? zamestnanec;

        [ObservableProperty]
        private ObservableCollection<string> listOfUvazky = new() { "Plný úvazek", "Brigádník" };

        [ObservableProperty]
        private ObservableCollection<PlnyUvazek> listOfVedouci = new(PlnyUvazekService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Supermarket> listOfSupermarkets = new(SupermarketService.GetAll());

        [ObservableProperty]
        private ObservableCollection<Role> listOfRoles = new(RoleService.GetAll());

        [ObservableProperty]
        private ObservableCollection<ObrazekZamestnance> listOfObrazky = new(ObrazekZamestnanceService.GetAll());

        [ObservableProperty]
        private string? selectedTypUvazku;

        public ZamestnanecEditWindowViewModel()
        {
            Zamestnanec = new()
            {
                Adresa = new(),
                Nastup = DateTime.Today
            };
        }

        partial void OnSelectedTypUvazkuChanged(string? value)
        {
            if (SelectedTypUvazku is not null && Zamestnanec is not null)
            {
                switch (SelectedTypUvazku)
                {
                    case "Plný úvazek":
                        Zamestnanec.TypUvazku = 1;
                        Zamestnanec = new PlnyUvazek(Zamestnanec, 0, DateTime.MinValue);
                        break;
                    case "Brigádník":
                        Zamestnanec.TypUvazku = 0;
                        Zamestnanec = new Brigadnik(Zamestnanec, 0, 0);
                        break;
                }
            }
        }

        public ZamestnanecEditWindowViewModel(Zamestnanec? zamestnanec)
        {
            if (zamestnanec?.TypUvazkuText == "Plný úvazek")
            {
                Zamestnanec = PlnyUvazekService.Get(new PlnyUvazek { Id = zamestnanec.Id });
                if (Zamestnanec is not null)
                {
                    if (Zamestnanec.Adresa is not null) Zamestnanec.Adresa = AdresaService.Get(Zamestnanec.Adresa);
                }
            }
            else if (zamestnanec?.TypUvazkuText == "Brigádník")
            {
                Zamestnanec = BrigadnikService.Get(new Brigadnik { Id = zamestnanec.Id });
            }
        }

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            Zamestnanec = null;
            window.Close();
        }

    }
}