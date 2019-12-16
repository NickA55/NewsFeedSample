using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewsFeedSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        List<CollectionSource> collectionSource = new List<CollectionSource>();

        public MainPage()
        {
            InitializeComponent();
        }

        private async Task<bool> GetNewsFeed()
        {
            bool retVal = false;

            var baseAddr = new Uri("https://newsapi.org/v2/top-headlines?sources=techcrunch&apiKey=274b03d60937473b9665c26d6dff167f");
            var client = new HttpClient { BaseAddress = baseAddr };

            try
            {
                var response = await client.GetAsync(baseAddr);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RootObject theArticles = JsonConvert.DeserializeObject<RootObject>(content);

                    List<Article> articles = theArticles.articles;

                    foreach (var article in articles)
                    {
                        collectionSource.Add(new CollectionSource
                        {
                            ArticleDesc = article.description,
                            ArticleImageUrl = article.urlToImage
                        });
                    }
                }
                else
                {
                    var r = response;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return retVal;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await GetNewsFeed();
            collectionViewItems.ItemsSource = collectionSource;
        }
    }

    public class CollectionSource
    {
        public string ArticleDesc { get; set; }
        public string ArticleImageUrl { get; set; }
    }

    public class Source
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Article
    {
        public Source source { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public int totalResults { get; set; }
        public List<Article> articles { get; set; }
    }
}
