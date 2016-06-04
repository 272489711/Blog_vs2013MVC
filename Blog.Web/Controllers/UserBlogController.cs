using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.BLL;


namespace Blog.Web.Controllers
{
    public class UserBlogController : Controller
    {
        static int pageSize = 15;

        //
        // GET: /UserBlog/

        #region 根据用户名以及博客id获取对应博客页面
        public ActionResult UserBlog(string name, int id)
        {
            return View(GetUserBlog(name, id));
        }

        public Dictionary<string, object> GetUserBlog(string name, int id)
        {
            BLL.BlogsBLL blogBLL = new BLL.BlogsBLL();
            var last = blogBLL.GetList(t => t.BlogId >= id && t.BlogUsers.UserName == name).OrderBy(t => t.BlogId).Take(2);
            var next = blogBLL.GetList(t => t.BlogId <= id && t.BlogUsers.UserName == name).OrderByDescending(t => t.BlogId).Take(2);
            var blogUni = (from c in last select c).Union(from a in next select a);
            var blogNext = blogUni.Where(t => t.BlogId > id).FirstOrDefault();
            var blogLast = blogUni.Where(t => t.BlogId < id).FirstOrDefault();
            var blogCurr = blogUni.Where(t => t.BlogId == id).FirstOrDefault();

            var blogContent = Helper.MyHtmlHelper.GetHtmlText(blogCurr.Content);
            blogContent = blogContent.Length > 300 ? blogContent.Substring(0, 300) : blogContent;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("blog", blogCurr);
            dictionary.Add("blogConText", blogContent);
            dictionary.Add("blogType", blogCurr.BlogTypes);//当前博文所属类型
            dictionary.Add("blogTags", blogCurr.BlogTags == null ? null : blogCurr.BlogTags.ToList());//当前博文所贴标签


            dictionary.Add("blogNext", blogNext);
            dictionary.Add("blogLast", blogLast);

            SetDic(dictionary, name);//设置其他字典数据
            

            #region 保存 标记 此文已经阅读过
            var BlogHasRead = "BlogHasRead";
            HttpCookie Cookie = Helper.CookieHelper.GetCookie(BlogHasRead);
            if (null == Cookie)
            {
                Cookie = new HttpCookie(BlogHasRead);
                Cookie.Values.Add(blogCurr.BlogId.ToString(), "true");
                //设置Cookie过期时间
                Cookie.Expires = DateTime.Now.AddHours(24);//一天 
                Helper.CookieHelper.AddCookie(Cookie);
                blogCurr.BlogReadNum++;
                new BlogsBLL().Mod(blogCurr, "BlogReadNum");
            }else
            {
                if(Cookie.Values[blogCurr.BlogId.ToString()]!="true")
                {
                    Cookie.Values.Add(blogCurr.BlogId.ToString(), "true");
                    //设置Cookie过期时间
                    Helper.CookieHelper.SetCookie(BlogHasRead, blogCurr.BlogId.ToString(), "true");
                    blogCurr.BlogReadNum++;
                    new BlogsBLL().Mod(blogCurr, "BlogReadNum");
                }
            }
            #endregion


            return dictionary;

        } 
        #endregion


        #region 获取用户博客主页（用户名，博客主页的页码），默认显示博主文章列表
        /// <summary>
        /// 获取博主首页
        /// </summary>
        /// <param name="name">博主用户名</param>
        /// <param name="id">博主首页博客列表当前页码（为空时为第一页）</param>
        /// <returns></returns>
        public ActionResult UserBlogHome(string name, int? id)
        {

            int myId = id ?? 1;
            int totalPage;
            BlogsBLL blogsBLL = new BlogsBLL();
            var blogsList = blogsBLL.GetList(myId, pageSize, out totalPage, t => t.BlogUsers.UserName == name, t => t.UpTime, false).ToList()     //linq 选择数据时候 不能new 已知的对象，只能匿名的。 但是如果从一个 List 列表 就可以new 已知的类。
                .Select(t => new Blog.Domain.Blogs
                {
                    BlogId = t.BlogId,
                    Title = t.Title,
                    Content = Blog.Helper.MyHtmlHelper.GetHtmlText(t.Content),
                    UpTime = t.UpTime,
                    CreateTime = t.CreateTime,
                    BlogComments = t.BlogComments,
                    BlogReadNum = t.BlogReadNum
                }).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("blogslist", blogsList);
            dic.Add("totalpage", totalPage);
            SetDic(dic, name);
            return View(dic);

        } 
        #endregion

        /// <summary>
        /// 根据博客类型或标签返回博文列表
        /// </summary>
        /// <param name="name">博客用户名</param>
        /// <param name="blogtypeid">博文类型id</param>
        /// /// <param name="typeortag">有路由器提供的参数判断是博客类型还是博客标签</param>
        /// <param name="id">页码id</param>
        /// <returns></returns>
        public ActionResult UserBlogHomeByTypeOrTag(string name,int typeortagid,string typeortag,int? id)
        {
            if (new BlogUsersBLL().GetList(t => t.UserName == name).Count() <= 0)
                return View("Error");
            int indexPage=id??1;
            int totalPage=1;
            Dictionary<string,object> dic = new Dictionary<string,object>();
            if (typeortag == "type")
            {
                int blogTypeId = typeortagid;
                
                var blogsList = new BlogsBLL().GetList(indexPage, pageSize, out totalPage, t => t.BlogTypes.BlogTypeId == blogTypeId, t => t.UpTime, false).ToList().Select(t => new Blog.Domain.Blogs
                {
                    BlogId = t.BlogId,
                    BlogReadNum = t.BlogReadNum,
                    BlogComments = t.BlogComments,
                    CreateTime = t.CreateTime,
                    Title=t.Title,
                    Content = Helper.MyHtmlHelper.GetHtmlText(t.Content)
                }).ToList();
                dic.Add("blogslist", blogsList);


                dic.Add("typeortagname","文章类型-"+ new BlogTypesBLL().GetList(t => t.BlogTypeId == blogTypeId).FirstOrDefault().TypeName);
           //     dic.Add("typeortag", "Type");
            }
            if(typeortag=="tag")
            {
                int blogTagId = typeortagid;
                var blogTag =  new BlogTagsBLL().GetList(t=>t.BlogTagId==blogTagId).FirstOrDefault();
                totalPage = blogTag.Blogs.Count() / pageSize;
                var blogsList =blogTag.Blogs.Skip((indexPage-1)*pageSize).Take(pageSize).ToList().Select(t => new Blog.Domain.Blogs
                {
                    BlogId = t.BlogId,
                    BlogReadNum = t.BlogReadNum,
                    BlogComments = t.BlogComments,
                    CreateTime = t.CreateTime,
                    Content = Helper.MyHtmlHelper.GetHtmlText(t.Content),
                    Title=t.Title
                }).ToList();
                dic.Add("blogslist", blogsList);


                dic.Add("typeortagname","文章标签-"+ new BlogTagsBLL().GetList(t => t.BlogTagId == blogTagId).FirstOrDefault().BlogTagName);
           //     dic.Add("typeortag", "Tag");

            }
            
            dic.Add("totalpage",totalPage );
            SetDic(dic, name);
            return View(dic);
        }


        #region 设置字典公共部分model数据(包括当前博文所属用户，该用户的所有博文类型及标签，当前登陆的用户（session）)
        /// <summary>
        /// 设置公共数据（包括当前博文所属用户，该用户的所有博文类型及标签，当前登陆的用户（session））
        /// </summary>
        /// <param name="dic">要填入的字典</param>
        /// <param name="name">当前博文用户名</param>
        private void SetDic(Dictionary<string, object> dic, string name)
        {
            var user = BLL.DataCache.GetUsersInfo().FirstOrDefault(t => t.UserName == name);
            dic.Add(BLL.Common.Constant.blogUser, user);
            dic.Add(BLL.Common.Constant.userBlogTag, BLL.Common.GetDataHelper.GetAllTag(user.UserId).ToList());
            dic.Add(BLL.Common.Constant.userBlogType, BLL.DataCache.GetAllType().Where(t => t.UserId == user.UserId).ToList());
            dic.Add(BLL.Common.Constant.SessionUser, BLL.Common.BLLSession.UserInfoSessioin);
        } 
        #endregion

    }
}
