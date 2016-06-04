using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Blog.Domain;
using Blog.BLL;

namespace Blog.Web.Controllers
{
    public class ImportHelperController : Controller
    {
        //
        // GET: /ImportHelper/

        public ActionResult Index()
        {
            return View();
        }

        public string btnOK(string userName,string BlogUserName ="admin", string iszf = "false")
        {
            try 
            {
                return Import(userName,BlogUserName,iszf);
            }catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public string Import(string userName,string BlogUserName ="admin", string iszf = "false")
        {
            int blosNumber = 0;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string url = "http://www.cnblogs.com/" + userName + @"/mvc/blog/sidecolumn.aspx";
            HtmlAgilityPack.HtmlWeb htmlweb = new HtmlAgilityPack.HtmlWeb();
            var docment = htmlweb.Load(url);
            string userid = GetCnblogUserId(userName);
            var liS = docment.DocumentNode.SelectNodes("//*[@id='sidebar_categories']/div[1]/ul/li");
            //if (liS != null)
            //    foreach (var item in liS)
            //    {
            //        var tXPath = item.XPath;
            //        var href = item.SelectSingleNode(tXPath + "/a").Attributes["href"].Value;
            //        var blogtype = htmlweb.Load(href);
            //        var entrylistItem = blogtype.DocumentNode.SelectNodes("//div[@class='entrylistItem']");
            //        if (null == entrylistItem)//做兼容
            //            entrylistItem = blogtype.DocumentNode.SelectNodes("//div[@class='post post-list-item']"); //    
            //        if (null == entrylistItem)
            //        {
            //            continue;
            //        }
            //        foreach (var typeitem in entrylistItem)
            //        {
            //            var typeitemXPath = typeitem.XPath;
            //            var typeitemhrefObj = typeitem.SelectSingleNode(typeitemXPath + "/div/a");
            //            if (null == typeitemhrefObj) //做兼容
            //                typeitemhrefObj = typeitem.SelectSingleNode(typeitemXPath + "/h2/a");
            //            ///////////////////////////////////
            //            var typeitemhref = typeitemhrefObj.Attributes["href"].Value;

            //            var bloghtml = htmlweb.Load(typeitemhref);

            //            var blogtimeobj = bloghtml.DocumentNode.SelectSingleNode("//*[@id='post-date']");
            //            DateTime blogtime;
            //            if (null != blogtimeobj)
            //                blogtime = Convert.ToDateTime(blogtimeobj.InnerText);
            //            else
            //                blogtime = DateTime.Now;
            //            if (bloghtml.DocumentNode.SelectSingleNode("//*[@id='cb_post_title_url']") == null) continue;
            //            var blogurl = "/" + BlogUserName + "/" + bloghtml.DocumentNode.SelectSingleNode("//*[@id='cb_post_title_url']").Attributes["href"].Value.Substring(typeitemhref.LastIndexOf('/') + 1, typeitemhref.LastIndexOf('.') - typeitemhref.LastIndexOf('/') - 1);
            //            if (IsAreBlog(blogurl))
            //                continue;
            //            var blogcontextobj = bloghtml.DocumentNode.SelectSingleNode("//*[@id='cnblogs_post_body']");//.InnerHtml;
            //            if (blogcontextobj == null) continue;//有可能是加密文章
            //            var blogcontext = blogcontextobj.InnerHtml;

            //            var blogNameObj = bloghtml.DocumentNode.SelectSingleNode("//*[@id='Header1_HeaderTitle']");
            //            if (null == blogNameObj)
            //                blogNameObj = bloghtml.DocumentNode.SelectSingleNode("//*[@id='lnkBlogTitle']");
            //            string blogName = string.Empty;
            //            try
            //            {
            //                blogName = blogNameObj.InnerText;
            //            }
            //            catch (Exception)
            //            { }

            //            var blogtitle = bloghtml.DocumentNode.SelectSingleNode("//*[@id='cb_post_title_url']").InnerText;
            //            var blogtypetagurl = "http://www.cnblogs.com/mvc/blog/CategoriesTags.aspx?blogApp=" + userName + "&blogId=" + userid + "&postId=" +
            //                typeitemhref.Substring(typeitemhref.LastIndexOf('/') + 1, typeitemhref.LastIndexOf('.') - typeitemhref.LastIndexOf('/') - 1);
            //            var blogtag = Blog.Helper.MyHtmlHelper.GetRequest(blogtypetagurl);
            //            var jsonobj = jss.Deserialize<Dictionary<string, string>>(blogtag);

            //            List<int> blogtagid = null;
            //            List<int> blogtypeid = null;
            //            if (null == jsonobj)
            //            {//如果没有分类及标签信息 则添加到默认分类中
            //                blogtypeid = new List<int>();
            //                blogtypeid.Add(GetTypeId("未分类", BlogUserName));
            //                goto skip_typeidAndtagid;
            //            }
            //            var tagSplit = jsonobj["Tags"].Split(',');
            //            blogtagid = new List<int>();
            //            for (int i = 0; i < tagSplit.Length; i++)
            //            {
            //                if (tagSplit[i].Length >= 1 && tagSplit[i].LastIndexOf('<') >= 1)
            //                {
            //                    var blogtagname = tagSplit[i].Substring(tagSplit[i].IndexOf('>') + 1, tagSplit[i].LastIndexOf('<') - tagSplit[i].IndexOf('>') - 1);
            //                    blogtagid.Add(this.GetTagId(blogtagname, BlogUserName));
            //                }
            //            }

            //            var categoriesSplit = jsonobj["Categories"].Split(',');
            //            blogtypeid = new List<int>();
            //            for (int i = 0; i < categoriesSplit.Length; i++)
            //            {
            //                if (categoriesSplit[i].Length >= 1 && categoriesSplit[i].LastIndexOf('<') >= 1)
            //                {
            //                    var blogtypename = categoriesSplit[i].Substring(categoriesSplit[i].IndexOf('>') + 1, categoriesSplit[i].LastIndexOf('<') - categoriesSplit[i].IndexOf('>') - 1);
            //                    blogtypeid.Add(this.GetTypeId(blogtypename, BlogUserName));


            //                }
            //            }
            //        skip_typeidAndtagid:



            //            BlogsBLL blog = new BlogsBLL();
            //            List<BlogTags> myBlogTags = null;
            //            if (blogtagid != null)
            //                myBlogTags = new BlogTagsBLL().GetList(t => blogtagid.Contains(t.BlogTagId), isAsNoTracking: false).ToList();//.ToList();                                        
            //            var myBlogTypes = new BLL.BlogTypesBLL().GetList(t => blogtypeid.Contains(t.BlogTypeId)).ToList().FirstOrDefault();//.ToList();
            //            var blogUserId = GetUserId(BlogUserName);
            //            try
            //            {
            //                var modelMyBlogs = new Blogs()
            //                {
            //                    Content = blogcontext,
            //                    CreateTime = blogtime,
            //                    Title = blogtitle,
            //                    IsDel = false,
            //                    BlogTags = myBlogTags,
            //                    BlogTypeId = myBlogTypes.BlogTypeId,
            //                    UserId = blogUserId,
            //                    IsForwarding = iszf == "checked",
            //                    IsShowHome = true
            //                };
            //                blog.Add(modelMyBlogs);
            //                blog.save(false);
            //                blosNumber++;
            //            }
            //            catch (Exception ex)
            //            {
            //                return ex.ToString();
            //            }
            //        }
            //    }
            BlogsBLL blogBLL = new BlogsBLL();
            var blogsList = blogBLL.GetList(t => true, isAsNoTracking: false, selectDel: true).ToList();
            blogsList.ForEach(t => { t.BlogUrl = "/" + BlogUserName + "/" + t.BlogId + ".html"; t.BlogRemarks = Blog.Helper.MyHtmlHelper.GetHtmlText(t.Content); });
            blogBLL.save(false);
            if (blosNumber > 0)
                return "成功导入" + blosNumber + "篇Blog";
            return "ok";
        }

        #region 获取博客园真实userid
        /// <summary>
        /// 获取cnblog用户id
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetCnblogUserId(string url)
        {
            HtmlAgilityPack.HtmlWeb htmlweb = new HtmlAgilityPack.HtmlWeb();
            var docment = htmlweb.Load("http://www.cnblogs.com/" + url);
            var list = docment.DocumentNode.SelectNodes("//link[@rel='stylesheet']");
            foreach (var item in list)
            {
                if (null != item.Attributes && item.Attributes.Contains("href"))
                {
                    var href = item.Attributes["href"].Value;
                    href = href.Substring(href.LastIndexOf("/") + 1, href.IndexOf(".") - href.LastIndexOf("/") - 1);
                    int userid = -1;
                    if (int.TryParse(href, out userid))
                        return userid.ToString();
                }
            }
            return "";
        } 
        #endregion

        #region 文章是否已添加到数据库
        /// <summary>
        /// 检查 这个 url地址 是否被添加过
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsAreBlog(string url)
        {
            //BLL.BlogsBLL blog = new BLL.BlogsBLL();
            //var blogs = blog.GetList(t => t.BlogUrl == url);
            //return blogs.Count() >= 1;
            BlogsBLL blog = new BlogsBLL();
            var blogs = blog.GetList(t => t.BlogUrl == url);
            return blogs.Count() >= 1;
        } 
        #endregion

        #region 从数据库中获取给定用户名的用户id
        /// <summary>
        /// 若在数据库找到该用户名，则返回该用户id，否则创建该用户名用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private int GetUserId(string userName)
        {
            BlogTypesBLL type = new BlogTypesBLL();
            BlogUsersBLL user = new BlogUsersBLL();
            var blogtagmode = user.GetList(t => t.UserName == userName);
            if (blogtagmode.Count() >= 1)
                return blogtagmode.FirstOrDefault().UserId;
            else if(userName=="admin")
            {
                
                user.Add(new BlogUsers()
                {
                    UserName = userName,
                    IsDel = false,
                    UserPass = "123456",
                    UserNickname = "小mu虫",
                    Email="yxbyx@21cn.com"
                });
                user.save();
                if (type.GetList(t => t.TypeName == "未分类").Count() < 1)
                {
                    type.Add(new BlogTypes()
                    {
                        TypeName = "未分类",
                        UserId = user.GetList(t => t.UserName == userName).FirstOrDefault().UserId
                    });
                    type.save();
                }
                return GetUserId(userName);
            }
            else { Exception ex = new Exception( "在博客家园中没有该用户，请先新建该用户！"); throw ex; }
        } 
        #endregion

        #region 从数据库中获取给定博客所属类型名的类型对象id
        /// <summary>
        /// 由给定的博文类型名查找该类型对象ID，没有找到时就新建一个
        /// </summary>
        /// <param name="typename">博文类型名</param>
        /// <param name="userName">该博文所属用户的用户名</param>
        /// <returns></returns>
        private int GetTypeId(string typename, string userName)
        {
            BlogTypesBLL blogType = new BlogTypesBLL();
            var r = blogType.GetList(t => t.TypeName == typename);
            if (r.Count() >= 1)
            {
                int id=-1;
                r.ToList().ForEach(w =>
                {
                    if (w.UserId == GetUserId(userName)) id = w.BlogTypeId;
                });
                if (id != -1)
                    return id;
                else
                {
                    blogType.Add(new BlogTypes()
                    {
                        TypeName = typename,
                        UserId = GetUserId(userName),
  //                      BlogUsers=BLL.Common.GetDataHelper.GetUser(userName)
                    });
                    blogType.save(false);
                    return GetTypeId(typename, userName);
                }
            }
            else
            {
                blogType.Add(new BlogTypes()
                {
                    TypeName = typename,
                    UserId = GetUserId(userName),
    //                BlogUsers = BLL.Common.GetDataHelper.GetUser(userName)
                });
                blogType.save(false);
                return GetTypeId(typename, userName);
            }
        } 
        #endregion

        #region 从数据库中获取特定标签名的标签id
        /// <summary>
        /// 由给定的博文标签名查找该标签的ID，没有找到就新建一个
        /// </summary>
        /// <param name="tagname">标签名</param>
        /// <param name="userName">该标签所属用户的用户名</param>
        /// <returns></returns>
        private int GetTagId(string tagname, string userName)
        {
            BlogTagsBLL tagbll = new BlogTagsBLL();
            var r = tagbll.GetList(t => t.BlogTagName == tagname);
            if (r.Count() >= 1)
            {
                int id = -1;
                r.ToList().ForEach(w =>
                    {
                        if (w.UserId == GetUserId(userName)) id = w.BlogTagId;
                    });
                if (id != -1) return id;
                else
                {
                    tagbll.Add(new BlogTags()
                    {
                        BlogTagName = tagname,
                        UserId = GetUserId(userName)
                    });
                    tagbll.save();
                    return GetTagId(tagname, userName);
                }
            }
            else
            {
                tagbll.Add(new BlogTags()
                {
                    BlogTagName = tagname,
                    UserId = GetUserId(userName)
                });
                tagbll.save();
                return GetTagId(tagname, userName);
            }
        } 
        #endregion
    }
}
