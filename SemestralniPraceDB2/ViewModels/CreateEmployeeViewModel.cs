using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SemestralniPraceDB2.ViewModels
{
    partial class CreateEmployeeViewModel : BaseViewModel
    {
        //private IMessenger messenger = WeakReferenceMessenger.Default;

        [ObservableProperty]
        public Zamestnanec zamestnanec = new();

        [ObservableProperty]
        public Adresa adresa = new();

        [ObservableProperty]
        public ObservableCollection<string> typyUvazku = new() { "Brigádník", "Plný úvazek" };

        [ObservableProperty]
        public ObservableCollection<string> supermarkety = new() { "S1", "S2" };

        [ObservableProperty]
        public ObservableCollection<string> vedouci = new() { "Jozo", "Karlos" };
        
        [ObservableProperty]
        public ObservableCollection<string> role = new() { "Skladnik", "Vedouci prodejny", "Pracovník úklidu" };

        [ObservableProperty]
        public string imagePath = string.Empty;

        [ObservableProperty]
        public BitmapImage zamestnanecImage = new();

        [RelayCommand]
        private void CreateEmployee()
        {
            MessageBox.Show($"Vytvářím nového zaměstnance {Zamestnanec}, s adresou {Adresa}");
        } 
        
        [RelayCommand]
        private void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                zamestnanecImage = new BitmapImage(new Uri(imagePath));
            }
        }
    }
}
