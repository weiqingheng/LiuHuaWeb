using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiuHuaWeb
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string html = CommonHelper.RenderHtml("Front/Index.htm",null);
            context.Response.Write(html);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}