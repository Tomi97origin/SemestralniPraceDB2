using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels;

public partial class ChooseContractorViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Dodavatel> dodavatele;

    [ObservableProperty]
    private string pocetDniText = string.Empty;

    [ObservableProperty]
    public Dodavatel? vybranyDodavatel;

    public ChooseContractorViewModel()
    {
        Dodavatele = new(DodavatelService.GetAll());
    }


    [RelayCommand]
    private void OK(Window window)
    {
        int pocetDni = 7;
        if (PocetDniText.Length > 0)
        {
            try
            {
                pocetDni = int.Parse(PocetDniText);
            }
            catch (FormatException)
            {
                MessageBox.Show("Zadejte celé platné číslo");
                return;
            }
            catch (ArgumentNullException)
            {
            }
        }


        if (VybranyDodavatel is null)
        {
            MessageBox.Show("Vyberte dodavatele.");
            return;
        }
        else if (pocetDni <= 0 || pocetDni > 30)
        {
            MessageBox.Show("Zadejte počet dní z intervalu <1;30>.");
            return;
        }
        else
        {
            ObjednavkaService.ProdlouzeniSplatnostiUDodavatele(VybranyDodavatel, pocetDni);

            if (window != null)
            {
                window.Close();
            }

            MessageBox.Show($"Objednávkám dodavatele {VybranyDodavatel} byla prodloužena splatnost o {pocetDni} dní.");
        }
    }


    [RelayCommand]
    private void Cancel(Window window)
    {
        if (window != null)
        {
            window.Close();
        }
    }

}
