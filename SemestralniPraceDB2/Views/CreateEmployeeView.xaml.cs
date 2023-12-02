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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Skryj všechny TextBlocky
            textBlockElement1.Visibility = Visibility.Collapsed;
            textBlockElement2.Visibility = Visibility.Collapsed;

            // Zobraz příslušný TextBlock podle výběru v ComboBoxu
            var i = comboBox.SelectedIndex;
            switch (i)
            {
                case 0:
                    textBlockElement1.Visibility = Visibility.Visible;
                    break;
                case 1:
                    textBlockElement2.Visibility = Visibility.Visible;
                    break;
                    // Další případy pro další elementy...
            }
        }
        private void ComboBoxTypUvazku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Skryj všechny TextBlocky
            textBlockElement1.Visibility = Visibility.Collapsed;
            textBlockElement2.Visibility = Visibility.Collapsed;

            // Zobraz příslušný TextBlock podle výběru v ComboBoxu
            var i = typUvazkuComboBox.SelectedIndex;
            var x = typUvazkuComboBox.SelectedItem;
            switch (x)
            {
                case "Plný úvazek":
                    textBlockElement1.Visibility = Visibility.Visible;
                    break;
                case "Brigádník":
                    textBlockElement2.Visibility = Visibility.Visible;
                    break;
                    // Další případy pro další elementy...
            }
        }
    }
}
