using Reprocess.Model.Metrics.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Service.Metrics.IService
{
    public interface IReprocessService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearQuarter"></param>
        /// <param name="inboxID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        string GetIntegrationStatusInbox(string yearQuarter, string inboxID, string customerID);
        string GetIntegrationStatusOutbox(string yearQuarter, string outboxID, string customerID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inboxID"></param>
        /// <param name="docType"></param>
        /// <param name="yearQuarter"></param>
        /// <returns></returns>
        bool IsExistOnDiASCIIR9ByInbox(string inboxID, string docType, string yearQuarter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inboxID"></param>
        /// <param name="docType"></param>
        /// <param name="yearQuarter"></param>
        /// <returns></returns>
        bool IsExistOnDiASCIIR9ByInbox_Chanel(string inboxID, string docType, string yearQuarter);
        bool IsExistOnDiMetrics(string transactionType, int inoutboxID, string yearQuarter, string hubid = "");
        bool IsExistOnDiASCIIR9ByOutbox(string outboxID, string docType, string yearQuarter);
        bool IsExistOnDiASCIIR9ByOutbox_Chanel(string outboxID, string docType, string yearQuarter);
        List<TrackDataGrid> GetTrackDataGrids(TrackSearchData searchParams);
        List<TrackDataGrid> GetTrackDataGridsV1(List<TrackSearchData> searchParams);
        bool CheckInputTrackData(TrackSearchData searchParams);
        List<ReprocessDataGrid> GetMTReprocessDataGrid(ReprocessSearchData reprocessSearchData);
        int ImportFile(string fileName, out string exception);
      
    }
}
