using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Model.Metrics.Dto
{
   public  class TrackSearchData
    {
        public TrackSearchData()
        {
            Files = "";
        }
        // public FlowData Flow { get; set; }
        public string Files { get; set; }
        public int FlowId { get; set; }
        public string PTransKeyIdIndex { get; set; }
        public string PInboxIdIndex { get; set; }
        public string POutboxIdIndex { get; set; }
        public string PYearQuaterIdIndex { get; set; }
        public string PFromCustIdIndex { get; set; }
        public string PToCustIdIndex { get; set; }
        public string PTransactionIdIndex { get; set; }
        public string PVersionIndex { get; set; }
        public string PCodePage { get; set; }           
        public string Item08 { get; set; }
        public string Item09 { get; set; }
        public string Item10 { get; set; }
        public string EndItem { get; set; }
        public string Extension { get; set; }
    }
   
}
