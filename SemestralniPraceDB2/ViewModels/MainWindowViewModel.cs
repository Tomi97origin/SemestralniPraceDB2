using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
    [ObservableRecipient]
    partial class MainWindowViewModel : BaseViewModel, IRecipient<ViewChanged>
    {
        //Inicializace ViewModels
        private static readonly MakingOrderForWarehouseViewModel makingOrderForWarehouseVM = new();
        private static readonly WelcomeViewModel welcomeVM = new();
        private static readonly UserLoginViewModel userLoginVM = new();
        private static readonly UserRegistrationViewModel userRegistrationVM = new();
        private static readonly CreateEmployeeViewModel CreateEmployeeVM = new();
        private static readonly DatabaseExplorerViewModel DatabaseExplorerVM = new();

        [ObservableProperty]
        public BaseViewModel selectedViewModel = userLoginVM;

        public BaseViewModel? lastSelectedViewModel = null;

        [ObservableProperty]
        private TopMenuViewModel topMenuViewModel = new();

        [ObservableProperty]
        private DatabaseResult? _databaseResult;

        [ObservableProperty]
        private string statusLabelText = "Status OK";

        [ObservableProperty]
        private string loggedAs = "Nepřihlášený uživatel";



        public MainWindowViewModel()
        {
            Messenger = WeakReferenceMessenger.Default;
            Messenger.Register<ViewChanged>(this);
        }

        public void Receive(ViewChanged message)
        {
            BaseViewModel newVM = message.ViewName switch
            {
                "MakingOrderForWarehouse" => makingOrderForWarehouseVM,
                "UserRegistration" => userRegistrationVM,
                "CreateEmployee" => CreateEmployeeVM,
                "DatabaseExplorer" => DatabaseExplorerVM,
                _ => welcomeVM,
            };

            if (newVM != SelectedViewModel)
            {
                lastSelectedViewModel = SelectedViewModel;
                SelectedViewModel = newVM;
            }
        }

        [RelayCommand]
        private void ConnectAndGet()
        {
            DatabaseResult = new DatabaseResult
            {
                Text = DatabaseConnector.GetFromDatabase()
            };
        }

        [RelayCommand]
        private void UpdateView(string parameter)
        {
            if (lastSelectedViewModel is not null)
            {
                SelectedViewModel = lastSelectedViewModel;
                lastSelectedViewModel = null;
            }
        }
    }
}
