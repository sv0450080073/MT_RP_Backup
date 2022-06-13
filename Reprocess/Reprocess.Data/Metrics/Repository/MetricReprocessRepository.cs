using DImetrics.BLL.MetricManager;
using DImetrics.DAL.ImportEngine;

//using DImetrics.DAL.ImportEngine;

using DImetrics.Entity.Common;
using Microsoft.ApplicationBlocks.Data;
using Reprocess.Common;
using Reprocess.Data.Metrics.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Data.Metrics.Repository
{
    public class MetricReprocessRepository : IMetricReprocessRepository
    {
        internal class DiUniteServiceProxy : DiUnite.DiUniteManager.IServices.ServicesProxy, IDisposable
        {
            public DiUniteServiceProxy(string endpointConfigurationName)
                : base(endpointConfigurationName)
            {
            }

            /// <summary>
            /// Closes the proxy(or Abort if Faulted.)
            /// </summary>
            public void Dispose()
            {
                if (State == System.ServiceModel.CommunicationState.Faulted)
                {
                    Abort();
                }
                else
                {
                    try
                    {
                        Close();
                    }
                    catch
                    {
                        Abort();
                    }
                }
            }
        }
        #region TRACK FILE
        public string GetIntegrationStatusInbox(string yearQuarter, string inboxID, string customerID)
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

        public bool IsExistOnDiASCIIR9ByInbox(string inboxID, string docType, string yearQuarter)
        {

            string commandText = "select * from inbox where inboxid=@inboxID and YearQuarter=@yearQuarter";
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

        public bool IsExistOnDiASCIIR9ByInbox_Chanel(string inboxID, string docType, string yearQuarter)
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

        public bool IsExistOnDiMetrics(string transactionType, int inoutboxID, string yearQuarter, string hubid = "")
        {
            DataSet ds;
            SqlParameter[] param = null;
            string commandText = null;
            if (string.IsNullOrEmpty(hubid))
            {
                switch (transactionType)
                {
                    case "180":
                        commandText = "select * from DIM_180_RETURN where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "204":
                        commandText = "select * from DIM_204_LOADTENDER where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "812":
                        commandText = "select * from DIM_812_CDA where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "820":
                        commandText = "select * from DIM_820_PAYMENTORDER where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "824":
                        commandText = "select * from DIM_824_APPADVICE where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "832":
                        commandText = "select * from DIM_832_PRICESALECATALOG where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "852":
                        commandText = "select * from DIM_852_POS where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "861":
                        commandText = "select * from DIM_861_RAAC where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "940":
                        commandText = "select * from DIM_940_DELIVERYREQUEST where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "997":
                        commandText = "select * from DIM_997_ACK where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "850":
                        commandText = "select * from DIM_850_PO where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "856":
                        commandText = "select * from DIM_856_ASN where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "810":
                        commandText = "select * from DIM_810_INVOICE where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "830":
                        commandText = "select * from DIM_830_PLAN where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "860":
                        commandText = "select * from DIM_860_POC where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "865":
                        commandText = "select * from DIM_865_POCHANGEACK where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "855":
                        commandText = "select * from DIM_855_POACK where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "846":
                        commandText = "select * from DIM_846_INVENTORYADV where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "864":
                        commandText = "select * from DIM_864_TEXT where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "875":
                        commandText = "select * from DIM_850_PO where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "ORDERS":
                        commandText = "select * from DIM_850_PO where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "PRODAT":
                        commandText = "select * from DIM_832_PRICESALECATALOG where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "INSDES":
                        commandText = "select * from DIM_861_RAAC where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "IFTMIN":
                        commandText = "select * from DIM_204_LOADTENDER where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "DESADV":
                        commandText = "select * from DIM_856_ASN where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "APERAK":
                        commandText = "select * from DIM_824_APPADVICE where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "PRICAT":
                        commandText = "select * from DIM_832_PRICESALECATALOG where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "DELFOR":
                        commandText = "select * from DIM_830_PLAN where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "INVOIC":
                        commandText = "select * from DIM_810_INVOICE where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "ORDCHG":
                        commandText = "select * from DIM_860_POC where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    default:
                        throw new Exception("The reprocess tool does not support transaction " + transactionType);
                }


                param = new SqlParameter[2];
                param[0] = new SqlParameter("@inoutboxID", SqlDbType.NVarChar);
                param[0].Value = inoutboxID;

                param[1] = new SqlParameter("@yearQuarter", SqlDbType.NVarChar);
                param[1].Value = yearQuarter;
            }
            else
            {
                switch (transactionType)
                {
                    case "180":
                        commandText = "select * from DIM_180_RETURN where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "204":
                        commandText = "select * from DIM_204_LOADTENDER where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "812":
                        commandText = "select * from DIM_812_CDA where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "820":
                        commandText = "select * from DIM_820_PAYMENTORDER where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "824":
                        commandText = "select * from DIM_824_APPADVICE where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "832":
                        commandText = "select * from DIM_832_PRICESALECATALOG where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "852":
                        commandText = "select * from DIM_852_POS where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "861":
                        commandText = "select * from DIM_861_RAAC where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "940":
                        commandText = "select * from DIM_940_DELIVERYREQUEST where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "997":
                        commandText = "select * from DIM_997_ACK where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "850":
                        commandText = "select * from DIM_850_PO where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "856":
                        commandText = "select * from DIM_856_ASN where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "810":
                        commandText = "select * from DIM_810_INVOICE where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "830":
                        commandText = "select * from DIM_830_PLAN where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "860":
                        commandText = "select * from DIM_860_POC where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "865":
                        commandText = "select * from DIM_865_POCHANGEACK where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "855":
                        commandText = "select * from DIM_855_POACK where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "846":
                        commandText = "select * from DIM_846_INVENTORYADV where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "864":
                        commandText = "select * from DIM_864_TEXT where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "875":
                        commandText = "select * from DIM_850_PO where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "ORDERS":
                        commandText = "select * from DIM_850_PO where inboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "PRODAT":
                        commandText = "select * from DIM_832_PRICESALECATALOG where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "INSDES":
                        commandText = "select * from DIM_861_RAAC where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "IFTMIN":
                        commandText = "select * from DIM_204_LOADTENDER where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "DESADV":
                        commandText = "select * from DIM_856_ASN where outboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "APERAK":
                        commandText = "select * from DIM_824_APPADVICE where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    case "PRICAT":
                        commandText = "select * from DIM_832_PRICESALECATALOG where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "DELFOR":
                        commandText = "select * from DIM_830_PLAN where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "INVOIC":
                        commandText = "select * from DIM_810_INVOICE where outboxid=@inoutboxID and YearQuarter=@yearQuarter";
                        break;
                    case "ORDCHG":
                        commandText = "select * from DIM_860_POC where inboxid=@inoutboxID and YearQuarter=@yearQuarter and Hub_id = @hubid";
                        break;
                    default:
                        throw new Exception("The reprocess tool does not support transaction " + transactionType);
                }

                param = new SqlParameter[3];
                param[0] = new SqlParameter("@inoutboxID", SqlDbType.NVarChar);
                param[0].Value = inoutboxID;

                param[1] = new SqlParameter("@yearQuarter", SqlDbType.NVarChar);
                param[1].Value = yearQuarter;

                param[2] = new SqlParameter("@hubid", SqlDbType.NVarChar);
                param[2].Value = hubid;
            }
            try
            {
                ds = SqlHelper.ExecuteDataset(DAL.GetDiMetricsConnectionString(), CommandType.Text, commandText, param);
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

        public bool IsExistOnDiASCIIR9ByOutbox(string outboxID, string docType, string yearQuarter)
        {
            string commandText = "select * from outbox where outboxid=@outboxID and YearQuarter=@yearQuarter";
            DataSet ds;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@outboxID", SqlDbType.Int);
            param[0].Value = outboxID;

            param[1] = new SqlParameter("@yearQuarter", SqlDbType.NVarChar);
            param[1].Value = yearQuarter;

            try
            {
                ds = SqlHelper.ExecuteDataset(DAL.GetDiASCIIR9ConnectionString(), CommandType.Text, commandText, param);
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

        public bool IsExistOnDiASCIIR9ByOutbox_Chanel(string outboxID, string docType, string yearQuarter)
        {
            string commandText = "select * from inbox_tmp where inboxid=@outboxID and YearQuarter=@yearQuarter";
            DataSet ds;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@outboxID", SqlDbType.Int);
            param[0].Value = outboxID;

            param[1] = new SqlParameter("@yearQuarter", SqlDbType.NVarChar);
            param[1].Value = yearQuarter;

            try
            {
                ds = SqlHelper.ExecuteDataset(DAL.GetDiASCIIR9ConnectionString(), CommandType.Text, commandText, param);
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

        public string GetIntegrationStatusOutbox(string yearQuarter, string outboxID, string customerID)
        {
            string table = "outbox_Qr" + yearQuarter.Substring(yearQuarter.Length - 1, 1) + "_" + yearQuarter.Substring(0, 4);

            string commandText = "Select IntegrationStatus from " + table + " where outboxid=@outboxID and custid=@custID";
            DataSet ds;

            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@outboxID", SqlDbType.Int);
            param[0].Value = outboxID;

            param[1] = new SqlParameter("@custID", SqlDbType.Int);
            param[1].Value = customerID;

            try
            {
                ds = SqlHelper.ExecuteDataset(DAL.GetMainConnectionString(), CommandType.Text, commandText, param);
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
        #endregion 

        #region IMPORT FILE
        public int ImportFile(string fileName, out string exception)
        {
            exception = null;
            string transactionId = null;
            string customerId = null;
            string participantId = null;
            string version = null;
            string codepage = null;

            DateTime startDiUnite = DateTime.Now, endDiUnite = DateTime.Now;
            //string errorMessage;
            string inoutboxID = null;
            string yearQuater = null;
            string transKey = null;

            string message = "";
            int importResult = Constants.ResultSuccess;
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            TimeSpan span;
            string log = null;
            string mapID = null;
            try
            {
                string[] parameters = Path.GetFileNameWithoutExtension(fileName).Split('_');
                yearQuater = parameters[Constants.PYearQuaterIdIndex];
                transactionId = parameters[Constants.PTransactionIdIndex];
                transKey = parameters[Constants.PTransKeyIdIndex];

                if (parameters.Length >= Constants.PArgsIndexMaxLength)
                {
                    if (parameters[parameters.Length - 1].ToLower() == Constants.PInbox)
                    {
                        inoutboxID = parameters[Constants.PInboxIdIndex];
                        customerId = parameters[Constants.PToCustIdIndex];
                        participantId = parameters[Constants.PFromCustIdIndex];
                        if (Constants.PVersionIndex < parameters.Length)
                            version = parameters[Constants.PVersionIndex];
                        if (Constants.PCodePage < parameters.Length)
                            codepage = parameters[Constants.PCodePage];
                    }
                    else if (parameters[parameters.Length - 1].ToLower() == Constants.POutbox)
                    {
                        inoutboxID = parameters[Constants.POutboxIdIndex];
                        customerId = parameters[Constants.PFromCustIdIndex];
                        participantId = parameters[Constants.PToCustIdIndex];
                        if (Constants.PVersionIndex < parameters.Length)
                            version = parameters[Constants.PVersionIndex];
                        if (Constants.PCodePage < parameters.Length)
                            codepage = parameters[Constants.PCodePage];
                    }
                }


                mapID = new MapController().GetMap(customerId, participantId, transactionId);

                if (string.IsNullOrEmpty(mapID))
                {
                    importResult = Constants.ResultProcessing;

                    log += "There is no map\n";

                    end = DateTime.Now;
                    log += " End process file at " + end.ToString() + ".";
                    span = end - start;
                    log += " Time span = " + span.TotalMilliseconds.ToString() + " miliseconds. Result = " + importResult.ToString();
                    Utility.LogHelper.GetLogHelper().WriteLog(log);
                }

                if (mapID.Trim().ToLower().Contains("import directly"))
                {
                    string customerType = null;
                    if (mapID.Trim().ToLower().Contains("hub"))
                        customerType = "hub";
                    else
                        customerType = "vendor";
                    startDiUnite = DateTime.Now;
                    Utility.LogHelper.GetLogHelper().WriteLog("Import file " + fileName + " directly at " + startDiUnite.ToString());

                    try
                    {
                        string SQLErrorMessage = null;
                        bool result = ImportTransaction(transKey, transactionId, customerId, participantId, inoutboxID, yearQuater, customerType, ref SQLErrorMessage);

                        if (!string.IsNullOrEmpty(SQLErrorMessage))
                        {
                            exception = SQLErrorMessage;
                            Utility.LogHelper.GetLogHelper().WriteLog("Importing directly error: " + SQLErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        exception = ex.Message;
                        message += fileName + "\n";
                        importResult = Constants.ResultProcessing;
                        Utility.LogHelper.GetLogHelper().WriteLog(ex, "Service", "public ResultHelper ImportData(string args)");
                    }
                    endDiUnite = DateTime.Now;
                    Utility.LogHelper.GetLogHelper().WriteLog("End import directly at " + endDiUnite.ToString() + ". Time span=" + (endDiUnite - startDiUnite).TotalMilliseconds.ToString() + " miliseconds.");
                }
                else
                {
                    using (var client = new DiUniteServiceProxy("DiUnite.DiUniteManager.IServices"))
                    {
                        try
                        {
                            startDiUnite = DateTime.Now;
                            log += "Call DiUnite Service with file " + fileName + " and map ID " + mapID + " at " + startDiUnite.ToString();
                            DiUnite.DiUniteManager.IServices.DiUniteFlatFileResult result = null;
                            string[] maps = mapID.Split(';');
                            for (int k = 0; k < maps.Length; k++)
                            {
                                result = client.TranslateFile(maps[k], fileName);

                                if (result != null)
                                {
                                    exception = result.ErrorMessage;
                                }
                            }


                            endDiUnite = DateTime.Now;
                            log += ". End call DiUnite Service with result " + result.ErrorMessage + " at " + endDiUnite.ToString() + ". DiUnite time span=" + (endDiUnite - startDiUnite).TotalMilliseconds.ToString() + " miliseconds.\n";
                        }
                        catch (Exception ex)
                        {
                            message += fileName + "\n";
                            importResult = Constants.ResultProcessing;
                            Utility.LogHelper.GetLogHelper().WriteLog(ex, "Service", "public ResultHelper ImportData(string args)");
                            exception = ex.Message;
                        }
                    }
                }

                try
                {
                    log += "Start call store to update Participant Id =" + participantId + "; inboxoutboxId=" + inoutboxID + " at " + DateTime.Now;
                    UpdateParticipant(transactionId, customerId, participantId, inoutboxID, yearQuater);
                    log += " ...End call store to update Participant Id =" + participantId + "; inboxoutboxId=" + inoutboxID + " at " + DateTime.Now + "\n";

                    List<EventItem> eventItems = new List<DImetrics.Entity.Common.EventItem>();
                    DImetrics.Entity.Common.EventItem eventItem = new EventItemController().GetEventItemByTransaction(transactionId, customerId, participantId, inoutboxID);
                    if (eventItem != null)
                    {
                        eventItems.Add(eventItem);
                        importResult = Constants.ResultSuccess;
                    }
                    for (int j = 0; j < eventItems.Count; j++)
                    {
                        //Enable event to let Metric Engine checks data
                        new MetricEngine(customerId).EnableEvent(eventItems[j]);
                    }
                    if (eventItems.Count == 0)
                        importResult = Constants.ResultProcessing;
                }
                catch (Exception ex)
                {
                    importResult = Constants.ResultProcessing;
                    Utility.LogHelper.GetLogHelper().WriteLog(ex, "Service", "public ResultHelper ImportData(string args)");
                }
                try
                {
                    //if ((new List<string> { Constants.doc810, Constants.doc850, Constants.doc856, Constants.doc180 }).Contains(transactionId))
                    //{        
                    string docTypes = Constants.doc810 + "," + Constants.doc850 + "," + Constants.doc856 + "," + Constants.doc180;
                    if (IsMatchViolationExist(docTypes, customerId.Trim(), participantId, 0))
                    {
                        Utility.LogHelper.GetLogHelper().WriteLog("Start Update Is Match Table Violation Data with InOutboxID Ref: " + inoutboxID + " and Trans: " + transactionId.Trim());
                        UpdateIsMatchViolation(Convert.ToInt32(inoutboxID), customerId.Trim(), participantId, transactionId.Trim(), Convert.ToInt32(yearQuater));
                        Utility.LogHelper.GetLogHelper().WriteLog("End Update Is Match Table Violation Data Successfully");
                    }
                }    //}

                catch (Exception ex)
                {
                    string message1 = "Dear Admin <br /><br /> Update Is Match Violation Data with inoutboxid: " + inoutboxID + " unsuccessfully ";
                    Utility.LogHelper.GetLogHelper().WriteLog(ex, "BLL", "public int ImportFile");
                    new EmailProcessor().SendImportEngineExceptionEmail(new Exception(message1));
                }

            }
            catch (Exception ex)
            {
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "Form1", "public void Import()");
                importResult = Constants.ResultProcessing;
            }
            end = DateTime.Now;
            log += " End process file at " + end.ToString() + ".";
            span = end - start;
            log += " Time span = " + span.TotalMilliseconds.ToString() + " miliseconds. Result = " + importResult.ToString();
            Utility.LogHelper.GetLogHelper().WriteLog(log);

            if (importResult == Constants.ResultProcessing)
            {
                if (!mapID.Trim().ToLower().Contains("import directly") && string.IsNullOrEmpty(exception))
                    exception += ". DiUnite does not return the reason of fail.";
                else if (mapID.Trim().ToLower().Contains("import directly"))
                    exception += "Cannot reprocessing because lacking of data.";
            }

            return importResult;
        }


        public void UpdateIntegrationStatusInbox(string inboxID, string customerID, string yearQuarter, string integrationStatus = "P1_MT_3")
        {

            string table = "inbox_Qr" + yearQuarter.Substring(yearQuarter.Length - 1, 1) + "_" + yearQuarter.Substring(0, 4);

            string commandText = "Update " + table + " set IntegrationStatus='" + integrationStatus + "' where inboxid=@inboxID and custid=@custID";

            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@inboxID", SqlDbType.Int);
            param[0].Value = inboxID;

            param[1] = new SqlParameter("@custID", SqlDbType.Int);
            param[1].Value = customerID;

            try
            {
                SqlHelper.ExecuteNonQuery(DAL.GetMainConnectionString(), CommandType.Text, commandText, param);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateIntegrationStatusOutbox(string outboxID, string customerID, string yearQuarter, string integrationStatus = "P1_MT_3")
        {

            string table = "outbox_Qr" + yearQuarter.Substring(yearQuarter.Length - 1, 1) + "_" + yearQuarter.Substring(0, 4);

            string commandText = "Update " + table + " set IntegrationStatus='" + integrationStatus + "' where outboxid=@outboxID and custid=@custID";

            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@outboxID", SqlDbType.Int);
            param[0].Value = outboxID;

            param[1] = new SqlParameter("@custID", SqlDbType.Int);
            param[1].Value = customerID;

            try
            {
                SqlHelper.ExecuteNonQuery(DAL.GetMainConnectionString(), CommandType.Text, commandText, param);
            }
            catch
            {
                throw;
            }

        }


        #endregion

        #region Import Transaction CIP - EDI Extract - All Hub All Vendor
        public bool ImportTransaction(string transactionKey, string transaction, string hubID, string participantID, string inboxID, string yearquarter, string type, ref string errorMessage)
        {
            bool result = false;
            errorMessage = null;

            SqlParameter[] parameters = new SqlParameter[7];
            try
            {
                parameters[0] = new SqlParameter("@TransKey", transactionKey);
                parameters[1] = new SqlParameter("@HubID", hubID);
                parameters[2] = new SqlParameter("@ParticipantID", participantID);
                parameters[3] = new SqlParameter("@InboxID", inboxID);
                parameters[4] = new SqlParameter("@YearQuarter", yearquarter);
                parameters[5] = new SqlParameter("@CustomerType", type);
                parameters[6] = new SqlParameter("@errorMessage", errorMessage);
                parameters[6].SqlDbType = SqlDbType.NVarChar;
                parameters[6].Size = 4000;
                parameters[6].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_Import_Transaction_" + transaction, parameters);
                errorMessage = string.IsNullOrEmpty(parameters[6].Value.ToString()) ? null : parameters[6].Value.ToString();
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void ImportTransaction");

                throw;
            }

            return result;
        }
        /// <summary>
        /// All hubs All vendors Queue Data
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="inoutboxID"></param>
        /// <param name="yearquarter"></param>
        /// <param name="isInbox"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool ImportTransactionFromDiRep(string transaction, string inoutboxID, string yearquarter, string isInbox, out string errorMessage)
        {
            bool result = false;
            errorMessage = null;

            SqlParameter[] parameters = new SqlParameter[4];
            try
            {
                parameters[0] = new SqlParameter("@InOutboxID", inoutboxID);
                parameters[1] = new SqlParameter("@IsInbox", isInbox);
                parameters[2] = new SqlParameter("@YearQuarter", yearquarter);
                parameters[3] = new SqlParameter("@errorMessage", errorMessage);
                parameters[3].SqlDbType = SqlDbType.NVarChar;
                parameters[3].Size = 4000;
                parameters[3].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_DiMetric_Import_DiRep_" + transaction, parameters);
                errorMessage = string.IsNullOrEmpty(parameters[3].Value.ToString()) ? null : parameters[3].Value.ToString();
                result = true;
            }
            catch (Exception ex)
            {
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void ImportTransactionFromDiRep");
                result = false;

                throw;
            }

            return result;
        }

        /// <summary>
        /// EDi Extract Queue Data
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="inoutboxID"></param>
        /// <param name="yearquarter"></param>
        /// <param name="isInbox"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string ImportExTransactionFromDiRep(string transaction, string senderID, string receiverID, string inoutboxID, int yearquarter, string isInbox, out string errorMessage)
        {

            errorMessage = null;

            SqlParameter[] parameters = new SqlParameter[6];
            try
            {
                parameters[0] = new SqlParameter("@SenderId", senderID);
                parameters[1] = new SqlParameter("@ReceiverId", receiverID);
                parameters[2] = new SqlParameter("@InOutboxID", inoutboxID);
                parameters[3] = new SqlParameter("@IsInbox", isInbox);
                parameters[4] = new SqlParameter("@YearQuarter", yearquarter);
                parameters[5] = new SqlParameter("@errorMessage", errorMessage);
                parameters[5].SqlDbType = SqlDbType.NVarChar;
                parameters[5].Size = 4000;
                parameters[5].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_Extract_Import_DiRep_" + transaction, parameters);
                errorMessage = string.IsNullOrEmpty(parameters[5].Value.ToString()) ? null : parameters[5].Value.ToString();
            }
            catch (Exception ex)
            {
                //errorMessage = ex.InnerException.Message;
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void ImportExTransactionFromDiRep");
                errorMessage = ex.Message;
            }

            return errorMessage;
        }
        /// <summary>
        /// New Flow Import Directly DiASCIIR9_Rep to DiMetrics 
        /// </summary>
        /// <returns></returns>
        public bool ImportTransactionFromDiRepExtract(string transaction, string hubID, string participantID, string inoutboxID, string yearQuarter, string isInbox, string errorMessage)
        {
            bool result = false;
            errorMessage = null;

            SqlParameter[] parameters = new SqlParameter[6];
            try
            {
                parameters[0] = new SqlParameter("@SenderId", hubID);
                parameters[1] = new SqlParameter("@ReceiverId", participantID);
                parameters[2] = new SqlParameter("@InOutboxID", inoutboxID);
                parameters[3] = new SqlParameter("@IsInbox", isInbox);
                parameters[4] = new SqlParameter("@YearQuarter", yearQuarter);
                parameters[5] = new SqlParameter("@errorMessage", errorMessage);
                parameters[5].SqlDbType = SqlDbType.NVarChar;
                parameters[5].Size = 4000;
                parameters[5].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_Extract_Import_DiRep_" + transaction, parameters);

                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void ImportTransactionFromDiRepExtract");
                throw;
            }
            return result;
        }

        /// <summary>
        /// New Flow Import Directly RelationalDB to DiMetrics (New Schema JSON_[Trans]_HEADER, JSON_[Trans]_DETAIL)
        /// </summary>
        /// <returns></returns>
        public bool ImportJSONTransactionFromRelationalDB(string transactionKey, string transaction, string customerId, string tradingPartnerId, string inOutboxID, string yearquarter, string type, ref string errorMessage)
        {
            bool result = false;
            errorMessage = null;

            SqlParameter[] parameters = new SqlParameter[7];
            try
            {
                parameters[0] = new SqlParameter("@TransKey", transactionKey);
                parameters[1] = new SqlParameter("@CustomerID", customerId);
                parameters[2] = new SqlParameter("@TradingPartnerID", tradingPartnerId);
                parameters[3] = new SqlParameter("@InOutboxID", inOutboxID);
                parameters[4] = new SqlParameter("@YearQuarter", yearquarter);
                parameters[5] = new SqlParameter("@CustomerType", type);
                parameters[6] = new SqlParameter("@errorMessage", errorMessage);
                parameters[6].SqlDbType = SqlDbType.NVarChar;
                parameters[6].Size = 4000;
                parameters[6].Direction = ParameterDirection.Output;

                var log = Utility.LogHelper.GetLogHelper();
                if (log != null)
                    log.WriteLog($" key {transactionKey}, customer {customerId}, tradingpartner {tradingPartnerId}, inoutbox {inOutboxID}, yearquarter {yearquarter} , type {type}");

                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_JSON_Import_Transaction_" + transaction, parameters);

                //Insert JSON_Inbox; JSON_Outbox 
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_JSON_Import_Inbox_Outbox", parameters);

                errorMessage = string.IsNullOrEmpty(parameters[6].Value.ToString()) ? null : parameters[6].Value.ToString();
                result = true;

            }
            catch (Exception ex)
            {
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void ImportJSONTransactionFromRelationalDB(int docId)");
                result = false;

                throw;
            }

            return result;
        }


        public void UpdateParticipant(string transNo, string hubId, string participantNo, string inboxOutboxId, string yearQuarter)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            try
            {
                parameters[0] = new SqlParameter("@TransNo", transNo);
                parameters[1] = new SqlParameter("@ParticipantNo", participantNo);
                parameters[2] = new SqlParameter("@HubId", hubId);
                parameters[3] = new SqlParameter("@YearQuarter", yearQuarter);
                parameters[4] = new SqlParameter("@InboxOutboxId", inboxOutboxId);
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, "sp_UpdateParticipantDocument", parameters);

            }
            catch (Exception ex)
            {
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void UpdateParticipant(string transNo, string hubId, string participantNo, string inboxOutboxId, int yearQuarter)");
            }
        }
        public bool IsMatchViolationExist(string transaction, string customerId, string participantNo, int isMatch)
        {
            SqlParameter[] parameters = new SqlParameter[4];
            using (SqlConnection _sqlConnection = new SqlConnection(Constants.DiMetricsConnectionString))
            {
                _sqlConnection.Open();

                parameters[0] = new SqlParameter("@trans", transaction);
                parameters[1] = new SqlParameter("@CustomerId", customerId);
                parameters[2] = new SqlParameter("@ParticipantNo", participantNo);
                parameters[3] = new SqlParameter("@isMatch", isMatch);

                DataSet ds = SqlHelper.ExecuteDataset(_sqlConnection, CommandType.StoredProcedure, Constants.SP_GetViolation_IsMatchExists, parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #region Is Match = 0 defaule violate rule => update isMatch=1 unviolate rule
        public void UpdateIsMatchViolation(int inOutboxIdRef, string customerId, string participantId, string transNoRef, int yearQuarterRef)
        {
            try
            {
                SqlParameter[] parames = new SqlParameter[5];
                parames[0] = new SqlParameter("@InOutboxIdRef", inOutboxIdRef);
                parames[1] = new SqlParameter("@CustomerId", customerId);
                parames[2] = new SqlParameter("@ParticipantNo", participantId);
                parames[3] = new SqlParameter("@TransNoRef", transNoRef);
                parames[4] = new SqlParameter("@YearQuarterRef", yearQuarterRef);

                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.StoredProcedure, Constants.SP_UPDATEISMATCHHVIOLATION_DATA, parames);
            }
            catch (Exception ex)
            {
                Utility.LogHelper.GetLogHelper().WriteLog(ex, "EDITransactionController", "public void UpdateIsMatchViolation");
                throw;
            }
        }

        #endregion
        #endregion



    }
}
