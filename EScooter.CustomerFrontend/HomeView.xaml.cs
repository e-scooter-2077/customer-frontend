using EScooter.CustomerFrontend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static EasyDesk.Tools.Collections.EnumerableUtils;

namespace EScooter.CustomerFrontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomeView : Window
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            if (_customersListBox.SelectedItem is not CustomerViewModel selectedCustomer)
            {
                MessageBox.Show("No customer is selected");
                return;
            }
            new CustomerView(selectedCustomer).Show();
        }

        private async void RefreshClicked(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        private async Task Refresh()
        {
            var customers = await LoadCustomers();
            _customersListBox.ItemsSource = customers;
            if (customers.Any())
            {
                _customersListBox.SelectedIndex = 0;
            }
        }

        private Task<IEnumerable<CustomerViewModel>> LoadCustomers()
        {
            return Task.FromResult(Items(
                new CustomerViewModel(Guid.NewGuid()),
                new CustomerViewModel(Guid.NewGuid())));
        }
    }
}
