using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace System
{
    public static class ExtentionClass
    {
        #region 把 字符串集合 合并到一个字符串中 用逗号分割
        /// <summary>
        /// 把 字符串集合 合并到一个字符串中 用逗号分割
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string MyToString(this List<string> t)
        {
            string str = string.Empty;
            for (int i = 0; i < t.Count; i++)
            {
                if (t.Count > 1 && i < t.Count - 1)
                {
                    str += t[i] + ",";
                }
                else
                    str += t[i];
            }
            return str;
        }
        #endregion

        public static string ToJson(this object myclass)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(myclass);
        }   
    }
}
