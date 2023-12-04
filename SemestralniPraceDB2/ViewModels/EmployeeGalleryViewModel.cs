using CommunityToolkit.Mvvm.ComponentModel;
using SemestralniPraceDB2.Models;
using SemestralniPraceDB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SemestralniPraceDB2.ViewModels
{
    partial class EmployeeGalleryViewModel : BaseViewModel
    {

        [ObservableProperty]
        private ObservableCollection<Zamestnanec> zamestnanciList;

        [ObservableProperty]
        private Zamestnanec? selectedZamestnanec;

        [ObservableProperty]
        private string? zamestnanecJmeno;

        [ObservableProperty]
        private BitmapImage? imageToShow;


        public EmployeeGalleryViewModel()
        {
            ZamestnanciList = new();

            Refresh();
        }

        partial void OnSelectedZamestnanecChanged(Zamestnanec? value)
        {
            ImageToShow = null;
            if (SelectedZamestnanec is not null)
            {
                ZamestnanecJmeno = $"{SelectedZamestnanec.Jmeno} {SelectedZamestnanec.Prijmeni}";

                if (SelectedZamestnanec.ObrazekZamestnance is not null) SelectedZamestnanec.ObrazekZamestnance = ObrazekZamestnanceService.Get(SelectedZamestnanec.ObrazekZamestnance);

                var img = SelectedZamestnanec.ObrazekZamestnance?.Image;
                if (img != null)
                {
                    ImageToShow = ConvertDrawingImageToBitmapImage(img);
                }
            }
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

        private void Refresh()
        {
            ZamestnanciList = new(ZamestnanecService.GetAll());
        }
    }
}
