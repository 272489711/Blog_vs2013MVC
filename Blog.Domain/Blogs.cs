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
    
    [MetadataType(typeof(BlogsMeta))]  //metaTypeGenerate
    public partial class Blogs:MyBaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Blogs()
        {
            this.IsForwarding = false;
            this.BlogReadNum = 0;
            this.IsDel = false;
            this.BlogTags = new HashSet<BlogTags>();
            this.BlogComments = new HashSet<BlogComments>();
            this.ReadInfo = new HashSet<ReadInfo>();
        }
    
        public int BlogId { get; set; }
        public System.DateTime CreateTime { get { return base.CreateTime; } set { base.CreateTime = value; } }
        public Nullable<System.DateTime> UpTime { get { return base.UpTime; } set { base.UpTime = value; } }
        public string Title { get; set; }
        public string Content { get; set; }
        public string BlogUrl { get; set; }
        public Nullable<bool> IsForwarding { get; set; }
        public int BlogReadNum { get; set; }
        public string BlogRemarks { get; set; }
        public bool IsDel { get { return base.IsDel; } set { base.IsDel = value; } }
        public Nullable<bool> IsShowHome { get; set; }
        public int BlogTypeId { get; set; }
        public int UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogTags> BlogTags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogComments> BlogComments { get; set; }
        public virtual BlogTypes BlogTypes { get; set; }
        public virtual BlogUsers BlogUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReadInfo> ReadInfo { get; set; }
    }
}
