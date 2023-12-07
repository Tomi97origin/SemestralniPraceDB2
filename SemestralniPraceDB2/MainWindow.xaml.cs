using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SemestralniPraceDB2.ViewModels;
using System;
using System.Windows;

namespace SemestralniPraceDB2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[ObservableRecipient]
[ObservableObject]
public partial class MainWindow : Window, IRecipient<UserEmulation>, IRecipient<UserStopEmulation>, IRecipient<UserLogout>
{
    public MainWindow()
    {
        try
        {
            DataContext = new MainWindowViewModel();
        }
        catch (Exception)
        {
            MessageBox.Show("Aplikaci se nepodařilo spustit - chybí data v databázi. Ověřte připojení k DB.");
            Environment.Exit(-1);
        }

        Messenger = WeakReferenceMessenger.Default;
        Messenger.Register<UserEmulation>(this);
        Messenger.Register<UserStopEmulation>(this);
        Messenger.Register<UserLogout>(this);

        InitializeComponent();
    }



    public void Receive(UserEmulation message)
    {
        ButtonStopEmulation.Visibility = Visibility.Visible;
    }

    public void Receive(UserStopEmulation message)
    {
        ButtonStopEmulation.Visibility = Visibility.Collapsed;
    }

    public void Receive(UserLogout message)
    {
        ButtonStopEmulation.Visibility = Visibility.Collapsed;
    }
}
