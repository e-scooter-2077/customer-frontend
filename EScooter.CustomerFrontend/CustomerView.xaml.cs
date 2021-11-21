using EScooter.CustomerFrontend.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EScooter.CustomerFrontend
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml.
    /// </summary>
    public partial class CustomerView : Window
    {
        private readonly CustomerViewModel _customer;
        private readonly IQueryService _queryService;
        private readonly IRentService _rentService;

        public CustomerView(CustomerViewModel customer, IQueryService queryService, IRentService rentService)
        {
            InitializeComponent();
            _customer = customer;
            _queryService = queryService;
            _rentService = rentService;
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _titleLabel.Content = $"Customer: {_customer.Id}";
            await Refresh();
        }

        private async Task Refresh()
        {
            var scooters = await _queryService.GetScooters();
            _scootersListBox.ItemsSource = scooters;
            if (scooters.Any())
            {
                _scootersListBox.SelectedIndex = 0;
            }
        }
    }
}
