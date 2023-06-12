using Newtonsoft.Json;
using PrisonAdministration.Models;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace PrisonAdministration
{
    public partial class MainWindow : Window
    {
        HttpClient httpClient;

        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }

        private async void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                UserRegister userRegister = new UserRegister();

                userRegister.UserLogin = tbLoginUser.Text;
                userRegister.UserPassword = tbPasswordUser.Password;

                StringContent content = new StringContent(JsonConvert.SerializeObject(userRegister), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://localhost:7087/api/Auth/login-employee", content);

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    App.token = apiResponse;
                    App.login = tbLoginUser.Text;
                    App.password = tbPasswordUser.Password;
                    HomeWindow homeWindow = new HomeWindow();
                    homeWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
                
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserRegister userRegister = new UserRegister();
                RegistryTrash.GetUserCredentials();
                userRegister.UserPassword = App.password;
                userRegister.UserLogin = App.login;

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(userRegister), Encoding.UTF8, "application/json");
                var response1 = await httpClient.PostAsync("https://localhost:7087/api/Auth/login-employee", content1);
                string apiResponse = await response1.Content.ReadAsStringAsync();
                App.token = apiResponse;
                if(userRegister.UserLogin != "")
                {
                    HomeWindow homeWindow = new HomeWindow();
                    homeWindow.Show();
                    this.Close();
                }
                
            }
            catch { }
        }
    }
}
