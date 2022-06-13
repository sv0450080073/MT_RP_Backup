using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Model.Metrics.Dto
{
   public class TrackDataGrid
    {
        public bool IsChecked { get; set; }
        public string InOutboxId { get; set; }
        public string TransactionType { get; set; }
        public string IntergrationStatus { get; set; }
        public string FileName { get; set; }
        public string Note { get; set; }
        public string Files { get; set; }

    
    }
}
