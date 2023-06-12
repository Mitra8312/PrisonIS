using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PrisonAdministration
{
    public partial class ReceiptWindow : Window
    {
        private Prisoner prisoner = new Prisoner();
        private Employee employee = new Employee();
        private Order order = new Order();
        private Receipt receipt = new Receipt();
        private Product product = new Product();
        private Product productInOrder = new Product();
        private List<Product> productInOrderList = new List<Product>();
        private int id = 0;

        public ReceiptWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillProduct();
            FillReceipt();
            FillReceiptOrder();
            FillEmployee();
            FillUsers();
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Prisoner selectRow = (Prisoner)dgUsers.SelectedItems[0];
                prisoner = selectRow;

                lbSelectionUser.Content = prisoner.IdPrisoner.ToString();
            }
            catch { }
        }

        private void dgProductsOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Product selectRow = (Product)dgProductsOrder.SelectedItems[0];
                productInOrder = selectRow;
                if(productInOrder.CountProduct == 0)
                {
                    MessageBox.Show("");
                    productInOrder = new Product();
                }
            }
            catch { }
        }

        private void dgReceipts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Receipt selectRow = (Receipt)dgReceipts.SelectedItems[0];
                receipt = selectRow;

                lbSelectionUser.Content = receipt.PrisonerId.ToString();
                lbSelectionEmployee.Content = receipt.EmployeeId.ToString();
                lbFinalPrice.Content = receipt.FinalPrice.ToString();
            }
            catch { }
        }

        private async void btUpReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (!(receipt.IdReceipt == 0))
            {
                receipt.EmployeeId = Convert.ToInt32(lbSelectionEmployee.Content);
                receipt.PrisonerId = Convert.ToInt32(lbSelectionUser.Content);

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Receipts/" + receipt.IdReceipt, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                FillReceipt();
            }
        }

        private async void btDelReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (!(receipt.IdReceipt == 0))
            {
                List<Receipt> receipts = new List<Receipt>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Receipts");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receipts = JsonConvert.DeserializeObject<List<Receipt>>(apiResponse);
                    }
                }

                List<Order> orders = new List<Order>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Orders");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        orders = JsonConvert.DeserializeObject<List<Order>>(apiResponse);
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Order orderDel = new Order();
                    orderDel.RecieptId = receipt.IdReceipt;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    foreach (var a in orders)
                    {
                        if (orderDel.RecieptId == a.RecieptId)
                        {
                            orderDel = a;
                            var response = await httpClient.DeleteAsync("https://localhost:7087/api/Orders/" + orderDel.IdOrder);
                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Receipt receiptDel = new Receipt();
                    receiptDel.IdReceipt = Convert.ToInt32(receipt.IdReceipt);

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Receipts/" + receiptDel.IdReceipt);
                }

                FillReceiptOrder();
                FillReceipt();
            }
        }

        private async void btAddReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (!(lbSelectionEmployee.Content.ToString() == "") && !(lbSelectionUser.Content.ToString() == "") && !(lbFinalPrice.Content.ToString() == ""))
            {
                try
                {
                    receipt = new Receipt();
                    receipt.EmployeeId = Convert.ToInt32(lbSelectionEmployee.Content);
                    receipt.PrisonerId = Convert.ToInt32(lbSelectionUser.Content);

                    receipt.FinalPrice = 0;
                    foreach(var a in productInOrderList)
                    {
                        receipt.FinalPrice += a.PriceProduct;
                    }

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(receipt), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Receipts", content);

                        receipt = new Receipt();
                        FillReceipt();
                    }

                    List<Receipt> receipts = new List<Receipt>();
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        var response = await httpClient.GetAsync("https://localhost:7087/api/Receipts");

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            receipts = JsonConvert.DeserializeObject<List<Receipt>>(apiResponse);
                            foreach(var a in receipts)
                            {
                                id = a.IdReceipt;
                            }
                        }
                    }

                    Order order = new Order();
                    order.RecieptId = id;

                    using (var httpClient = new HttpClient())
                    {

                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        foreach (var i in productInOrderList)
                        {
                            order.ProductId = i.IdProduct;
                            StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                            var response = await httpClient.PostAsync("https://localhost:7087/api/Orders", content);
                        }
                    }

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        Product productUp = new Product();
                        foreach (var i in productInOrderList)
                        {
                            productUp = i;
                            productUp.CountProduct--;

                            StringContent content = new StringContent(JsonConvert.SerializeObject(productUp), Encoding.UTF8, "application/json");

                            var response = await httpClient.PutAsync("https://localhost:7087/api/Products/" + productUp.IdProduct, content);

                            string apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }

                    order = new Order();
                    FillReceiptOrder();
                }
                catch { }
            }
        }

        private void dgProducts_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Product selectRow = (Product)dgProducts.SelectedItems[0];
                product = selectRow;
            }
            catch { }
        }

        private void dgEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Employee selectRow = (Employee)dgEmployee.SelectedItems[0];
                employee = selectRow;

                lbSelectionEmployee.Content = employee.IdEmployee.ToString();
            }
            catch { }
        }

        private void btAddProd_Click(object sender, RoutedEventArgs e)
        {
            if(!(product.IdProduct == 0))
            {
                productInOrderList.Add(product);

                dgProductsOrder.ItemsSource = null;
                dgProductsOrder.ItemsSource = productInOrderList;

                receipt.FinalPrice = 0;
                foreach (var a in productInOrderList)
                {
                    receipt.FinalPrice += a.PriceProduct;
                }

                lbFinalPrice.Content = Convert.ToInt64(receipt.FinalPrice);

                product = new Product();
            }
        }

        private void btDellProd_Click(object sender, RoutedEventArgs e)
        {
            if (!(productInOrder.IdProduct == 0))
            {
                foreach (var i in productInOrderList)
                {
                    if (i.IdProduct == productInOrder.IdProduct)
                    {
                        productInOrderList.Remove(i);
                        break;
                    }
                }

                dgProductsOrder.ItemsSource = null;
                dgProductsOrder.ItemsSource = productInOrderList;

                receipt.FinalPrice = 0;
                foreach (var a in productInOrderList)
                {
                    receipt.FinalPrice += a.PriceProduct;
                }

                lbFinalPrice.Content = Convert.ToInt64(receipt.FinalPrice);

                productInOrder = new Product();
            }
        }

        private async void FillProduct()
        {
            List<Product> products = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Products");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    dgProducts.ItemsSource = products;
                }
            }
        }

        private async void FillReceipt()
        {
            List<Receipt> receipts = new List<Receipt>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Receipts");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receipts = JsonConvert.DeserializeObject<List<Receipt>>(apiResponse);
                    dgReceipts.ItemsSource = receipts;
                }
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
                    dgEmployee.ItemsSource = employees;
                }
            }
        }

        private async void FillReceiptOrder()
        {
            List<Order> orders = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Orders");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<List<Order>>(apiResponse);
                    dgReceiptsOrder.ItemsSource = orders;
                }
            }
        }

        private async void FillUsers()
        {
            List<Prisoner> prisoners = new List<Prisoner>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Prisoners");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    prisoners = JsonConvert.DeserializeObject<List<Prisoner>>(apiResponse);
                    dgUsers.ItemsSource = prisoners;
                }
            }
        }
    }
}
