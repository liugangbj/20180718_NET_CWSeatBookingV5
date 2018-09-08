using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolPocketBookWeb.Code
{
    /// <summary>
    /// 电子科技大学第三方登陆请求返回用户结果
    /// </summary>
    public class DZKDJsonResultObj
    {
        private string uniNo;
        private string userName;

        public string UniNo
        {
            get
            {
                return uniNo;
            }

            set
            {
                uniNo = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }
    }
}