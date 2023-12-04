using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.Models;
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
            if(string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Zadejte platné uživatelské jméno.");
            }
            else if(string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Zadejte platné heslo.");
            }
            else
            {
                var prihlasenyUzivatel = UzivateleService.Login(Username, Password);

                if(prihlasenyUzivatel is null)
                {
                    MessageBox.Show("Přihlášení se nezdařilo, zkuste to znovu.");
                    return;
                }

                MessageBox.Show($"Přihlášení úspěšné.");
                MainWindowViewModel.topMenuVM.UpdateView("default");
                messenger.Send(new UserLogin(prihlasenyUzivatel));
            }
        }

        [RelayCommand]
        private void SwitchToRegistration()
        {
            MainWindowViewModel.topMenuVM.UpdateView("UserRegistration");
        }


    }
}