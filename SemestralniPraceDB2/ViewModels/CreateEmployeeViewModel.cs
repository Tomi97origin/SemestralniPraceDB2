using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SemestralniPraceDB2.ViewModels
{
    partial class CreateEmployeeViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Zamestnanec zamestnanec;

        [ObservableProperty]
        public ObservableCollection<string> typyUvazku = new() { "Plný úvazek", "Brigádník" };

        [ObservableProperty]
        public string? vybranyTypUvazku;

        [ObservableProperty]
        public string? hodinovaSazba;

        [ObservableProperty]
        public string? hodiny;

        [ObservableProperty]
        public string? plat;

        [ObservableProperty]
        public DateTime platnostDo = DateTime.Today;

        [ObservableProperty]
        public ObservableCollection<Supermarket> supermarkety;

        [ObservableProperty]
        public ObservableCollection<Zamestnanec> vedouci;

        [ObservableProperty]
        public ObservableCollection<Role> role;

        [ObservableProperty]
        public string imagePath = string.Empty;

        [ObservableProperty]
        public BitmapImage imageToShow = new();

        private Bitmap? ZamestnanecImage;

        [RelayCommand]
        private void CreateEmployee()
        {
            switch (VybranyTypUvazku)
            {
                case "Plný úvazek":
                    VlozPlnyUvazekdoDB();
                    break;

                case "Brigádník":
                    VlozBrigadnikDoDB();
                    break;
                default:
                    MessageBox.Show("Vyberte typ úvazku.");
                    return;
            }
        }

        public void Refresh()
        {
            Zamestnanec zamestnanec = new();
            zamestnanec.Adresa = new();
            zamestnanec.Nastup = DateTime.Now;
            Zamestnanec = zamestnanec;
            Supermarkety = new(SupermarketService.GetAll());
            Vedouci = new(PlnyUvazekService.GetAll());
            Role = new(RoleService.GetAll());
            VybranyTypUvazku = null;
            HodinovaSazba = null;
            Hodiny = null;
            Plat = null;
            PlatnostDo = DateTime.Today.AddDays(365);
            ImagePath = string.Empty;
            ImageToShow = new();
            ZamestnanecImage = null;
        }

        private void VlozBrigadnikDoDB()
        {
            string chybnePole;

            chybnePole = ZamestnanecIsValid();
            if (chybnePole != string.Empty)
            {
                MessageBox.Show($"Chybně vyplněné pole {chybnePole}");
                return;
            }
            else if (HodinovaSazba is null)
            {
                MessageBox.Show("Zadejte Hodinovou sazbu");
                return;
            }
            else if (Hodiny is null)
            {
                MessageBox.Show("Zadejte Hodiny");
                return;
            }
            else
            {
                double hodinovaSazbaNumber;
                double hodinyNumber;
                try
                {
                    hodinovaSazbaNumber = Double.Parse(HodinovaSazba);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Nesprávná hodnota Hodinová sazba.");
                    return;
                }
                try
                {
                    hodinyNumber = Double.Parse(Hodiny);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Nesprávná hodnota Hodiny.");
                    return;
                }

                Zamestnanec.TypUvazku = 0;
                Brigadnik novyZamestnanec = new(Zamestnanec, hodinovaSazbaNumber, hodinyNumber);
                var uspech = ZamestnanecService.Create(novyZamestnanec);
                if (uspech)
                {
                    MessageBox.Show("Zaměstnanec brigádník uložen.");
                    Refresh();
                }
                else
                {
                    MessageBox.Show("Nepodarilo se uložit.");
                }
            }
        }

        private void VlozPlnyUvazekdoDB()
        {
            string chybnePole;

            chybnePole = ZamestnanecIsValid();
            if (chybnePole != string.Empty)
            {
                MessageBox.Show($"Chybně vyplněné pole {chybnePole}");
                return;
            }
            else if (Plat is null)
            {
                MessageBox.Show("Zadejte plat");
                return;
            }
            else if (PlatnostDo < DateTime.Now)
            {
                MessageBox.Show("Zadejte Platnost do");
                return;
            }
            else
            {
                double platNumber;
                try
                {
                    platNumber = Double.Parse(Plat);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Nesprávná hodnota Plat.");
                    return;
                }

                Zamestnanec.TypUvazku = 1;
                PlnyUvazek novyZamestnanec = new(Zamestnanec, platNumber, PlatnostDo);
                var uspech = ZamestnanecService.Create(novyZamestnanec);
                if (uspech)
                {
                    MessageBox.Show("Zaměstnanec na plný úvazek uložen.");
                    Refresh();
                }
                else
                {
                    MessageBox.Show("Nepodarilo se uložit.");
                }
                
            }
        }

        private string ZamestnanecIsValid()
        {
            if (string.IsNullOrEmpty(Zamestnanec.Jmeno))
            {
                return "Jméno";
            }

            if (string.IsNullOrEmpty(Zamestnanec.Prijmeni))
            {
                return "Příjmení";
            }

            if (string.IsNullOrEmpty(Zamestnanec.OsobniCislo) && IsNumeric(Zamestnanec.OsobniCislo))
            {
                return "Osobní číslo";
            }

            if (string.IsNullOrEmpty(Zamestnanec.TelCislo))
            {
                return "Telefonní číslo";
            }

            if (Zamestnanec.Nastup == DateTime.MinValue)
            {
                return "Nástup";
            }

            //if (Zamestnanec.TypUvazku == 0 && Vedouci == null)
            //{
            //    return "Vedoucí";
            //}

            if (Zamestnanec.Supermarket == null)
            {
                return "Supermarket";
            }

            if (Zamestnanec.Adresa == null)
            {
                return "Adresa";
            }

            if (string.IsNullOrEmpty(Zamestnanec.Adresa.Ulice))
            {
                return "Ulice";
            }

            if (Zamestnanec.Adresa.Cp == null)
            {
                return "ČP";
            }

            if (string.IsNullOrEmpty(Zamestnanec.Adresa.Mesto))
            {
                return "Město";
            }

            if (string.IsNullOrEmpty(Zamestnanec.Adresa.Stat))
            {
                return "Stát";
            }

            if (string.IsNullOrEmpty(Zamestnanec.Adresa.Psc))
            {
                return "PSČ";
            }

            if (Role == null)
            {
                return "Role";
            }

            if (Zamestnanec.ObrazekZamestnance?.Image == null)
            {
                return "Obrázek zaměstnance";
            }

            return string.Empty; // Třída je v pořádku
        }

        private bool IsNumeric(string? input)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(input);
        }

        [RelayCommand]
        private void ChooseImage()
        {
            OpenFileDialog openFileDialog = new();

            // Nastavení filtru na pouze PNG soubory
            openFileDialog.Filter = "PNG soubory (*.png)|*.png";

            // Titulek dialogu
            openFileDialog.Title = "Vyberte PNG soubor";

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                ZamestnanecImage = new Bitmap(ImagePath);
                ImageToShow = new(new Uri(ImagePath));
            }
            ObrazekZamestnance obr = new() { Image = ZamestnanecImage, Soubor = System.IO.Path.GetFileName(ImagePath) };
            Zamestnanec.ObrazekZamestnance = obr;
        }

        public CreateEmployeeViewModel()
        {
            Zamestnanec = new();
            Zamestnanec.Adresa = new();
            Supermarkety = new(SupermarketService.GetAll());
            Vedouci = new(PlnyUvazekService.GetAll());
            Role = new(RoleService.GetAll());
            Zamestnanec.Nastup = DateTime.Now;

        }
    }
}
