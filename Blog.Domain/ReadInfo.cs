//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blog.Domain
{
        using System.ComponentModel.DataAnnotations; //引入DataMeta空间
    using System;
    using System.Collections.Generic;
    
    [MetadataType(typeof(ReadInfoMeta))]  //metaTypeGenerate
    public partial class ReadInfo:MyBaseModel
    {
        public int ReadInfoId { get; set; }
        public int UserId { get; set; }
        public int BlogId { get; set; }
        public string GoodOrBad { get; set; }
        public bool IsDel { get { return base.IsDel; } set { base.IsDel = value; } }
        public System.DateTime CreateTime { get { return base.CreateTime; } set { base.CreateTime = value; } }
        public Nullable<System.DateTime> UpTime { get { return base.UpTime; } set { base.UpTime = value; } }
    
        public virtual BlogUsers BlogUsers { get; set; }
        public virtual Blogs Blogs { get; set; }
    }
}
