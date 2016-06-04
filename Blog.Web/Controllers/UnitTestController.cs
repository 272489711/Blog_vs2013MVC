using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.BLL;
using Blog.Domain;

namespace Blog.Web.Controllers
{
    public class UnitTestController : Controller
    {
        //
        // GET: /UnitTest/

        public ActionResult Test()
        {
            BlogTypesBLL typeBLL = new BlogTypesBLL();
            typeBLL.Mod(new BlogTypes { TypeName = "", BlogTypeId = 1007 }, "TypeName");
            try
            {
                var result = typeBLL.save();
                return View(result.ToString());
            }catch(Exception ex)
            {
                return View(ex.ToString());
            }
            
        }

   

    }
}
