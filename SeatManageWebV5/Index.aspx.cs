using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeatManageWebV5
{
    public partial class Index : DefaultBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int a = 0;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                Response.Write(@"<script language='javascript'>alert('用户名和密码不能为空！'); </script> ");
                return;
            }
            try
            {
                //验证用户登录
                string loginID = txtUserName.Text;
                string Password = txtPassword.Text;

                //解密
                //loginID = AESDecrypt(loginID, "AjQ0YQ0MvKKC1uTr", "AjQ0YQ0MvKKC1uTr");
                //Password = AESDecrypt(Password, "AjQ0YQ0MvKKC1uTr", "AjQ0YQ0MvKKC1uTr");
                SeatManage.Bll.Users_ALL userinfocheck = new SeatManage.Bll.Users_ALL();
                loginID = userinfocheck.CheckUser(loginID, Password);
                //判断返回信息是否为空
                if (string.IsNullOrEmpty(loginID))
                {
                    Response.Write(@"<script language='javascript'>alert('用户或密码错误，请重新输入'); </script> ");
                }
                else
                {
                    this.LoginId = loginID;
                    Response.Redirect("Florms/FormSYS.aspx");
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                Response.Write(@"<script language='javascript'>alert('数据库连接出错！'); </script> ");
            }
        }
    }
}