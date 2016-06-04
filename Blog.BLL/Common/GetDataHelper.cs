using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.BLL.Common
{
    public class GetDataHelper
    {
        /// <summary>
        /// 获取用户所有文章类型
        /// </summary>
        /// <returns></returns>
        public static IQueryable<BlogTypes> GetAllType(string name)
        {
            BLL.BlogTypesBLL type = new BLL.BlogTypesBLL();
            return type.GetList(t => t.BlogUsers.UserName == name);
        }

        /// <summary>
        /// 获取用户所有文章标签
        /// </summary>
        /// <returns></returns>
        public static IQueryable<BlogTags> GetAllTag(string name)
        {
            BLL.BlogTagsBLL tag = new BLL.BlogTagsBLL();
            return tag.GetList(t => t.BlogUsers.UserName == name);
        }

        public static IQueryable<BlogTags> GetAllTag(int id)
        {
            BLL.BlogTagsBLL tag = new BLL.BlogTagsBLL();
            return tag.GetList(t => t.UserId == id);
        }

        public static IQueryable<BlogTags> GetAllTag()
        {
            BLL.BlogTagsBLL tag = new BLL.BlogTagsBLL();
            return tag.GetList(t => true);
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public static IQueryable<BlogUsers> GetAllUser()
        {
            BLL.BlogUsersBLL user = new BLL.BlogUsersBLL();
            return user.GetList(t => true);
        }



        /// <summary>
        /// 根据用户名  获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BlogUsers GetUser(string name)
        {
            BLL.BlogUsersBLL user = new BlogUsersBLL();
            return user.GetList(t => t.UserName == name).FirstOrDefault();
        }
    }
}
