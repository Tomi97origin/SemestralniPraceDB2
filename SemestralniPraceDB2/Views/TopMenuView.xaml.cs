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
public partial class TopMenuView : UserControl, IRecipient<UserLogin>, IRecipient<UserLogout>
{
    public TopMenuView()
    {
        DataContext = new TopMenuViewModel();
        InitializeComponent();

        Messenger = WeakReferenceMessenger.Default;
        Messenger.Register<UserLogin>(this);
        Messenger.Register<UserLogout>(this);
    }

    public void Receive(UserLogin message)
    {
        MenuTlacitkaProPrihlasene.Visibility = Visibility.Visible;
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
}

