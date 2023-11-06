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
        private static readonly WelcomeViewModel        welcomeVM       = new();

        [ObservableProperty]
        public BaseViewModel selectedViewModel = welcomeVM;
        
        public BaseViewModel? lastSelectedViewModel = null;

        [ObservableProperty]
        private TopMenuViewModel topMenuViewModel = new TopMenuViewModel();

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
            switch(message.ViewName)
            {
                case "MakingOrderForWarehouse":
                    lastSelectedViewModel = SelectedViewModel;
                    SelectedViewModel = makingOrderForWarehouseVM;
                    break;
                case "GoBack":
                    if (lastSelectedViewModel is not null)
                    {
                        SelectedViewModel = lastSelectedViewModel;
                        lastSelectedViewModel = null;
                    }
                    break;
                case "Default": 
                    SelectedViewModel = welcomeVM;
                    break;
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
    }
}
