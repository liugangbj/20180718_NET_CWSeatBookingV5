using DianziKejiDaXueYKTInterface.Model;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft;

namespace DianziKejiDaXueYKTInterface
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler, IHttpModule
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (context.Request.Url.ToString().Contains("modifyCard"))
            {

            }
            else if (context.Request.Url.ToString().Contains("addPerson"))
            {
                
                string json = context.Request.Params[ConfigurationManager.AppSettings["addParm"].ToString()].ToString();

                //解釋json 

                //foreach (var item in collection)
                //{


                //    T_SM_Reader reader = new T_SM_Reader();
                //   if(DbSession.Default.Count<T_SM_Reader>(" CardNo='"+ reader.CardNo+ "' ")>0)
                //    {
                        
                //    }

                  
                //    DbSession.Default.Insert<T_SM_Reader>(reader);
                //}
           
                



            }

            context.Response.Write("Hello World");
        }

        public void Init(HttpApplication context)
        {
        }

        public void Dispose()
        {
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