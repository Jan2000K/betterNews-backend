using System.Net.Http;
using System.Xml;
using System.Text.RegularExpressions;
namespace WebApplication1

{
    public class NewsSource
    {

        public static readonly NewsSource[] allNewsSourcesSLO =
        {
            new NewsSource("https://img.rtvslo.si/feeds/01.xml","RTV",Languages.SLO,NewsArticle.Categories.Slovenia),
            new NewsSource("https://img.rtvslo.si/feeds/02.xml","RTV",Languages.SLO,NewsArticle.Categories.World),
            new NewsSource("https://img.rtvslo.si/feeds/03.xml","RTV",Languages.SLO,NewsArticle.Categories.Sport),
            new NewsSource("https://img.rtvslo.si/feeds/09.xml","RTV",Languages.SLO,NewsArticle.Categories.Technology),
            new NewsSource("https://img.rtvslo.si/feeds/06.xml","RTV",Languages.SLO,NewsArticle.Categories.Other)

        };

        public static readonly NewsSource[] allNewsSourcesENG =
{
            new NewsSource("https://rss.nytimes.com/services/xml/rss/nyt/Technology.xml","New York Times",Languages.ENG,NewsArticle.Categories.Technology),
            new NewsSource("https://www.france24.com/en/rss","France 24",Languages.ENG,NewsArticle.Categories.World),
            new NewsSource("http://en.espn.co.uk/rss/sport/story/feeds/0.xml?type=2","ESPN UK",Languages.ENG,NewsArticle.Categories.Sport),
            new NewsSource("https://feeds.a.dj.com/rss/RSSWSJD.xml","Wall Street Journal",Languages.ENG,NewsArticle.Categories.Technology),
            new NewsSource("https://rss.dw.com/rdf/rss-en-cul","Deutsche Welle",Languages.ENG,NewsArticle.Categories.Other),
        };


        public enum Languages
        {
            SLO,
            ENG
        }

        public string Uri;

        public string Name;
        public Languages language;


        public NewsArticle.Categories Category;


        public NewsSource(string URI, string name, Languages language, NewsArticle.Categories category)
        {
            this.Uri = URI;
            this.Name = name;
            this.language = language;
            this.Category = category;

        }

        public async Task<List<NewsArticle>> parseNews()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(Uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error Parsing News Feeed from {this.Uri}");
            }
            var body = await response.Content.ReadAsStringAsync();
            XmlDocument xmlDocument = new XmlDocument();
            try { xmlDocument.LoadXml(body); }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing XML doucment! \n {ex.Message}");
            }
            var articleItems = xmlDocument.GetElementsByTagName("item");
            List<NewsArticle> newsArticles = new List<NewsArticle>();
            if (articleItems == null)
            {
                return newsArticles;
            }
            foreach (XmlElement article in articleItems)
            {

                string Title = article.GetElementsByTagName("title")[0].InnerText.Replace("![CDATA[", "").Replace("]]", "");
                string Desc = article.GetElementsByTagName("description")[0].InnerText.Replace("![CDATA[", "").Replace("]]", "");
                string Link = article.GetElementsByTagName("link")[0].InnerText.Replace("![CDATA[", "").Replace("]]", "");
                newsArticles.Add(new NewsArticle(Title, Desc, Link,this.Category));
            }
            return newsArticles;
        }

        public static NewsArticle.Categories assignCategory(string category, Languages language)
        {
            NewsArticle.Categories Category;
            if (language == Languages.SLO)
            {
                switch (category)
                {
                    case "slovenija":
                        Category = NewsArticle.Categories.Slovenia;
                        break;
                    case "šport":
                        Category = NewsArticle.Categories.Sport;
                        break;
                    case "tehnologija":
                        Category = NewsArticle.Categories.Technology;
                        break;
                    default:
                        Category = NewsArticle.Categories.Other;
                        break;
                }
            }
            else
            {
                switch (category)
                {
                    case "england":
                        Category = NewsArticle.Categories.Slovenia;
                        break;
                    case "sport":
                        Category = NewsArticle.Categories.Sport;
                        break;
                    case "technology":
                        Category = NewsArticle.Categories.Technology;
                        break;
                    default:
                        Category = NewsArticle.Categories.Other;
                        break;
                }
            }
            return Category;
        }
    }
}
