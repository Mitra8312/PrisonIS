using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PrisonAdministration
{
    public partial class ArticlesWindow : Window
    {
        public ArticlesWindow()
        {
            InitializeComponent();
        }

        private Prisoner prisoner = new Prisoner();
        private Article article = new Article();
        private ArticlePublisher articlePublisher = new ArticlePublisher();
        private FavoriteArticle favoriteArticle = new FavoriteArticle();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillPrisoners();
            FillArticles();
            FillFavoriteArticles();
            FillArticlePublisher();
        }

        private void dgArticles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                article = (Article)dgArticles.SelectedItems[0];

                tbShortName.Text = article.ShortNameArticle;
                tbTegs.Text = article.TegsArticle;
                tbTextArticle.Text = article.TextArticle;

                articlePublisher.IdArticlePublisher = article.ArticlePublisherId;
            }
            catch { }
        }

        private async void btAddArticle_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbShortName.Text == "") && !(tbTegs.Text == "") && !(tbTextArticle.Text == "") && !(articlePublisher.IdArticlePublisher == 0) && !(articlePublisher.IdArticlePublisher == 0))
            {
                Article newarticle = new Article();
                newarticle.ArticlePublisherId = articlePublisher.IdArticlePublisher;
                newarticle.TegsArticle = tbTegs.Text;
                newarticle.TextArticle = tbTextArticle.Text;
                newarticle.ShortNameArticle = tbShortName.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(newarticle), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PostAsync("https://localhost:7087/api/Articles", content);
                }

                article = new Article();
                tbShortName.Text = null;
                tbTegs.Text = null;
                tbTextArticle.Text = null;
                FillArticles();
            }
        }

        private async void btDelArticle_Click(object sender, RoutedEventArgs e)
        {
            if (article.IdArticle != 0)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/Articles/" + article.IdArticle);
                }

                article = new Article();
                tbShortName.Text = null;
                tbTegs.Text = null;
                tbTextArticle.Text = null;
                FillArticles();
            }
        }

        private async void btUpArticle_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbShortName.Text == "") && !(tbTegs.Text == "") && !(tbTextArticle.Text == "") && !(articlePublisher.IdArticlePublisher == 0))
            {
                article.TegsArticle = tbTegs.Text;
                article.TextArticle = tbTextArticle.Text;
                article.ShortNameArticle = tbShortName.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/Articles/" + article.IdArticle, content);
                }

                article = new Article();
                tbShortName.Text = null;
                tbTegs.Text = null;
                tbTextArticle.Text = null;
                FillArticles();
            }
        }

        private void dgArticlePublishers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                articlePublisher = (ArticlePublisher)dgArticlePublishers.SelectedItems[0];

                tbNameArticlePublisher.Text = articlePublisher.NameArticlePublisher;
            }
            catch { }
        }

        private async void btAddArticlePublisher_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameArticlePublisher.Text == ""))
            {
                ArticlePublisher articlePublisher = new ArticlePublisher();
                articlePublisher.NameArticlePublisher = tbNameArticlePublisher.Text;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(articlePublisher), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://localhost:7087/api/ArticlePublishers", content);

                    tbNameArticlePublisher.Text = null;
                    FillArticlePublisher();
                }
            }
        }

        private async void btDelArticlePublisher_Click(object sender, RoutedEventArgs e)
        {
            if (articlePublisher.IdArticlePublisher != 0)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/ArticlePublishers/" + articlePublisher.IdArticlePublisher);
                }
                tbNameArticlePublisher.Text = null;
                FillArticlePublisher();
            }
        }

        private async void btUpArticlePublisher_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbNameArticlePublisher.Text == ""))
            {
                articlePublisher.NameArticlePublisher = tbNameArticlePublisher.Text;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(articlePublisher), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/ArticlePublishers/" + articlePublisher.IdArticlePublisher, content);
                }
                tbNameArticlePublisher.Text = null;
                FillArticlePublisher();
            }
        }

        private void dgFavoriteArticle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                favoriteArticle = (FavoriteArticle)dgFavoriteArticle.SelectedItems[0];

                prisoner.IdPrisoner = favoriteArticle.PrisonerId;
                article.IdArticle = favoriteArticle.ArticleId;
            }
            catch { }
        }

        private void tbNameOffer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                article = (Article)tbNameOffer.SelectedItems[0];
            }
            catch { }
        }

        private void tbNameDesc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                prisoner = (Prisoner)tbNameDesc.SelectedItems[0];
            }
            catch { }
        }

        private async void btAddFavoriteArticle_Click(object sender, RoutedEventArgs e)
        {
            if (!(prisoner.IdPrisoner == 0) && !(article.IdArticle == 0))
            {
                FavoriteArticle favoriteArticle = new FavoriteArticle();
                favoriteArticle.ArticleId = article.IdArticle;
                favoriteArticle.PrisonerId = prisoner.IdPrisoner;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(favoriteArticle), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://localhost:7087/api/FavoriteArticles", content);

                    FillFavoriteArticles();
                }
            }
        }

        private async void btDelFavoriteArticle_Click(object sender, RoutedEventArgs e)
        {
            if (favoriteArticle.IdFavoriteArticle != 0)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.DeleteAsync("https://localhost:7087/api/FavoriteArticles/" + favoriteArticle.IdFavoriteArticle);
                }

                FillFavoriteArticles();
            }
        }

        private async void btUpFavoriteArticle_Click(object sender, RoutedEventArgs e)
        {
            if (!(prisoner.IdPrisoner == 0) && !(article.IdArticle == 0))
            {
                favoriteArticle.ArticleId = article.IdArticle;
                favoriteArticle.PrisonerId = prisoner.IdPrisoner;

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(favoriteArticle), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                    var response = await httpClient.PutAsync("https://localhost:7087/api/FavoriteArticles/" + favoriteArticle.IdFavoriteArticle, content);
                }

                FillFavoriteArticles();
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
                    tbNameDesc.ItemsSource = prisoners;
                }
            }
        }

        private async void FillArticles()
        {
            List<Article> articles = new List<Article>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/Articles");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    articles = JsonConvert.DeserializeObject<List<Article>>(apiResponse);
                    dgArticles.ItemsSource = articles;
                    tbNameOffer.ItemsSource = articles;
                }
            }
        }

        private async void FillArticlePublisher()
        {
            List<ArticlePublisher> articlesPublisher = new List<ArticlePublisher>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/ArticlePublishers");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    articlesPublisher = JsonConvert.DeserializeObject<List<ArticlePublisher>>(apiResponse);
                    dgArticlePublishers.ItemsSource = articlesPublisher;
                }
            }
        }

        private async void FillFavoriteArticles()
        {
            List<FavoriteArticle> favoriteArticles = new List<FavoriteArticle>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.token);
                var response = await httpClient.GetAsync("https://localhost:7087/api/FavoriteArticles");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    favoriteArticles = JsonConvert.DeserializeObject<List<FavoriteArticle>>(apiResponse);
                    dgFavoriteArticle.ItemsSource = favoriteArticles;
                }
            }
        }
    }
}
