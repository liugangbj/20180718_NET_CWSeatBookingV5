using DianziKejiDaXueYKTInterface.Model;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            try
            {
                if (context.Request.Url.ToString().Contains("modifyCard"))
                {
                    string cardJson = context.Request.Params[ConfigurationManager.AppSettings["ModifyKey"]].ToString();//"{'salaryno': '99900234','cardserialno': '3AC1435F','cardstatus': '0','validdate': '20251231','checkdata': '7CC79D4DD2AE505282EABEA071FC2610'}";
                    var jsonObj = JObject.Parse(cardJson);
                    T_SM_Reader reader = new T_SM_Reader();
                    reader.CardID = jsonObj["cardserialno"].ToString();
                    reader.CardNo = jsonObj["salaryno"].ToString();
                    reader.Attach();
                    int updateCount  = DbSession.Default.Update<T_SM_Reader>(reader);
                    if (updateCount > 0)
                    {
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { retcode = "0", retmsg = "成功修改1条数据" }));
                    }
                }
                else if (context.Request.Url.ToString().Contains("addPerson"))
                {
                    string personJson = context.Request.Params[ConfigurationManager.AppSettings["AddKey"]].ToString();// "[{ 'salaryno':'999002347','name':'张三','sex':'男','idno':'36212619970212023X','deptno':'电子科技大学/计算机学院/院办','status':'学生','picture':'','cardserialno':'2AC1235F','cardstatus':'0','validdate':'20251231','checkdata':'048CF228644883D730C632E83496F4FB'},{'salaryno':'99900235','name':'李四','sex':'女','idno':'36212619980212023X','deptno':'电子科技大学/计算机学院/院办','status':'教师','picture':'','cardserialno':'3AC1435F','cardstatus':'0','validdate':'20251231','checkdata':'C972B6C667180CEB17C924A387536E14'}]";
                    var jaList = JArray.Parse(personJson);

                    int addCount = 0;

                    foreach (JObject item in jaList)
                    {
                        T_SM_Reader reader = new T_SM_Reader();
                        JObject temp = JObject.Parse(item.ToString());
                        reader.CardID = temp["cardserialno"].ToString();
                        reader.CardNo = temp["salaryno"].ToString();
                        reader.ReaderName = temp["name"].ToString();
                        reader.Sex = temp["sex"].ToString();
                        reader.ReaderTypeName = temp["idno"].ToString();
                        reader.ReaderDeptName = temp["deptno"].ToString();
                        if (DbSession.Default.Count<T_SM_Reader>(" CardNo='" + reader.CardNo + "' ") > 0)
                        {
                            continue;
                        }
                        reader.Attach();
                        if (DbSession.Default.Insert<T_SM_Reader>(reader) > 0)
                        {
                            addCount++;
                        }
                    }
                    context.Response.Write(JsonConvert.SerializeObject(new { retcode = "0", retmsg = "成功添加"+ addCount + "条数据" }));
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                context.Response.Write(JsonConvert.SerializeObject(new { retcode = "1", retmsg = "异常:"+ex }));
            }

            //context.Response.Write("Hello World");
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