using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region 博客园博文导入专用路由
            routes.MapRoute(
                name: "ImportHelper",
                url: "ImportHelper/btnOK/{userName}/{BlogUserName}/{iszf}",
                defaults: new { controller = "ImportHelper", action = "btnOK", userName = UrlParameter.Optional, BlogUserName = UrlParameter.Optional, iszf = UrlParameter.Optional }
                ); 
            #endregion
            //第一种情况：以 域名 + 用户名 + id + html 后缀组合（默认控制器）
            routes.MapRoute(
               name: "BlogPage",
               url: "{name}/{id}.html",
               defaults: new { action = "UserBlog", controller = "UserBlog" }
           );
            //第二种情况：以域名 + 用户名+Type/Tag+类型或者标签的id+当前页码id+html
            routes.MapRoute(
                name:"UserBlogsByType",
                url:"{name}/Type/{typeortagid}/{id}.html",
                defaults: new { action="UserBlogHomeByTypeOrTag",controller="UserBlog",typeortag="type"}
                );
            routes.MapRoute(
                name: "UserBlogsByTag",
                url: "{name}/Tag/{typeortagid}/{id}.html",
                defaults: new { action = "UserBlogHomeByTypeOrTag", controller = "UserBlog", typeortag = "tag" }
                );

            //以 域名+center+{action}
            routes.MapRoute(
             name: "Center",
             url: "Center/{name}/{action}",
             defaults: new { action = "Info", controller = "UserInfoCenter", name = UrlParameter.Optional }
          );
            //第三种情况：以 域名 + 用户名 + action  + id + html 后缀组合
            routes.MapRoute(
             name: "UserBlogHome",
             url: "{name}/{action}/{id}.html/{controller}",
             defaults: new { action = "UserBlogHome", controller = "UserBlog" }
          );
  
          
            routes.MapRoute(
            name: "ControllerAction",
            url: "{controller}/{action}/{id}",
            defaults: new { id = UrlParameter.Optional },
            namespaces: new string[1] { "Blog.Controllers" }); //到命名空间下 找控制器
      //);
            

            //以 域名 + 用户名
            routes.MapRoute(
             name: "UserHome",
             url: "{name}/{controller}/{action}/{id}",
             defaults: new { action = "UserBlogHome", controller = "UserBlog", id = UrlParameter.Optional }
          );
            //之输入 域名
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
                //, constraints: new { id = "[a-z]" }//约束
           );

        }
    }
}