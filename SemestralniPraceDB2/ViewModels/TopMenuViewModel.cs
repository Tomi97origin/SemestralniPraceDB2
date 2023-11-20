using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Xps;

namespace SemestralniPraceDB2.ViewModels;

[ObservableRecipient]
partial class TopMenuViewModel : BaseViewModel, IRecipient<UserLogin>, IRecipient<UserLogout>
{
    private readonly IMessenger messenger = WeakReferenceMessenger.Default;

    private bool isSomebodyLogged = false;

    [ObservableProperty]
    public string loginLogoutButtonText = "Přihlásit";

    public TopMenuViewModel()
    {

        Messenger = WeakReferenceMessenger.Default;
        Messenger.Register<UserLogin>(this);
        Messenger.Register<UserLogout>(this);
    }

    [RelayCommand]
    private void UpdateView(string parameter)
    {
        messenger.Send(new ViewChanged(parameter));
    }

    [RelayCommand]
    private void MenuItem(string parameter)
    {
        MessageBox.Show("Klikli jste na položku menu.");
    }

    [RelayCommand]
    private void ShowContextMenu(string parameter)
    {

        MessageBox.Show("Klikli jste na položku menu.");
    }

    [RelayCommand]
    private void LoginLogout()
    {
        if (!isSomebodyLogged)
        {
            //proveď přihlášení
            messenger.Send(new ViewChanged("UserLogin"));

        }
        else if (isSomebodyLogged)
        {
            //proveď odhlášení
            //todo call logoutCurrentUser();

            //uměle vyvolána zpráva o odhlášení:
            messenger.Send(new UserLogout()); //todo smazat, toto musí volat funkce obsluhující odhlašování
        }


    }

    public void Receive(UserLogin message)
    {
        if (!isSomebodyLogged)
        {
            LoginLogoutButtonText = "Odhlásit";
            isSomebodyLogged = true;
        }
        else throw new ApplicationException("Unexpected Error: There cannot be logged 2 users at the time.");
    }

    public void Receive(UserLogout message)
    {
        if (isSomebodyLogged)
        {
            isSomebodyLogged = false;
            LoginLogoutButtonText = "Přihlásit";
        }
        else throw new ApplicationException("Unexpected Error: Can not logout when no user is logged in.");
    }
}
