using SemestralniPraceDB2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemestralniPraceDB2.Views
{
    /// <summary>
    /// Interakční logika pro CreateEmployeeView.xaml
    /// </summary>
    public partial class CreateEmployeeView : UserControl
    {
        public CreateEmployeeView()
        {
            DataContext = MainWindowViewModel.createEmployeeVM;
            InitializeComponent();
        }

       
        private void ComboBoxTypUvazku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Skryj všechny elementy
            labelHodinovaSazba.Visibility = Visibility.Hidden;
            textBoxHodinovaSazba.Visibility = Visibility.Hidden;
            labelHodiny.Visibility = Visibility.Hidden;
            textBoxHodiny.Visibility = Visibility.Hidden;

            labelPlat.Visibility = Visibility.Hidden;
            textBoxPlat.Visibility = Visibility.Hidden;
            labelPlatnostDo.Visibility = Visibility.Hidden;
            datePickerPlatnostDo.Visibility = Visibility.Hidden;

            // Zobraz příslušný elementy podle výběru v ComboBoxu
            switch (typUvazkuComboBox.SelectedItem)
            {
                case "Plný úvazek":
                    labelPlat.Visibility = Visibility.Visible;
                    textBoxPlat.Visibility = Visibility.Visible;
                    labelPlatnostDo.Visibility = Visibility.Visible;
                    datePickerPlatnostDo.Visibility = Visibility.Visible;
                    break;
                case "Brigádník":
                    labelHodinovaSazba.Visibility = Visibility.Visible;
                    textBoxHodinovaSazba.Visibility = Visibility.Visible;
                    labelHodiny.Visibility = Visibility.Visible;
                    textBoxHodiny.Visibility = Visibility.Visible;
                    break;
                    // Další případy pro další elementy...
            }
        }
    }
}
