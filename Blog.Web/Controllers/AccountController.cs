using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Domain;
using Blog.Helper;
using Blog.BLL.Common;
using Blog.BLL;
namespace Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        #region 用户登录
        [HttpGet]
        public ActionResult Login()
        {
            if (BLLSession.UserInfoSessioin != null)
                ViewBag.State = "isOn";
            HttpCookie cookie = CookieHelper.GetCookie("userInfo");
            if (cookie != null)
            {
                ViewBag.historyName = cookie.Values["userName"];
                ViewBag.historyPass = cookie.Values["userPass"];
                ViewBag.isCheck = "checked";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(BlogUsers user, bool RememberMe)
        {
            JSData json = new JSData();
            BLL.BlogUsersBLL userBLL = new BLL.BlogUsersBLL();
            BlogUsers userResult = userBLL.GetList(t => t.UserName == user.UserName).FirstOrDefault();
            if (userResult == null) //用户不存在
            {
                json.Message = "用户不存在！";
            }
            else if (userResult.UserPass == user.UserPass)     //登录成功
            {
                BLLSession.UserInfoSessioin = userResult;

                if (!string.IsNullOrEmpty(Request.QueryString["href"]))
                {
                    json.JSurl = Request.QueryString["href"];
                }
                else
                    json.JSurl = "/";
                if (RememberMe == true)
                {
                    HttpCookie cookie = CookieHelper.GetCookie("userInfo");
                    if (cookie == null)
                    {
                        cookie = new HttpCookie("userInfo");
                        cookie.Values.Add("userName", user.UserName);
                        cookie.Values.Add("userPass", user.UserPass);
                        cookie.Expires = DateTime.Now.AddMonths(6); //setting the valid time of the cookie  [6 months]
                        CookieHelper.AddCookie(cookie);
                    }
                    else
                    {

                        if (!cookie.Values["userName"].Equals(user.UserName))
                            CookieHelper.SetCookie("userInfo", "userName", user.UserName,DateTime.Now.AddMonths(6));
                        if (!cookie.Values["userPass"].Equals(user.UserPass))
                            CookieHelper.SetCookie("userInfo", "userPass", user.UserPass, DateTime.Now.AddMonths(6));

                    }
                }
                else
                {
                    CookieHelper.RemoveCookie("userInfo");
                }
            }
            else    //密码错误，登录失败
            {
                json.Message = "密码错误！";
            }
            return Json(json);

        } 
        #endregion

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(BlogUsers user)
        {
            JSData json = new JSData();
            BlogTypesBLL typeBLL = new BlogTypesBLL();
            BlogUsersBLL userBLL = new BlogUsersBLL();
            BlogUsers checkName = userBLL.GetList(t => t.UserName == user.UserName).FirstOrDefault();
             BlogUsers checkEmail = userBLL.GetList(t => t.Email == user.Email).FirstOrDefault();
            if(checkName!=null)
            {
                json.State = EnumState.失败;
                json.Message = "用户名已存在！";
            }
            else if(checkEmail!=null)
            {
                json.State = EnumState.失败;
                json.Message = "该邮箱已被注册！";
            }
            else
            {
                user.UserNickname = user.UserName;
                userBLL.Add(user);
                typeBLL.Add(new BlogTypes { TypeName = "未分类", UserId = user.UserId, BlogUsers = user });
                try
                {
                    userBLL.save();
                    typeBLL.save();
                }catch(Exception ex)
                {
                    throw ex;
                }
                BLL.DataCache.GetUsersInfo(true);//更新缓存
                BLLSession.UserInfoSessioin = user;
                json.State = EnumState.正常重定向;
                json.JSurl = "/";
                json.Message = "恭喜你，"+user.UserName+"注册成功！";
            }
            return Json(json);
        }
        public void Cancellation()
        {
            BLLSession.UserInfoSessioin = null;//注销
        }
    }
}
