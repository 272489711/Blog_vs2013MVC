using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
namespace Blog.Helper
{
    public class MyHtmlHelper
    {

        /// <summary>
        /// 获取get请求返回的数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetRequest(string url)
        {
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = httpclient.GetAsync(new Uri(url)).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        ///获取 Post 请求 的返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="str_content"></param>
        /// <returns></returns>
        public static string PostReqest(string url, string str_content = "")
        {
            HttpClient httpclient = new HttpClient();
            try
            {
                StringContent fromurlcontent = new StringContent(str_content);
                fromurlcontent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = httpclient.PostAsync(new Uri(url), fromurlcontent).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 返回Html字符中的Text 类似jquery中.text()方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetHtmlText(string str)
        {
            var docment = new HtmlAgilityPack.HtmlDocument();
            docment.LoadHtml(str);
            return docment.DocumentNode.InnerText;
        }
   
    }
}
