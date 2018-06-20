using PagedList;
using RSSWepApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RSSWepApp.Controllers
{
    public class PostController : Controller
    {
        NewsContext db = new NewsContext();


        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            if (page < 1)
                return HttpNotFound();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = currentFilter;

            var groups = from a in db.Articles
                         group a by a.Source.Name;
            ViewBag.Sources = new List<string>();
            ViewBag.Sources.Add("Все");
            foreach (var g in groups)
            {
                ViewBag.Sources.Add(g.Key);
            }

            var articles = from a in db.Articles
                           select a;

            if (!String.IsNullOrEmpty(currentFilter) && currentFilter.CompareTo("Все") != 0)
            {
                articles = articles.Where(a => a.Source.Name.CompareTo(currentFilter) == 0);
            }

            switch (sortOrder)
            {
                case "Date":
                    articles = articles.OrderByDescending(a => a.PublishDate);
                    break;
                case "Source":
                    articles = articles.OrderBy(a => a.Source.Name);
                    break;
                default:
                    articles = articles.OrderBy(a => a.PublishDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }
    }
}