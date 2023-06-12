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
    public partial class PrisonerWindow : Window
    {
        Caste caste = new Caste();
        Prisoner prisoner = new Prisoner();
        IndividualOffer individualOffer = new IndividualOffer();
        Work work = new Work();
        ArticleOfTheConclusion articleOfTheConclusion = new ArticleOfTheConclusion();
        Nation nation = new Nation();
        TypeOfActivity typeOfActivity = new TypeOfActivity();
        Health health = new Health();

        public PrisonerWindow()
        {
            InitializeComponent();
        }

        private void dgCaste_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                caste = (Caste)dgCaste.SelectedItems[0];

                tbNameCaste.Text = caste.NameCaste;
            }
            catch { }
        }

        private void dgNation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                nation = (Nation)dgNation.SelectedItems[0];

                tbNameNation.Text = nation.NameNation;
            }
            catch { }
        }

        private void ArticleOfConc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                articleOfTheConclusion = (ArticleOfTheConclusion)dgArticleOfConc.SelectedItems[0];

                tbNameArticleOfConc.Text = articleOfTheConclusion.ArticleNumber;
            }
            catch { }
        }

        private void dgWork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                work = (Work)dgWork.SelectedItems[0];

                tbNameWork.Text = work.NameWork;
            }
            catch { }
        }

        private void dgTypeOfActivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                typeOfActivity = (TypeOfActivity)dgTypeOfActivity.SelectedItems[0];

                tbNameTypeOfActivity.Text = typeOfActivity.NameTypeOfActivity;
            }
            catch { }
        }

        private async void btUpTypeOfActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameTypeOfActivity.Text == ""))
            {
                TypeOfActivity onzhectUp = new TypeOfActivity();
                onzhectUp.IdTypeOfActivity = typeOfActivity.IdTypeOfActivity;
                onzhectUp.NameTypeOfActivity = tbNameTypeOfActivity.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/TypeOfActivities/" + onzhectUp.IdTypeOfActivity, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                typeOfActivity = new TypeOfActivity();
                tbNameTypeOfActivity.Text = null;
                FillTypeOfActivity();
            }
        }

        private async void btDelTypeOfActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameTypeOfActivity.Text == ""))
            {
                List<TypeOfActivity> onzhects = new List<TypeOfActivity>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/TypeOfActivities");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        onzhects = JsonConvert.DeserializeObject<List<TypeOfActivity>>(apiResponse);
                        dgTypeOfActivity.ItemsSource = onzhects;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    TypeOfActivity onzhectDel = new TypeOfActivity();
                    onzhectDel.IdTypeOfActivity = Convert.ToInt32(typeOfActivity.IdTypeOfActivity);
                    foreach (var a in onzhects)
                    {
                        if (onzhectDel.IdTypeOfActivity == a.IdTypeOfActivity) onzhectDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/TypeOfActivities/" + onzhectDel.IdTypeOfActivity);
                }

                typeOfActivity = new TypeOfActivity();
                tbNameTypeOfActivity.Text = null;
                FillTypeOfActivity();
            }
        }

        private async void btAddTypeOfActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameTypeOfActivity.Text == ""))
            {
                try
                {
                    TypeOfActivity onzhect = new TypeOfActivity();
                    onzhect.NameTypeOfActivity = tbNameTypeOfActivity.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/TypeOfActivities", content);

                        tbNameTypeOfActivity.Text = null;
                        FillTypeOfActivity();
                    }

                }
                catch { }
            }
        }

        private async void btUpWork_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameWork.Text == ""))
            {
                Work onzhectUp = new Work();
                onzhectUp.IdWork = work.IdWork;
                onzhectUp.NameWork = tbNameWork.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Works/" + onzhectUp.IdWork, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                work = new Work();
                tbNameWork.Text = null;
                FillWork();
            }
        }

        private async void btDelWork_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameWork.Text == ""))
            {
                List<Work> onzhects = new List<Work>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Works");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        onzhects = JsonConvert.DeserializeObject<List<Work>>(apiResponse);
                        dgWork.ItemsSource = onzhects;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Work onzhectDel = new Work();
                    onzhectDel.IdWork = Convert.ToInt32(work.IdWork);
                    foreach (var a in onzhects)
                    {
                        if (onzhectDel.IdWork == a.IdWork) onzhectDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Works/" + onzhectDel.IdWork);
                }

                work = new Work();
                tbNameWork.Text = null;
                FillWork();
            }
        }

        private async void btAddWork_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameWork.Text == ""))
            {
                try
                {
                    Work onzhect = new Work();
                    onzhect.NameWork = tbNameWork.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Works", content);

                        tbNameArticleOfConc.Text = null;
                        FillWork();
                    }
                }
                catch { }
            }
        }

        private async void btUpArticleOfConc_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameArticleOfConc.Text == ""))
            {
                ArticleOfTheConclusion onzhectUp = new ArticleOfTheConclusion();
                onzhectUp.IdAotc = articleOfTheConclusion.IdAotc;
                onzhectUp.ArticleNumber = tbNameArticleOfConc.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/ArticleOfTheConclusions/" + onzhectUp.IdAotc, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                articleOfTheConclusion = new ArticleOfTheConclusion();
                tbNameArticleOfConc.Text = null;
                FillArtOfConclusion();
            }
        }

        private async void btDelArticleOfConc_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameArticleOfConc.Text == ""))
            {
                List<ArticleOfTheConclusion> onzhects = new List<ArticleOfTheConclusion>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/ArticleOfTheConclusions");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        onzhects = JsonConvert.DeserializeObject<List<ArticleOfTheConclusion>>(apiResponse);
                        dgArticleOfConc.ItemsSource = onzhects;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    ArticleOfTheConclusion onzhectDel = new ArticleOfTheConclusion();
                    onzhectDel.IdAotc = Convert.ToInt32(articleOfTheConclusion.IdAotc);
                    foreach (var a in onzhects)
                    {
                        if (onzhectDel.IdAotc == a.IdAotc) onzhectDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/ArticleOfTheConclusions/" + onzhectDel.IdAotc);
                }

                articleOfTheConclusion = new ArticleOfTheConclusion();
                tbNameArticleOfConc.Text = null;
                FillArtOfConclusion();
            }
        }

        private async void btAddArticleOfConc_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameArticleOfConc.Text == ""))
            {
                try
                {
                    ArticleOfTheConclusion onzhect = new ArticleOfTheConclusion();
                    onzhect.ArticleNumber = tbNameArticleOfConc.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/ArticleOfTheConclusions", content);

                        tbNameArticleOfConc.Text = null;
                        FillArtOfConclusion();
                    }
                }
                catch { }
            }
        }

        private async void btUpNation_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameNation.Text == ""))
            {
                Nation onzhectUp = new Nation();
                onzhectUp.IdNation = nation.IdNation;
                onzhectUp.NameNation = tbNameNation.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Nations/" + onzhectUp.IdNation, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                nation = new Nation();
                tbNameNation.Text = null;
                FillNation();
            }
        }

        private async void btDelNation_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameNation.Text == ""))
            {
                List<Nation> onzhects = new List<Nation>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Nations");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        onzhects = JsonConvert.DeserializeObject<List<Nation>>(apiResponse);
                        dgNation.ItemsSource = onzhects;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Nation onzhectDel = new Nation();
                    onzhectDel.IdNation = Convert.ToInt32(nation.IdNation);
                    foreach (var a in onzhects)
                    {
                        if (onzhectDel.IdNation == a.IdNation) onzhectDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Nations/" + onzhectDel.IdNation);
                }

                nation = new Nation();
                tbNameNation.Text = null;
                FillNation();
            }
        }

        private async void btAddNation_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameNation.Text == ""))
            {
                try
                {
                    Nation onzhect = new Nation();
                    onzhect.NameNation = tbNameNation.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Nations", content);

                        tbNameNation.Text = null;
                        FillNation();
                    }
                }
                catch { }
            }
        }

        private async void btAddCaste_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameCaste.Text == ""))
            {
                try
                {
                    Caste onzhect = new Caste();
                    onzhect.NameCaste = tbNameCaste.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Castes", content);

                        tbNameCaste.Text = null;
                        FillCaste();
                    }
                }
                catch { }
            }
        }

        private async void btDelCaste_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameCaste.Text == ""))
            {
                List<Caste> onzhects = new List<Caste>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Castes");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        onzhects = JsonConvert.DeserializeObject<List<Caste>>(apiResponse);
                        dgCaste.ItemsSource = onzhects;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Caste onzhectDel = new Caste();
                    onzhectDel.IdCaste = Convert.ToInt32(caste.IdCaste);
                    foreach (var a in onzhects)
                    {
                        if (onzhectDel.IdCaste == a.IdCaste) onzhectDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Castes/" + onzhectDel.IdCaste);
                }

                caste = new Caste();
                tbNameCaste.Text = null;
                FillCaste();
            }
        }

        private async void btUpCaste_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameCaste.Text == ""))
            {
                Caste onzhectUp = new Caste();
                onzhectUp.IdCaste = caste.IdCaste;
                onzhectUp.NameCaste = tbNameCaste.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Castes/" + onzhectUp.IdCaste, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                caste = new Caste();
                tbNameCaste.Text = null;
                FillCaste();
            }
        }

        private async void btAddPrisoner_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbLoginPrisoner.Text == "") && !(tbSecondNamePrisoner.Text == "") && !(tbNamePrisoner.Text == "") && !(tbPasswordPrisoner.Text == "") && !(tbMiddleNamePrisoner.Text == ""))
            {
                try
                {
                    Prisoner onzhect = new Prisoner();
                    onzhect.WorkId = work.IdWork;
                    onzhect.NationId = nation.IdNation;
                    onzhect.CasteId = caste.IdCaste;
                    onzhect.ArticleOfTheConclusionId = articleOfTheConclusion.IdAotc;
                    onzhect.IndividualOffersId = individualOffer.IdIndividualOffers;
                    onzhect.TypeOfActivityId = typeOfActivity.IdTypeOfActivity;
                    onzhect.HealthId = health.IdHealth;

                    onzhect.FirstNamePrisoner = tbNamePrisoner.Text;
                    onzhect.SecondNamePrisoner = tbSecondNamePrisoner.Text;
                    onzhect.MiddleNamePrisoner = tbMiddleNamePrisoner.Text;
                    onzhect.LoginPrisoner = tbLoginPrisoner.Text;

                    Random random = new Random();
                    onzhect.Salt = RandomString(random.Next(30));
                    onzhect.PasswordPrisoner = tbPasswordPrisoner.Text;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(onzhect), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Auth/register-prisoner", content);

                        tbNamePrisoner.Text = null;
                        tbSecondNamePrisoner.Text = null;
                        tbMiddleNamePrisoner.Text = null;
                        tbLoginPrisoner.Text = null;
                        tbPasswordPrisoner.Text = null;
                        FillPrisoners();
                    }
                }
                catch { }
            }
        }

        private async void btDelPrisoner_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbLoginPrisoner.Text == "") && !(tbSecondNamePrisoner.Text == "") && !(tbNamePrisoner.Text == "") && !(tbPasswordPrisoner.Text == "") && !(tbMiddleNamePrisoner.Text == ""))
            {
                List<Prisoner> onzhects = new List<Prisoner>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Prisoners");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        onzhects = JsonConvert.DeserializeObject<List<Prisoner>>(apiResponse);
                        dgPrisoner.ItemsSource = onzhects;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Prisoner onzhectDel = new Prisoner();
                    onzhectDel.IdPrisoner = Convert.ToInt32(prisoner.IdPrisoner);
                    foreach (var a in onzhects)
                    {
                        if (onzhectDel.IdPrisoner == a.IdPrisoner) onzhectDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Prisoners/" + onzhectDel.IdPrisoner);
                }

                prisoner = new Prisoner();
                tbNamePrisoner.Text = null;
                tbSecondNamePrisoner.Text = null;
                tbMiddleNamePrisoner.Text = null;
                tbLoginPrisoner.Text = null;
                tbPasswordPrisoner.Text = null;
                FillPrisoners();
            }
        }

        private async void btUpPrisoner_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbLoginPrisoner.Text == "") && !(tbSecondNamePrisoner.Text == "") && !(tbNamePrisoner.Text == "") && !(tbPasswordPrisoner.Text == "") && !(tbMiddleNamePrisoner.Text == ""))
            {
                Prisoner onzhectUp = new Prisoner();
                onzhectUp.WorkId = work.IdWork;
                onzhectUp.NationId = nation.IdNation;
                onzhectUp.CasteId = caste.IdCaste;
                onzhectUp.ArticleOfTheConclusionId = articleOfTheConclusion.IdAotc;
                onzhectUp.IndividualOffersId = individualOffer.IdIndividualOffers;
                onzhectUp.TypeOfActivityId = typeOfActivity.IdTypeOfActivity;
                onzhectUp.HealthId = health.IdHealth;

                onzhectUp.FirstNamePrisoner = tbNamePrisoner.Text;
                onzhectUp.SecondNamePrisoner = tbSecondNamePrisoner.Text;
                onzhectUp.MiddleNamePrisoner = tbMiddleNamePrisoner.Text;
                onzhectUp.LoginPrisoner = tbLoginPrisoner.Text;

                Random random = new Random();
                onzhectUp.Salt = RandomString(random.Next(30));
                onzhectUp.PasswordPrisoner = HashString(tbPasswordPrisoner.Text + onzhectUp.Salt);

                onzhectUp.IdPrisoner = prisoner.IdPrisoner;

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(onzhectUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Prisoners/" + onzhectUp.IdPrisoner, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

                prisoner = new Prisoner();
                tbNamePrisoner.Text = null;
                tbSecondNamePrisoner.Text = null;
                tbMiddleNamePrisoner.Text = null;
                tbLoginPrisoner.Text = null;
                tbPasswordPrisoner.Text = null;
                FillPrisoners();
            }
        }

        private void dgPrisoner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                prisoner = (Prisoner)dgPrisoner.SelectedItems[0];

                tbLoginPrisoner.Text = prisoner.LoginPrisoner;
                tbMiddleNamePrisoner.Text = prisoner.MiddleNamePrisoner;
                tbNamePrisoner.Text = prisoner.FirstNamePrisoner;
                tbSecondNamePrisoner.Text = prisoner.SecondNamePrisoner;
                tbPasswordPrisoner.Text = prisoner.PasswordPrisoner;
            }
            catch { }
        }

        private void dgIndOffers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                individualOffer = (IndividualOffer)dgIndOffers.SelectedItems[0];

                tbNameOffer.Text = individualOffer.NameIndividualOffers;
                tbNameDesc.Text = individualOffer.DescriptionIndividualOffer;
            }
            catch { }
        }

        private async void btAddIndOffer_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameOffer.Text == "") && !(tbNameDesc.Text == ""))
            {
                try
                {
                    IndividualOffer individualOffer = new IndividualOffer();
                    individualOffer.NameIndividualOffers = tbNameOffer.Text;
                    individualOffer.DescriptionIndividualOffer = tbNameDesc.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(individualOffer), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/IndividualOffers", content);

                        tbNameOffer.Text = null;
                        tbNameDesc.Text = null;
                        FillIndividualOffers();
                    }
                }
                catch { }
            }
        }

        private async void btDelIndOffer_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameOffer.Text == "") && !(tbNameDesc.Text == ""))
            {
                List<IndividualOffer> individualOffers = new List<IndividualOffer>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/IndividualOffers");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        individualOffers = JsonConvert.DeserializeObject<List<IndividualOffer>>(apiResponse);
                        dgIndOffers.ItemsSource = individualOffers;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    IndividualOffer individualOfferDel = new IndividualOffer();
                    individualOfferDel.IdIndividualOffers = Convert.ToInt32(individualOffer.IdIndividualOffers);
                    foreach (var a in individualOffers)
                    {
                        if (individualOfferDel.IdIndividualOffers == a.IdIndividualOffers) individualOfferDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/IndividualOffers/" + individualOfferDel.IdIndividualOffers);
                }

                individualOffer = new IndividualOffer();
                tbNameOffer.Text = null;
                tbNameDesc.Text = null;
                FillIndividualOffers();
            }
        }

        private async void btUpIndOffer_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameOffer.Text == "") && !(tbNameDesc.Text == ""))
            {
                IndividualOffer individualOfferUp = new IndividualOffer();
                individualOfferUp.IdIndividualOffers = individualOffer.IdIndividualOffers;
                individualOfferUp.NameIndividualOffers = tbNameOffer.Text;
                individualOfferUp.DescriptionIndividualOffer = tbNameDesc.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(individualOfferUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/IndividualOffers/" + individualOfferUp.IdIndividualOffers, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();

                }

                individualOffer = new IndividualOffer();
                tbNameOffer.Text = null;
                tbNameDesc.Text = null;
                FillIndividualOffers();
            }
        }

        private void dgHealth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                health = (Health)dgHealth.SelectedItems[0];

                tbNameHealth.Text = health.NameHealth;
            }
            catch { }
        }

        private async void btAddHealth_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameHealth.Text == ""))
            {
                try
                {
                    Health healths = new Health();
                    healths.NameHealth = tbNameHealth.Text;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(healths), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("https://localhost:7087/api/Healths", content);

                        health = new Health();
                        tbNameHealth.Text = null;
                        FillHealth();
                    }

                }
                catch { }
            }
        }

        private async void btDelHealth_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameHealth.Text == ""))
            {
                List<Health> healths = new List<Health>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.GetAsync("https://localhost:7087/api/Healths");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        healths = JsonConvert.DeserializeObject<List<Health>>(apiResponse);
                        dgHealth.ItemsSource = healths;
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    Health healthDel = new Health();
                    healthDel.IdHealth = Convert.ToInt32(health.IdHealth);
                    foreach (var a in healths)
                    {
                        if (healthDel.IdHealth == a.IdHealth) healthDel = a;
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Healths/" + healthDel.IdHealth);
                }

                health = new Health();
                tbNameHealth.Text = null;
                FillHealth();
            }
        }

        private async void btUpHealth_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameHealth.Text == ""))
            {
                Health healthUp = new Health();
                healthUp.IdHealth = health.IdHealth;
                healthUp.NameHealth = tbNameHealth.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(healthUp), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Healths/" + healthUp.IdHealth, content);

                    string apiResponse = await response.Content.ReadAsStringAsync();

                }

                health = new Health();
                tbNameHealth.Text = null;
                FillHealth();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillNation();
            FillWork();
            FillCaste();
            FillTypeOfActivity();
            FillArtOfConclusion();
            FillPrisoners();
            FillIndividualOffers();
            FillPrisonerInfo();
            FillHealth();
        }

        private async void FillNation()
        {
            List<Nation> nations = new List<Nation>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Nations");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    nations = JsonConvert.DeserializeObject<List<Nation>>(apiResponse);
                    dgNation.ItemsSource = nations;
                }
            }
        }

        private async void FillHealth()
        {
            List<Health> healths = new List<Health>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Healths");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    healths = JsonConvert.DeserializeObject<List<Health>>(apiResponse);
                    dgHealth.ItemsSource = healths;
                }
            }
        }

        private async void FillWork()
        {
            List<Work> works = new List<Work>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Works");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    works = JsonConvert.DeserializeObject<List<Work>>(apiResponse);
                    dgWork.ItemsSource = works;
                }
            }
        }

        private async void FillCaste()
        {
            List<Caste> caste = new List<Caste>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Castes");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    caste = JsonConvert.DeserializeObject<List<Caste>>(apiResponse);
                    dgCaste.ItemsSource = caste;
                }
            }
        }

        private async void FillTypeOfActivity()
        {
            List<TypeOfActivity> typeOfActivities = new List<TypeOfActivity>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/TypeOfActivities");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    typeOfActivities = JsonConvert.DeserializeObject<List<TypeOfActivity>>(apiResponse);
                    dgTypeOfActivity.ItemsSource = typeOfActivities;
                }
            }
        }

        private async void FillArtOfConclusion()
        {
            List<ArticleOfTheConclusion> articleOfTheConclusions = new List<ArticleOfTheConclusion>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/ArticleOfTheConclusions");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    articleOfTheConclusions = JsonConvert.DeserializeObject<List<ArticleOfTheConclusion>>(apiResponse);
                    dgArticleOfConc.ItemsSource = articleOfTheConclusions;
                }
            }
        }

        private async void FillPrisoners()
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
                    dgPrisoner.ItemsSource = prisoners;
                }
            }
        }

        private async void FillIndividualOffers()
        {
            List<IndividualOffer> individualOffers = new List<IndividualOffer>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/IndividualOffers");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    individualOffers = JsonConvert.DeserializeObject<List<IndividualOffer>>(apiResponse);
                    dgIndOffers.ItemsSource = individualOffers;
                }
            }
        }

        private async void FillPrisonerInfo()
        {
            List<PrisonerInfo> prisonerInfos = new List<PrisonerInfo>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Views/GetPrisoners");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    prisonerInfos = JsonConvert.DeserializeObject<List<PrisonerInfo>>(apiResponse);
                    dgPrisonerInfo.ItemsSource = prisonerInfos;
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

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012!#$^%*().,/@3456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
