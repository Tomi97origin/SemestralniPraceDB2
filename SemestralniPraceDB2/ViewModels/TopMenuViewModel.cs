using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPraceDB2.ViewModels;

partial class TopMenuViewModel : BaseViewModel
{
    private IMessenger messenger = WeakReferenceMessenger.Default;

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

    

}
