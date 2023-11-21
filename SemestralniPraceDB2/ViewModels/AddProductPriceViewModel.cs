using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public ObservableCollection<Zbozi> seznamZbozi = new()
        {
        new Zbozi (0,"Ručník","S výšivkami","123123",new Kategorie(1,"Drogerie","dro"),new Vyrobce(2,"Řeznictví Mikulov", "řez. mik."))
        };

    public AddProductPriceViewModel()
    {
        NovaCena.PlatnostOd = DateTime.Today;
        NovaCena.PlatnostDo = DateTime.Today.AddDays(7);
    }

    [RelayCommand]
    public void AddProductPrice()
    {
        MessageBox.Show($"Přidávám novou cenu: {NovaCena}.");
    }
}
