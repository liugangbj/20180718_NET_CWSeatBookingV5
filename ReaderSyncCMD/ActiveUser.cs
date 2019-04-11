using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderSyncCMD
{
    class ActiveUser
    {
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
         void ExecuteSqlTran(List<string> SQLStringList,string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }

        public int Active()
        {
            int result =0;
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionToDB"]))
                {
                    string sql = "select * from [dbo].[T_SM_Reader] where CardNo not in (select LoginID from Users_ALL)";
                    SqlDataAdapter adapt = new SqlDataAdapter(sql, conn);
                    adapt.Fill(dt);
                }

                List<string> sqlInsertUsers = new List<string>();
                List<string> sqlRoles = new List<string>();
                foreach (DataRow r in dt.Rows)
                {
                    //ReaderProName为汇文密码字段
                    string sqlUser = "insert into [Users_ALL](LoginID,UsrName,UsrPwd,UsrType,UsrEnabled,Remark,IPLockIPAdress) values('" + r["CardNo"] + "','" + r["ReaderName"] + "','"+r["ReaderProName"] +"',1,1,'系统自动激活','')";
                    sqlInsertUsers.Add(sqlUser);

                    string sqlRole = "insert into sysEmpRoles(RoleID,LoginID) values(3,'" + r["CardNo"] + "')";
                    sqlRoles.Add(sqlRole);
                }

                ExecuteSqlTran(sqlInsertUsers, ConfigurationManager.AppSettings["ConnectionToDB"]);
                ExecuteSqlTran(sqlRoles, ConfigurationManager.AppSettings["ConnectionToDB"]);
                result = sqlInsertUsers.Count;

            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                throw;
            }
            return result;

        }
    }
}
