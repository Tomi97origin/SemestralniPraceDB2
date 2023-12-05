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
    /// Interakční logika pro ObjednavkaView.xaml
    /// </summary>
    public partial class MakingOrderForCustomerView : UserControl
    {
        public MakingOrderForCustomerView()
        {
            DataContext = MainWindowViewModel.makingOrderForCustomerVM;
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (labelVraceno is null) {  return; }
            labelVraceno.Visibility = Visibility.Hidden;
            textBoxVraceno.Visibility = Visibility.Hidden;
            labelCisloKarty.Visibility = Visibility.Hidden;
            textBoxCisloKarty.Visibility = Visibility.Hidden;
            labelDebit.Visibility = Visibility.Hidden;
            checkBoxDebit.Visibility = Visibility.Hidden;
            comboBoxVydavatele.Visibility = Visibility.Hidden;
            switch (comboBoxTypPlatby.SelectedItem)
            {
                case "Hotovost":
                    labelVraceno.Visibility = Visibility.Visible;
                    textBoxVraceno.Visibility = Visibility.Visible;
                    break;
                case "Karta":
                    labelCisloKarty.Visibility = Visibility.Visible;
                    textBoxCisloKarty.Visibility = Visibility.Visible;
                    labelDebit.Visibility = Visibility.Visible;
                    checkBoxDebit.Visibility = Visibility.Visible;
                    comboBoxVydavatele.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
