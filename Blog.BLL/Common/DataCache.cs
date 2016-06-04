using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web;

namespace Blog.BLL
{
    public static class DataCache
    {
        /// <summary>
        /// 获得所有标签
        /// </summary>
        /// <param name="newCache">是否更新缓存并返回最新信息</param>
        /// <returns></returns>
        public static List<Blog.Domain.BlogTags> GetAllTag(bool newCache = false)
        {
            if (null == HttpRuntime.Cache["BlogTags"] || newCache)
            {
                BlogTagsBLL tagBll = new BlogTagsBLL();
                HttpRuntime.Cache["BlogTags"] = tagBll.GetList(t => true).ToList();
            }
            return HttpRuntime.Cache["BlogTags"] as List<Blog.Domain.BlogTags>;
        }
        /// <summary>
        /// 获得所有种类
        /// </summary>
        /// <param name="newCache">是否更新缓存并返回最新信息</param>
        /// <returns></returns>
        public static List<Blog.Domain.BlogTypes> GetAllType(bool newCache = false)
        {
            if(null == HttpRuntime.Cache["BlogTypes"]||newCache)
            {
                BlogTypesBLL typeBll = new BlogTypesBLL();
                HttpRuntime.Cache["BlogTypes"] = typeBll.GetList(t => true).ToList();
            }
            return HttpRuntime.Cache["BlogTypes"] as List<Blog.Domain.BlogTypes>;
        }
        /// <summary>
        /// 获得所有用户信息
        /// </summary>
        /// <param name="newCache">是否更新缓存并返回最新信息</param>
        /// <returns></returns>
        public static List<Blog.Domain.BlogUsers> GetUsersInfo(bool newCache = false)
        {
            if(null == HttpRuntime.Cache["UsersInfo"]||newCache)
            {
                BlogUsersBLL userBll = new BlogUsersBLL();
                HttpRuntime.Cache["UsersInfo"] = userBll.GetList(t => true).ToList();
            }
            return HttpRuntime.Cache["UsersInfo"] as List<Blog.Domain.BlogUsers>;
        }
    }
}
