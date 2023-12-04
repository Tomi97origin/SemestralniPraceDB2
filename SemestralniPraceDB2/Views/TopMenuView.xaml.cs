using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SemestralniPraceDB2.Views;

/// <summary>
/// Interakční logika pro TopMenuView.xaml
/// </summary>
/// 
[ObservableRecipient]
[ObservableObject]
public partial class TopMenuView : UserControl, IRecipient<UserLogin>, IRecipient<UserLogout>, IRecipient<UserEmulation>, IRecipient<UserStopEmulation>
{
    public TopMenuView()
    {
        DataContext = new TopMenuViewModel();
        InitializeComponent();

        Messenger = WeakReferenceMessenger.Default;
        Messenger.Register<UserLogin>(this);
        Messenger.Register<UserLogout>(this);
        Messenger.Register<UserEmulation>(this);
        Messenger.Register<UserStopEmulation>(this);
    }

    public void Receive(UserLogin message)
    {
        if (message.prihlasenyUzivatel.Active)
        {
            MenuTlacitkaProPrihlasene.Visibility = Visibility.Visible;
        }

        if (message.prihlasenyUzivatel.Admin)
        {
            MenuTlacitkaProAdmina.Visibility = Visibility.Visible;
        }
    }

    public void Receive(UserLogout message)
    {
        MenuTlacitkaProPrihlasene.Visibility = Visibility.Collapsed;
        MenuTlacitkaProAdmina.Visibility = Visibility.Hidden;
    }

    public void Receive(UserEmulation message)
    {
        MenuTlacitkaProPrihlasene.Visibility = Visibility.Collapsed;
        MenuTlacitkaProAdmina.Visibility = Visibility.Hidden;

        if (message.emulovanyUzivatel.Active)
        {
            MenuTlacitkaProPrihlasene.Visibility = Visibility.Visible;
        }

        if (message.emulovanyUzivatel.Admin)
        {
            MenuTlacitkaProAdmina.Visibility = Visibility.Visible;
        }
    }

    public void Receive(UserStopEmulation message)
    {
        //Zobraz oba panely, protože po konci emulace nemůže být přihlášen nikdo jiný než Admin
        MenuTlacitkaProPrihlasene.Visibility = Visibility.Visible;
        MenuTlacitkaProAdmina.Visibility = Visibility.Visible;
    }
}

