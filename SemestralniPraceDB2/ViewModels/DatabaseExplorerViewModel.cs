using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels
{
    partial class DatabaseExplorerViewModel : BaseViewModel
    {

        [ObservableProperty]
        public DBTable? selectedTable;



        [ObservableProperty]
        ObservableCollection<DBTable> tables = new()
        {
            new DBTable ("AdresyLokalniTestovaci",10 ),
            new DBTable ("RoleLokalniTestovaci",20 )
        };


        [ObservableProperty]
        private ObservableCollection<object>? selectedTableData;

        public DatabaseExplorerViewModel()
        {
            //SelectedTableData.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedMethod);
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                MessageBox.Show($"Změna Add v tabulce");
            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                MessageBox.Show($"Změna Replace v tabulce");
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                MessageBox.Show($"Změna Remove v tabulce");
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                MessageBox.Show($"Změna Move v tabulce");
            }
        }

        partial void OnSelectedTableChanged(DBTable? value)
        {
            if (SelectedTable != null)
            {

                MessageBox.Show($"Vybraná tabulka se změnila. Načítám data z {value}.");

                switch (SelectedTable.TableName)
                {
                    case "AdresyLokalniTestovaci":
                        SelectedTableData = new()
                        {
                            new Adresa(1,"malá ulice",23,"Praha","CZ","45655"),
                            new Adresa(2,"velká ulice",125,"Liberec","CZ","56551"),
                            new Adresa(3,"střední ulice",7,"Olomouc","CZ","12344")
                        };
                        break;
                    case "RoleLokalniTestovaci":
                        SelectedTableData = new()
                        {
                            new Role(1,"Big boss"),
                            new Role(2,"holka pro všechno"),
                            new Role(3,"Skladník"),
                            new Role(4,"Kvalifikovaný seřizovač pohonu pokladních pásů s aprobací na výuku českého jazyka z předhusitské doby")
                        };
                        break;
                    default: break;
                }
            }



        }

        partial void OnSelectedTableDataChanging(ObservableCollection<object>? value)
        {
            MessageBox.Show($"Data v Tabulce {SelectedTable} se budou měnit na {value}");
        }



        //private void UpdateAdressInDB()
        //{
        //    MessageBox.Show("Dont panic!");
        //}

        //partial void OnSelectedTableDataPropertyChanged(string value)
        //{
        //    GetSuggestions();
        //}

        //partial void OnSelectedTableChanged(string? value)
        //{
        //    MessageBox.Show("Dont panic!");
        //}

        
    }
}
