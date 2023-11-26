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
    }
}
