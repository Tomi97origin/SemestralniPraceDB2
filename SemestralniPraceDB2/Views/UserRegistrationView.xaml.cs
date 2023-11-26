using SemestralniPraceDB2.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SemestralniPraceDB2.Views
{
    /// <summary>
    /// Interakční logika pro UserRegistrationView.xaml
    /// </summary>
    public partial class UserRegistrationView : UserControl
    {
        public UserRegistrationView()
        {
            DataContext = MainWindowViewModel.userRegistrationVM;
            InitializeComponent();
        }

        // Pro bezpečné zacházení s heslem pomocí SecureString
        //private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
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
