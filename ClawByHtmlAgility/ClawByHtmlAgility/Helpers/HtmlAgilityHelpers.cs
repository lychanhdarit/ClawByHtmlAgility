using ClawByHtmlAgility.Models;
using HtmlAgilityPack;
using System.Text;

namespace ClawByHtmlAgility.Helpers
{
    public class HtmlAgilityHelpers
    {
        private readonly HtmlWeb htmlWeb = new HtmlWeb();
        public HtmlAgilityHelpers()
        {

        }

        public HtmlDocument GetDocument(string url)
        {
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            return htmlWeb.Load(url);
        }
        public List<LinkCategoryModel> GetLinkAndTextFromElement(HtmlDocument doc, string tagAttribute)
        {
            try
            {
                List<LinkCategoryModel> content = new List<LinkCategoryModel>();
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    var nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            try
                            {
                                var attributes = item.Attributes;
                                var srcList = attributes.AttributesWithName("href");
                                string link = "";
                                foreach (var value in srcList)
                                {
                                    link = value.Value;
                                }
                                var text = item.InnerText;
                                if (link != "/" && link != "#")
                                {
                                    content.Add(new LinkCategoryModel() { Link = link, Text = text });
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<LinkCategoryModel> GetLinkAndTextAndImageFromElement(HtmlDocument doc, string tagAttribute)
        {
            try
            {
                List<LinkCategoryModel> content = new List<LinkCategoryModel>();
                if (!String.IsNullOrEmpty(tagAttribute))
                {

                    var nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            try
                            {
                                if (item != null)
                                {
                                    var attributes = item.Attributes;
                                    string link = item.Attributes["href"].Value;
                                    var text = item.InnerHtml;
                                    var html = item.InnerHtml;
                                    var linkSRC = "";
                                    if (html != null)
                                    {
                                        try
                                        {
                                            HtmlDocument docHtml = new HtmlDocument();
                                            docHtml.LoadHtml(html);
                                            if (docHtml != null)
                                            {
                                                var src = docHtml.DocumentNode.SelectSingleNode("//img").Attributes["src"];
                                                if (src != null)
                                                {
                                                    linkSRC = src.Value;
                                                }
                                                else
                                                {
                                                    src = docHtml.DocumentNode.SelectSingleNode("//img").Attributes["data-original"];
                                                    if (src != null)
                                                    {
                                                        linkSRC = src.Value;
                                                    }
                                                }

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message); continue;
                                        }
                                    }

                                    if (link != "/" && link != "#")
                                    {
                                        content.Add(new LinkCategoryModel() { Link = link, Text = text, Image = linkSRC });
                                    }
                                }

                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
         public List<string> GetLinkFromElement(HtmlDocument doc, string tagAttribute, string domain)
        {
            try
            {
                List<string> content = new List<string>();
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    var nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            var attributes = item.Attributes;
                            var srcList = attributes.AttributesWithName("href");
                            foreach (var value in srcList)
                            {
                                if (value.Value.Length > 0 && value.Value.IndexOf("javascript:;") == -1)
                                {
                                    if (((value.Value.IndexOf(domain) > -1 && value.Value.IndexOf("http") > -1) || (value.Value.IndexOf(domain) == -1 && value.Value.IndexOf("http") == -1)))
                                    {
                                        if (value.Value.IndexOf(domain) == -1)
                                        {
                                            if (value.Value.IndexOf("/") == 0)
                                            {
                                                content.Add(domain + value.Value);
                                            }

                                        }
                                        else
                                        {
                                            content.Add(value.Value);
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
                return content;
            }
            catch
            {
                return null;
            }
        }

        
        public string GetAllLinkFromElement(HtmlDocument doc, string tagAttribute)
        {
            try
            {
                string content = "";
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    var nodes = doc.DocumentNode.SelectNodes(tagAttribute);
                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            var attributes = item.Attributes;
                            var srcList = attributes.AttributesWithName("href");
                            foreach (var value in srcList)
                            {
                                content += value.Value + "\n";
                            }
                        }
                    }
                }
                return content;
            }
            catch
            {
                return null;
            }
        }
        
        public List<string> GetLinkImageFromElement(HtmlDocument doc, string tagAttribute)
        {
            try
            {
                List<string> content = new List<string>();
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    var nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            var attributes = item.Attributes;
                            var srcList = attributes.AttributesWithName("src");
                            foreach (var value in srcList)
                            {
                                content.Add(value.Value);
                            }
                        }
                    }
                    if (content.Count == 0)
                    {
                        tagAttribute = "//meta[@property='og:image:secure_url']";
                        nodes = doc.DocumentNode.SelectNodes(tagAttribute);
                        content = new List<string>();
                        if (nodes != null)
                        {
                            foreach (var item in nodes)
                            {
                                var attributes = item.Attributes;
                                var srcList = attributes.AttributesWithName("content");
                                foreach (var value in srcList)
                                {
                                    content.Add(value.Value);
                                }
                            }
                        }
                        if (content.Count == 0)
                        {
                            tagAttribute = "//meta[@property='og:image']";
                            nodes = doc.DocumentNode.SelectNodes(tagAttribute);
                            content = new List<string>();
                            if (nodes != null)
                            {
                                foreach (var item in nodes)
                                {
                                    var attributes = item.Attributes;
                                    var srcList = attributes.AttributesWithName("content");
                                    foreach (var value in srcList)
                                    {
                                        content.Add(value.Value);
                                    }
                                }
                            }
                        }
                    }
                }
                return content;
            }
            catch
            {
                return null;
            }
        }
        public string GetImageFromMeta(HtmlDocument doc)
        {
            try
            {
                string content = "";
                string tagAttribute = "//meta[@property='og:image:secure_url']";
                var nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                if (nodes != null)
                {
                    foreach (var item in nodes)
                    {
                        var attributes = item.Attributes;
                        var srcList = attributes.AttributesWithName("content");
                        foreach (var value in srcList)
                        {
                            content = value.Value.Trim();
                        }
                    }
                }
                if (String.IsNullOrEmpty(content))
                {
                    tagAttribute = "//meta[@property='og:image']";
                    nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        int first = 1;
                        foreach (var item in nodes)
                        {
                            if (first == 1)
                            {
                                var attributes = item.Attributes;
                                var srcList = attributes.AttributesWithName("content");
                                foreach (var value in srcList)
                                {
                                    content = value.Value.Trim();
                                }
                            }

                            first++;
                        }
                    }
                }

                return content;
            }
            catch
            {
                return null;
            }
        }
        public string GetTitle(HtmlDocument doc)
        {
            try
            {
                string content = "";
                string tagAttribute = "//title";
                var nodes = doc.DocumentNode.SelectNodes(tagAttribute);
                if (nodes != null)
                {
                    foreach (var item in nodes)
                    {
                        content = item.InnerText + " ";
                    }
                    content = content.Trim();
                }


                return content;
            }
            catch
            {
                return null;
            }
        }
        public string GetDescription(HtmlDocument doc)
        {
            try
            {
                string content = "";
                string tagAttribute = "//meta[@name='description']";
                var nodes = doc.DocumentNode.SelectNodes(tagAttribute);
                if (nodes != null)
                {
                    foreach (var item in nodes)
                    {
                        var attributes = item.Attributes;
                        var srcList = attributes.AttributesWithName("content");
                        foreach (var value in srcList)
                        {
                            content = value.Value + " ";
                        }
                    }
                }

                return content;
            }
            catch
            {
                return null;
            }
        }
        public string GetKeywords(HtmlDocument doc)
        {
            try
            {
                string content = "";
                string tagAttribute = "//meta[@name='keywords']";
                var nodes = doc.DocumentNode.SelectNodes(tagAttribute);
                if (nodes != null)
                {
                    foreach (var item in nodes)
                    {
                        var attributes = item.Attributes;
                        var srcList = attributes.AttributesWithName("content");
                        foreach (var value in srcList)
                        {
                            content = value.Value + " ";
                        }
                    }
                }

                return content;
            }
            catch
            {
                return null;
            }
        }
        public string GetDataFromElement(HtmlDocument doc, string tagAttribute, bool removeLink)
        {
            try
            {
                string content = "";
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            if (removeLink == true)
                            {
                                content += item.InnerHtml + Environment.NewLine;
                            }
                            else
                            {
                                content += item.InnerHtml + Environment.NewLine;
                            }

                        }
                    }
                }
                return content;
            }
            catch
            {
                return "";
            }
        }
        public string GetTextOnlyFromElement(HtmlDocument doc, string tagAttribute, bool removeLink)
        {
            try
            {
                string content = "";
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            if (removeLink == true)
                            {
                                content += item.InnerHtml + Environment.NewLine;
                            }
                            else
                            {
                                content += item.InnerText + Environment.NewLine;
                            }

                        }
                    }
                }
                return content;
            }
            catch
            {
                return "";
            }
        }

        public List<string> GetListHtmlFromElement(HtmlDocument doc, string tagAttribute)
        {
            try
            {
                var content = new List<string>();
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            content.Add(item.InnerHtml);

                        }
                    }
                }
                return content;
            }
            catch
            {
                return null;
            }
        }
        public List<string> GetImages(HtmlDocument doc, string tagAttribute,string att)
        {
            try
            {
                List<string> content = new List<string>();
                if (!String.IsNullOrEmpty(tagAttribute))
                {
                    var nodes = doc.DocumentNode.SelectNodes(tagAttribute);

                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            var attributes = item.Attributes;
                            var srcList = attributes.AttributesWithName(att);
                            foreach (var value in srcList)
                            {
                                content.Add(value.Value);
                            }
                        }
                    } 
                }
                return content;
            }
            catch
            {
                return null;
            }
        }

    }
}