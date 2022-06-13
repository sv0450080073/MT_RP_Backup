﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Model.Metrics.Dto
{
    public class ReprocessSearchData
    {
      public  SearchCondition SearchCondition { get; set; } = new SearchCondition();
      public  List<TrackDataGrid> TrackDataGrid { get; set; } = new List<TrackDataGrid>();
    }
    public class SearchCondition
    {
        public int FlowId { get; set; }
        public bool IsImportMetric { get; set; }
        public int RemainProcData { get; set; }
        public bool IsRunMultiThread { get; set; }
        public bool IsRunParallel { get; set; }
        public int NumberOfThread { get; set; }
        public string  ImportServiceUrl { get; set; }
       
    }
}
