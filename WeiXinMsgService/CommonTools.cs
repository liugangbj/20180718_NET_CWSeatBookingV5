using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
    }
}
