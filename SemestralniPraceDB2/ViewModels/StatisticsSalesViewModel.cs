using CommunityToolkit.Mvvm.ComponentModel;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.ViewModels
{
    partial class StatisticsSalesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Supermarket> supermarketList;

        [ObservableProperty]
        private Supermarket? selectedSupermarket;

        [ObservableProperty]
        private ObservableCollection<ZboziStatisticka>? salesList;

        public StatisticsSalesViewModel()
        {
            SupermarketList = new();

            Refresh();
        }

        partial void OnSelectedSupermarketChanged(Supermarket? value)
        {
            if (SelectedSupermarket is not null)
            {
                SalesList = new(SupermarketProdejeService.ProdejeSupermarketu(SelectedSupermarket));
            }
        }

        internal void Refresh()
        {
            SupermarketList = new(SupermarketService.GetAllInOrderOfSales());
            SalesList = new();
        }
    }
}
