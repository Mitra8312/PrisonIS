using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace PrisonAdministration
{
    public partial class ProductsWindow : Window
    {
        TypeProduct typeProduct = new TypeProduct();
        Product product = new Product();

        public ProductsWindow()
        {
            InitializeComponent();
        }

        private async void btAddProd_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbDescProd.Text == "") && !(tbCountProd.Text == "") && !(tbNameProd.Text == "") && !(tbTypeProd.Text == "") && !(tbPriceProd.Text == ""))
            {
                try
                {
                    product = new Product();
                    product.NameProduct = tbNameProd.Text;
                    product.DescriptionProduct = tbDescProd.Text;
                    product.TypeProductId = Convert.ToInt32(tbTypeProd.Text);
                    product.CountProduct = Convert.ToInt32(tbCountProd.Text);
                    product.PriceProduct = Convert.ToInt32(tbPriceProd.Text.Remove(tbPriceProd.Text.Length - 3, 3));

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Products", content);
                    }

                    product = new Product();
                    tbPriceProd.Text = "";
                    tbCountProd.Text = "";
                    tbTypeProd.Text = "";
                    tbDescProd.Text = "";
                    tbNameProd.Text = "";
                    FillProduct(); 
                    FillProdInfo();
                }
                catch { }
            }
        }

        private async void btDelProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(tbDescProd.Text == "") && !(tbCountProd.Text == "") && !(tbNameProd.Text == "") && !(tbTypeProd.Text == "") && !(tbPriceProd.Text == ""))
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

                    using (var httpClient = new HttpClient())
                    {
                        Product productDel = new Product();
                        productDel.IdProduct = Convert.ToInt32(productDel.IdProduct);
                        foreach (var a in products)
                        {
                            if (product.IdProduct == a.IdProduct) productDel = a;
                        }

                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        var response = await httpClient.DeleteAsync("https://localhost:7087/api/Products/" + productDel.IdProduct);
                    }

                    product = new Product();
                    tbPriceProd.Text = "";
                    tbCountProd.Text = "";
                    tbTypeProd.Text = "";
                    tbDescProd.Text = "";
                    tbNameProd.Text = "";
                    FillProduct();
                }
            }
            catch { }

        }

        private async void btUpProd_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbDescProd.Text == "") && !(tbCountProd.Text == "") && !(tbNameProd.Text == "") && !(tbTypeProd.Text == "") && !(tbPriceProd.Text == ""))
            {
                product.NameProduct = tbNameProd.Text;
                product.DescriptionProduct = tbDescProd.Text;
                product.TypeProductId = Convert.ToInt32(tbTypeProd.Text);
                product.CountProduct = Convert.ToInt32(tbCountProd.Text);
                product.PriceProduct = Convert.ToInt32(tbPriceProd.Text.Remove(tbPriceProd.Text.Length - 3, 3));

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Products/" + product.IdProduct, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                product = new Product();
                tbPriceProd.Text = "";
                tbCountProd.Text = "";
                tbTypeProd.Text = "";
                tbDescProd.Text = "";
                tbNameProd.Text = "";
                FillProduct();
            }
        }

        private async void btAddTypeProd_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameType.Text == "") && !(tbIdDanger.Text == ""))
            {
                try
                {
                    typeProduct = new TypeProduct();
                    typeProduct.NameTypeProduct = tbNameType.Text;
                    typeProduct.DangerId = Convert.ToInt32(tbIdDanger.Text);
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(typeProduct), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/TypeProducts", content);

                        typeProduct = new TypeProduct();
                        FillTypeProduct();
                    }
                }
                catch { }
            }
        }

        private async void btDelTypeProd_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameType.Text == ""))
            {
                List<TypeProduct> typeProducts = new List<TypeProduct>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/TypeProducts");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        typeProducts = JsonConvert.DeserializeObject<List<TypeProduct>>(apiResponse);
                        dgTypeProdNames.ItemsSource = typeProducts;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    TypeProduct typeProductDel = new TypeProduct();
                    typeProductDel.IdTypeProduct = Convert.ToInt32(typeProduct.IdTypeProduct);
                    foreach (var a in typeProducts)
                    {
                        if (typeProductDel.IdTypeProduct == a.IdTypeProduct) typeProductDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/TypeProducts/" + typeProductDel.IdTypeProduct);
                }

                tbNameType.Text = "";
                tbTypeProd.Text = "";
                typeProduct = new TypeProduct();
                FillTypeProduct();
            }
        }

        private async void btUpTypeProd_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameType.Text == ""))
            {
                typeProduct.NameTypeProduct = tbNameType.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(typeProduct), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/TypeProducts/" + typeProduct.IdTypeProduct, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                tbNameType.Text = null;
                tbTypeProd.Text = "";
                FillTypeProduct();
            }
        }

        private async void btAddDanger_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameDanger.Text == ""))
            {
                try
                {
                    Danger danger = new Danger();
                    danger.NameDanger = tbNameDanger.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(danger), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Dangers", content);

                        tbIdDanger.Text = null;
                        tbNameDanger.Text = null;
                        FillDanger();
                    }
                }
                catch { }
            }
        }

        private async void btDelDanger_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbIdDanger.Text == ""))
            {
                List<Danger> danger = new List<Danger>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Dangers");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        danger = JsonConvert.DeserializeObject<List<Danger>>(apiResponse);
                        dgDangerNames.ItemsSource = danger;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Danger dangerDel = new Danger();
                    dangerDel.IdDanger = Convert.ToInt32(tbIdDanger.Text);
                    foreach (var a in danger)
                    {
                        if (dangerDel.IdDanger == a.IdDanger) dangerDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Dangers/" + dangerDel.IdDanger);
                }

                tbIdDanger.Text = null;
                tbNameDanger.Text = null;
                FillDanger();
            }
        }

        private async void btUpDanger_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameDanger.Text == "") && !(tbIdDanger.Text == ""))
            {
                Danger danger = new Danger();
                danger.NameDanger = tbNameDanger.Text;
                danger.IdDanger = Convert.ToInt32(tbIdDanger.Text);
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(danger), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Dangers/" + danger.IdDanger, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();

                }

                tbIdDanger.Text = null;
                tbNameDanger.Text = null;
                FillDanger();
            }
        }

        private void lbDangerNames_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                Danger selectRow = (Danger)dgDangerNames.SelectedItems[0];
                tbIdDanger.Text = selectRow.IdDanger.ToString();
                tbNameDanger.Text = selectRow.NameDanger;
            }
            catch { }
        }

        private void dgTypeProdNames_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                TypeProduct selectRow = (TypeProduct)dgTypeProdNames.SelectedItems[0];
                typeProduct.DangerId = selectRow.DangerId;
                typeProduct.IdTypeProduct = selectRow.IdTypeProduct;
                typeProduct.NameTypeProduct = selectRow.NameTypeProduct;

                tbNameType.Text = selectRow.NameTypeProduct;
                 tbTypeProd.Text = selectRow.IdTypeProduct.ToString();
            }
            catch { }
        }

        private void dgProducts_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                Product selectRow = (Product)dgProducts.SelectedItems[0];
                product.IdProduct = selectRow.IdProduct;
                product.DescriptionProduct = selectRow.DescriptionProduct;
                product.NameProduct = selectRow.NameProduct;
                product.CountProduct = selectRow.CountProduct;
                product.PriceProduct = selectRow.PriceProduct;
                product.TypeProductId = selectRow.TypeProductId;    

                tbPriceProd.Text = product.PriceProduct.ToString();
                tbTypeProd.Text = product.TypeProductId.ToString();
                tbNameProd.Text = product.NameProduct;
                tbCountProd.Text = product.CountProduct.ToString();
                tbDescProd.Text = product.DescriptionProduct;
                tbTypeProd.Text = product.TypeProductId.ToString();
            }
            catch { }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillProdInfo();
            FillDanger();
            FillTypeProduct();
            FillProduct();
        }

        private async void FillProdInfo()
        {
            List<ProductInfo> ProductInfos = new List<ProductInfo>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Views/GetProducts");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ProductInfos = JsonConvert.DeserializeObject<List<ProductInfo>>(apiResponse);
                    dgViewProdInfo.ItemsSource = ProductInfos;
                }
            }
        }

        private async void FillDanger()
        {
            List<Danger> danger = new List<Danger>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Dangers");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    danger = JsonConvert.DeserializeObject<List<Danger>>(apiResponse);
                    dgDangerNames.ItemsSource = danger;
                }
            }
        }

        private async void FillTypeProduct()
        {
            List<TypeProduct> typeProducts = new List<TypeProduct>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/TypeProducts");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    typeProducts = JsonConvert.DeserializeObject<List<TypeProduct>>(apiResponse);
                    dgTypeProdNames.ItemsSource = typeProducts;
                }
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
    }
}
