using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;

namespace PrisonAdministration
{
    public partial class HomeWindow : Window
    {
        List<Employee> employees;

        public HomeWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryTrash.SaveUserCredentials();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);

                var response = await client.GetAsync("https://localhost:7087/api/Employees");

                string apiResponse = await response.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);

                foreach(var a in employees)
                {
                    if (a.LoginEmployee.Equals(App.login)) App.authUser = a;
                }

                tbUserLogin.Text = $"Логин авторизованного пользователя: {App.authUser.LoginEmployee}";
            }
        }

        private void btProsucts_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow productsWindow = new ProductsWindow();
            productsWindow.Show();
        }

        private void btPrisoners_Click(object sender, RoutedEventArgs e)
        {
            PrisonerWindow prisonerWindow = new PrisonerWindow();
            prisonerWindow.Show();
        }

        private void btEmployees_Click(object sender, RoutedEventArgs e)
        {
            EmployeesWindow employeesWindow = new EmployeesWindow();
            employeesWindow.Show();
        }

        private void Articles_Click(object sender, RoutedEventArgs e)
        {
            ArticlesWindow articles = new ArticlesWindow();
            articles.Show();
        }

        private void Reciepts_Click(object sender, RoutedEventArgs e)
        {
            ReceiptWindow receiptWindow = new ReceiptWindow();
            receiptWindow.Show();
        }
    }
}
