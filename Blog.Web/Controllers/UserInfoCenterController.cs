using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Helper;
using Blog.BLL;
using System.IO;

namespace Blog.Web.Controllers
{
    public class UserInfoCenterController : Controller
    {
        //
        // GET: /UserInfoCenter/
        [HttpGet]
        public ActionResult Info(string name)
        {
            if(BLL.Common.BLLSession.UserInfoSessioin==null)
            {
                Response.Redirect("/Account/Login?href=/Center/default");
                return null;
            }
            if(name!=BLL.Common.BLLSession.UserInfoSessioin.UserName)
            {
                Response.Redirect("/Center/" + BLL.Common.BLLSession.UserInfoSessioin.UserName);
                return null;
            }

            return View(BLL.Common.BLLSession.UserInfoSessioin);
        }


        [HttpGet]
        public ActionResult Portrait(string name)
        {
            if (BLL.Common.BLLSession.UserInfoSessioin == null)
            {
                Response.Redirect("/Account/Login?href=/Center/default/Portrait");
                return null;
            }
            if (name != BLL.Common.BLLSession.UserInfoSessioin.UserName)
            {
                Response.Redirect("/Center/" + BLL.Common.BLLSession.UserInfoSessioin.UserName+"/Portrait");
                return null;
            }

            return View(BLL.Common.BLLSession.UserInfoSessioin);
        }


        [HttpPost]
        public ActionResult Portrait(FormCollection form)
        {
            if (Request.Files.Count == 0)
            {
　　　　　　//Request.Files.Count 文件数为0上传不成功
                Response.Write("<script>alert('上传文件出错！')</script>");
　　　　　　return null;　
　　　　　}

            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                Response.Write("<script>alert('文件上传大小为0！请检测文件或网络状况')</script>");
                return null;
            }
            else
            {
                if (BLL.Common.BLLSession.UserInfoSessioin == null)
                {
                    Response.Redirect("/Account/Login?href=/Center/default/Portrait");
                    
                }
                string path = Server.MapPath("/UserFile/" + BLL.Common.BLLSession.UserInfoSessioin.UserName + "/Portrait/");
                string relativePath = "/UserFile/" + BLL.Common.BLLSession.UserInfoSessioin.UserName + "/Portrait/";
                if (!Directory.Exists(path))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(path);
                }
                //文件大小不为0
                HttpPostedFileBase theFile = Request.Files[0];
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,　　　　
                theFile.SaveAs(path + newFile);
                BLL.Common.BLLSession.UserInfoSessioin.Portrait = relativePath + newFile;
                Response.Write("<script>alert('头像更改成功！')</script>");
            }
            new BlogUsersBLL().Mod(BLL.Common.BLLSession.UserInfoSessioin, "Portrait");
            BLL.DataCache.GetUsersInfo(true);
           return Portrait(BLL.Common.BLLSession.UserInfoSessioin.UserName);

        }

        [HttpGet]
        public ActionResult Password(string name)
        {
            if (BLL.Common.BLLSession.UserInfoSessioin == null)
            {
                Response.Redirect("/Account/Login?href=/Center/default/Password");
                return null;
            }
            if (name != BLL.Common.BLLSession.UserInfoSessioin.UserName)
            {
                Response.Redirect("/Center/" + BLL.Common.BLLSession.UserInfoSessioin.UserName + "/Password");
                return null;
            }

            return View(BLL.Common.BLLSession.UserInfoSessioin);
        }
        [HttpPost]
        public String Password(string prepwd,string nowpwd)
        {
            if (BLL.Common.BLLSession.UserInfoSessioin == null)
            {
                Response.Redirect("/Account/Login?href=/Center/default/Password");
                return null;
            }
            if(prepwd!=BLL.Common.BLLSession.UserInfoSessioin.UserPass)
            {
                return "原密码验证错误！";
            }
            try
            {
                BLL.Common.BLLSession.UserInfoSessioin.UserPass = nowpwd;
                new BlogUsersBLL().Mod(BLL.Common.BLLSession.UserInfoSessioin, "UserPass");
            }catch(Exception ex)
            {
                return ex.ToString();
            }
            return "修改成功！";
        }

    }
}
