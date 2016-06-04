using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.BLL;
using Blog.Helper;
using Blog.Domain;
using System.Globalization;

namespace Blog.Web.Controllers
{
    public class SearchController : Controller
    {
        private int pageSize = 10;
        //
        // GET: /Search/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult SearchBlogResult()
        {
            if (!Request.QueryString.AllKeys.Contains("keyword"))
                return null;
            string keyword = Request.QueryString["keyword"].Trim();
            if (String.IsNullOrEmpty(keyword))
                return null;
            int type = int.Parse (Request.QueryString["type"]);
            if (type == 1)
            {
                BlogsBLL blogBLL = new BlogsBLL();
                string pIndex = Request.QueryString.AllKeys.Contains("page") ? Request.QueryString["page"] : "";
                int PageIndex = 1;
                int.TryParse(pIndex, out PageIndex);
                int totalPage = 1;
                int totalCount = blogBLL.GetList(t => t.Title.Contains(keyword)).Count();
                var searchlist = blogBLL.GetList(PageIndex, pageSize, out totalPage, t => t.Title.Contains(keyword), t => t.CreateTime).ToList().Select(t => new Blogs
                {
                    Title = t.Title,
                    Content = Helper.MyHtmlHelper.GetHtmlText(t.Content).Count() > 300 ? Helper.MyHtmlHelper.GetHtmlText(t.Content).Substring(0, 300) : Helper.MyHtmlHelper.GetHtmlText(t.Content),
                    CreateTime = t.CreateTime,
                    BlogUrl = t.BlogUrl,
                    BlogUsers = t.BlogUsers
                }).ToList();
                CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                searchlist.ForEach(t =>
                {

                    t.Title = t.Title.Insert(compareInfo.IndexOf(t.Title, keyword, CompareOptions.IgnoreCase), "<strong>");
                    t.Title = t.Title.Insert(compareInfo.IndexOf(t.Title, keyword, CompareOptions.IgnoreCase) + keyword.Count(), "</strong>");
                });
                Dictionary<string, object> dicResult = new Dictionary<string, object>();
                dicResult.Add("blogslist", searchlist);
                dicResult.Add("totalpage", totalPage);
                dicResult.Add("count", totalCount);
                return PartialView(dicResult);
            }
            else if(type ==2)
            {
                BlogsBLL blogBLL = new BlogsBLL();
                CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                string pIndex = Request.QueryString.AllKeys.Contains("page") ? Request.QueryString["page"] : "";
                int PageIndex = 1;
                int.TryParse(pIndex, out PageIndex);
                int totalPage = 1;
                int totalCount = blogBLL.GetList(t => t.BlogRemarks.Contains(keyword)).Count();
                var searchlist = blogBLL.GetList(PageIndex, pageSize, out totalPage, t => (t.BlogRemarks).Contains(keyword), t => t.CreateTime, isAsc: false).ToList().Select(t => new Blogs
                {
                    Title = t.Title,
                    Content = t.BlogRemarks.Substring(compareInfo.IndexOf(t.BlogRemarks, keyword, CompareOptions.IgnoreCase) >= 10 ? compareInfo.IndexOf(t.BlogRemarks, keyword, CompareOptions.IgnoreCase) - 10 : compareInfo.IndexOf(t.BlogRemarks, keyword, CompareOptions.IgnoreCase)
                    , t.BlogRemarks.Length - compareInfo.IndexOf(t.BlogRemarks, keyword, CompareOptions.IgnoreCase) > 300 ? 300 : t.BlogRemarks.Length -compareInfo.IndexOf(t.BlogRemarks,keyword,CompareOptions.IgnoreCase)),
                    CreateTime = t.CreateTime,
                    BlogUrl = t.BlogUrl,
                    BlogUsers = t.BlogUsers
                }).ToList();
                searchlist.ForEach(t =>
                {

                    t.Content = t.Content.Insert(compareInfo.IndexOf(t.Content, keyword, CompareOptions.IgnoreCase), "<strong>");
                    t.Content = t.Content.Insert(compareInfo.IndexOf(t.Content, keyword, CompareOptions.IgnoreCase) + keyword.Count(), "</strong>");
                });
                Dictionary<string, object> dicResult = new Dictionary<string, object>();
                dicResult.Add("blogslist", searchlist);
                dicResult.Add("totalpage", totalPage);
                dicResult.Add("count", totalCount);
                return PartialView(dicResult);
            }
            else { return null; }
        }


    }
}
