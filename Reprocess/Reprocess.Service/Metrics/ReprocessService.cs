using Reprocess.Common;
using Reprocess.Data.Metrics;
using Reprocess.Service.Metrics.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reprocess.Data;
using Reprocess.Data.Metrics.Repository;
using Reprocess.Data.Metrics.Repository.IRepository;
using Reprocess.Model.Metrics.Dto;
using System.Collections.Concurrent;

namespace Reprocess.Service.Metrics
{
    public class ReprocessService : IReprocessService
    {
        private readonly IMetricReprocessRepository _metricReprocessRepository;
        public ReprocessService(IMetricReprocessRepository metricReprocessRepository)
        {
            _metricReprocessRepository = metricReprocessRepository;
        }
        #region TRACK FILE 
        public string GetIntegrationStatusInbox(string yearQuarter, string inboxID, string customerID)
        {
            string status = _metricReprocessRepository.GetIntegrationStatusInbox(yearQuarter, inboxID, customerID);
            switch (status)
            {
                case null:
                    return "Not exist";
                case "P1_MT_5":
                    return "Fail";
                case "P1_MT_4":
                    return "Processing";
                case "P1_MT_3":
                    return "Success";
                case "P1_MT_2":
                    return "Invalid";
                case "P1_MT_1":
                    return "Pass";
                case "P1_MT_0":
                    return "Hold";
                case "P1_MT_8":
                    return "Importing";
            }
            return "Unknown";

        }
        public List<TrackDataGrid> GetTrackDataGridsV1(List<TrackSearchData> dataList)
        {
            List<TrackDataGrid> result = new List<TrackDataGrid>();
            try
            {
                if (dataList.FirstOrDefault().FlowId == (int)Flow.CIP)
                {
                    result = HandlerFilesV1(dataList);

                }
                else if (dataList.FirstOrDefault().FlowId == (int)Flow.EDIExtract)
                {
                    //todo
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        public List<TrackDataGrid> HandlerFilesV1(List<TrackSearchData> dataList)
        {
            List<TrackDataGrid> TrackDataGridList = new List<TrackDataGrid>();
            try
            {
                if (dataList.Any())
                {
                    foreach (var item in dataList)
                    {
                        TrackDataGrid TrackDataGridItem = new TrackDataGrid();
                        //check từng item trog list
                        string integrationStatus = string.Empty;
                        string note = string.Empty;
                        switch ((item.EndItem.ToLower()))
                        {
                            case Constants.PInbox:
                                integrationStatus = GetIntegrationStatusInbox(item.PYearQuaterIdIndex, item.PInboxIdIndex, item.PToCustIdIndex);
                                note = GetNoteInboxV1(item);
                                break;
                            case Constants.POutbox:
                                integrationStatus = GetIntegrationStatusOutbox(item.PYearQuaterIdIndex, item.POutboxIdIndex, item.PFromCustIdIndex);
                                note = GetNoteInboxV1(item);
                                break;

                        }
                        TrackDataGridList.Add(FillTrackDataGrid(item.PInboxIdIndex, item.PTransactionIdIndex, integrationStatus, GetFileName(item), note));
                    }

                }
                else
                {
                    //todo Log file null
                }
                return TrackDataGridList;
            }
            catch (Exception ex)
            {
                //todo log 
                throw;
            }
        }

        public string GetFileName(TrackSearchData trackData)
        {
            //check data input
            try
            {
                string fileName = "Test";
                return Path.GetFileName(fileName);
            }
            catch (Exception ex)
            {
                //to do log
                throw;
            }
        }
        public string GetNoteInboxV1(TrackSearchData trackData)
        {
            string result = string.Empty;
            try
            {
                //intergrate
                if (trackData.PToCustIdIndex != "21412")
                {
                    if (IsExistOnDiASCIIR9ByInbox(trackData.PInboxIdIndex, trackData.PTransactionIdIndex, trackData.PYearQuaterIdIndex))
                    {
                        result = "Exist in DiASCIIR9 database";
                    }
                    else
                    {
                        result = "Not exist in DiASCIIR9 database";
                    }
                }
                else
                {
                    if (IsExistOnDiASCIIR9ByInbox_Chanel(trackData.PInboxIdIndex, trackData.PTransactionIdIndex, trackData.PYearQuaterIdIndex))
                    {
                        result = "Exist in DiASCIIR9 database";
                    }
                    else
                    {
                        result = "Not exist in DiASCIIR9 database";
                    }
                }
                //metrics 
                if (IsExistOnDiMetrics(trackData.PTransactionIdIndex, int.Parse(trackData.PInboxIdIndex), trackData.PYearQuaterIdIndex, trackData.PToCustIdIndex))
                    result += " & Exist in DiMetrics database";
                else
                    result += " & Not exist in DiMetrics database";
                return result;
            }
            catch (Exception ex)
            {
                //todo log
                return result;
                throw;
            }
        }
        public List<TrackDataGrid> GetTrackDataGrids(TrackSearchData searchParams)
        {
            List<TrackDataGrid> result = new List<TrackDataGrid>();
            try
            {
                string[] files;
                if (CheckInputTrackData(searchParams))
                {
                    if (searchParams.FlowId == (int)Flow.CIP)
                    {
                        files = Extension.GetFiles(searchParams.Files);
                        result = HandlerFiles(files);
                    }
                    else if (searchParams.FlowId == (int)Flow.EDIExtract)
                    {
                        //todo
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw;
            }
        }

        public List<TrackDataGrid> HandlerFiles(string[] files)
        {
            List<TrackDataGrid> TrackDataGridList = new List<TrackDataGrid>();
            try
            {
                if (files.Length > 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        TrackDataGrid TrackDataGridItem = new TrackDataGrid();
                        if (!string.IsNullOrEmpty(files[i]))
                        {
                            string integrationStatus = string.Empty;
                            string note = string.Empty;
                            string[] fields = Extension.ParseFileName(files[i]);
                            switch ((fields[fields.Length - 1].ToLower()))
                            {
                                case Constants.PInbox:
                                    integrationStatus = GetIntegrationStatusInbox(fields[Constants.PYearQuaterIdIndex], fields[Constants.PInboxIdIndex], fields[Constants.PToCustIdIndex]);
                                    note = GetNoteInbox(fields);
                                    break;
                                case Constants.POutbox:
                                    integrationStatus = GetIntegrationStatusOutbox(fields[Constants.PYearQuaterIdIndex], fields[Constants.POutboxIdIndex], fields[Constants.PFromCustIdIndex]);
                                    note = GetNoteOutbox(fields);
                                    break;
                            }
                            TrackDataGridList.Add(FillTrackDataGrid(fields[Constants.PInboxIdIndex], fields[Constants.PTransactionIdIndex], integrationStatus, files[i], note));
                        }
                    }
                }
                else
                {
                    //todo Log file null
                }
                return TrackDataGridList;
            }
            catch (Exception ex)
            {
                //todo log 
                throw;
            }
        }
        public string GetNoteInbox(string[] fields)
        {
            string result = string.Empty;
            try
            {
                //intergrate
                if (fields[Constants.PToCustIdIndex] != "21412")
                {
                    if (IsExistOnDiASCIIR9ByInbox(fields[Constants.PInboxIdIndex], fields[Constants.PTransactionIdIndex], fields[Constants.PYearQuaterIdIndex]))
                    {
                        result = "Exist in DiASCIIR9 database";
                    }
                    else
                    {
                        result = "Not exist in DiASCIIR9 database";
                    }
                }
                else
                {
                    if (IsExistOnDiASCIIR9ByInbox_Chanel(fields[Constants.PInboxIdIndex], fields[Constants.PTransactionIdIndex], fields[Constants.PYearQuaterIdIndex]))
                    {
                        result = "Exist in DiASCIIR9 database";
                    }
                    else
                    {
                        result = "Not exist in DiASCIIR9 database";
                    }
                }
                //metrics 
                if (IsExistOnDiMetrics(fields[Constants.PTransactionIdIndex], int.Parse(fields[Constants.PInboxIdIndex]), fields[Constants.PYearQuaterIdIndex], fields[Constants.PToCustIdIndex]))
                    result += " & Exist in DiMetrics database";
                else
                    result += " & Not exist in DiMetrics database";
                return result;
            }
            catch (Exception ex)
            {
                //todo log
                return result;
                throw;
            }
        }
        public string GetNoteOutbox(string[] fields)
        {
            string result = string.Empty;
            try
            {
                if (fields[Constants.PFromCustIdIndex] != "21412")
                {
                    if (IsExistOnDiASCIIR9ByOutbox(fields[Constants.POutboxIdIndex], fields[Constants.PTransactionIdIndex], fields[Constants.PYearQuaterIdIndex]))
                    {
                        result = "Exist in DiASCIIR9 database";
                    }
                    else
                    {
                        result = "Not exist in DiASCIIR9 database";
                    }
                }
                else
                {
                    if (IsExistOnDiASCIIR9ByOutbox_Chanel(fields[Constants.POutboxIdIndex], fields[Constants.PTransactionIdIndex], fields[Constants.PYearQuaterIdIndex]))
                    {
                        result = "Exist in DiASCIIR9 database";
                    }
                    else
                    {
                        result = "Not exist in DiASCIIR9 database";
                    }
                }
                if (IsExistOnDiMetrics(fields[Constants.PTransactionIdIndex], int.Parse(fields[Constants.POutboxIdIndex]), fields[Constants.PYearQuaterIdIndex], fields[Constants.PFromCustIdIndex]))
                    result += " & Exist in DiMetrics database";
                else
                    result += " & Not exist in DiMetrics database";
                return result;
            }
            catch (Exception ex)
            {
                return result;
                //todo log
            }
        }
        public bool IsCheckedRow(string integrationStatus, string note)
        {
            if ((!string.IsNullOrEmpty(integrationStatus)
                && integrationStatus != "Processing"
                && integrationStatus != "Invalid"
                && integrationStatus != "Fail"
                && integrationStatus != "Unknown")
                || note.Contains("Not exist in DiASCIIR9 database")) return true;
            return false;
        }
        public bool CheckInputTrackData(TrackSearchData searchParams)
        {
            bool result = false;
            try
            {
                if (searchParams.FlowId > 0 && !string.IsNullOrEmpty(searchParams.Files)
                    && !string.IsNullOrWhiteSpace(searchParams.Files))
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            return result;
        }
        public TrackDataGrid FillTrackDataGrid(string inOutboxId, string transactionType, string intergrationStatus, string fileName, string note)
        {
            TrackDataGrid result = new TrackDataGrid();
            try
            {
                result.InOutboxId = inOutboxId ?? "";
                result.TransactionType = transactionType ?? "";
                result.IntergrationStatus = intergrationStatus ?? "";
                result.FileName = Path.GetFileName(fileName) ?? "";
                result.Note = note ?? "";
                result.IsChecked = IsCheckedRow(intergrationStatus, note);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool IsExistOnDiASCIIR9ByInbox(string inboxID, string docType, string yearQuarter)
        {
            return _metricReprocessRepository.IsExistOnDiASCIIR9ByInbox(inboxID, docType, yearQuarter);
        }

        public bool IsExistOnDiASCIIR9ByInbox_Chanel(string inboxID, string docType, string yearQuarter)
        {
            return _metricReprocessRepository.IsExistOnDiASCIIR9ByInbox_Chanel(inboxID, docType, yearQuarter);
        }

        public bool IsExistOnDiASCIIR9ByOutbox(string outboxID, string docType, string yearQuarter)
        {
            return _metricReprocessRepository.IsExistOnDiASCIIR9ByOutbox(outboxID, docType, yearQuarter);
        }

        public bool IsExistOnDiASCIIR9ByOutbox_Chanel(string outboxID, string docType, string yearQuarter)
        {
            return _metricReprocessRepository.IsExistOnDiASCIIR9ByOutbox_Chanel(outboxID, docType, yearQuarter);
        }

        public bool IsExistOnDiMetrics(string transactionType, int inoutboxID, string yearQuarter, string hubid = "")
        {
            return _metricReprocessRepository.IsExistOnDiMetrics(transactionType, inoutboxID, yearQuarter, hubid);
        }

        public string GetIntegrationStatusOutbox(string yearQuarter, string outboxID, string customerID)
        {
            return _metricReprocessRepository.GetIntegrationStatusOutbox(yearQuarter, outboxID, customerID);
        }
        #endregion
        #region METRICS REPROCESS 
        public List<ReprocessDataGrid> GetMTReprocessDataGrid(ReprocessSearchData reprocessSearchData)
        {
            var searchParams = reprocessSearchData.SearchCondition;
            if (searchParams.FlowId == (int)Flow.CIP) //CIP FLOW
            {
                ReprocessCIPFlowHandler(reprocessSearchData);
            }
            else
            {
                //EXTRACT....
            }
            return null;
        }
        public List<ReprocessDataGrid> ReprocessCIPFlowHandler(ReprocessSearchData reprocessSearchData)
        {
            var searchParams = reprocessSearchData.SearchCondition;
            var trackDataList = reprocessSearchData.TrackDataGrid;
            var myQueue = new ConcurrentQueue<string>();
            //check count
            foreach (var item in trackDataList)
            {
                if (item.IsChecked)
                {
                    if (item.Note.Contains(Constants.NotExistInMTDb)) //Not exist in DiMetrics database
                    {
                        myQueue.Enqueue(item.Files);
                        if (searchParams.IsImportMetric)
                        {
                            var mtImportResult = GetStatusImportFile(item);
                        }
                    }
                    else
                    {
                        UpdateIntegrationStatusInOutbox(item.Files);
                    }
                }
            }
            if (searchParams.IsImportMetric && myQueue.Count > 0)
            {
                MTImportFileCIPFlow(searchParams, myQueue);
            }
            return null;
        }
        public void MTImportFileCIPFlow(SearchCondition searchParams, ConcurrentQueue<string> myQueue)
        {

            if (searchParams.IsRunMultiThread)
            {
                var numberOfThreads = searchParams.NumberOfThread;
                MultiThreadCIPHandler(myQueue, numberOfThreads, searchParams);
            }
            else if (searchParams.IsRunParallel)
            {
                ParallelCIPHandler(myQueue, searchParams);
            }
        }
        public void MultiThreadCIPHandler(ConcurrentQueue<string> myQueue, int numberOfThreads, SearchCondition searchParams)
        {
            List<Task> tasksToWait = new List<Task>();
            var noOfFiles = myQueue.Count;
            var startProcess = DateTime.Now;
            //Utility.LogHelper.GetLogHelper().WriteLog(startProcess + ":PROCESSING START to call import service for the new batch " + noOfFiles + " files");
            for (int i = 0; i < numberOfThreads; i++)
            {
                Task task = new Task(() =>
                {
                    var item = string.Empty;
                    while (myQueue.TryDequeue(out item))
                    {
                        try
                        {
                            searchParams.RemainProcData = myQueue.Count;
                            //Call Import Service
                            var helper = new DiMetrics.Helper.DiMetricsHelper(searchParams.ImportServiceUrl);
                            var resultUrl = string.Empty;
                            var start = DateTime.Now;
                            //Utility.LogHelper.GetLogHelper().WriteLog(start + ": START to call import service for file " + item);
                            var result = helper.ImportData(out resultUrl, "-filepath " + item);
                            var end = DateTime.Now;
                            //Utility.LogHelper.GetLogHelper().WriteLog("COMPLETE in " + (end - start).Seconds + " seconds to call import service for file " + item);
                            if (result == Constants.ResultPass || result == Constants.ResultSuccess || result == Constants.ResultFail || result == Constants.ResultAccepted)
                            {
                                UpdateIntegrationStatusInOutbox(item, "P1_MT_" + result);
                            }
                            searchParams.RemainProcData = myQueue.Count;
                        }
                        catch (Exception ex)
                        {
                            //Utility.LogHelper.GetLogHelper().WriteLog(ex);
                            //throw;
                        }
                    }
                });
                task.Start();
                tasksToWait.Add(task);
            }
            Task.WaitAll(tasksToWait.ToArray());
            var endProcess = DateTime.Now;
            //Tracking();
            //Utility.LogHelper.GetLogHelper().WriteLog("PROCESSING COMPLETE to call import service for the new batch " + noOfFiles + " files in " + (endProcess - startProcess).Minutes + " minutes");
        }
        public void ParallelCIPHandler(ConcurrentQueue<string> myQueue, SearchCondition searchParams)
        {
            var noOfFiles = myQueue.Count;
            searchParams.RemainProcData = myQueue.Count;
            var startProcess = DateTime.Now;
            Parallel.ForEach(myQueue, item =>
            {
                //Call Import Service
                var helper = new DiMetrics.Helper.DiMetricsHelper(searchParams.ImportServiceUrl);
                var resultUrl = string.Empty;
                var start = DateTime.Now;
                //Utility.LogHelper.GetLogHelper().WriteLog(start + ": START to call import service for file " + item);
                var result = helper.ImportData(out resultUrl, "-filepath " + item);
                var end = DateTime.Now;
                //Utility.LogHelper.GetLogHelper().WriteLog("COMPLETE in " + (end - start).Seconds + " seconds to call import service for file " + item);
                if (result == Constants.ResultPass || result == Constants.ResultSuccess || result == Constants.ResultFail || result == Constants.ResultAccepted)
                {
                    UpdateIntegrationStatusInOutbox(item, "P1_MT_" + result);
                }
                searchParams.RemainProcData = myQueue.Count;
            });
            var endProcess = DateTime.Now;
            //  Tracking();
            // Utility.LogHelper.GetLogHelper().WriteLog("PROCESSING COMPLETE to call import service for the new batch " + noOfFiles + " files in " + (endProcess - startProcess).Minutes + " minutes");

        }
        public MTImportResult GetStatusImportFile(TrackDataGrid trackDataGrid)
        {
            MTImportResult mTImportResult = new MTImportResult();
            try
            {
                string exception = null;

                int result = ImportFile(trackDataGrid.Files, out exception);
                string resultMessage = string.Empty;
                switch (result)
                {
                    case Constants.ResultPass:
                        resultMessage = "Success";
                        break;
                    case Constants.ResultSuccess:
                        resultMessage = "Success";
                        break;
                    case Constants.ResultProcessing:
                        resultMessage = "Fail";
                        break;
                    case Constants.ResultFail:
                        resultMessage = "Success";
                        break;
                    //case Constants.ResultException:
                    //    resultMessage = "Fail";
                    //    break;
                    case Constants.ResultAccepted:
                        resultMessage = "Success";
                        break;
                    case Constants.ResultIgnore:
                        resultMessage = "Fail";
                        break;
                    case Constants.ResultImporting:
                        resultMessage = "Importing";
                        break;
                }
                if (string.IsNullOrEmpty(exception) == false)
                    resultMessage += ". " + exception;
                if (result == Constants.ResultPass || result == Constants.ResultSuccess || result == Constants.ResultFail || result == Constants.ResultAccepted)
                {
                    UpdateIntegrationStatusInOutbox(trackDataGrid.Files);
                }
                mTImportResult.Result = result;
                mTImportResult.ExceptionMess = resultMessage;
            }
            catch (Exception ex)
            {
                //to do log
            }
            return mTImportResult;
        }
        public void UpdateIntegrationStatusInOutbox(string file, string integrationStatus = "P1_MT_3")
        {

            string[] fields = Extension.ParseFileName(file);
            try
            {
                if (fields[fields.Length - 1].ToLower() == "inbox")
                    UpdateIntegrationStatusInbox(fields[Constants.PInboxIdIndex], fields[Constants.PToCustIdIndex], fields[Constants.PYearQuaterIdIndex], integrationStatus = "P1_MT_3");
                if (fields[fields.Length - 1].ToLower() == "outbox")
                    UpdateIntegrationStatusOutbox(fields[Constants.POutboxIdIndex], fields[Constants.PFromCustIdIndex], fields[Constants.PYearQuaterIdIndex], integrationStatus = "P1_MT_3");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public int ImportFile(string fileName, out string exception)
        {
            return _metricReprocessRepository.ImportFile(fileName, out exception);
        }
        void UpdateIntegrationStatusInbox(string inboxID, string customerID, string yearQuarter, string integrationStatus = "P1_MT_3")
        {
            _metricReprocessRepository.UpdateIntegrationStatusInbox(inboxID, customerID, yearQuarter, integrationStatus = "P1_MT_3");
        }
        void UpdateIntegrationStatusOutbox(string outboxID, string customerID, string yearQuarter, string integrationStatus = "P1_MT_3")
        {
            _metricReprocessRepository.UpdateIntegrationStatusOutbox(outboxID, customerID, yearQuarter, integrationStatus);
        }
        #endregion
    }
}
