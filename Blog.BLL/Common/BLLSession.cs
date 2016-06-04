using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blog.BLL.Common
{
    public class BLLSession
    {
        #region 01 户信息Session
        /// <summary>
        /// 用户信息Session
        /// </summary>
        public static Blog.Domain.BlogUsers UserInfoSessioin
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["userinfo"] != null)
                    return HttpContext.Current.Session["userinfo"] as Blog.Domain.BlogUsers;
                else
                {
                    //HttpCookie Cookie =Helper.CookieHelper.GetCookie("userInfo");
                    //if (Cookie != null)
                    //{
                    //    var username = Cookie.Values["userName"];
                    //    var userpass = Cookie.Values["userPass"];
                    //    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userpass))
                    //    {

                    //        var pass = userpass.Trim();
                    //        var objuser =GetDataHelper.GetAllUser().Where(t => t.UserName == username && t.UserPass == pass).FirstOrDefault();

                    //        if (null != objuser)
                    //        {
                    //            HttpContext.Current.Session["userinfo"] = objuser;
                    //            return objuser;
                    //        }
                    //    }
                    //}
                    return null;
                }
            }
            set
            {
                    HttpContext.Current.Session["userinfo"] = value;
            }
        }
        #endregion
    }
}
