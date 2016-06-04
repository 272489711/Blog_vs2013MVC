using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Domain;
using Blog.BLL;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        static int pageSize = 12;
        //
        // GET: /Home/

        public ActionResult Index(int? page,int? sorttype)
        {
            BlogsBLL blogBll = new BlogsBLL();
            int total=0;
            int pageIndex = page ?? 1;
            int sortType = sorttype ?? 1;
            List<Blogs> bloglist=null;
            if (sortType == 2)
            {
                bloglist = blogBll.GetList(pageIndex, pageSize, out total, t => t.IsShowHome == true, t => t.CreateTime, false).ToList();
            }else if(sortType==4)
            {
                bloglist = blogBll.GetList(pageIndex, pageSize, out total, t => t.IsShowHome == true, t => t.CreateTime, false).ToList();
            }else if(sortType==3)
            {
                bloglist = blogBll.GetList(pageIndex, pageSize, out total, t => t.IsShowHome == true, t => t.BlogReadNum, false).ToList();
            }else
            {
                bloglist = blogBll.GetList(pageIndex, pageSize, out total, t => t.IsShowHome == true, t => t.UpTime, false).ToList();
            }
            bloglist.ForEach(t =>
            {
                t.BlogRemarks = Blog.Helper.MyHtmlHelper.GetHtmlText(t.Content);
            });
            Dictionary<string, Object> dic = new Dictionary<string, object>();
            dic.Add("blog", bloglist);
            dic.Add("users", BLL.DataCache.GetUsersInfo());
            dic.Add("userSession", BLL.Common.BLLSession.UserInfoSessioin);
            dic.Add("total", total);
            return View(dic);
        }
        public ActionResult BlogHead()
        {
            return PartialView(BLL.Common.BLLSession.UserInfoSessioin);
        }

    }
}
