using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.Models.Entities;
using SemestralniPraceDB2.Models;
using System.Security;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SemestralniPraceDB2.ViewModels
{

    partial class PasswordChangeViewModel : BaseViewModel
    {
        //private IMessenger messenger = WeakReferenceMessenger.Default;


        //public SecureString SecurePassword { private get; set; } //pro bezpečné heslo (see: UserLoginView)
        public string Password1 { private get; set; } = string.Empty; //pro heslo napsané ve stringu
        public string Password2 { private get; set; } = string.Empty;


        [RelayCommand]
        private void ChangePassword()
        {
            if (UzivateleService.Aktualni is not null)
            {
                if (Password1 == Password2)
                {
                    UzivateleService.UpdateWithNewPassword(UzivateleService.Aktualni, Password1);
                }
                else
                {
                    MessageBox.Show("Zadaná hesla se neshodují!", "Zadejte hesla znovu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Nejprve se musíte přihlásit.", "Nejste přihlášeni", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}