using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuangXiCaiJingReaderSyncCMD
{
    class GetReaderSource
    {
        /// <summary>
        /// 获取原始表数据
        /// </summary>
        /// <param name="connstr"></param>
        /// <returns></returns>
        public DataTable GetReaderInfo(string connstr)
        {
            SqlConnection conn = new SqlConnection(connstr);
            string cmdstr = System.Configuration.ConfigurationManager.AppSettings["sqlFrom"];
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmdstr, conn);
                adapt.Fill(ds);
                conn.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// 将原始表数据转换为目标表数据架构
        /// </summary>
        /// <returns></returns>
        public DataTable GetReaderList()
        {
            DataTable readerFrom = GetReaderInfo(System.Configuration.ConfigurationManager.AppSettings["ConnectionFromDB"]);
            DataTable dt = new DataTable();
            dt.Columns.Add("CardNo");
            dt.Columns.Add("CardID");
            dt.Columns.Add("ReaderName");
            dt.Columns.Add("Sex");
            dt.Columns.Add("ReaderTypeName");
            dt.Columns.Add("ReaderDeptName");
            dt.Columns.Add("ReaderProName");
            dt.Columns.Add("Flag");
            foreach (DataRow dr in readerFrom.Rows)
            {
                DataRow ndr = dt.NewRow();
                ndr["CardNo"] = dr["OUTID"];
                ndr["CardID"] = dr["SCARDSNR"];
                ndr["ReaderName"] = dr["NAME"];
                ndr["Sex"] = "";
                ndr["ReaderTypeName"] = "";
                ndr["ReaderDeptName"] = "";
                ndr["ReaderProName"] = "";
                ndr["Flag"] = "";
                if (string.IsNullOrEmpty(ndr["CardNo"].ToString()))
                {
                    continue;
                }
                dt.Rows.Add(ndr);
            }
            return dt;
        }

    }
}
