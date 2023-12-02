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
    /// Interakční logika pro PasswordChangeView.xaml
    /// </summary>
    public partial class PasswordChangeView : UserControl
    {
        public PasswordChangeView()
        {
            DataContext = MainWindowViewModel.passwordChangeVM;
            InitializeComponent();
        }

        // Pro bezpečné zacházení s heslem pomocí SecureString
        //private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (this.DataContext != null)
        //    { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        //}


        //pro normální zacházení s heslem ve stringu
        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password1 = ((PasswordBox)sender).Password; }
        }
        private void PasswordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password2 = ((PasswordBox)sender).Password; }
        }
    }
}
