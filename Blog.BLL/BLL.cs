
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Domain;

namespace Blog.BLL
{
    #region 01  BlogCommentsBLL
	public partial class BlogCommentsBLL : baseBLL<BlogComments>
    {               
    }
    #endregion

    #region 02  BlogsBLL
	public partial class BlogsBLL : baseBLL<Blogs>
    {               
    }
    #endregion

    #region 03  BlogTagsBLL
	public partial class BlogTagsBLL : baseBLL<BlogTags>
    {               
    }
    #endregion

    #region 04  BlogTypesBLL
	public partial class BlogTypesBLL : baseBLL<BlogTypes>
    {               
    }
    #endregion

    #region 05  BlogUsersBLL
	public partial class BlogUsersBLL : baseBLL<BlogUsers>
    {               
    }
    #endregion

}