using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace SemestralniPraceDB2.ViewModels
{
    
    partial class MakingOrderForWarehouseViewModel : BaseViewModel
    {
        private IMessenger messenger = WeakReferenceMessenger.Default;

        [RelayCommand]
        private void UpdateView(string parameter)
        {
            messenger.Send(new ViewChanged(parameter));
        }
    }
}