using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace LiuHuaWeb.Admin
{
    /// <summary>
    /// ProductList 的摘要说明
    /// </summary>
    public class ProductList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            DataTable products = SqlHelper.ExecuteDataTable("select p.Id as Id,p.Name as Name,c.Name as CategoryName from T_Products p left join T_ProductCategories c on p.CategoryId=c.Id");
            var data = new { Title="产品列表", Products = products.Rows };
            string html = CommonHelper.RenderHtml("Admin/ProductList.htm", data);
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