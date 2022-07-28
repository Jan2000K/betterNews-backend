namespace WebApplication1
{

    public class NewsArticle
    {
        public enum Categories
        {
            World,
            Slovenia,
            Sport,
            Technology,
            Other
        }
        
        public string Title { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }

        public Categories Category { get; set; }

        public NewsArticle(string title,string description,string link,Categories category)
        {
            Title = title;
            Description = description;
            Link = link;
            Category = category;
        }

    }
    
}
