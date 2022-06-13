using Microsoft.ApplicationBlocks.Data;
using Reprocess.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Data.Metrics
{
  public  class DAL
    {
        public static string GetDiMetricsConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DImetricsConnectionString"].ConnectionString;
        }
        public static string GetMainConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString;
        }
        public static string GetDiASCIIR9ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DiASCIIR9ConnectionString"].ConnectionString;
        }
        public static string GetDiASCIIR9RepConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DiASCIIR9_RepConnectionString"].ConnectionString;
       
        }
        public static string GetDiMetricsLogConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DiMetricsLogConnectionString"].ConnectionString;
        }
        public static  string GetIntegrationStatusInbox(string yearQuarter, string inboxID, string customerID)
        {
            string table = "inbox_Qr" + yearQuarter.Substring(yearQuarter.Length - 1, 1) + "_" + yearQuarter.Substring(0, 4);

            string commandText = "Select IntegrationStatus from " + "dbo." + table + " where inboxid=@inboxID and custid=@custID";

            DataSet ds;
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@inboxID", SqlDbType.Int);
            param[0].Value = inboxID;

            param[1] = new SqlParameter("@custID", SqlDbType.Int);
            param[1].Value = customerID;
            try
            {
                ds = SqlHelper.ExecuteDataset(Constants.MainConnectionString, CommandType.Text, commandText, param);
            }
            catch
            {
                throw;
            }

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["IntegrationStatus"].ToString();
            else
                return null;
        }
        public static  bool IsExistOnDiASCIIR9ByInbox(string inboxID, string docType, string yearQuarter)
        {
            string commandText = "select * from inbox where inboxid=@inboxID and YearQuarter=@yearQuarter";
            DataSet ds;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@inboxID", SqlDbType.Int);
            param[0].Value = inboxID;

            param[1] = new SqlParameter("@yearQuarter", SqlDbType.NVarChar);
            param[1].Value = yearQuarter;

            // viet , doc , han tu: 
            // tạp đọc het bai thay dua ,
            // han tu co linh, han tu thay dua, thuoc tu vung trong sach : chua lắm
            // 
            try
            {
                ds = SqlHelper.ExecuteDataset(Constants.DiASCIIR9ConnectionString, CommandType.Text, commandText, param);
            }
            catch
            {
                throw;
            }

            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }
        public static bool IsExistOnDiASCIIR9ByInbox_Chanel(string inboxID, string docType, string yearQuarter)
        {
            string commandText = "select * from outbox_tmp where outboxid=@inboxID and YearQuarter=@yearQuarter";
            DataSet ds;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@inboxID", SqlDbType.Int);
            param[0].Value = inboxID;

            param[1] = new SqlParameter("@yearQuarter", SqlDbType.NVarChar);
            param[1].Value = yearQuarter;

            try
            {
                ds = SqlHelper.ExecuteDataset(Constants.DiASCIIR9ConnectionString, CommandType.Text, commandText, param);
            }
            catch
            {
                throw;
            }

            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}
