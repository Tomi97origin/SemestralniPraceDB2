﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Windows;

namespace SemestralniPraceDB2.ViewModels
{
    [ObservableRecipient]
    partial class MainWindowViewModel : BaseViewModel, IRecipient<ViewChanged>, IRecipient<UserLogin>, IRecipient<UserLogout>
    {
        //Inicializace ViewModels
        private static readonly MakingOrderForWarehouseViewModel makingOrderForWarehouseVM = new();
        private static readonly EmptyViewModel messageVM = new();
        private static readonly UserLoginViewModel userLoginVM = new();
        private static readonly UserRegistrationViewModel userRegistrationVM = new();
        private static readonly CreateEmployeeViewModel createEmployeeVM = new();
        private static readonly DatabaseExplorerViewModel databaseExplorerVM = new();
        private static readonly CustomerPurchaseHistoryViewModel customerPurchaseHistoryVM = new();
        private static readonly GoodsImportViewModel goodsImportVM = new();

        private static readonly BaseViewModel defaultVM = messageVM;
        
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
            Messenger.Register<UserLogin>(this);
            Messenger.Register<UserLogout>(this);
        }

        public void Receive(ViewChanged message)
        {
            BaseViewModel newVM = message.ViewName switch
            {
                "MakingOrderForWarehouse" => makingOrderForWarehouseVM,
                "UserRegistration" => userRegistrationVM,
                "CreateEmployee" => createEmployeeVM,
                "DatabaseExplorer" => databaseExplorerVM,
                "CustomerPurchaseHistory" => customerPurchaseHistoryVM,
                "UserLogin" => userLoginVM,
                "GoodsImport" => goodsImportVM,
                _ => messageVM,
            };

            if (newVM != SelectedViewModel)
            {
                lastSelectedViewModel = SelectedViewModel;
                SelectedViewModel = newVM;
            }
        }

        public void Receive(UserLogout message)
        {
            LoggedAs = "Nepřihlášený uživatel";
            SelectedViewModel = defaultVM;
            MessageBox.Show("Odhlášení úspěšné.");
        }

        public void Receive(UserLogin message)
        {
            LoggedAs = $"Přihlášen jako {message.UserName}.";
            SelectedViewModel = defaultVM;
            MessageBox.Show("Přihlášení úspěšné.");
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
