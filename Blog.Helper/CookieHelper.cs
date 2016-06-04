using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Helper
{
    public class CookieHelper
    {
        #region 获取cookie方法
        /// <summary>
        /// 获取指定名称的cookie值
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <returns>字符串值</returns>
        public static string GetCookieValue(string cookieName)
        {
            return GetCookieValue(cookieName, null);
        }
        /// <summary>
        /// 获取指定名称及对应子cookie关键字的cookie值
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="key">该名称下的子cookie关键字</param>
        /// <returns>字符串值</returns>
        public static string GetCookieValue(string cookieName, string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return GetCookieValue(request.Cookies[cookieName], key);
            else return "";
        }
        private static string GetCookieValue(HttpCookie cookie, string key)
        {
            if (cookie.HasKeys && !string.IsNullOrEmpty(key))
                return cookie.Values[key];
            else
                return cookie.Value;
        }



        /// <summary>
        /// 根据cookie名称获取cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns>cookie对象</returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return request.Cookies[cookieName];
            return null;
        }
        #endregion

        #region 删除cookie方法

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void RemoveCookie(string cookieName)
        {
            RemoveCookie(cookieName, null);
        }

        /// <summary>
        /// 删除Cookie的子键
        /// </summary>
        /// <param name="cookieName">要删除的cookie名称</param>
        /// <param name="key">对应cookie名称下的子cookie的关键值</param>
        public static void RemoveCookie(string cookieName, string key)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {

                HttpCookie cookie = GetCookie(cookieName);
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Remove(key);
                    else
                        cookie.Expires = DateTime.Now.AddDays(-3);
                    response.Cookies.Add(cookie);     //注意： 不管是移除 还是修改  这里一定要add 保存 不然无效。               
                }
            }
        }

        #endregion

        #region 设置/修改cookie方法

        /// <summary>
        /// 设置Cookie子键的值
        /// </summary>
        /// <param name="cookieName">要设置的cookie名称</param>
        /// <param name="key">对应cookie名称下的子cookie的关键字</param>
        /// <param name="value">要设置子cookie的值</param>
        public static void SetCookie(string cookieName, string key, string value)
        {
            SetCookie(cookieName, key, value, null);
        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="key">要设置的cookie名称</param>
        /// <param name="value">要设置cookie的值</param>
        public static void SetCookie(string cookieName, string value)
        {
            SetCookie(cookieName, null, value, null);
        }

        /// <summary>
        /// 设置Cookie值和过期时间
        /// </summary>
        /// <param name="key">要设置的cookie的名称</param>
        /// <param name="value">要设置cookie的值</param>
        /// <param name="expires">要设置cookie的过期时间</param>
        public static void SetCookie(string key, string value, DateTime expires)
        {
            SetCookie(key, null, value, expires);
        }

        /// <summary>
        /// 设置Cookie过期时间
        /// </summary>
        /// <param name="cookieName">要设置的cookie的名称</param>
        /// <param name="expires">要设置cookie的过期时间</param>
        public static void SetCookie(string cookieName, DateTime expires)
        {
            SetCookie(cookieName, null, null, expires);
        }

        /// <summary>
        /// 具体设置Cookie
        /// </summary>
        /// <param name="cookieName">要设置的cookie的名称</param>
        /// <param name="key">对应的cookie的名称下的子cookie的关键字</param>
        /// <param name="value">要设置子cookie的值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string cookieName, string key, string value, DateTime? expires)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = GetCookie(cookieName); 
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Set(key, value);
                    else
                        if (!string.IsNullOrEmpty(value))
                            cookie.Value = value;
                    if (expires != null)
                        cookie.Expires = expires.Value;
                    response.SetCookie(cookie);
                }
            }

        }

        #endregion

        #region 添加cookie方法

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="key">cookie名称</param>
        /// <param name="value">值</param>
        public static void AddCookie(string key, string value)
        {
            AddCookie(new HttpCookie(key, value));
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="key">cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void AddCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = expires;
            AddCookie(cookie);
        }

        /// <summary>
        /// 添加为Cookie.Values集合
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="key">改名称下子cookie的关键名称</param>
        /// <param name="value">值</param>
        public static void AddCookie(string cookieName, string key, string value)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Add(key, value);
            AddCookie(cookie);
        }

        /// <summary>
        /// 添加为Cookie集合
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="expires">过期时间</param>
        public static void AddCookie(string cookieName, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = expires;
            AddCookie(cookie);
        }

        /// <summary>
        /// 添加为Cookie.Values集合
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        public static void AddCookie(string cookieName, string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = expires;
            cookie.Values.Add(key, value);
            AddCookie(cookie);
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie">添加的cookie对象</param>
        public static void AddCookie(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                //指定客户端脚本是否可以访问[默认为false]
                cookie.HttpOnly = true;
                //指定统一的Path，以便能通存通取
                cookie.Path = "/";
                
                response.AppendCookie(cookie);
            }
        }

        #endregion
    }
}
