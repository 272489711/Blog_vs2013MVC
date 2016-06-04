using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.BLL;
using Blog.Helper;

namespace Blog.BLL
{
    /// <summary>
    /// 评论数据 操作逻辑类
    /// </summary>
    public class CommentHandle
    {
        #region 根据博客文章id 取相关评论  ()
        /// <summary>
        /// 根据博客文章id 取相关评论  ()
        /// </summary>
        /// <param name="blogId"></param>
        public List<List<BlogComments>> GetComment(int blogId, int pageIndex)
        {

            int total;
            BlogCommentsBLL commentBLL = new BlogCommentsBLL();
            //IsReply == true 父评论 （第一次数据库查询：查询30条父评论）
            List<int> disCom = commentBLL.GetList(pageIndex, 30, out total, t => t.IsReply == true && t.BlogId == blogId,
                                t => t.BlogCommentId).Select(t => t.BlogCommentId).ToList();
            if (pageIndex > total)//已经没有评论信息了
            {
                return null;
            }
            //第二次数据库查询：查询30条父评论 和30条父评论下的子评论
            var listCom = commentBLL.GetList(t => disCom.Contains((t.ToCommentId==null)?-1:(int)t.ToCommentId) || disCom.Contains(t.BlogCommentId)).ToList();
            List<List<BlogComments>> ComObj = new List<List<BlogComments>>();
            var fatherComments = listCom.Where(t => t.IsReply == true).ToList();//这里就不查数据库了直接进行集合筛选
            //对评论进行分组（以父评论 分组）
            foreach (BlogComments item in fatherComments)
            {
                item.BlogUsers = DataCache.GetUsersInfo().Where(t => t.UserId == item.UserId).FirstOrDefault();
                var userobj = DataCache.GetUsersInfo().Where(t => t.UserId == item.ToUserId).FirstOrDefault();
                //if (null != userobj)
                //    item.ReplyUserName = string.IsNullOrEmpty(userobj.UserNickname) ? item.AnonymousName : userobj.UserNickname;
                //添加 以父评论 为一分组 的评论
                ComObj.Add(GetCom(item, listCom));
            }
            return ComObj;
        }
        #endregion

        #region 取 顶级评论 及下的子评论
        /// <summary>
        /// 取 顶级评论 及下的子评论
        /// </summary>
        /// <param name="com"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<BlogComments> GetCom(BlogComments com, List<BlogComments> list)
        {
            var li = list.Where(t => t.ToCommentId == com.BlogCommentId).ToList();
            li.Insert(0, com);
            return li;
        }
        #endregion
    }
}

