using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Domain;
using Blog.BLL;
using Blog.Helper;

namespace Blog.Web.Controllers
{
    public class CommentController : Controller
    {

        // 邮件配置
       

        #region 01写入评论内容
        /// <summary>
        /// 写入评论内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string WriteContent()
        {
            #region 评论前先检查是否已经登录
            var AnonymousName = string.Empty;//匿名登录
            if (Request.Form.AllKeys.Contains("AnonymousName") && !string.IsNullOrEmpty(Request.Form["AnonymousName"]))
            {
                AnonymousName = Request.Form["AnonymousName"];
            }
            else if (null ==BLL.Common. BLLSession.UserInfoSessioin)
            {
                return new Helper.JSData()
                {
                    Message = "您还未登录~",
                    State = EnumState.异常或Session超时
                }.ToJson();
            }
            var sessionUser = BLL.Common.BLLSession.UserInfoSessioin;

            //if (BLLSession.UserInfoSessioin.IsLock)
            //{
            //    return new JSData()
            //    {
            //        Messg = "您的账户已经被锁定,请联系管理员~",
            //        State = EnumState.失败
            //    }.ToJson();
            //}
            #endregion

            var BlogId = int.Parse(Request.Form["BlogId"]);
            var UserId = sessionUser.UserId == 0 ? 1 : sessionUser.UserId; //int.Parse(Request.Form["UserId"]);
            var CommentID = int.Parse(Request.Form["CommentID"]);
            var Content = Request.Form["Content"];
            var ReplyUserID = int.Parse(Request.Form["ReplyUser"]);

            if (Content.Length >= 1000)
            {
                return new JSData()
                {
                    State = EnumState.失败
                }.ToJson();
            }

            var ReplyUserName = string.Empty;
            var User = DataCache.GetUsersInfo().Where(t => t.UserId == ReplyUserID).FirstOrDefault();

            if (null != User)
            {
                ReplyUserName = string.IsNullOrEmpty(User.UserNickname) ? User.UserName : User.UserNickname;
            }

            BLL.BlogCommentsBLL comment = new BLL.BlogCommentsBLL();
            comment.Add(new BlogComments()
            {
                UserId = UserId,
                BlogId = BlogId,
                Content = Content,
                ToCommentId = CommentID,
                ToUserId = ReplyUserID,
                IsReply = CommentID == -1
            });

            //BLL.BlogsBLL blogbll = new BLL.BlogsBLL();
            //var blog = blogbll.GetList(t =>t.BlogId== BlogId).FirstOrDefault();
            //if (null == blog.BlogComments)
            //{
            //    blog.BlogCommentNum = comment.GetList(t => t.BlogsId == BlogId).Count() + 1;
            //}
            //else
            //{
            //    blog.BlogCommentNum++;
            //}
            //blogbll.Up(blog, "BlogCommentNum");

            comment.save(false);

            // 评论邮件通知


            return new JSData()
            {
                //这里发表成功    就不提示了。
                State = EnumState.成功
            }.ToJson();
        }
        #endregion

        #region 02加载 分部视图 （评论部分）
        /// <summary>
        /// 加载 分部视图 （评论部分）
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadComment()
        {
            int blogId = int.Parse(Request.Form["blogID"]);
            int pageIndex = int.Parse(Request.Form["pageIndex"]);
            BLL.CommentHandle com = new BLL.CommentHandle();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var comObj = com.GetComment(blogId, pageIndex);
            if (null == comObj)
                return PartialView("Null");
            dic.Add("commentList", comObj);//对应的评论
            dic.Add("SessionUser", BLL.Common.BLLSession.UserInfoSessioin);
            return PartialView(dic);
        }
        #endregion

        public ActionResult Message()
        {
            return View();
        }

    }
}
