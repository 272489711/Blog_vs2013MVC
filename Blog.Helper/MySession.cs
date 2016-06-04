using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Helper
{
    public class MySession
    {
        public static Blog.Domain.Model1Container dbEntities
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["dbEntities"] != null)
                    return HttpContext.Current.Session["dbEntities"] as Blog.Domain.Model1Container;
                else
                    return null;
            }
            set
            {
                //防止session不存在情况
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["dbEntities"] = value;
            }
        }

        

    }
}
