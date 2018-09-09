using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using WeiXinMsgService.Model;
using SendHelp = Senparc.Weixin.CommonAPIs.CommonJsonSend;

namespace WeiXinMsgService
{
  public  class CommonTools
    {
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public bool CheckSignature(string signature, string timestamp, string nonce)
        {
            List<string> l = new List<string>();
            l.Add(timestamp);
            l.Add(WeiXinJKPram.TOKEN);
            l.Add(nonce);
            l.Sort();
            StringBuilder tmpStr = new StringBuilder();
            for (int i = 0; i < l.Count; i++)
            {
                tmpStr.Append(l[i]);
            }
            SHA1 sha = new SHA1CryptoServiceProvider();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(tmpStr.ToString());//Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);//将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "").ToLower();
            return hash.Equals(signature);
        }

        public string IsExistAccess_TokenV2()
        {
            string Token = string.Empty;
            string filepath = System.Web.HttpContext.Current.Server.MapPath("XMLFile.xml");
            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            return Token;
        }

        /// <summary>
        /// 获取Access_token值
        /// </summary>
        /// <returns></returns>
        public string IsExistAccess_Token(string appid, string secret)
        {
            string Token = string.Empty;

            try
            {
                DateTime YouXRQ;

                // 读取XML文件中的数据，并显示出来 ，注意文件路径  
                string filepath = System.Web.HttpContext.Current.Server.MapPath("XMLFile.xml");
                StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
                XmlDocument xml = new XmlDocument();
                xml.Load(str);
                str.Close();
                str.Dispose();
                Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
                YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);
                if (DateTime.Now > YouXRQ)
                {
                    DateTime _youxrq = DateTime.Now;
                    WXAccess_token mode = GetAccess_token(appid, secret);
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.Token;
                    _youxrq = _youxrq.AddSeconds(mode.expires_in);
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                    xml.Save(filepath);
                    Token = mode.Token;
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                throw;
            }

            return Token;
        }

        /// <summary>
        /// 通过appID和appsecret获取Access_token
        /// </summary>
        /// <returns></returns>
        private WXAccess_token GetAccess_token(string appid, string secret)
        {
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
            WXAccess_token mode = new WXAccess_token();
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                req.Method = "GET";

                using (WebResponse wr = req.GetResponse())
                {

                    HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();//在这里对Access_token 赋值  
                    SeatManage.SeatManageComm.WriteLog.Write(content);

                    //mode = Newtonsoft.Json.JsonConvert.DeserializeObject<WXAccess_token>(content);
                    content = content.Replace("{", "").Replace("}", "").Replace(",", ":").Replace("\"", ""); ;

                    string[] ar = content.Split(':');
                    mode.Token = ar[1].ToString();
                    //SeatManage.SeatManageComm.WriteLog.Write("new content:" + content);
                    mode.expires_in = int.Parse(ar[3].ToString());
                    SeatManage.SeatManageComm.WriteLog.Write("" + mode.Token + "&&" + mode.expires_in + "");
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                throw;
            }
            return mode;
        }

        public OpenApiResult SendTemplateMessage(string token, TemplateModel model)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", token);
            try
            {
                var res = SendHelp.Send<OpenApiResult>(token, url, model);
                return res;
            }
            catch (Exception e)
            {
                return new OpenApiResult() { error_code = -1, error_msg = e.Message };
            }

        }
    }
}
