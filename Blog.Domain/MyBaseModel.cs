using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public abstract class MyBaseModel
    {
        /// <summary>
        /// 通用模型中创建该模型的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 通用模型中更新该模型的时间
        /// </summary>
        public Nullable<System.DateTime> UpTime { get; set; }
        /// <summary>
        /// 通用模型中软删除标记
        /// </summary>
        public bool IsDel { get; set; }
    }
}
