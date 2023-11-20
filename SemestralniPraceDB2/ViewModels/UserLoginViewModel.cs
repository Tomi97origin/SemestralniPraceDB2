using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Security;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SemestralniPraceDB2.ViewModels
{

    partial class UserLoginViewModel : BaseViewModel
    {
        private readonly IMessenger messenger = WeakReferenceMessenger.Default;

        [ObservableProperty]
        public string username = "Username";

        //public SecureString SecurePassword { private get; set; } //pro bezpečné heslo (see: UserLoginView)
        public string Password { private get; set; } = string.Empty; //pro heslo napsané ve stringu


        [RelayCommand]
        private void LoginUser()
        {
            MessageBox.Show($"Přihlašuji uživatele {Username} s heslem {Password} ");
            messenger.Send(new UserLogin($"{Username}"));
        }

        [RelayCommand]
        private void UpdateView(string parameter)
        {
            messenger.Send(new ViewChanged(parameter));
        }


    }
}