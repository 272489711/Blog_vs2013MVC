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
    
    [MetadataType(typeof(BlogTypesMeta))]  //metaTypeGenerate
    public partial class BlogTypes:MyBaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BlogTypes()
        {
            this.IsDel = false;
            this.Blogs = new HashSet<Blogs>();
        }
    
        public int BlogTypeId { get; set; }
        public string TypeName { get; set; }
        public System.DateTime CreateTime { get { return base.CreateTime; } set { base.CreateTime = value; } }
        public Nullable<System.DateTime> UpTime { get { return base.UpTime; } set { base.UpTime = value; } }
        public string TypeRemarks { get; set; }
        public bool IsDel { get { return base.IsDel; } set { base.IsDel = value; } }
        public int UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blogs> Blogs { get; set; }
        public virtual BlogUsers BlogUsers { get; set; }
    }
}