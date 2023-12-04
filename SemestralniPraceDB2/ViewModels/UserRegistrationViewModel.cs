using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
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
            if(string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Zadejte platné uživatelské jméno.");
                return;
            }
            else if (Password1 != Password2)
            {
                MessageBox.Show("Zadaná hesla se neshodují.");
                return;
            }
            else
            {
                var success = UzivateleService.Registration(Username, Password1);
                if ( !success)
                {
                    MessageBox.Show("Registrace se nezdařila.");
                    return;
                }

                MainWindowViewModel.topMenuVM.UpdateView("UserLogin");

                MessageBox.Show("Registrace proběhla úspěšně, po aktivaci účtu adminem se můžete přihlásit.","Registrace OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

    }
}