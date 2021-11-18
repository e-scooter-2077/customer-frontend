using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace EScooter.CustomerFrontend
{
    public record ScooterViewModel(Guid Id, double Latitude, double Longitude, double BatteryLevel);

    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window
    {
        public CustomerViewModel Customer { get; private set; }

        public CustomerView(CustomerViewModel customer)
        {
            InitializeComponent();
            Customer = customer;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _titleLabel.Content = $"Customer: {Customer.Id}";            
        }

        private async Task Refresh()
        {

        }

        private async Task<IEnumerable<ScooterViewModel>> LoadScooters()
        {
            await Task.Yield();
            return new ScooterViewModel[]
            {
                new(Guid.NewGuid(), 10, 30, 10),
                new(Guid.NewGuid(), 11, 22, 90)
            };
        }
    }
}
