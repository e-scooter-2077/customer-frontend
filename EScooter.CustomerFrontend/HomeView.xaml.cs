using EScooter.CustomerFrontend.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EScooter.CustomerFrontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class HomeView : Window
    {
        private readonly IQueryService _queryService;
        private readonly IRentService _rentService;

        public HomeView(IQueryService queryService, IRentService rentService)
        {
            InitializeComponent();
            _queryService = queryService;
            _rentService = rentService;
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
            new CustomerView(selectedCustomer, _queryService, _rentService).Show();
        }

        private async void RefreshClicked(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        private async Task Refresh()
        {
            var customers = await _queryService.GetCustomers();
            _customersListBox.ItemsSource = customers;
            if (customers.Any())
            {
                _customersListBox.SelectedIndex = 0;
            }
        }
    }
}
