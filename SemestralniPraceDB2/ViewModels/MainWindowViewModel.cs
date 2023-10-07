using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SemestralniPraceDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPraceDB2.ViewModels
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {

        [ObservableProperty]
        private DatabaseResult? _databaseResult;

        [RelayCommand]
        private void ConnectAndGet()
        {
            DatabaseResult = new DatabaseResult
            {
                Text = DatabaseConnector.GetFromDatabase()
            };
        }
    }
}
