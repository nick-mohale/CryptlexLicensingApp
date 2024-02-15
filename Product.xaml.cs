using Cryptlex;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Page
    {
        private const string AccessToken  = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzY29wZSI6WyJwcm9kdWN0OnJlYWQiLCJwcm9kdWN0OndyaXRlIl0sInN1YiI6IjE3ODYxMmY2LWRlZTItNGIzZC05ZDVhLTQyY2E2NTVkOGQwYSIsImVtYWlsIjoibmlja21vaGFsZUBnbWFpbC5jb20iLCJqdGkiOiJkM2M1MmNhYy04MTFhLTRiNDktYmQwOC1lYzAzZTI4NDVjYzkiLCJpYXQiOjE3MDc5ODA5MTAsInRva2VuX3VzYWdlIjoicGVyc29uYWxfYWNjZXNzX3Rva2VuIiwidGVuYW50aWQiOiIxZDQ0ZDM0NS04OWJjLTRjMDAtYTI1Ni05NmJiZDc1N2JiOTgiLCJleHAiOjE3MDkxNTc2MDAsImF1ZCI6Imh0dHBzOi8vYXBpLmNyeXB0bGV4LmNvbSJ9.mh598oIf4_KR49Sur8KGnIG3MaZigxnW3j6d6i01LEN2u2LvDdaFV7tQTT4FXur8lSmiLHawUvn4-WKjuGgJCy2-8ROCRVzgA8WFMAFLriXHAhg1zzS-gi1bav05NbYUglV4UgQx_PJSRED0n5WGvei4EFuTqiL7zz22R5x2z3NfIXkgTuQP6uTULH_auoCu9JgsePv9RYa79f_AuDrtZ1mzWPtows88wSLS4Bo8mWQiedJEjNzCLr8T91NYYov2mM0zT4pWZIx9Lpvo4bpRCsP9ATAZaALMDEqtYjUnvJ2K6T6ONIInO-Jmqab6iz64omJA3xtxKrxnZilW88maKw"; 


        public Product()
        {
            InitializeComponent();
            LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
               
                // Call your API endpoint to get the list of products
                string endpointUrl = "https://api.cryptlex.com/v3/products";

                HttpClient client = new HttpClient();

                 // Add access token to request headers
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
                HttpResponseMessage response = await client.GetAsync(endpointUrl);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the JSON response to a list of products
                    string json = await response.Content.ReadAsStringAsync();
                    List<ProductModel> products = JsonConvert.DeserializeObject<List<ProductModel>>(json);

                    // Bind the Element 
                    Products.ItemsSource = products;
                    Products.DisplayMemberPath = "Name";
    }
                else
                {
                    // Handle unsuccessful response
                    MessageBox.Show("Failed to fetch products. Please try again later.");
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
                // Retrieve the selected product from the ComboBox
                ProductModel selectedProduct = Products.SelectedItem as ProductModel;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Please select a product to update.");
                    return;
                }

                // Construct the updated product data based on the values on the screen
                selectedProduct.TotalLicenses = TotalLicensesTextBox.Text;
                selectedProduct.TotalTrialActivations = TotalTrialActivations.Text;
              

                // Serialize the updated product data to JSON
                string updatedProductJson = JsonConvert.SerializeObject(selectedProduct);

                // Construct the endpoint URL with the product ID
                string endpointUrl = $"https://api.cryptlex.com/v3/products/{selectedProduct.ID}";

                // Create a HttpClient instance
                HttpClient client = new HttpClient();

                // Add access token to request headers
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                // Create a StringContent with the updated product data
                var content = new StringContent(updatedProductJson, Encoding.UTF8, "application/json");

                // Send an HTTP PATCH request to update the product
                HttpResponseMessage response = await client.PatchAsync(endpointUrl, content); 

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product updated successfully.");
                }
                else
                {
                    // Handle unsuccessful response
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to update product: {errorMessage}");
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
                // Clear the text in each TextBox
                TotalLicensesTextBox.Text = "";
                TotalTrialActivations.Text = "";
                Products.SelectedItem = null;

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
