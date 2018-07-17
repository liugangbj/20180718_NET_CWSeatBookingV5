using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinMsgService.Model
{
    public class WXAccess_token
    {
        private string _token;

        public string Token
        {
            get
            {
                return _token;
            }

            set
            {
                _token = value;
            }
        }

        private int _expires_in;

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }

 
    }
}
