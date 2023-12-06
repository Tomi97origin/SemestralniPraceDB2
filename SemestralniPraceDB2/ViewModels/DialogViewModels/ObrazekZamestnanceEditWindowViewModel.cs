using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SemestralniPraceDB2.ViewModels.DialogViewModels
{
    partial class ObrazekZamestnanceEditWindowViewModel : BaseViewModel
    {

        [ObservableProperty]
        public ObrazekZamestnance? obrazekZamestnance;

        [ObservableProperty]
        public BitmapImage? imageToShow;

        [ObservableProperty]
        public string imagePathToShow = string.Empty;

        //private Bitmap? ZamestnanecImage;
        public ObrazekZamestnanceEditWindowViewModel()
        {
            ObrazekZamestnance = new();
        }

        public ObrazekZamestnanceEditWindowViewModel(ObrazekZamestnance? obrazekZamestnance)
        {

            ObrazekZamestnance = obrazekZamestnance;
            ImageToShow = null;

            if (ObrazekZamestnance is not null)
            {
                var img = ObrazekZamestnance.Image;
                if (img != null)
                {
                    ImageToShow = ConvertDrawingImageToBitmapImage(img);
                    ImagePathToShow = ObrazekZamestnance.Soubor ?? String.Empty;
                }
            }

        }

        [RelayCommand]
        private void ChooseImage()
        {
            if (ObrazekZamestnance is not null)
            {
                OpenFileDialog openFileDialog = new();

                // Nastavení filtru na pouze PNG soubory
                openFileDialog.Filter = "PNG soubory (*.png)|*.png";

                // Titulek dialogu
                openFileDialog.Title = "Vyberte PNG soubor";

                if (openFileDialog.ShowDialog() == true)
                {
                    ObrazekZamestnance.Soubor = System.IO.Path.GetFileName(openFileDialog.FileName);
                    ObrazekZamestnance.Image = new Bitmap(openFileDialog.FileName);
                    ImageToShow = new(new Uri(openFileDialog.FileName));
                    ImagePathToShow = ObrazekZamestnance.Soubor ?? String.Empty;
                }
            }
        }

        [RelayCommand]
        private void Ok(Window window)
        {
            window.Close();
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            ObrazekZamestnance = null;
            window.Close();
        }

        private BitmapImage ConvertDrawingImageToBitmapImage(System.Drawing.Image drawingImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Uložení System.Drawing.Image do MemoryStream
                drawingImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                // Vytvoření nového objektu BitmapImage
                BitmapImage bitmapImage = new BitmapImage();

                // Načtení BitmapImage ze MemoryStream
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

    }
}