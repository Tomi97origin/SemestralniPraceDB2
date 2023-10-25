using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.ViewModels
{
    [ObservableObject]
    partial class MainWindowViewModel //: BaseViewModel
    {

        //Inicializace ViewModels
        private static readonly ObjednavkaViewModel objednavkaVM = new ObjednavkaViewModel();

        [ObservableProperty]
        public BaseViewModel selectedViewModel = objednavkaVM;

        [ObservableProperty]
        private TopMenuViewModel topMenuViewModel = new TopMenuViewModel();

        [ObservableProperty]
        private DatabaseResult? _databaseResult;

        [ObservableProperty]
        private string statusLabelText = "Status OK";

        [RelayCommand]
        private void ConnectAndGet()
        {
            DatabaseResult = new DatabaseResult
            {
                Text = DatabaseConnector.GetFromDatabase()
            };
        }
    }
}
