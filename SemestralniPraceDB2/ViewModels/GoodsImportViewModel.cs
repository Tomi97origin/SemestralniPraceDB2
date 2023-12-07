using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels
{
    partial class GoodsImportViewModel : BaseViewModel
    {

        public class AcceptedItem
        {
            public int Count { get; set; }
            public string EAN { get; set; }

            public AcceptedItem(int count, string eAN)
            {
                Count = count;
                EAN = eAN;
            }
        }

        [ObservableProperty]
        public ObservableCollection<Objednavka> allOrders = new();

        [ObservableProperty]
        public Objednavka? selectedOrder;

        [ObservableProperty]
        public ObservableCollection<ObjednaneZbozi>? selectedOrderItems;

        [ObservableProperty]
        public ObjednaneZbozi? selectedOrderSelectedItem;

        [ObservableProperty]
        public ObservableCollection<AcceptedItem> acceptedItems = new();

        [ObservableProperty]
        public string? eANToAccept;

        [ObservableProperty]
        public string? countOfItemsToAccept;

        private int loadedOrderId = -1;

        public GoodsImportViewModel()
        {
            Refresh();
        }

        [RelayCommand]
        public void LoadOrder()
        {
            if (SelectedOrder is not null)
            {
                SelectedOrderItems = new(ObjednaneZboziService.GetFromObjednavka(SelectedOrder));
                foreach (var i in SelectedOrderItems)
                {
                    i.Zbozi = ZboziService.Get(i.Zbozi) ?? new();
                }
                AcceptedItems = new();
                loadedOrderId = SelectedOrder.Id;
            }
            else
            {
                MessageBox.Show("Vyberte nejdříve objednávku.");
            }
        }

        [RelayCommand]
        public void AddItem()
        {
            if (SelectedOrderItems is null)
            {
                MessageBox.Show("Nejdřívě načtěte objednávku.");
                return;
            }

            if (CountOfItemsToAccept is null)
            {
                MessageBox.Show("Zadejte počet.");
                return;
            }
            else if (EANToAccept is null)
            {
                MessageBox.Show("Zadejte čárový kód.");
                return;
            }
            else
            {
                AcceptedItems.Add(new(int.Parse(CountOfItemsToAccept), EANToAccept));
                CountOfItemsToAccept = null;
                EANToAccept = null;
            }
        }

        [RelayCommand]
        public void ImportSelectedItem()
        {
            if (SelectedOrderSelectedItem is not null)
            {
                CountOfItemsToAccept = SelectedOrderSelectedItem.Mnozstvi.ToString();
                EANToAccept = SelectedOrderSelectedItem.Zbozi.EAN;
            }
            else
            {
                MessageBox.Show("Vyberte zboží z objednávky.");
                return;
            }
        }

        [RelayCommand]
        public void ConfirmOrder()
        {

            if (SelectedOrderItems is null)
            {
                MessageBox.Show("Nejdříve načtěte objednávku.");
                return;
            }
            if (AccepterEqualsSelectedOrder())
            {
                ObjednavkaService.Prijato(new() { Id = loadedOrderId });
                MessageBox.Show("Objednávka přijatá");
                Refresh();
                SelectedOrder = null;
                SelectedOrderItems = null;
                SelectedOrderSelectedItem = null;
                AcceptedItems = new();
                EANToAccept = null;
                CountOfItemsToAccept = null;
            }
            else
            {
                MessageBox.Show("Přijaté zboží se neshoduje, nelze přijmout objednávku.");
            }
        }

        private bool AccepterEqualsSelectedOrder()
        {
            if (SelectedOrderItems is not null)
            {

                if (AcceptedItems.Count != SelectedOrderItems.Count)
                {
                    return false;
                }

                //vytvořit dictionary <ean, množství>
                Dictionary<string, int> objednavkaDict = new();
                foreach (var i in SelectedOrderItems)
                {
                    objednavkaDict.Add(i.Zbozi.EAN, i.Mnozstvi);
                }

                //zkontorlovat, že veškeré přijaté zboží je v objednávce
                foreach (var i in AcceptedItems)
                {
                    if (!objednavkaDict.ContainsKey(i.EAN))
                    {
                        return false;
                    }

                    if (objednavkaDict[i.EAN] != i.Count)
                    {
                        return false;
                    }

                }
                return true;
            }
            else return false;
        }

        public void Refresh()
        {
            AllOrders = new(ObjednavkaService.GetAll(true));

            foreach (var i in AllOrders)
            {
                i.Supermarket = SupermarketService.Get(i.Supermarket) ?? new();
                i.Dodavatel = DodavatelService.Get(i.Dodavatel) ?? new();
            }
        }
    }
}