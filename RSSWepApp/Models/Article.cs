using System;

namespace RSSWepApp.Models
{
    public class Article
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Source Source { get; set; }
    }
}