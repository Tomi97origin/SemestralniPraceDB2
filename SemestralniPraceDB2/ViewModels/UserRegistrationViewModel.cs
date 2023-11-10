using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Security;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SemestralniPraceDB2.ViewModels
{

    partial class UserRegistrationViewModel : BaseViewModel
    {
        //private IMessenger messenger = WeakReferenceMessenger.Default;

        [ObservableProperty]
        public string username = "Username";

        //public SecureString SecurePassword { private get; set; } //pro bezpečné heslo (see: UserLoginView)
        public string Password1 { private get; set; } = string.Empty; //pro heslo napsané ve stringu
        public string Password2 { private get; set; } = string.Empty;


        [RelayCommand]
        private void RegisterUser()
        {
            MessageBox.Show($"Registruji uživatele {Username} s heslem {Password1} a {Password2}");
        }

    }
}