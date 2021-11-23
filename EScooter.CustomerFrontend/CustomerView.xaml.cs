using EasyDesk.Tools.Options;
using EScooter.CustomerFrontend.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using static EasyDesk.Tools.Options.OptionImports;

namespace EScooter.CustomerFrontend
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml.
    /// </summary>
    public partial class CustomerView : Window, INotifyPropertyChanged
    {
        private readonly CustomerViewModel _customer;
        private readonly IQueryService _queryService;
        private readonly IRentService _rentService;
        private IEnumerable<ScooterViewModel> _scooters;
        private Option<RentViewModel> _currentRent;

        public IEnumerable<ScooterViewModel> Scooters
        {
            get => _scooters;
            private set
            {
                _scooters = value;
                PropertyChanged?.Invoke(this, new(nameof(Scooters)));
            }
        }

        public Option<RentViewModel> CurrentRent
        {
            get => _currentRent;
            private set
            {
                _currentRent = value;
                PropertyChanged?.Invoke(this, new(nameof(CurrentRent)));
            }
        }

        public CustomerView(CustomerViewModel customer, IQueryService queryService, IRentService rentService)
        {
            InitializeComponent();
            _customer = customer;
            _queryService = queryService;
            _rentService = rentService;
            DataContext = this;
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _titleLabel.Content = $"Customer: {_customer.Id}";
            await Refresh();
        }

        private async Task Refresh()
        {
            CurrentRent = await _queryService.GetRent(_customer.Id);
            Scooters = await _queryService.GetScooters();
        }

        private async void RentScooterClicked(object sender, RoutedEventArgs e)
        {
            if (_scootersListBox.SelectedItem is not ScooterViewModel scooter)
            {
                MessageBox.Show("No scooter selected");
                return;
            }
            CurrentRent = await _rentService.StartRent(_customer.Id, scooter.Id);
        }

        private async void StopRentClicked(object sender, RoutedEventArgs e)
        {
            await CurrentRent
                .IfAbsent(() => MessageBox.Show("No ongoing rent"))
                .IfPresentAsync(async rent =>
                {
                    await _rentService.StopRent(rent.Id);
                    CurrentRent = None;
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
