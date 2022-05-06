using ClawByHtmlAgility.Helpers;
using ClawByHtmlAgility.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClawByHtmlAgility.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Claw(InputModel model)
        {
            var data = new List<PostModel>();
            var urls = model.Urls.Split('\n');
            foreach (var url in urls)
            {
                string urlget = url.Trim();
                if(urlget.Length > 5)
                {
                    data.Add(GetPost(urlget, model));
                }
                
            }
            return Json(data);
        }
        public PostModel GetPost(string url, InputModel model)
        {
            var post = new PostModel();
            var htmlHelper = new HtmlAgilityHelpers();
            var doc = htmlHelper.GetDocument(url);
            if (doc != null)
            {
                post.Title = htmlHelper.GetTitle(doc);
                post.MetaDescription = htmlHelper.GetDescription(doc);
                post.MetaKeywords = htmlHelper.GetKeywords(doc);
                post.MetaImage = htmlHelper.GetImageFromMeta(doc);
                post.Image =string.Join("\n", htmlHelper.GetImages(doc, model.Image, model.ImageAtt)); 
                post.Content = htmlHelper.GetTextOnlyFromElement(doc, model.Content, true);
                post.Name = htmlHelper.GetTextOnlyFromElement(doc, model.Name, true);
                post.ShortContent = htmlHelper.GetTextOnlyFromElement(doc, model.ShortContent, true);
            }
            return post;
        }
    }
}