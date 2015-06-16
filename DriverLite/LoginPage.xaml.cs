using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using DriverLite.Common;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DriverLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public const bool EnforceLogin = false;

        private const string LastEmailKey = "LastEmail";
        private const string LastVehicleKey = "LastVehicles";

        public LoginPage()
        {
            InitializeComponent();
            LoadDefaults();

            IpHelper.GetIp();
        }

        private void LoadDefaults()
        {
            EmailTextBox.Text = ApplicationData.Current.LocalSettings.Values.ContainsKey(LastEmailKey)
                ? ApplicationData.Current.LocalSettings.Values[LastEmailKey].ToString()
                : string.Empty;

            VehicleTextBox.Text = ApplicationData.Current.LocalSettings.Values.ContainsKey(LastVehicleKey)
                ? ApplicationData.Current.LocalSettings.Values[LastVehicleKey].ToString()
                : string.Empty;

            RememberMeCheckBox.IsChecked = ApplicationData.Current.LocalSettings.Values.ContainsKey(LastEmailKey) ||
                                           ApplicationData.Current.LocalSettings.Values.ContainsKey(LastVehicleKey);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Email => \"{0}\", Password => \"{1}\"", EmailTextBox.Text, PasswordTextBox.Password);
            if (string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(VehicleTextBox.Text) || 
                string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                ErrorTextBlock.Text = "Invalid Email, Vehicle, or Password.";
                return;
            }

            if (!EnforceLogin || await LogIn())
            {
                ErrorTextBlock.Text = string.Empty;
                SaveOrClearDefaults();
                Frame.Navigate(typeof (PivotPage));
            }
            else
            {
                ErrorTextBlock.Text = "Error Logging In - Please check Email/Vehicle/Password";
            }
        }

        private async Task<bool> LogIn()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.0.11:11720/"); //new Uri("http://localhost:11720/"); 
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("api/Login", new { email = EmailTextBox.Text, vehicle = VehicleTextBox.Text, password = PasswordTextBox.Password });

                return response.IsSuccessStatusCode;
            }
        }

        private void SaveOrClearDefaults()
        {
            if (RememberMeCheckBox.IsChecked.HasValue && RememberMeCheckBox.IsChecked.Value)
            {
                ApplicationData.Current.LocalSettings.Values[LastEmailKey] = EmailTextBox.Text;
                ApplicationData.Current.LocalSettings.Values[LastVehicleKey] = VehicleTextBox.Text;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values.Remove(LastEmailKey);
                ApplicationData.Current.LocalSettings.Values.Remove(LastVehicleKey);
            }
        }
    }
}
