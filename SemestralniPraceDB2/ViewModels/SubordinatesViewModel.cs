using CommunityToolkit.Mvvm.ComponentModel;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.ViewModels
{
    partial class SubordinatesViewModel : BaseViewModel
    {
        [ObservableProperty]
        Zamestnanec? selectedEmployee;

        [ObservableProperty]
        ObservableCollection<Zamestnanec> employees = new(ZamestnanecService.GetAll());

        [ObservableProperty]
        ObservableCollection<Zamestnanec>? selectedEmployeeSubordinates;

        partial void OnSelectedEmployeeChanged(Zamestnanec? value)
        {
            if (SelectedEmployee != null)
            {
                SelectedEmployeeSubordinates = new(ZamestnanecService.GetSubordinates(SelectedEmployee));

                foreach (var i in SelectedEmployeeSubordinates)
                {
                    if (i.Supermarket is not null) i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                    if (i.Adresa is not null) i.Adresa = AdresaService.Get(i.Adresa) ?? new();
                    if (i.Role is not null) i.Role = RoleService.Get(i.Role) ?? new();
                    if (i.ObrazekZamestnance is not null) i.ObrazekZamestnance = ObrazekZamestnanceService.Get(i.ObrazekZamestnance) ?? new();
                }
            }
        }
    }
}
