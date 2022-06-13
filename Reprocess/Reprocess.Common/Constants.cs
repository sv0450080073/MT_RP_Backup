using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Common
{
    public static class Constants
    {
        public static char argsDelimiter = '_';
        public static string DiMetricsConnectionString = ConfigurationManager.ConnectionStrings["DImetricsConnectionString"].ConnectionString ?? "" ;
        public static string MainConnectionString = ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString ?? "";
        public static string DiASCIIR9ConnectionString = ConfigurationManager.ConnectionStrings["DiASCIIR9ConnectionString"].ConnectionString ?? "";
        public static string DiASCIIR9RepConnectionString = ConfigurationManager.ConnectionStrings["DiASCIIR9_RepConnectionString"].ConnectionString ?? "";
        public const string PInbox = "inbox";
        public const string POutbox = "outbox";
        public static string doc850 = "850";
        public static string doc810 = "810";
        public static string doc856 = "856";
        public static string doc855 = "855";
        public static string doc860 = "860";
        public static string doc865 = "865";
        public static string doc180 = "180";
        public static int PArgsIndexMaxLength = 6;
        public static int PFromCustIdIndex = 3;
        public static int PVersionIndex = 6;
        public static int PCodePage = 7;
        public static int PToCustIdIndex = 4;
        public static int PInboxIdIndex = 1;
        public static int POutboxIdIndex = 1;
        public static int PYearQuaterIdIndex = 2;
        public static int PTransKeyIdIndex = 0;
        public static int PTransactionIdIndex = 5;
        public static string NotExistInMTDb = "Not exist in DiMetrics database";


        public const int ResultFail = 0;
        public const int ResultPass = 1;
        public const int ResultException = 4;
        public const int ResultParamInvalid = 2;
        public const int ResultSuccess = 3;
        public const int ResultProcessing = 4;
        public const int ResultAccepted = 6;//skip to check metrics
        public const int ResultIgnore = 7;// ignore the transaction
        public const int ResultImporting = 8; // Import to Queue Data table
        public const int ResultInvalidFile = 2;
        public const int ResultPONotFound = 2;
        public const int ResultErrorException = 5;


        public const string SP_GetViolation_IsMatchExists = "sp_GetViolation_IsMatchExist";
        public const string SP_UPDATEISMATCHHVIOLATION_DATA = "sp_UpdateIsMatch_ViolationData";
    }
}
