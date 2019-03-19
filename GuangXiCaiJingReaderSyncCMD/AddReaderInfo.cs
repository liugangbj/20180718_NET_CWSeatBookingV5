using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuangXiCaiJingReaderSyncCMD
{
    class AddReaderInfo
    {
        /// <summary>
        /// 清空T_SM_Reader表
        /// </summary>
        /// <param name="connStr"></param>
        public void ClearDB(string connStr)
        {
            using (SqlConnection cn = new SqlConnection(connStr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("TRUNCATE table T_SM_Reader", cn);
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
            }
        }


        /// <summary>
        /// 读者信息添加到数据库
        /// </summary>
        /// <param name="readerDT"></param>
        public void AddNewData(DataTable readerDT, string connStr)
        {
            try
            {
                ClearDB(connStr);
                SqlBulkCopy sbc = new SqlBulkCopy(connStr);
                sbc.DestinationTableName = "[T_SM_Reader]";
                sbc.BatchSize = 100;
                sbc.NotifyAfter = 100;
                sbc.ColumnMappings.Add(0, 0);
                sbc.ColumnMappings.Add(1, 1);
                sbc.ColumnMappings.Add(2, 2);
                sbc.ColumnMappings.Add(3, 3);
                sbc.ColumnMappings.Add(4, 4);
                sbc.ColumnMappings.Add(5, 5);
                sbc.ColumnMappings.Add(6, 6);
                sbc.ColumnMappings.Add(7, 7);
                sbc.WriteToServer(readerDT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
