using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PrisonAdministration
{
    public partial class EmployeesWindow : Window
    {
        Employee employee = new Employee();
        Post post = new Post();

        public EmployeesWindow()
        {
            InitializeComponent();
        }

        private void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Employee selectRow = (Employee)dgEmployees.SelectedItems[0];
                employee = selectRow;

                tbNameEmployee.Text = employee.FirstNameEmployee;
                tbMiddleNameEmployee.Text = employee.MiddleNameEmployee;
                tbSecondNameEmployee.Text = employee.SecondNameEmployee;
                tbPasswordEmployee.Text = employee.PasswordEmployee;
                tbLoginEmployeer.Text = employee.LoginEmployee;
                post.IdPost = employee.PostId;
            }
            catch { }
        }

        private async void btAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbLoginEmployeer.Text == "") && !(tbNameEmployee.Text == "") && !(tbSecondNameEmployee.Text == "") && !(tbPasswordEmployee.Text == "") && !(tbMiddleNameEmployee.Text == "") && !(post.IdPost == 0))
            {
                try
                {
                    Employee onzhect = new Employee();
                    onzhect.PostId = post.IdPost;

                    onzhect.FirstNameEmployee = tbNameEmployee.Text;
                    onzhect.SecondNameEmployee = tbSecondNameEmployee.Text;
                    onzhect.MiddleNameEmployee = tbMiddleNameEmployee.Text;
                    onzhect.LoginEmployee = tbLoginEmployeer.Text;

                    Random random = new Random();
                    onzhect.SaltEmployee = RandomString();
                    onzhect.PasswordEmployee = tbPasswordEmployee.Text;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Auth/register-employee", content);

                        tbNameEmployee.Text = null;
                        tbSecondNameEmployee.Text = null;
                        tbMiddleNameEmployee.Text = null;
                        tbLoginEmployeer.Text = null;
                        tbPasswordEmployee.Text = null;
                        FillEmployee();
                    }
                }
                catch { }
            }
        }

        private async void btDelEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!(employee.IdEmployee == 0))
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Employees/" + employee.IdEmployee);
                }

                tbNameEmployee.Text = null;
                tbSecondNameEmployee.Text = null;
                tbMiddleNameEmployee.Text = null;
                tbLoginEmployeer.Text = null;
                tbPasswordEmployee.Text = null;
                FillEmployee();
            }
        }

        private async void btUpEmployee_Click(object sender, RoutedEventArgs e)
        {
            if(!(tbLoginEmployeer.Text == "") && !(tbNameEmployee.Text == "") && !(tbSecondNameEmployee.Text == "") && !(tbPasswordEmployee.Text == "") && !(tbMiddleNameEmployee.Text == "") && !(post.IdPost == 0))
            {
                Employee onzhectUp = new Employee();
                onzhectUp.PostId = post.IdPost;

                onzhectUp.FirstNameEmployee = tbNameEmployee.Text;
                onzhectUp.SecondNameEmployee = tbSecondNameEmployee.Text;
                onzhectUp.MiddleNameEmployee = tbMiddleNameEmployee.Text;
                onzhectUp.LoginEmployee = tbLoginEmployeer.Text;

                onzhectUp.SaltEmployee = RandomString();
                onzhectUp.PasswordEmployee = HashString(tbPasswordEmployee.Text + onzhectUp.SaltEmployee);

                onzhectUp.IdEmployee = employee.IdEmployee;

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Employees/" + onzhectUp.IdEmployee, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                tbNameEmployee.Text = null;
                tbSecondNameEmployee.Text = null;
                tbMiddleNameEmployee.Text = null;
                tbLoginEmployeer.Text = null;
                tbPasswordEmployee.Text = null;
                FillEmployee();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillEmployee();
            FillPost();
        }

        private void dgPosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Post selectRow = (Post)dgPosts.SelectedItems[0];
                post = selectRow;

                tbNamePost.Text = post.NamePost;
            }
            catch { }
        }

        private async void btAddPost_Click(object sender, RoutedEventArgs e)
        {
            if(!(tbNamePost.Text == ""))
            {
                try
                {
                    post = new Post();
                    post.NamePost = tbNamePost.Text;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Posts", content);
                    }

                    post = new Post();
                    FillPost();
                }
                catch { }
            }
        }

        private async void btDelPost_Click(object sender, RoutedEventArgs e)
        {
            if (!(post.IdPost == 0))
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Posts/" + post.IdPost);
                }

                tbNamePost.Text = null;
                FillPost();
            }
        }

        private async void btUpPost_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNamePost.Text == ""))
            {
                Post postUp = new Post();
                postUp.NamePost = tbNamePost.Text;
                postUp.IdPost = post.IdPost;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(postUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Posts/" + postUp.IdPost, content);
                }

                tbNamePost.Text = null;
                FillPost();
            }
        }

        private async void FillEmployee()
        {
            List<Employee> employees = new List<Employee>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Employees");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                    dgEmployees.ItemsSource = employees;
                }
            }
        }

        private async void FillPost()
        {
            List<Post> posts = new List<Post>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Posts");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<Post>>(apiResponse);
                    dgPosts.ItemsSource = posts;
                }
            }
        }

        private static string HashString(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string RandomString()
        {
            Random random = new Random();
            int length = random.Next(30);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012!#$^%*().,/@3456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
