using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    class BlogUsersMeta
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string UserPass { get; set; }     
               [Required(ErrorMessage = "邮箱不能为空")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "邮箱地址错误")]
        public string Email { get; set; }
        [Compare("UserPass",ErrorMessage="密码不一致")]
        public string TUserPass { get; set; }
    }
    partial class BlogUsers
    {
        public string TUserPass { set; get; }
    }
}
