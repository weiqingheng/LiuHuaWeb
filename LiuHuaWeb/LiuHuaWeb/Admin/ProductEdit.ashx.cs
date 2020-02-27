using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace LiuHuaWeb.Admin
{
    /// <summary>
    /// ProductEdit 的摘要说明
    /// </summary>
    public class ProductEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool isPostBack = !string.IsNullOrEmpty(context.Request["IsPostBack"]);
            string action = context.Request["Action"];
            if (isPostBack)
            {
               
                //todo 数据合法性检查（服务器端、客户端都要做）数据格式合法性；是否为空
                if(action=="AddNew")
                {
                    string name = context.Request["Name"];
                    long categoryId = Convert.ToInt64(context.Request["CategoryId"]);
                    //图片要保存到项目的文件下才可以通过web来访问图片                 
                    HttpPostedFile productImage = context.Request.Files["ProductImage"];
                    //MapPath 可以把一个相对网站根目录的文件或者文件夹路径
                    //转化成为服务器磁盘上的物理全路径
                    //string fff = context.Server.MapPath("~/uploadfile");
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(productImage.FileName);
                    productImage.SaveAs(context.Server.MapPath("~/uploadfile/" + filename));
                    string msg = context.Request["Msg"];

                    // 使用@就可以分行不错报错
//                  SqlHelper.ExecuteNonQuery(@"insert into T_Products(Name,ImagePath,Msg,CategoryId)
//                     values(@Name,ImagePath,Msg,CategoryId)");
                    SqlHelper.ExecuteNonQuery("insert into T_Products(Name,ImagePath,Msg,CategoryId) values(@Name,@ImagePath,@Msg,@CategoryId)"
                        , new SqlParameter("@Name", name)
                        ,new SqlParameter("@ImagePath", "/uploadfile/" + filename) 
                        , new SqlParameter("@Msg", msg)
                        , new SqlParameter("@CategoryId", categoryId));
                    context.Response.Redirect("ProductList.ashx");
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    string name = context.Request["Name"];
                    long categoryId = Convert.ToInt64(context.Request["CategoryId"]);
                    HttpPostedFile productImg = context.Request.Files["ProductImage"];
                    string msg = context.Request["Msg"];

                    if (CommonHelper.HasFile(productImg))
                    {
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(productImg.FileName);
                        productImg.SaveAs(context.Server.MapPath("~/uploadfile/" + filename));
                        SqlHelper.ExecuteNonQuery("update T_Products set Name=@Name,Msg=@Msg,CategoryId=@CategoryId,ImagePath=@ImagePath where Id=@Id"
                           , new SqlParameter("@Name", name)
                           , new SqlParameter("@Msg", msg)
                           , new SqlParameter("@CategoryId", categoryId)
                           , new SqlParameter("@ImagePath", "/uploadfile/" + filename)
                           , new SqlParameter("@Id", id));
                        context.Response.Redirect("ProductList.ashx");    
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery("update T_Products set Name=@Name,Msg=@Msg,CategoryId=@CategoryId where Id=@Id"
                           , new SqlParameter("@Name", name)
                           , new SqlParameter("@Msg", msg)
                           , new SqlParameter("@CategoryId", categoryId)
                           , new SqlParameter("@Id", id));
                        context.Response.Redirect("ProductList.ashx");
                    }
                }

            }
            else
            {
                DataTable categories = SqlHelper.ExecuteDataTable("select * from T_ProductCategories");
                     
                //string action = context.Request["Action"];
                if(action=="AddNew")
                {
                    var data = new { Title = "新增产品", Action = action, Product = new {ID=0,Name="",CategoriesId=0,Msg="" }
                        , Categories = categories.Rows };
                    string html=  CommonHelper.RenderHtml("Admin/ProductEdit.htm", data);
                    context.Response.Write(html);
                }
                else if (action=="Edit")
                {
                    long id= Convert.ToInt64(context.Request["Id"]);
                    DataTable products = SqlHelper.ExecuteDataTable("select * from T_Products where Id=@Id",new SqlParameter("Id",id));
                    if(products.Rows.Count<=0)
                    {
                        context.Response.Write("找不到Id="+id+"的产品");
                    }
                    else if(products.Rows.Count>1)
                    {
                         context.Response.Write("找到多条Id="+id+"的产品");
                    }
                    else
                    {
                        DataRow row = products.Rows[0];
                          var data = new
                        {
                            Title = "编辑产品",
                            Action = action,
                            Product = row, Categories = categories.Rows
                        };
                        string html = CommonHelper.RenderHtml("Admin/ProductEdit.htm", data);
                        context.Response.Write(html);
                    }
                  
                }
                else
                {
                    context.Response.Write("Action错误！");
                }


            }
         
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