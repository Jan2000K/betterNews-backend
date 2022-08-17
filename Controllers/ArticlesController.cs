using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;


namespace WebApplication1.Controllers
{
    [Route("news/{language}")]
    [ApiController]
    
    public class ArticlesController : ControllerBase
    {

        [HttpGet]
        
        public ActionResult<IEnumerable<string[]>> getSources(string language)
        {

            if(language == "SLO")
            {
                string[] retArray= NewsSource.allNewsSourcesSLO.Select(s => s.Name).ToArray();
                return Ok(retArray.Distinct());
            }
            else if(language == "ENG")
            {
   
                string[] retArray = NewsSource.allNewsSourcesENG.Select(s => s.Name).ToArray();

                return Ok(retArray.Distinct());
            }
            else
            {
                return BadRequest();
            }


        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> getNewsArticles(string language, postRequest request)
        {

            if (request == null)
            {
                return BadRequest();
            }
            try
            {
                List<NewsArticle> articles = await getListOfArticles(language, request);

                return Ok(articles);
            }
            catch
            {
                return StatusCode(500);
            }


        }

        public async Task<List<NewsArticle>> getListOfArticles(string language, postRequest request)
        {
            List<NewsArticle> news = new List<NewsArticle>();
            NewsSource[] newsSources;
            if (language == "SLO")
            {
                newsSources = NewsSource.allNewsSourcesSLO;
            }
            else
            {
                newsSources = NewsSource.allNewsSourcesENG;
            }
            foreach (var source in newsSources)
            {

                if (!request.blockedSources.Contains(source.Name))
                {
                    List<NewsArticle> parsedNews;

                    try
                    {
                        parsedNews = await source.parseNews();
                    }
                    catch
                    {
                        throw new Exception("Error parsing news");
                    }
                    foreach (NewsArticle article in parsedNews)
                    {
                        bool ok = true;
                        var articleDesc = article.Description.Replace("\n", "");

                        string[] splitTitle = article.Title
                            .Replace(".",String.Empty)
                            .Replace(",", String.Empty)
                            .Replace("!", String.Empty)
                            .Replace(":", String.Empty)
                            .Replace(";", String.Empty)
                            .Replace("-", String.Empty)
                            .Split(' ');
                        string[] descSplit =articleDesc
                            .Replace(".", String.Empty)
                            .Replace(",", String.Empty)
                            .Replace("!", String.Empty)
                            .Replace(":", String.Empty)
                            .Replace(";", String.Empty)
                            .Replace("-", String.Empty)
                            .Split(' ');
                        
                        string[] blockedWordsLower = request.blockedWords.Select(word => word.ToLower()).ToArray();
                        foreach (string word in splitTitle)
                        {
                            if (blockedWordsLower.Contains(word.ToLower()))
                            {

                                ok = false; 
                                break;
                            }
                        }
                        if (ok)
                        {
                            foreach (string word in descSplit)
                            {
                                if (blockedWordsLower.Contains(word.ToLower()))
                                {

                                    ok = false;
                                    break;
                                }
                            }
                        }   
                        if (ok)
                        {
                            news.Add(article);
                        }
                    }
                }
            }
            return news;

        }
    }
}
