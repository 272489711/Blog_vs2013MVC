using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Domain;
using Blog.BLL.Common;
using Blog.BLL;
using Blog.Helper;
namespace Blog.Web.Controllers
{
    public class ManageBlogController : Controller
    {
        //管理员用户名（具有特权）
        private static readonly string admin = "admin";



        #region 根据id修改文章或发布新文章模块&&获取编辑文章页面
        /// <summary>
        /// 根据id获取修改的文章也或获取新发布文章页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Release(int? id)
        {
            if (BLLSession.UserInfoSessioin == null)
            {
                Response.Redirect("/Account/Login?href=/ManageBlog/Release");
                return null;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("blogTag", GetDataHelper.GetAllTag(BLLSession.UserInfoSessioin.UserId).ToList());
            dic.Add("blogType", DataCache.GetAllType().Where(t => t.UserId == BLLSession.UserInfoSessioin.UserId).ToList());
            Blogs blog = null;
            if (null != id)
            {
                blog = new Blogs();
                BlogsBLL blogbll = new BlogsBLL();
                blog = blogbll.GetList(t => t.BlogId == id && (t.UserId == BLLSession.UserInfoSessioin.UserId || BLLSession.UserInfoSessioin.UserName == admin)).FirstOrDefault();
                if (blog == null) return View("Error");
            }
            dic.Add("blog", blog);
            return View(dic);
        }



        /// <summary>
        /// 修改或发布文章操作（返回JSData类型的json字符串）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public string Release()
        {
            JSData jsdata = new JSData();

            var content = Request.Form["content"];//正文内容
            var title = Request.Form["title"];//标题
            var oldtag = Request.Form["oldtag"];//旧的标签（从checkbox中选取的）
            var newtag = Request.Form["newtag"];//新的标签(在tag_text中输入的)
            var type = Request.Form["chk_type"];//文章类型
            var isshowhome = Request.Form["isshowhome"];//是否显示在主页
            var blogid = Request.Form["blogid"];//

            int numblogid = -1;
            int.TryParse(blogid, out numblogid);

            #region 数据验证
            if (null == BLL.Common.BLLSession.UserInfoSessioin)
                jsdata.Message = "您还未登录~";
            /*else if (BLL.Common.BLLSession.UserInfoSessioin.IsLock)
                jsdata.Message = "您的账户未激活，暂只能评论。~";*/
            else if (string.IsNullOrEmpty(content))
                jsdata.Message = "内容不能为空~";
            else if (content.Length >= 300000)
                jsdata.Message = "发布内容过多~";
            else if (string.IsNullOrEmpty(title))
                jsdata.Message = "标题不能为空~";
            else if (title.Length >= 100)
                jsdata.Message = "标题过长~";

            if (!string.IsNullOrEmpty(jsdata.Message))
            {
                jsdata.State = EnumState.失败;
                return jsdata.ToJson();
            }
            #endregion

            BLL.BlogsBLL blogbll = new BLL.BlogsBLL();
            var blogtemp = blogbll.GetList(t => t.BlogId == numblogid, isAsNoTracking: false).FirstOrDefault();
            var userid = numblogid > 0 ? blogtemp.UserId : BLLSession.UserInfoSessioin.UserId;//如果numblogid大于〇证明 是编辑修改
            var sessionuserid = BLLSession.UserInfoSessioin.UserId;

            //获取得 文章 类型
            BlogTypes myBlogType;
            if (type == null)
            {
                myBlogType = new BlogTypesBLL().GetList(t =>(t.TypeName == "未分类"&&t.UserId==BLLSession.UserInfoSessioin.UserId), isAsNoTracking: false).FirstOrDefault();
            }
            else
            {
                var blogtype = int.Parse(type);
                myBlogType = new BLL.BlogTypesBLL().GetList(t => t.BlogTypeId == blogtype, isAsNoTracking: false).ToList().FirstOrDefault();
            }
            //获取得 文章 tag标签集合 对象
            //old
            var oldtaglist = oldtag.Split(',').ToList();
            var myOldTag = new BLL.BlogTagsBLL().GetList(t => t.UserId == userid && oldtaglist.Contains(t.BlogTagName), isAsNoTracking: false).ToList();
            //new           
            var newtaglistname = newtag.Split(',').ToList();

            //保存newtags到数据库
            BlogTagsBLL tagBLL = new BlogTagsBLL();
            List<string> distinctTemp = new List<string>();
            foreach (string tagName in newtaglistname)
            {
                if (String.IsNullOrEmpty(tagName.Trim())) continue;
                if (distinctTemp.Contains(tagName.Trim())) continue;
                tagBLL.Add(new BlogTags
                {
                    BlogTagName = tagName.Trim(),
                    UserId = userid
                });
                try { tagBLL.save(); }
                catch (Exception ex)
                {
                    jsdata.Message = ex.ToString();
                    jsdata.State = EnumState.失败;
                    return jsdata.ToJson();
                }
                distinctTemp.Add(tagName.Trim());
                BLL.DataCache.GetAllTag(true);
            }
            //////////////////////

            var myTags = new BLL.BlogTagsBLL().GetList(t => t.UserId == userid && newtaglistname.Contains(t.BlogTagName), isAsNoTracking: false).ToList();
            myOldTag.ForEach(t => myTags.Add(t));



            //ModelDB.Blogs blogtemp = new ModelDB.Blogs();
            if (numblogid > 0)  //如果有 blogid 则修改
            {
                //blog = blogbll.GetList(t => t.Id == numblogid, isAsNoTracking: false).FirstOrDefault();
                if (sessionuserid == blogtemp.UserId || BLLSession.UserInfoSessioin.UserName == admin) //一定要验证更新的博客是否是登陆的用户
                {
                    blogtemp.Content = content;
                    blogtemp.BlogRemarks = MyHtmlHelper.GetHtmlText(content);
                    blogtemp.Title = title;
                    blogtemp.IsShowHome = isshowhome == "true";
                    blogtemp.BlogTypes = myBlogType;
                    blogtemp.BlogTags.Clear();//更新之前要清空      否则会存在主外键约束异常
                    blogtemp.BlogTags = myTags;
                    blogtemp.IsDel = false;
                    blogtemp.IsForwarding = false;
                    jsdata.Message = "修改成功~";
                }
                else
                {
                    jsdata.Message = "您没有编辑此博文的权限~";
                    jsdata.JSurl = "/";
                    jsdata.State = EnumState.失败;
                    return jsdata.ToJson();
                }
            }
            else  //否则是新发布
            {
                var blogfirst = blogbll.GetList(t => t.UserId == sessionuserid).OrderByDescending(t => t.BlogId).FirstOrDefault();
                //var blogtitle = blogtemp.BlogTitle;
                //if (blogfirst != null)
                //    blogtitle = blogtemp.BlogTitle;
                if (null != blogfirst && blogfirst.Title == title)
                {
                    jsdata.Message = "不能同时发表两篇一样标题的文章~";
                }
                else
                {
                    blogtemp = new Blogs()
                    {
                        UserId = sessionuserid,
                        Content = content,
                        BlogRemarks=MyHtmlHelper.GetHtmlText(content),
                        Title = title,
                        IsShowHome = isshowhome == "true",
                        BlogTypes = myBlogType,
                        BlogTags = myTags,
                        IsDel = false,
                        IsForwarding = false
                    };
                    blogbll.Add(blogtemp);
                    jsdata.Message = "发布成功~";
                }
            }

            //
            try
            {
                if (blogbll.save(false) > 0)
                {
                    blogtemp.BlogUrl = "/" + BLLSession.UserInfoSessioin.UserName + "/" + blogtemp.BlogId + ".html";
                    blogbll.save();
                    BLL.DataCache.GetAllType(true);
                    #region 添加 或 修改搜索索引

                    var newtagList = string.Empty;
                    blogtemp.BlogTags.Where(t => true).ToList().ForEach(t => newtagList += t.BlogTagName + " ");
                    var newblogurl = "/" + BLLSession.UserInfoSessioin.UserName + "/" + blogtemp.BlogId + ".html";
                    //    SearchResult search = new SearchResult()
                    //    {
                    //        flag = blogtemp.UsersId,
                    //        id = blogtemp.Id,
                    //        title = blogtemp.BlogTitle,
                    //        clickQuantity = 0,
                    //        blogTag = newtagList,
                    //        content = Blogs.Common.Helper.MyHtmlHelper.GetHtmlText(blogtemp.BlogContent),
                    //        url = newblogurl
                    //    };
                    //    SafetyWriteHelper<SearchResult>.logWrite(search, PanGuLuceneHelper.instance.CreateIndex);
                    //

                    #endregion
                    jsdata.State = EnumState.成功;
                    jsdata.JSurl = "/" + DataCache.GetUsersInfo().Where(t => t.UserId == blogtemp.UserId).First().UserName + "/" + blogtemp.BlogId + ".html";
                    return jsdata.ToJson();
                }
            }
            catch (Exception ex)
            {
                jsdata.State = EnumState.失败;
                jsdata.Message = ex.ToString();
                return jsdata.ToJson();
            }

            jsdata.State = EnumState.失败;
            return jsdata.ToJson();
        }
        
        #endregion

        #region 删除  删除 文章
        /// <summary>
        /// 删除 文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Del(int? id)
        {
            var userinfo = BLLSession.UserInfoSessioin;
            List<Blogs> blogs = new List<Blogs>();
            bool isdelok = false;
            if (null != id)
            {
                BLL.BlogsBLL blogbll = new BlogsBLL();
                BLL.BlogTagsBLL tagbll = new BlogTagsBLL();
                var delBlog = blogbll.GetList(t => t.BlogId == id,isAsNoTracking:false).FirstOrDefault();
                List<BlogTags> tagList = delBlog.BlogTags.ToList();
                
                

                
                //blogbll.Mod(new Blogs() { BlogId = (int)id,IsDel=true }, "IsDel");
                try
                {
                    foreach (var tag in tagList)
                    {
                        if (tag.Blogs.Count(t => !t.IsDel) <= 1)
                        {
                           isdelok= tagbll.Del(tag,isAsTracking:false);
                        }
                    }

                    blogbll.Del(delBlog,isAsTracking:false);
                    isdelok=blogbll.save()>0;
                    BLL.DataCache.GetAllType(true);
                }
                catch (Exception ex)
                {
                    isdelok = false;
                    return Content(ex.ToString());
                }
            }
            return Content((isdelok).ToString());
        }
        #endregion



        #region 新增文章类型
        /// <summary>
        /// 新增文章类型
        /// </summary>
        /// <param name="newtypename"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult NewAddType(string newtypename)
        {
            JSData jsdata = new JSData();

            #region 数据验证
            if (null == BLLSession.UserInfoSessioin)
                jsdata.Message = "您还未登录~";
            else if (string.IsNullOrEmpty(newtypename))
                jsdata.Message = "类型不能为空~";

            if (!string.IsNullOrEmpty(jsdata.Message))
            {
                jsdata.State = EnumState.失败;
                return Json(jsdata);
            }
            #endregion

            int userid = BLLSession.UserInfoSessioin.UserId;
            BLL.BlogTypesBLL bll = new BlogTypesBLL();
            bll.Add(
                new BlogTypes()
                {
                    TypeName = newtypename,
                    UserId = userid,
                    IsDel = false
                }
                );

            if (bll.save() > 0)//保存
            {
                BLL.DataCache.GetAllType(true);//更新缓存
                jsdata.State = EnumState.成功;
                jsdata.Message = "新增成功~";

            }
            else
            {
                jsdata.State = EnumState.失败;
                jsdata.Message = "新增失败~";
            }
            return Json(jsdata);
        }
        #endregion

        #region 删除文章类型（返回字符串true或错误信息）

        /// <summary>
        /// 删除文章类型
        /// </summary>
        /// <param name="id">要删除的类型id</param>
        /// <returns></returns>
        public String DeleteTypeById(int id)
        {
            bool result = false;
            BlogsBLL blogBLL = new BlogsBLL();
            BlogTypesBLL typeBLL = new BlogTypesBLL();
            var theTypeBlogs = blogBLL.GetList(t => t.BlogTypeId == id, isAsNoTracking: false).ToList();
            var defaultTypeID = typeBLL.GetList(t => t.TypeName == "未分类").FirstOrDefault().BlogTypeId;
            foreach (Blogs blog in theTypeBlogs)  //将属于要删除类型的博客分到 未分类 中
            {
                blog.BlogTypeId = defaultTypeID;
            }
            try
            {
                blogBLL.save();
                result = typeBLL.Del(t => t.BlogTypeId == id);
                BLL.DataCache.GetAllType(true);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return result.ToString();

        }
        
        #endregion

        #region 编辑文章类型(返回JSdata类型)
        /// <summary>
        /// 编辑文章类型
        /// </summary>
        /// <param name="typename"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public ActionResult EditType(string typename, int typeid)
        {
            JSData jsdata = new JSData();

            #region 数据验证
            if (null == BLLSession.UserInfoSessioin)
                jsdata.Message = "您还未登录~";
            else if (string.IsNullOrEmpty(typename))
                jsdata.Message = "类型不能为空~";
            if (!string.IsNullOrEmpty(jsdata.Message))
            {
                jsdata.State = EnumState.失败;
                return Json(jsdata);
            }
            #endregion

            BLL.BlogTypesBLL bll = new BlogTypesBLL();
            var blogtype = new BlogTypes()
            {
                BlogTypeId = typeid,
                TypeName = typename
            };
          

            if (bll.Mod(blogtype, "TypeName"))//保存
            {
                BLL.DataCache.GetAllType(true);//更新缓存
                jsdata.State = EnumState.成功;
                // jsdata.Messg = "修改成功~";
            }
            else
            {
                jsdata.State = EnumState.失败;
                jsdata.Message = "操作失败~";
            }
            return Json(jsdata);
        }
        #endregion



        #region 根据类型id获取所属类型文章列表（返回分部视图）
        /// <summary>
        /// 根据类型id获取所属类型文章列表（返回分部视图）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult EditBlogsByType(int id)
        {
            var sessionUser = BLLSession.UserInfoSessioin;
            if (sessionUser == null)
            {
                Response.Redirect("/Account/Login?href=/ManageBlog/Release");
                return null;
            }
            List<Blogs> blogList = new List<Blogs>();
            BlogsBLL blogBLL = new BlogsBLL();
            blogList = blogBLL.GetList(t => t.BlogTypes.BlogTypeId == id).ToList();
            return PartialView(blogList);
        }
        #endregion



        public PartialViewResult FlushTypeList(int? blogid)
        {
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("blogType", DataCache.GetAllType().Where(t => t.UserId == BLLSession.UserInfoSessioin.UserId).ToList());
            Blogs blog = null;
            if (null != blogid)
            {
                blog = new Blogs();
                BlogsBLL blogbll = new BlogsBLL();
                blog = blogbll.GetList(t => t.BlogId == blogid && (t.UserId == BLLSession.UserInfoSessioin.UserId || BLLSession.UserInfoSessioin.UserName == admin)).FirstOrDefault();
            }
            dic.Add("blog", blog);
            return PartialView(dic);

        }

    }
}
