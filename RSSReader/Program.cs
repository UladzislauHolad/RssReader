using System;
using System.Text;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace RSSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Source[] sources =
            {
                new Source
                {
                    Name = "Интерфакс",
                    Url = "https://www.interfax.by/news/feed"
                },
                new Source
                {
                    Name = "Хабрахабр",
                    Url = "https://habrahabr.ru/rss/"
                }
            };

            NewsContext db = new NewsContext();

            try
            {
                foreach (Source source in sources)
                {
                    int readNews = 0;
                    int recordedNews = 0;

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(source.Url);
                    HttpWebResponse httpWebesponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    Stream dataStream = httpWebesponse.GetResponseStream();
                    StreamReader streamreader = new StreamReader(dataStream, Encoding.UTF8);
                    string response = streamreader.ReadToEnd();
                    streamreader.Close();

                    response = response.TrimStart('\n');

                    XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(response));
                    reader.Read();

                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    foreach (SyndicationItem item in feed.Items)
                    {
                        Article article = new Article();

                        article.Title = item.Title.Text;
                        article.PublishDate = item.PublishDate.DateTime;
                        article.Description = Regex.Replace(item.Summary.Text, "<.*?>", string.Empty);
                        article.Url = item.Links[0].Uri.AbsoluteUri;
                        article.Source = source;

                        if (db.Articles.Find(article.Title, article.PublishDate) == null)
                        {
                            db.Articles.Add(article);
                            recordedNews++;
                        }
                        readNews++;
                    }

                    db.SaveChanges();
                    Console.WriteLine(source.Name);
                    Console.WriteLine($"Прочитано новостей: {readNews}");
                    Console.WriteLine($"Сохранено новостей: {recordedNews}");
                }
            }
            catch(WebException ex)
            {
                Console.WriteLine(ex.Message + "\nПроверьте подключение к интернету");
            }

            Console.ReadKey();
        }
    }
}
