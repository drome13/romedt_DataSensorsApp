using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace romedt_DataSensorsApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        CancellationTokenSource cts;
  
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, \nLongitude: {location.Longitude}, \nAltitude: {location.Altitude}");
                    labelResult.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine("Location services are not supported on this device.");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine("Location services are not enabled. Please enable them in your device settings.");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("The app doesn't have permission to access location. Please grant permission.");

            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("An error occurred while getting location: " + ex.Message);

            }
        }

            public async Task SendEmail(string subject, string body, List<string> recipients)
            {
                try
                {
                    var message = new EmailMessage
                    {
                        Subject = subject,
                        Body = body,
                        To = recipients,
                        //Cc = ccRecipients,
                        //Bcc = bccRecipients
                    };
                    await Email.ComposeAsync(message);
                }
                catch (FeatureNotSupportedException fbsEx)
                {
                // Email is not supported on this device
                Console.WriteLine("Email is not supported on this device");
                }
                catch (Exception ex)
                {
                    // Some other exception occurred
                    Console.Write(ex.Message);
                }
            }

        private void btnLocate_Clicked(object sender, EventArgs e)
        {
            GetCurrentLocation();
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            string subject = $"{Name_Entry.Text}'s Requested GPS Location";
            string body = labelResult.Text;
            List<string> recipient = new List<string>() { Email_Entry.Text };

            SendEmail(subject, body, recipient);

            labelSubmit.Text = "Email sent!";
        }
    }

}
