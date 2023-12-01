using SemestralniPraceDB2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    /// Interakční logika pro CustomerPurchaseHistoryView.xaml
    /// </summary>
    public partial class CustomerPurchaseHistoryView : UserControl
    {
        public CustomerPurchaseHistoryView()
        {
            DataContext = MainWindowViewModel.customerPurchaseHistoryVM;
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
            {
                // Skryj sloupce s atributem [Browsable(false)]
                e.Cancel = true;
            }
            else
            {
                // Změň název sloupce pomocí atributu DisplayName
                var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

                if (!string.IsNullOrEmpty(displayName))
                {
                    e.Column.Header = displayName;
                }
            }
        }

        //Github (https://stackoverflow.com/questions/13579034/how-do-you-rename-datagrid-columns-when-autogeneratecolumns-true)
        public static string GetPropertyDisplayName(object descriptor)
        {
            if (descriptor is PropertyDescriptor pd)
            {
                // Check for DisplayName attribute and set the column header accordingly

                if (pd.Attributes[typeof(DisplayNameAttribute)] is DisplayNameAttribute displayName && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        if (attributes[i] is DisplayNameAttribute displayName && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return string.Empty;
        }

    }
}
