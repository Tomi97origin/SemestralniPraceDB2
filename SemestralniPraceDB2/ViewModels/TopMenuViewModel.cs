using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.ViewModels;

partial class TopMenuViewModel : BaseViewModel
{
    private IMessenger messenger = WeakReferenceMessenger.Default;

    [RelayCommand]
    private void UpdateView(string parameter)
    {
        messenger.Send(new ViewChanged(parameter));
    }
}
