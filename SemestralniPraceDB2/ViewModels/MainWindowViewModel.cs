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
    partial class MainWindowViewModel : BaseViewModel, IRecipient<ViewChanged>, IRecipient<UserLogin>, IRecipient<UserLogout>, IRecipient<UserEmulation>
    {
        //Inicializace ViewModels
        public static readonly MakingOrderForWarehouseViewModel makingOrderForWarehouseVM = new();
        public static readonly MakingOrderForCustomerViewModel makingOrderForCustomerVM = new();
        public static readonly EmptyViewModel emptyVM = new();
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
        public static readonly StatisticsSalesViewModel statisticsSalesVM = new();
        public static readonly EmployeeGalleryViewModel employeeGalleryVM = new();
        public static readonly UserEmulationViewModel userEmulationVM = new();

        private static readonly BaseViewModel defaultVM = emptyVM;

        public static readonly TopMenuViewModel topMenuVM = new();

        [ObservableProperty]
        //public BaseViewModel selectedViewModel = databaseExplorerVM;//pracovní
        public BaseViewModel selectedViewModel = userLoginVM;

        public BaseViewModel? lastSelectedViewModel = null;

        [ObservableProperty]
        private TopMenuViewModel topMenuViewModel = topMenuVM;

        [ObservableProperty]
        private DatabaseResult? _databaseResult;

        [ObservableProperty]
        private string loggedAs = "Nepřihlášený uživatel";



        public MainWindowViewModel()
        {
            Messenger = WeakReferenceMessenger.Default;
            Messenger.Register<ViewChanged>(this);
            Messenger.Register<UserLogin>(this);
            Messenger.Register<UserLogout>(this);
            Messenger.Register<UserEmulation>(this);
        }

        public void Receive(ViewChanged message)
        {
            //změna viewModelu
            BaseViewModel newVM = message.ViewName switch
            {
                "MakingOrderForWarehouse" => makingOrderForWarehouseVM,
                "MakingOrderForCustomer" => makingOrderForCustomerVM,
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
                "StatisticsSales" => statisticsSalesVM,
                "EmployeeGallery" => employeeGalleryVM,
                "UserEmulation" => userEmulationVM,
                "default" => emptyVM,
                _ => emptyVM,
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
                case "Subordinates": subordinatesVM.Refresh();
                    break;
                case "MakingOrderForWarehouse": makingOrderForWarehouseVM.Refresh();
                    break;
                case "AddProduct": addProductVM.Refresh();
                    break;
                case "AddProductPrice": addProductPriceVM.Refresh();
                    break;
                case "EmployeeGallery": employeeGalleryVM.Refresh();
                    break;
                case "MakingOrderForCustomer":
                    makingOrderForCustomerVM.SetUp();
                    break;
                case "UserEmulation":
                    userEmulationVM.Refresh();
                    break;
                case "StatisticsSales":
                    statisticsSalesVM.Refresh();
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
            LoggedAs = $"Přihlášen jako {message.prihlasenyUzivatel.Username}.";
        }

        public void Receive(UserEmulation message)
        {
            LoggedAs = $"EMULUJE SE UŽIVATEL {message.emulovanyUzivatel.Username}.";
        }
       

        [RelayCommand]
        private void StopEmulation()
        {
            UzivateleService.StopEmulating();
            LoggedAs = $"Přihlášen jako {UzivateleService.Aktualni?.Username}.";
            SelectedViewModel = defaultVM;
            Messenger.Send<UserStopEmulation>(new());
            MessageBox.Show("Konec emulace jiného uživatele.");
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
