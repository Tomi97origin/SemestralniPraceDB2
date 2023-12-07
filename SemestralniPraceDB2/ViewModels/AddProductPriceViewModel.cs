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

namespace SemestralniPraceDB2.ViewModels;

partial class AddProductPriceViewModel : BaseViewModel
{
    [ObservableProperty]
    public Cena novaCena = new();

    [ObservableProperty]
    public ObservableCollection<Zbozi> seznamZbozi;

    public AddProductPriceViewModel()
    {
        SeznamZbozi = new(ZboziService.GetAll());
        NovaCena.PlatnostOd = DateTime.Today;
    }

    [RelayCommand]
    public void AddProductPrice()
    {
        string chybnePole = NewPriceIsValid();
        if (chybnePole == string.Empty)
        {
            CenaService.Create(NovaCena);
            MessageBox.Show($"Přidána nová cena:\n{NovaCena}.");
        }
        else
        {
            MessageBox.Show($"Pole {chybnePole} není vyplněno validně");
        }
    }

    private string NewPriceIsValid()
    {
        if (NovaCena.Castka <= 0) return "Částka";
        if (NovaCena.PlatnostOd <= DateTime.Now) return "Platnost od";
        if (NovaCena.Zbozi is null) return "Zboží";


        return string.Empty;
    }
}
