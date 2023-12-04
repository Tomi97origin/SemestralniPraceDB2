using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.ViewModels.DialogViewModels;
using SemestralniPraceDB2.Views;
using SemestralniPraceDB2.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
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
        public static readonly MakingOrderForWarehouseViewModel makingOrderForWarehouseVM = new();
        public static readonly EmptyViewModel messageVM = new();
        public static readonly UserLoginViewModel userLoginVM = new();
        public static readonly UserRegistrationViewModel userRegistrationVM = new();
        public static readonly CreateEmployeeViewModel createEmployeeVM = new();
        public static readonly DatabaseExplorerViewModel databaseExplorerVM = new();
        public static readonly CustomerPurchaseHistoryViewModel customerPurchaseHistoryVM = new();
        public static readonly GoodsImportViewModel goodsImportVM = new();
        public static readonly AddProductViewModel addProductVM = new();
        public static readonly AddProductPriceViewModel addProductPriceVM = new();
        public static readonly SystemCatalogViewModel systemCatalogVM = new();
        public static readonly SubordinatesViewModel subordinatesVM = new();
        public static readonly UserActivationViewModel userActivationVM = new();
        public static readonly PasswordChangeViewModel passwordChangeVM = new();

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
            //změna viewModelu
            BaseViewModel newVM = message.ViewName switch
            {
                "MakingOrderForWarehouse" => makingOrderForWarehouseVM,
                "UserRegistration" => userRegistrationVM,
                "CreateEmployee" => createEmployeeVM,
                "DatabaseExplorer" => databaseExplorerVM,
                "CustomerPurchaseHistory" => customerPurchaseHistoryVM,
                "UserLogin" => userLoginVM,
                "GoodsImport" => goodsImportVM,
                "AddProduct" => addProductVM,
                "AddProductPrice" => addProductPriceVM,
                "SystemCatalog" => systemCatalogVM,
                "Subordinates" => subordinatesVM,
                "UserActivation" => userActivationVM,
                "PasswordChange" => passwordChangeVM,
                _ => messageVM,
            } ;

            //refresh potřebných ViewModelů
            switch (message.ViewName)
            {
                case "GoodsImport": goodsImportVM.Refresh();
                    break;
                case "UserActivation": userActivationVM.Refresh();
                    break;
                case "DatabaseExplorer": databaseExplorerVM.Refresh();
                    break;
                case "CreateEmployee": createEmployeeVM.Refresh();
                    break;
            }

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
