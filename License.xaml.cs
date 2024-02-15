using Cryptlex;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace CryptlexLicensingApp
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class License : Page
    {
        private const string LicenseAccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzY29wZSI6WyJsaWNlbnNlOnJlYWQiLCJsaWNlbnNlOndyaXRlIl0sInN1YiI6IjE3ODYxMmY2LWRlZTItNGIzZC05ZDVhLTQyY2E2NTVkOGQwYSIsImVtYWlsIjoibmlja21vaGFsZUBnbWFpbC5jb20iLCJqdGkiOiJhZjIyMGU2Yy01MDg1LTQ5NWYtYTk3My1mMjg4OTliNDExNWIiLCJpYXQiOjE3MDc5ODk2NjIsInRva2VuX3VzYWdlIjoicGVyc29uYWxfYWNjZXNzX3Rva2VuIiwidGVuYW50aWQiOiIxZDQ0ZDM0NS04OWJjLTRjMDAtYTI1Ni05NmJiZDc1N2JiOTgiLCJleHAiOjE3MDkxNTc2MDAsImF1ZCI6Imh0dHBzOi8vYXBpLmNyeXB0bGV4LmNvbSJ9.Wq_rKEEETe8sWiHCvEqmGC6Y5gRkGZ4nydXxYPtbCNv2zT949NP_4mUsdQwz2VuBFJ1D93P8kRUhcbGAn_6aHYQNwTqwqdy2jbbMxxo3YEQuaUjwPLGUvV2-0gNSplUIpBkLR47oSkOn4VcWhETBQllNYb-OA5oMB3EAqBuNmoItzEbdZrLNy9wQjFrnoLpRPRKYWbTGolOa_LGY6pL7eb0xKhSefb1OSFb2uQvuv8FcGlzmT9CaTDyEwOECNOa7Yxc3pcf4uMmcRjvmkkuYUjqTrPpLaWjSCEcCK4_YA8_22HDnArcDh4pGygN_40_3LLyCyiQexF7K1tHQAo60TQ"; 


        public License()
        {
            InitializeComponent();
            LoadLicensesAsync();
        }

        private async Task LoadLicensesAsync()
        {
            try
            {

                // Call your API endpoint to get the list of Licenses
                string endpointUrl = "https://api.cryptlex.com/v3/licenses";

                HttpClient client = new HttpClient();

                // Add access token to request headers
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", LicenseAccessToken);
                HttpResponseMessage response = await client.GetAsync(endpointUrl);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the JSON response to a list of products
                    string json = await response.Content.ReadAsStringAsync();
                    List<LicenseModel> LicenseModel = JsonConvert.DeserializeObject<List<LicenseModel>>(json);

                    // Bind the Element 
                    Keys.ItemsSource = LicenseModel;
                    Keys.DisplayMemberPath = "Key";
                }
                else
                {
                    // Handle unsuccessful response
                    MessageBox.Show("Failed to fetch License Key. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the selected license from the ComboBox
                LicenseModel selectedLicense = Keys.SelectedItem as LicenseModel;
                if (selectedLicense == null)
                {
                    MessageBox.Show("Please select a license to update.");
                    return;
                }

                // Construct the updated license data based on the values on the screen
                selectedLicense.Validity = Validity.Text;
                selectedLicense.TotalActivations = TotalActivations.Text;
                selectedLicense.Organization.name = Organization.Text; 

                // Serialize the updated license data to JSON
                string updatedLicenseJson = JsonConvert.SerializeObject(selectedLicense);

                // Construct the endpoint URL with the license ID
                string endpointUrl = $"https://api.cryptlex.com/v3/licenses/{selectedLicense.ID}";

                // Create a HttpClient instance
                HttpClient client = new HttpClient();

                // Add access token to request headers
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LicenseAccessToken);

                // Create a StringContent with the updated license data
                var content = new StringContent(updatedLicenseJson, Encoding.UTF8, "application/json");

                // Send an HTTP PATCH request to update the license
                HttpResponseMessage response = await client.PatchAsync(endpointUrl, content);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("License updated successfully.");
                }
                else
                {
                    // Handle unsuccessful response
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to update license: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reset the ComboBox selection
                Keys.SelectedItem = null;

                // Clear the text of TextBoxes
                productId.Text = string.Empty;
                Validity.Text = string.Empty;
                TotalActivations.Text = string.Empty;
                LeaseDuration.Text = string.Empty;
                Organization.Text = string.Empty;
                createdAt.Text = string.Empty;
                MessageBox.Show("Fields reset successfully.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

    
    }
}
